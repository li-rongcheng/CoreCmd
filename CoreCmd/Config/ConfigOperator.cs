﻿using CoreCmd.CommandLoading;
using NetCoreUtils.Text.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CoreCmd.Config
{
    interface IConfigOperator
    {
        /// <param name="dllfile">Either full path or just file name with extension in the current directory</param>
        void AddCommandAssembly(string dllfile);

        /// <param name="dllfile">Either full path or just file name with extension in the current directory</param>
        void RemoveCommandAssembly(string dllfile);

        /// <returns>Full path of all registered assembly DLLs</returns>
        IEnumerable<string> ListCommandAssemblies();

        void SaveChanges();
    }

    class ConfigOperator : IConfigOperator
    {
        XmlUtil<CoreCmdConfig> _xmlUtil = new XmlUtil<CoreCmdConfig>();
        CoreCmdConfig _config = null;
        string _configFileFullPath;

        public ConfigOperator()
        {
            // load config file
            _configFileFullPath = Global.ConfigFileFullPath;
            if (File.Exists(_configFileFullPath))
                _config = _xmlUtil.ReadFromFile(_configFileFullPath);
            else
                _config = new CoreCmdConfig();
        }

        /// <summary>
        /// If the commands in the specified dll do not exist, add the dll to the config
        /// </summary>
        public void AddCommandAssembly(string dllfile)
        {
            ICommandClassLoader _loader = new CommandClassLoader();
            IAssemblyLoadable _assemblyLoadable = new AssemblyLoadable();

            if (_config != null)
            {
                // if dllname is not full path, compose the current dir to make a full path
                string targetFilePath = Path.IsPathFullyQualified(dllfile) ? dllfile : Path.Combine(Directory.GetCurrentDirectory(), dllfile);
                if (File.Exists(targetFilePath))
                {
                    // add wehn the assembly does not exist
                    if (_config.CommandAssemblies.Where(c => c.Path.ToLower().Equals(targetFilePath.ToLower())).Count() == 0)
                    {
                        _config.AddCommandAssembly(targetFilePath);
                        Console.WriteLine($"Successfully added assembly: {targetFilePath}");
                    }
                    else
                        Console.WriteLine($"Command assembly already exsits: {targetFilePath}");
                }
                else
                    Console.WriteLine($"Assembly file not exists: {targetFilePath}");
            }
            else
                Console.WriteLine("Error: configuration not loaded");
        }

        public IEnumerable<string> ListCommandAssemblies()
        {
            if( _config != null)
                return _config.CommandAssemblies.Select(a => a.Path);
            else
            {
                Console.WriteLine("Error: configuration not loaded");
                return null;
            }
        }

        public void RemoveCommandAssembly(string dllPath)
        {
            if (_config != null)
                _config.CommandAssemblies.RemoveAll(a => a.Path.ToLower().Equals(dllPath.ToLower()));
            else
                Console.WriteLine("Error: configuration not loaded");
        }

        public void SaveChanges()
        {
            var _xmlUtil = new XmlUtil<CoreCmdConfig>();
            _xmlUtil.WriteToFile(_config,_configFileFullPath);
        }
    }
}
