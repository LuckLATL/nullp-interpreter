using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class AssignmentOperator : ASTNode
    {
        public ASTNode LeftNode { get; set; }
        public Token Operator { get; set; }
        public ASTNode RightNode { get; set; }

        public AssignmentOperator(ASTNode leftNode, Token @operator, ASTNode rightNode)
        {
            LeftNode = leftNode;
            Operator = @operator;
            RightNode = rightNode;
        }
    }
}
