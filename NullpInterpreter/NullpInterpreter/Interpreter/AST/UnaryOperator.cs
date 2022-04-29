using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class UnaryOperator : ASTNode
    {
        public Token Token { get; set; }
        public Token Operator { get; set; }
        public ASTNode Expression { get; set; }

        public UnaryOperator(Token @operator, ASTNode expression)
        {
            Token = @operator;
            Operator = @operator;
            Expression = expression;
        }
    }
}
