﻿using NullPInterpreter.Interpreter.AST;
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
                case VariableDeclaration n:
                    return VisitVariableDeclaration(n);
            }

            throw new Exception("Unsupported type");
        }

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
