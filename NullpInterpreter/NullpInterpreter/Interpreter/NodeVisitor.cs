using NullPInterpreter.Interpreter.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter
{
    public abstract class NodeVisitor
    {
        public object Visit(ASTNode node)
        {
            if (node is BinaryOperator)
                return VisitBinaryOperation((BinaryOperator)node);
            else if (node is UnaryOperator)
                return VisitUnaryOperator((UnaryOperator)node);
            else if (node is AssignmentOperator)
                return VisitAssignmentOperator((AssignmentOperator)node);
            else if (node is Variable)
                return VisitVariable((Variable)node);
            else if (node is NoOperator)
                return VisitNoOperator((NoOperator)node);
            else if (node is NamespaceDeclaration)
                return VisitNamespace((NamespaceDeclaration)node);
            else if (node is Block)
                return VisitBlock((Block)node);
            else if (node is VariableDeclaration)
                return VisitVariableDeclaration((VariableDeclaration)node);
            else if (node is FunctionCall)
                return VisitFunctionCall((FunctionCall)node);
            else if (node is IfStatement)
                return VisitIfStatement((IfStatement)node);

                throw new Exception("Unsupported type");
        }

        protected abstract object VisitIfStatement(IfStatement node);
        protected abstract object VisitFunctionCall(FunctionCall node);
        protected abstract object VisitVariableDeclaration(VariableDeclaration node);
        protected abstract object VisitBlock(Block node);
        protected abstract object VisitNamespace(NamespaceDeclaration node);
        protected abstract object VisitNoOperator(NoOperator node);
        protected abstract object VisitVariable(Variable node);
        protected abstract object VisitAssignmentOperator(AssignmentOperator node);
        protected abstract object VisitUnaryOperator(UnaryOperator node);
        protected abstract object VisitBinaryOperation(BinaryOperator node);
    }
}
