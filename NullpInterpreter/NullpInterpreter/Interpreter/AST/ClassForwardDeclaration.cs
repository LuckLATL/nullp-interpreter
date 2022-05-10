using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    [DebuggerDisplay("[Class Forward Declaration '{Name, nq}']")]
    public class ClassForwardDeclaration : ASTNode
    {
        public string Name { get; set; }

        public ClassForwardDeclaration(string name)
        {
            Name = name;
        }
    }
}
