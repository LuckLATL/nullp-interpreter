using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.BuiltIn
{
    public static class NullPConsole
    {
        public static void WriteLine(List<object> args)
        {
            string outputString = GetStringForObject(args[0]);
            Console.WriteLine($" [{DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}] " + outputString);
        }

        public static string ReadLine(List<object> args)
        {
            Console.Write(" > ");
            return Console.ReadLine();
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
