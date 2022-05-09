using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    [DebuggerDisplay("[Return '{ReturnNode, nq}']")]
    public class ReturnStatement : ASTNode
    {
        public ASTNode ReturnNode { get; set; }
    }
}
