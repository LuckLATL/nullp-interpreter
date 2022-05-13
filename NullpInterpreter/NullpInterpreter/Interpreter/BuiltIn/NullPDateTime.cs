using NullPInterpreter.Interpreter.AST;
using NullPInterpreter.Interpreter.Symbols;
using NullPInterpreter.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.BuiltIn
{
    public class NullPDateTime : BuiltInClassSymbol
    {
        public override string Name { get; set; } = "DateTime";
        public override SymbolType Type { get; set; } = SymbolType.Class;
        public override ClassDeclaration Declaration { get; set; } = GetClass();

        private DateTime dateTime = new DateTime();

        public NullPDateTime()
        {
            Functions.Add("Now", new Func<List<object>, object>((args) => { Now(); return new NullObject(); }));
            Functions.Add("ToString", new Func<List<object>, object>((args) => { return ToString(args[0].ToString()); }));
            ClassSymbols = GetScope();
        }

        public void Now()
        {
            dateTime = DateTime.Now;
        }

        public string ToString(string format)
        {
            return dateTime.ToString(format);
        }


        public override BuiltInClassSymbol New() { return new NullPDateTime(); }
    }
}
