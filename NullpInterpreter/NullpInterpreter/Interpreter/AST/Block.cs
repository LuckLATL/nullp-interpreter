using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class Block : ASTNode
    {
        public List<ASTNode> Children { get; set; } = new List<ASTNode>();

        public Block() { }
    }
}
