using NullPInterpreter.Interpreter.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class ClassInstanceCreation : ASTNode
    {
        public FunctionCall ClassToCreate { get; set; }
        public ClassSymbol ClassSymbol { get; set; }
    }
}
