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
            object def = Visit(n.InitialDefinition);
            CurrentScope.AddSymbol(new VariableSymbol() { Name = n.Variable.Name, Type = SymbolType.Variable });
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
            NamespaceSymbol namespaceSymbol = new NamespaceSymbol() { Name = n.Name, Type = SymbolType.Namespace };
            CurrentScope.AddSymbol(namespaceSymbol);
            CurrentScope = new ScopedSymbolTable(n.Name, CurrentScope.ScopeLevel + 1, CurrentScope);
            namespaceSymbol.NamespaceSymbols = CurrentScope;
            n.SourceSymbol = namespaceSymbol;

            Visit(n.Block);

            CurrentScope = CurrentScope.EnclosingScope;

            return null;
        }

        protected override object VisitNamespacePropertyCall(NamespacePropertyCall n)
        {
            ASTNode temp = n.CalledNode;
            ScopedSymbolTable savedScope = CurrentScope;

            object callerSymbol = CurrentScope.LookUpSymbol(n.CallerName);
            n.SourceSymbol = (Symbol)callerSymbol;

            

            return null;

            //while (temp is NamespacePropertyCall nspc)
            //{
            //    object calledObj = CurrentScope.LookUpSymbol(nspc.CallerName);

            //    if (calledObj is NamespaceSymbol nsym)
            //    {
            //        CurrentScope = nsym.NamespaceSymbols;
            //    }
            //    else if (calledObj is ClassSymbol classSym)
            //    {
            //        CurrentScope = classSym.ClassSymbols;
            //    }

            //    temp = nspc.CalledNode;
            //}

            //if (temp is FunctionCall fcall)
            //{
            //    //fcall.FunctionSymbol = (FunctionSymbol)CurrentScope.LookUpSymbol(fcall.FunctionName);
            //}
            //else if (temp is Variable v)
            //{
            //    // ????
            //}

            //CurrentScope = savedScope;



            //Symbol source = CurrentScope.LookUpSymbol(n.CallerName);
            //n.SourceSymbol = source;

            //if (source is NamespaceSymbol ns)
            //{
            //    ScopedSymbolTable savedScope = CurrentScope;
            //    CurrentScope = ns.NamespaceSymbols;
            //    Visit(n.CalledNode);
            //    CurrentScope = savedScope;
            //}

            //return null;
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
            object obj = null;

            ASTNode temp = n.ClassToCreate;
            ScopedSymbolTable oldScope = CurrentScope;

            while (temp is NamespacePropertyCall nspc)
            {
                CurrentScope = ((NamespaceSymbol)CurrentScope.LookUpSymbol(nspc.CallerName)).NamespaceSymbols;
                temp = nspc.CalledNode;
            }

            obj = CurrentScope.LookUpSymbol(((FunctionCall)temp).FunctionName);

            CurrentScope = oldScope;

            n.ClassSymbol = (ClassSymbol)obj;
            return null;
        }

        protected override object VisitClassDeclaration(ClassDeclaration n)
        {
            ClassSymbol csymb = new ClassSymbol() { Name = n.Name, Type = SymbolType.Class, Declaration = n };
            csymb.ClassSymbols = new ScopedSymbolTable(n.Name, CurrentScope.ScopeLevel + 1, CurrentScope);
            CurrentScope.AddSymbol(csymb);
            CurrentScope = csymb.ClassSymbols;
            
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

        protected override object VisitWhileStatement(WhileStatement n)
        {
            Visit(n.BooleanExpression);
            Visit(n.Block);
            return null;
        }

        protected override object VisitClassForwardDeclaration(ClassForwardDeclaration n)
        {
            ClassSymbol cSym = new ClassSymbol() { Name = n.Name, Type = SymbolType.Class, Declaration = null };
            CurrentScope.AddSymbol(cSym);
            return null;
        }

        protected override object VisitBooleanExpressionCombination(BooleanExpressionCombination n)
        {
            Visit(n.Left);
            Visit(n.Right);
            return null;
        }

        protected override object VisitBuiltIn(AST.BuiltIn n)
        {
            return null;
        }
    }
}
