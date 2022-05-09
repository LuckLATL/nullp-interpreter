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
            if (node == null)
                return null;

            switch (node)
            {
                case AssignmentOperator n:
                    return VisitAssignmentOperator(n);
                case BinaryOperator n:
                    return VisitBinaryOperator(n);
                case Block n:
                    return VisitBlock(n);
                case BooleanExpression n:
                    return VisitBooleanExpression(n);
                case FunctionCall n:
                    return VisitFunctionCall(n);
                case FunctionDeclaration n:
                    return VisitFunctionDeclaration(n);
                case IfStatement n:
                    return VisitIfStatement(n);
                case WhileStatement n:
                    return VisitWhileStatement(n);
                case IntegerLiteral n:
                    return VisitIntegerLiteral(n);
                case NamespaceDeclaration n:
                    return VisitNamespaceDeclaration(n);
                case NamespacePropertyCall n:
                    return VisitNamespacePropertyCall(n);
                case NoOperator n:
                    return VisitNoOperator(n);
                case ProgramElement n:
                    return VisitProgramElement(n);
                case StringLiteral n:
                    return VisitStringLiteral(n);
                case UnaryOperator n:
                    return VisitUnaryOperator(n);
                case Variable n:
                    return VisitVariable(n);
                case ReturnStatement n:
                    return VisitReturnStatement(n);
                case VariableDeclaration n:
                    return VisitVariableDeclaration(n);
                case FunctionForwardDeclaration n:
                    return VisitFunctionForwardDeclaration(n);
                case NullLiteral n:
                    return VisitNullLiteral(n);
                case FalseLiteral n:
                    return VisitFalseLiteral(n);
                case ClassDeclaration n:
                    return VisitClassDeclaration(n);
                case ClassInstanceCreation n:
                    return VisitClassInstanceCreation(n);
                case TrueLiteral n:
                    return VisitTrueLiteral(n);
                case List n:
                    return VisitList(n);
                case Indexer n:
                    return VisitIndexer(n);
            }

            throw new Exception("Unsupported type");
        }

        protected abstract object VisitWhileStatement(WhileStatement n);
        protected abstract object VisitIndexer(Indexer n);
        protected abstract object VisitList(List n);
        protected abstract object VisitClassInstanceCreation(ClassInstanceCreation n);
        protected abstract object VisitClassDeclaration(ClassDeclaration n);
        protected abstract object VisitTrueLiteral(TrueLiteral n);
        protected abstract object VisitFalseLiteral(FalseLiteral n);
        protected abstract object VisitNullLiteral(NullLiteral n);
        protected abstract object VisitFunctionForwardDeclaration(FunctionForwardDeclaration n);
        protected abstract object VisitReturnStatement(ReturnStatement n);
        protected abstract object VisitVariableDeclaration(VariableDeclaration n);
        protected abstract object VisitVariable(Variable n);
        protected abstract object VisitUnaryOperator(UnaryOperator n);
        protected abstract object VisitStringLiteral(StringLiteral n);
        protected abstract object VisitProgramElement(ProgramElement n);
        protected abstract object VisitNoOperator(NoOperator n);
        protected abstract object VisitNamespacePropertyCall(NamespacePropertyCall n);
        protected abstract object VisitNamespaceDeclaration(NamespaceDeclaration n);
        protected abstract object VisitIntegerLiteral(IntegerLiteral n);
        protected abstract object VisitIfStatement(IfStatement n);
        protected abstract object VisitFunctionDeclaration(FunctionDeclaration n);
        protected abstract object VisitFunctionCall(FunctionCall n);
        protected abstract object VisitBooleanExpression(BooleanExpression n);
        protected abstract object VisitBlock(Block n);
        protected abstract object VisitBinaryOperator(BinaryOperator n);
        protected abstract object VisitAssignmentOperator(AssignmentOperator n);
    }
}
