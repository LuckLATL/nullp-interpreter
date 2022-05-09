using NullPInterpreter.Interpreter.AST;
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
        public ScopedSymbolTable CurrentScope { get; set; } = null;

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
            CurrentScope.AddSymbol(new Symbol() { Name = n.Variable.Name, Type = SymbolType.Variable });
            return null;
        }

        protected override object VisitFunctionCall(FunctionCall n)
        {
            object obj = CurrentScope.LookUpSymbol(n.FunctionName);
            n.FunctionSymbol = (FunctionSymbol)obj;
            foreach (ASTNode node in n.Arguments)
                Visit(node);
            return null;
        }

        protected override object VisitFunctionDeclaration(FunctionDeclaration n)
        {
            FunctionSymbol fsymb = new FunctionSymbol() { Name = n.FunctionName, Type = SymbolType.Function, Declaration = n };
            CurrentScope.AddSymbol(fsymb);
            CurrentScope = new ScopedSymbolTable(n.FunctionName, CurrentScope.ScopeLevel + 1, CurrentScope);

            n.Arguments.ForEach(argument => CurrentScope.AddSymbol(new Symbol { Name = argument.Name, Type = SymbolType.Argument }));
            Visit(n.Block);
            CurrentScope = CurrentScope.EnclosingScope;

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
            CurrentScope.AddSymbol(new Symbol() { Name = n.Name, Type = SymbolType.Namespace });
            CurrentScope = new ScopedSymbolTable(n.Name, CurrentScope.ScopeLevel + 1, CurrentScope);

            Visit(n.Block);

            CurrentScope = CurrentScope.EnclosingScope;

            return null;
        }

        protected override object VisitNamespacePropertyCall(NamespacePropertyCall n)
        {
            CurrentScope.LookUpSymbol(n.CallerName);
            //Visit(n.CalledNode);
            return null;
        }

        protected override object VisitNoOperator(NoOperator n)
        {
            return null;
        }

        protected override object VisitProgramElement(ProgramElement n)
        {
            CurrentScope = new ScopedSymbolTable("Program", 1);
            CurrentScope.InitializeBuiltIns();

            n.Children.ForEach(child => Visit(child));

            CurrentScope = CurrentScope.EnclosingScope;
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
            CurrentScope.LookUpSymbol(n.Name);
            return null;
        }

        protected override object VisitReturnStatement(ReturnStatement n)
        {
            Visit(n.ReturnNode);
            return null;
        }

        protected override object VisitFunctionForwardDeclaration(FunctionForwardDeclaration n)
        {
            FunctionSymbol fsymb = new FunctionSymbol() { Name = n.Name, Type = SymbolType.Function, Declaration = null };
            CurrentScope.AddSymbol(fsymb);
            return null;
        }

        protected override object VisitTrueLiteral(TrueLiteral n)
        {
            return null;
        }

        protected override object VisitFalseLiteral(FalseLiteral n)
        {
            return null;
        }

        protected override object VisitNullLiteral(NullLiteral n)
        {
            return null;
        }

        protected override object VisitClassInstanceCreation(ClassInstanceCreation n)
        {
            object obj = CurrentScope.LookUpSymbol(n.ClassToCreate.FunctionName);
            n.ClassSymbol = (ClassSymbol)obj;
            return null;
        }

        protected override object VisitClassDeclaration(ClassDeclaration n)
        {
            ClassSymbol csymb = new ClassSymbol() { Name = n.Name, Type = SymbolType.Class, Declaration = n };
            CurrentScope.AddSymbol(csymb);
            CurrentScope = csymb.ClassSymbols = new ScopedSymbolTable(n.Name, CurrentScope.ScopeLevel + 1, CurrentScope);
            
            Visit(n.Block);
            CurrentScope = CurrentScope.EnclosingScope;

            return null;
        }

        protected override object VisitList(List n)
        {
            foreach (ASTNode node in n.Items)
            {
                Visit(node);
            }
            return null;
        }

        protected override object VisitIndexer(Indexer n)
        {
            Visit(n.Variable);
            return null;
        }
    }
}
