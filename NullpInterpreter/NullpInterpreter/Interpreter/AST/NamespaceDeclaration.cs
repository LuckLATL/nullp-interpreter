using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class NamespaceDeclaration : ASTNode
    {
        public Block Block { get; set; }
        public string Name { get; set; }

        public NamespaceDeclaration(Block block, string name)
        {
            Block = block;
            Name = name;
        }
    }
}
