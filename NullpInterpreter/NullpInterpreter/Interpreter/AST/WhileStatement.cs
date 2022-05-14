using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    [DebuggerDisplay("[While '{BooleanExpression, nq}']")]
    public class WhileStatement : ASTNode
    {
        public ASTNode BooleanExpression { get; set; }
        public Block Block { get; set; }

        public override string? ToString()
        {
            return $"while ({BooleanExpression})";
        }
    }
}
