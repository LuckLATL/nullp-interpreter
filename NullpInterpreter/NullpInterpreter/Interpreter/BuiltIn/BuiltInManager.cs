using NullPInterpreter.Interpreter.Symbols;
using NullPInterpreter.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.BuiltIn
{
    public static class BuiltInManager
    {
        public static List<BuiltInBase> BuiltIns = new List<BuiltInBase>()
        {
            new BuiltInNamespace()
            {
                Name = "Console",
                Children = new List<BuiltInBase>()
                {
                    new BuiltInFunction()
                    {
                        Name = "WriteLine",
                        Function = (args) => { NullPConsole.WriteLine(args); return new NullObject(); }
                    },
                    new BuiltInFunction()
                    {
                        Name = "ReadLine",
                        Function = (args) => { return NullPConsole.ReadLine(args); }
                    }
                }
            }
        };
    }

    public class BuiltInBase
    {
        public string Name { get; set; }
    }

    public class BuiltInNamespace : BuiltInBase
    {
        public List<BuiltInBase> Children { get; set; }
    }

    public class BuiltInClass : BuiltInBase
    {

    }

    public class BuiltInFunction : BuiltInBase
    {
        public Func<List<object>, object> Function { get; set; }
    }
}
