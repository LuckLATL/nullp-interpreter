using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class IntegerLiteral : ASTNode
    {
        public double Value { get; set; }

        public IntegerLiteral(double value)
        {
            Value = value;
        }
    }
}
