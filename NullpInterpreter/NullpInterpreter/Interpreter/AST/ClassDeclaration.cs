using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class ClassDeclaration : ASTNode
    {
        public Block Block { get; set; }
        public string Name { get; set; }

        public ClassDeclaration(Block block, string name)
        {
            Block = block;
            Name = name;
        }
    }
}
