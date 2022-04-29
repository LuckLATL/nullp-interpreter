using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class StringLiteral : ASTNode
    {
        public string Value { get; set; }

        public StringLiteral(string value)
        {
            Value = value;
        }
    }
}
