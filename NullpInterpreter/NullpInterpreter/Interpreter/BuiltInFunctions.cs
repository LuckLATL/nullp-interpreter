﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter
{
    public static class BuiltInFunctions
    {
        private static List<string> functions = new List<string>()
        {
            "WriteLine",
            "ReadLine"
        };

        public static bool CheckIfBuiltInFunction(string functionName)
        {
            return (functions.Contains(functionName));
        }

        public static object ExecuteBuiltInFunction(string functionName, List<object> arguments)
        {
            switch (functionName)
            {
                case "WriteLine":
                    if (arguments[0] == null)
                        arguments[0] = "NULL";
                    Console.WriteLine($" [{DateTime.Now}] " + arguments[0].ToString());
                    break;
                case "ReadLine":
                    Console.Write(" > ");
                    return Console.ReadLine();
                default:
                    break;
            }
            return null;
        }
    }
}
