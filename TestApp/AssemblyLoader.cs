﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Thrift.Protocol;
using Thrift.Transport;

namespace TestApp
{
    public class Client
    {
        public Type ClientType { get; set; }
        public MethodInfo Method { get; set; }
    }

    public class AssemblyLoader
    {
        private readonly string _assembly;

        public AssemblyLoader(string assembly)
        {
            _assembly = assembly;
        }


        public List<string> MethodNames
        {
            get { return methodClassMappings.Keys.ToList(); }
        }


        readonly Dictionary<string, Client> methodClassMappings= new Dictionary<string, Client>();

        public Client GetClient(string methodName)
        {
            return methodClassMappings[methodName];
        }

        public void Load()
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), _assembly);

            var assembly = Assembly.LoadFile(fullPath);
            if (assembly != null)
            {
                IEnumerable<Type> types = assembly.GetTypes().Where(x => x.Name == "Client");
                foreach (var type in types)
                {
                    var interfaceType = type.GetInterface("Iface");
                    foreach (var method in interfaceType.GetMethods())
                    {
                        methodClassMappings.Add(method.Name, new Client
                                                                 {
                                                                     ClientType = type,
                                                                     Method = method
                                                                 });
                    }
                }
            }

            
            // Find Client classes... that inherit from IFace
            // 

        }
    }
}