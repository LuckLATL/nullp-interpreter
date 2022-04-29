using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    internal class RootElement : ASTNode
    {
        public List<ASTNode> Children { get; set; } = new List<ASTNode>();
    }
}
