using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class Argument : ASTNode
    {
        public Token Token { get; set; }
        public string Name { get; set; }

        public Argument(Token token, string value)
        {
            Token = token;
            Name = value;
        }
    }
}
