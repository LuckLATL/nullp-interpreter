using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    [DebuggerDisplay("[Function Forward Declaration '{Name, nq}']")]
    public class FunctionForwardDeclaration : ASTNode
    {
        public string Name { get; set; }
    }
}
