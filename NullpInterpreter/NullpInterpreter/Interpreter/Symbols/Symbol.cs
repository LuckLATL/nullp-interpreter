using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.Symbols
{
    public class Symbol
    {
        public virtual string Name { get; set; }
        public virtual SymbolType Type { get; set; }
    }
}
