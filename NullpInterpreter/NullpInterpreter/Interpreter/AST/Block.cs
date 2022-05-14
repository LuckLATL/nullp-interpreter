using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    [DebuggerDisplay("[Block '{Children.Count, nq}']")]
    public class Block : ASTNode
    {
        public List<ASTNode> Children { get; set; } = new List<ASTNode>();

        public Block() { }

        public override string? ToString()
        {
            return $"[Block]";
        }
    }
}
