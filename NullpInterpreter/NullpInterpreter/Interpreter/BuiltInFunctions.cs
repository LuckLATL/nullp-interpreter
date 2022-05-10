using System;
using System.Collections.Generic;
using System.Globalization;
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

                    string outputString = GetStringForObject(arguments[0]);
                    Console.WriteLine($" [{DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}] " + outputString);
                    break;
                case "ReadLine":
                    Console.Write(" > ");
                    return Console.ReadLine();
                default:
                    break;
            }
            return null;
        }

        public static string GetStringForObject(object o)
        {
            string output = "";

            switch (o)
            {
                case List<object> l:
                    output += "[ ";
                    foreach (var item in l)
                    {
                        output += GetStringForObject(item) + ", ";
                    }
                    output = output.Substring(0, output.Length - 2);
                    output += " ]";
                    break;
                case null:
                    output = "<NULL>";
                    break;
                default:
                    output = o.ToString();
                    break;
            }

            return output;
        }
    }
}
