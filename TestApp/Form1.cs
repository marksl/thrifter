using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Thrift.Protocol;
using Thrift.Transport;

namespace TestApp
{
    public partial class Form1 : Form
    {
        private AssemblyLoader a;
        private Client client;

        public Form1()
        {
            InitializeComponent();

            var items = new List<Item>
                            {
                                new Item("http://localhost:53959/HttpTest/json.thrift", "0"),
                                new Item("http://localhost:53959/HttpTest/binary.thrift", "1"),
                                new Item("http://localhost:53959/HttpTest/compact.thrift", "2"),
                                new Item("localhost:8080", "4")
                            };
            urlComboBox.DataSource = items;
            urlComboBox.DisplayMember = "Name";
            urlComboBox.ValueMember = "Id";

            var protocolItems = new List<Item>
                            {
                                new Item("TJSONProtocol", "0"),
                                new Item("TBinaryProtocol", "1"),
                                new Item("TCompactProtocol", "2"),
                            };
            protocolComboBox.DataSource = protocolItems;
            protocolComboBox.DisplayMember = "Name";
            protocolComboBox.ValueMember = "Id";

            methodComboBox.DisplayMember = "Name";
            methodComboBox.ValueMember = "Id";
        }

        private void Process_Click(object sender, EventArgs e)
        {
            var g = new CSharpGenerator(thriftText.Text);
            //string assembly = g.Generate();
            string assembly = ".\\compiled\\TempThriftGen.dll";

            a = new AssemblyLoader(assembly);
            a.Load();


            methodComboBox.DataSource = a.MethodNames.Select(x => new Item(x, x)).ToList();
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                var url = ((Item) urlComboBox.SelectedItem).Name;
                
                TTransport transport = GetTransport(url);
                TProtocol protocol = GetProtocol(transport);

                object instance = Activator.CreateInstance(client.ClientType, protocol);
                try
                {
                    client.Method.Invoke(instance, null);
                }
                catch
                {
                }
            }
        }

        private TProtocol GetProtocol(TTransport transport)
        {
            TProtocol protocol;
            switch (((Item) protocolComboBox.SelectedItem).Name)
            {
                case "TJSONProtocol":
                    protocol = new TJSONProtocol(transport);
                    break;

                case "TBinaryProtocol":
                    protocol = new TBinaryProtocol(transport);
                    break;

                case "TCompactProtocol":
                    protocol = new TCompactProtocol(transport);
                    break;

                default:
                    throw new InvalidOperationException();
            }
            return protocol;
        }

        private static TTransport GetTransport(string url)
        {
            TTransport transport;
            if (url.StartsWith("http"))
            {
                var uri = new Uri(url);
                var httpClient = new THttpClient(uri);
                transport = httpClient;
            }
            else
            {
                var parts = url.Split(new[] {':'});

                transport = new TSocket(parts[0], Convert.ToInt32(parts[1]));
            }
            return transport;
        }

        private void methodComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            client = a.GetClient((string) methodComboBox.SelectedValue);
        }
    }

    public class Item
    {
        public Item(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; set; }
        public string Id { get; set; }
    }
}