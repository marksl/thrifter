using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Microsoft.Build.Tasks;
using Microsoft.Build.Utilities;

namespace TestApp
{
 
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();

            List<Item> items = new List<Item>
                                   {
                                       new Item("https://lb-sm-cpqa2-van.dev-globalrelay.net:7443/core/message.thrift", 1), 
                                       new Item("https://lb-sm-cpqa1-van.dev-globalrelay.net:7443/core/message.thrift", 2),
                                       new Item("https://lb-sm-smsnap1-van.dev-globalrelay.net:7443/core/message.thrift", 3)
                                   };
            this.comboBox1.Width = 300;
            this.comboBox1.DataSource = items;
            this.comboBox1.DisplayMember = "Name";
            this.comboBox1.ValueMember = "Id";}

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Process_Click(object sender, EventArgs e)
        {
            // 1. Save textbox to .thrift file
            //thriftText.Text


            CreateThriftFile("temp.thrift");

            // 2. Run code gen on .thrift file

            var outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "gen-csharp");
            if (Directory.Exists(outputFolder))
            {
                try
                {
                    Directory.Delete(outputFolder, true);
                }
                catch
                {
                    // Delete failed. Overwrite the files and hope for the best.
                }
            }

            var process = new System.Diagnostics.Process();
            var startInfo = new System.Diagnostics.ProcessStartInfo
                                {
                                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                                    FileName = "cmd.exe",
                                    Arguments = "/C thrift-0.9.0.exe --gen csharp " + "temp.thrift"
                                };
            process.StartInfo = startInfo;
            process.Start();



            // 3. Compile thrift file.

            // Create a new Engine object.
            FileLogger logger = new FileLogger();
            // Set the logfile parameter to indicate the log destination
            logger.Parameters = @"logfile=C:\git\thrifter\build.log";

            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
<ItemGroup>
  <Reference Include=""Thrift"">
      <HintPath>{0}</HintPath>
      <Private>False</Private>
</Reference>
</ItemGroup>


    <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />

  
</Project>
";

  //          <PropertyGroup>
  //  <PrepareForRunDependsOn>$(PrepareForRunDependsOn);MyCopyFilesToOutputDirectory</PrepareForRunDependsOn>
  //</PropertyGroup>

  //<Target Name=""MyCopyFilesToOutputDirectory"">
  //  <Copy SourceFiles=""@(None)"" DestinationFolder=""$(OutDir)"" />
  //</Target>

            xml = string.Format(xml, Path.Combine(Directory.GetCurrentDirectory(), "Thrift.dll"));

            byte[] data = Encoding.Default.GetBytes(xml);


            XmlReader reader = new XmlTextReader(new MemoryStream(data));

            // Create a new Project object.
            var project = new Project(reader);
            project.FullPath = Path.Combine(Directory.GetCurrentDirectory(), ".\\foo\\");
            
            //var propertyGroup = project.Properties
            var properties = new Dictionary<string, string>
                                 {
                                     {"Configuration", "Debug"},
                                     {"Platform","AnyCPU"},
                                     {"ProjectGuid", Guid.NewGuid().ToString()},
                                     {"ProjectVersion", "8.0.30703"},
                                     {"SchemaVersion", "2.0"},
                                     {"AssemblyName", "TempThriftGen"},
                                     {"TargetFrameworkVersion", "v4.0"},
                                     {"OutputType", "Library"},
                                     {"FileAlignment", "512"},
                                     {"DebugSymbols", "true"},
                                     {"DebugType", "full"},
                                     {"Optimize", "false"},
                                     {"OutputPath", ".\\compiled\\"},
                                     {"DefineConstants", "DEBUG;TRACE"}
                                 };
            foreach (var prop in properties)
            {
                project.SetProperty(prop.Key, prop.Value);
            }

            foreach (string file in Directory.GetFiles(outputFolder, "*.cs"))
            {
                int index = file.LastIndexOf("\\gen-csharp", StringComparison.Ordinal);
                string classFileName = file.Substring(index + 1, file.Length - index -1);

                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), classFileName);
                project.AddItem("Compile", fullPath);
            }

            project.AddItem("Reference", "System.Core");

            project.Build("Build", new List<ILogger>{logger});


            // Load the project with the following XML, which contains 
            // two ItemGroups.


            // TODO: Make this recursive

            //csc.BuildEngine = new Microsoft.Build.Engine();
        }

        private void CreateThriftFile(string thriftFileName)
        {
            var fileName = Path.Combine(Directory.GetCurrentDirectory(), thriftFileName);

            File.Delete(fileName);

            using (FileStream fs = File.Create(fileName))
            {
                using (var sw = new StreamWriter(fs))
                {
                    sw.Write(thriftText.Text);
                }
            }
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public Item(string name, int id)
        {
            Name = name;
            Id = id;
        }
    }

    public class MyTask : Task
    {
        public override bool Execute()
        {
            return true;
        }
    }

}
