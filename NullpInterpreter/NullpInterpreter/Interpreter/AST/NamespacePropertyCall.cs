using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class NamespacePropertyCall : ASTNode
    {
        public string CallerName { get; set; }
        public ASTNode CalledNode { get; set; }
    }
}
