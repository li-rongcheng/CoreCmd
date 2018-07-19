﻿using CoreCmd.CommandFind;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCmd.BuiltinCommands
{
    class HelpCommand
    {
        ICommandFinder _commandFinder = new CommandFinder();

        public void Show(string command)
        {
            const string commandPostfix = "command";
            var allClassTypes = _commandFinder.GetAllCommandClasses(commandPostfix);
            var singleCommandExecutor = _commandFinder.GetSingleCommandExecutor(allClassTypes, new string[]{ command });

            if(singleCommandExecutor != null)
                singleCommandExecutor.PrintHelp();
            else
                Console.WriteLine("No command object found");
        }
    }
}
