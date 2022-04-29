using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class IfStatement : ASTNode
    {
        public ASTNode BooleanExpression { get; set; }
        public ASTNode Block { get; set; }
        public ASTNode ElseBlock { get; set; }

    }
}
