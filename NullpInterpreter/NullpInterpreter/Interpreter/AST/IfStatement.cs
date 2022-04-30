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
        public Block Block { get; set; }
        public Block ElseBlock { get; set; } = new Block();

    }
}
