using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    [DebuggerDisplay("[Namespace Declaration '{Name, nq}']")]
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
