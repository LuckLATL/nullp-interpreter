using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    [DebuggerDisplay("[Boolean Expression '{Left, nq} {Operator, nq} {Right, nq}']")]
    public class BooleanExpression : ASTNode
    {
        public ASTNode Left { get; set; }
        public TokenType Operator { get; set; }
        public ASTNode Right { get; set; }

        public override string? ToString()
        {
            return $"{Left} {TokenTypeExtension.TokenTypeToReadableString(Operator)} {Right}";
        }
    }
}
