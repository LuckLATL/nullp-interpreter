using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class Variable : ASTNode
    {
        public Token Token { get; set; }
        public object Value { get; set; }

        public Variable(Token token, object value)
        {
            Token = token;
            Value = value;
        }
    }
}
