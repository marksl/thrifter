using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;

namespace TestApp
{
    public class CSharpGenerator
    {
        private readonly string _thriftBody;

        public CSharpGenerator(string thriftBody)
        {
            _thriftBody = thriftBody;
        }

        private void CreateThriftFile(string thriftFileName)
        {
            var fileName = Path.Combine(Directory.GetCurrentDirectory(), thriftFileName);

            File.Delete(fileName);

            using (FileStream fs = File.Create(fileName))
            {
                using (var sw = new StreamWriter(fs))
                {
                    sw.Write(_thriftBody);
                }
            }
        }


        void DeleteFolder(string folderName)
        {
            var outputFolder = Path.Combine(Directory.GetCurrentDirectory(), folderName);
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
        }

        public string Generate()
        {
            // 1. Save textbox to .thrift file
            //thriftText.Text


            CreateThriftFile("temp.thrift");

            // 2. Run code gen on .thrift file

            DeleteFolder("gen-csharp");
            DeleteFolder("obj");
            DeleteFolder("compiled");

            var process = new System.Diagnostics.Process();
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C thrift-0.9.0.exe --gen csharp " + "temp.thrift"
            };
            process.StartInfo = startInfo;
            process.Start();



            // 3. Compile generated C# files

            // Create a new Engine object.
            var logger = new FileLogger {Parameters = @"logfile=C:\git\thrifter\build.log"};

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

            xml = string.Format(xml, Path.Combine(Directory.GetCurrentDirectory(), "Thrift.dll"));

            byte[] data = Encoding.Default.GetBytes(xml);


            using (var memoryStream = new MemoryStream(data))
            {
                using (XmlReader reader = new XmlTextReader(memoryStream))
                {

                    // Create a new Project object.
                    var project = new Project(reader);
                    project.ProjectCollection.UnloadAllProjects();
                    project.FullPath = Path.Combine(Directory.GetCurrentDirectory(), ".\\foo\\");

                    //var propertyGroup = project.Properties
                    var properties = new Dictionary<string, string>
                                         {
                                             {"Configuration", "Debug"},
                                             {"Platform", "AnyCPU"},
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

                    var outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "gen-csharp");
                    foreach (string file in Directory.GetFiles(outputFolder, "*.cs"))
                    {
                        int index = file.LastIndexOf("\\gen-csharp", StringComparison.Ordinal);
                        string classFileName = file.Substring(index + 1, file.Length - index - 1);

                        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), classFileName);
                        project.AddItem("Compile", fullPath);
                    }

                    project.AddItem("Reference", "System.Core");

                    bool success = project.Build("Build", new List<ILogger> {logger});
                    return success ? ".\\compiled\\TempThriftGen.dll" : null;
                }
            }
        }
    }
}