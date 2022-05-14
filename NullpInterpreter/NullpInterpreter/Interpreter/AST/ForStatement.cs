using NullPInterpreter.Interpreter.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class ForStatement : ASTNode
    {
        public VariableDeclaration VariableDeclaration { get; set; }
        public BooleanExpression BooleanExpression { get; set; }
        public ASTNode Statement { get; set; }
        public Block Block { get; set; }

        public BlockSymbol BlockSymbol { get; set; }
    }
}
