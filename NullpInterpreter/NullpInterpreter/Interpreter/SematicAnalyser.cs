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
            currentScope.AddSymbol(new Symbol() { Name = n.Variable.Name, Type = SymbolType.Variable });
            return null;
        }

        protected override object VisitFunctionCall(FunctionCall n)
        {
            currentScope.LookUpSymbol(n.FunctionName);
            return null;
        }

        protected override object VisitFunctionDeclaration(FunctionDeclaration n)
        {
            currentScope.AddSymbol(new Symbol() { Name = n.FunctionName, Type = SymbolType.Function });
            Console.WriteLine($"Enter Scope: {n.FunctionName}");
            currentScope = new ScopedSymbolTable(n.FunctionName, currentScope.ScopeLevel + 1, currentScope);

            n.Arguments.ForEach(argument => currentScope.AddSymbol(new Symbol { Name = argument.Name, Type = SymbolType.Argument }));
            Visit(n.Block);

            Console.WriteLine(currentScope);
            Console.WriteLine($"Leave scope: {n.FunctionName}");
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
            Console.WriteLine($"Enter Scope: {n.Name}");
            currentScope = new ScopedSymbolTable(n.Name, currentScope.ScopeLevel + 1, currentScope);

            Visit(n.Block);

            Console.WriteLine(currentScope);
            Console.WriteLine($"Leave scope: {n.Name}");
            currentScope = currentScope.EnclosingScope;

            return null;
        }

        protected override object VisitNamespacePropertyCall(NamespacePropertyCall n)
        {
            currentScope.LookUpSymbol(n.CallerName);
            return null;
        }

        protected override object VisitNoOperator(NoOperator n)
        {
            return null;
        }

        protected override object VisitProgramElement(ProgramElement n)
        {
            Console.WriteLine("Enter Scope: Program");
            currentScope = new ScopedSymbolTable("Program", 1);
            currentScope.InitializeBuiltIns();

            n.Children.ForEach(child => Visit(child));

            Console.WriteLine(currentScope);
            Console.WriteLine("Leave Scope: Program");
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
