using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class FunctionDeclaration : ASTNode
    {
        public string FunctionName { get; set; }
        public List<ASTNode> Arguments { get; set; } = new List<ASTNode>();
        public ASTNode Block { get; set; }
    }
}
