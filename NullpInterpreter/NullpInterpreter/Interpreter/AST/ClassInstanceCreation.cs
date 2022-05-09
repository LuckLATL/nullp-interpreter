using NullPInterpreter.Interpreter.Symbols;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    [DebuggerDisplay("[Class Instance Creation '{ClassSymbol.Name}']")]
    public class ClassInstanceCreation : ASTNode
    {
        public FunctionCall ClassToCreate { get; set; }
        public ClassSymbol ClassSymbol { get; set; }
    }
}
