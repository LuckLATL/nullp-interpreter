using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class BooleanExpressionCombination : ASTNode
    {
        public ASTNode Left { get; set; }
        public ASTNode Right { get; set; }
        public TokenType Operator { get; set; }
    }
}
