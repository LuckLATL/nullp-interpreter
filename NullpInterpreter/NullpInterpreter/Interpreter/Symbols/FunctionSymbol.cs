using NullPInterpreter.Interpreter.AST;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.Symbols
{
    [DebuggerDisplay("[Function Symbol '{Name, nq}']")]
    public class FunctionSymbol : Symbol
    {
        public FunctionDeclaration Declaration { get; set; }
    }
}
