using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class List : ASTNode
    {
        public List<ASTNode> Items { get; set; } = new List<ASTNode>();

        public override string? ToString()
        {
            return $"List[{Items.Count}]";
        }
    }
}
