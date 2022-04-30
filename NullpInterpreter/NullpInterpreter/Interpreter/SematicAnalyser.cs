﻿using NullPInterpreter.Interpreter.AST;
using NullPInterpreter.Interpreter.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter
{
    public class SematicAnalyser : NodeVisitor
    {
        ScopedSymbolTable currentScope = null;

        protected override object VisitAssignmentOperator(AssignmentOperator n)
        {
            Visit(n.LeftNode);
            Visit(n.RightNode);
            return null;
        }

        protected override object VisitBinaryOperator(BinaryOperator n)
        {
            Visit(n.LeftNode);
            Visit(n.RightNode);
            return null;
        }

        protected override object VisitBlock(Block n)
        {
            n.Children.ForEach(child => Visit(child));
            return null;
        }

        protected override object VisitBooleanExpression(BooleanExpression n)
        {
            Visit(n.Left);
            Visit(n.Right);
            return null;
        }

        protected override object VisitVariableDeclaration(VariableDeclaration n)
        {
            Visit(n.InitialDefinition);
            currentScope.AddSymbol(new Symbol() { Name = n.Variable.Name, Type = SymbolType.Variable });
            return null;
        }

        protected override object VisitFunctionCall(FunctionCall n)
        {
            object obj = currentScope.LookUpSymbol(n.FunctionName);
            n.FunctionSymbol = (FunctionSymbol)obj;
            foreach (ASTNode node in n.Arguments)
                Visit(node);
            return null;
        }

        protected override object VisitFunctionDeclaration(FunctionDeclaration n)
        {
            FunctionSymbol fsymb = new FunctionSymbol() { Name = n.FunctionName, Type = SymbolType.Function, Declaration = n };
            currentScope.AddSymbol(fsymb);
            currentScope = new ScopedSymbolTable(n.FunctionName, currentScope.ScopeLevel + 1, currentScope);

            n.Arguments.ForEach(argument => currentScope.AddSymbol(new Symbol { Name = argument.Name, Type = SymbolType.Argument }));
            Visit(n.Block);
            currentScope = currentScope.EnclosingScope;

            return null;
        }

        protected override object VisitIfStatement(IfStatement n)
        {
            Visit(n.BooleanExpression);
            n.Block.Children.ForEach(child => Visit(child));
            n.ElseBlock.Children.ForEach(child => Visit(child));
            return null;
        }

        protected override object VisitIntegerLiteral(IntegerLiteral n)
        {
            return null;
        }

        protected override object VisitNamespaceDeclaration(NamespaceDeclaration n)
        {
            currentScope.AddSymbol(new Symbol() { Name = n.Name, Type = SymbolType.Namespace });
            currentScope = new ScopedSymbolTable(n.Name, currentScope.ScopeLevel + 1, currentScope);

            Visit(n.Block);

            currentScope = currentScope.EnclosingScope;

            return null;
        }

        protected override object VisitNamespacePropertyCall(NamespacePropertyCall n)
        {
            currentScope.LookUpSymbol(n.CallerName);
            Visit(n.CalledNode);
            return null;
        }

        protected override object VisitNoOperator(NoOperator n)
        {
            return null;
        }

        protected override object VisitProgramElement(ProgramElement n)
        {
            currentScope = new ScopedSymbolTable("Program", 1);
            currentScope.InitializeBuiltIns();

            n.Children.ForEach(child => Visit(child));

            currentScope = currentScope.EnclosingScope;
            return null;
        }

        protected override object VisitStringLiteral(StringLiteral n)
        {
            return null;
        }

        protected override object VisitUnaryOperator(UnaryOperator n)
        {
            Visit(n.Expression);
            return null;
        }

        protected override object VisitVariable(Variable n)
        {
            currentScope.LookUpSymbol(n.Name);
            return null;
        }
    }
}