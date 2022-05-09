using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class WhileStatement : ASTNode
    {
        public ASTNode BooleanExpression { get; set; }
        public Block Block { get; set; }
    }
}
