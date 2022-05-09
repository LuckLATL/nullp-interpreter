using NullPInterpreter.Interpreter.AST;
using NullPInterpreter.Interpreter.CallStackManagement;
using NullPInterpreter.Interpreter.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter
{
    public class Interpreter : NodeVisitor
    {
        public Parser Parser { get; set; }

        public SematicAnalyser SematicAnalyser { get; set; } = new SematicAnalyser();

        public CallStack CallStack = new CallStack();
        public ActivationRecord LastProgramActivationRecord = null;

        public Interpreter(Parser parser)
        {
            Parser = parser;
        }

        public ProgramElement rootNode;

        public void SematicAnalysis(ASTNode _rootNode)
        {
            rootNode = (ProgramElement)_rootNode;
            SematicAnalyser.Visit(rootNode);
        }

        public object Interpret()
        {
            return Visit(rootNode);
        }

        protected override object VisitAssignmentOperator(AssignmentOperator n)
        {
            CallStack.Peek().SetMember(((Variable)n.LeftNode).Name, Visit(n.RightNode));
            return null;
        }

        protected override object VisitBinaryOperator(BinaryOperator n)
        {
            object leftNode = Visit(n.LeftNode);
            object rightNode = Visit(n.RightNode);

            if (leftNode is string || rightNode is string)
            {
                if (n.Operator.Type == TokenType.Plus)
                {
                    if (leftNode == null)
                        leftNode = "";
                    if (rightNode == null)
                        rightNode = "";
                    return leftNode.ToString() + rightNode.ToString();
                }
                throw new InvalidOperationException();
            }
            else
            {
                if (!(leftNode is double && rightNode is double))
                    throw new Exception("Invalid operation. Tried calculate number with non-number.");

                double leftDouble = (double)leftNode;
                double rightDouble = (double)rightNode;

                switch (n.Operator.Type)
                {
                    case TokenType.Plus:
                        return leftDouble + rightDouble;
                    case TokenType.Minus:
                        return leftDouble - rightDouble;
                    case TokenType.Multiply:
                        return leftDouble * rightDouble;
                    case TokenType.Divide:
                        if (rightDouble == 0)
                            throw new Exception("Division by 0. Wormhole successfully created. Please stand by while we terminate your code.");
                        return leftDouble / rightDouble;
                }
            }
            return null;
        }

        protected override object VisitBlock(Block n)
        {
            foreach (var child in n.Children)
            {
                if (CallStack.Peek().ReturnValue != null)
                    break;

                if (child is ReturnStatement)
                {
                    CallStack.Peek().ReturnValue = Visit(child);
                    return null;
                }
                Visit(child);
            }
            return null;
        }

        protected override object VisitBooleanExpression(BooleanExpression n)
        {
            object leftResult = Visit(n.Left);
            object rightResult = Visit(n.Right);

            if ((leftResult == null && rightResult != null) || (leftResult != null && rightResult == null))
                return false;

            if (leftResult == null && rightResult == null)
                return true;

            switch (n.Operator)
            {
                case TokenType.Equals:
                    return leftResult.Equals(rightResult);
                case TokenType.NotEquals:
                    return !leftResult.Equals(rightResult);
            }
            throw new Exception($"Invalid operator ('{TokenTypeExtension.TokenTypeToReadableString(n.Operator)}') for boolean expression found.");
        }

        protected override object VisitVariableDeclaration(VariableDeclaration n)
        {
            if (n.InitialDefinition != null)
            {
                CallStack.Peek().SetMember(n.Variable.Name, Visit(n.InitialDefinition));
            }
            return null;
        }

        protected override object VisitFunctionCall(FunctionCall n)
        {
            object returnValue;

            if (BuiltInFunctions.CheckIfBuiltInFunction(n.FunctionName))
            {
                List<object> args = new List<object>();

                foreach (var argument in n.Arguments)
                {
                    args.Add(Visit(argument));
                }

                returnValue = BuiltInFunctions.ExecuteBuiltInFunction(n.FunctionName, args);
            }
            else
            {
                ActivationRecord ar = new ActivationRecord(n.FunctionName, ActivationRecordType.Function, CallStack.Count == 0 ? 1 : CallStack.Peek().NestingLevel + 1);
                CallStack.ExtendedPush(ar);

                for (int i = 0; i < n.Arguments.Count; i++)
                {
                    ar.SetMember(n.FunctionSymbol.Declaration.Arguments[i].Name, Visit(n.Arguments[i]));
                }

                Visit(n.FunctionSymbol.Declaration.Block);
                returnValue = ar.ReturnValue;
                CallStack.Pop();
            }

            return returnValue;
        }

        protected override object VisitFunctionDeclaration(FunctionDeclaration n)
        {
            return null;
        }

        protected override object VisitIfStatement(IfStatement n)
        {
            bool result = (bool)Visit(n.BooleanExpression);
            if (result)
            {
                Visit(n.Block);
            }
            else
            {
                Visit(n.ElseBlock);
            }
            return null;
        }

        protected override object VisitIntegerLiteral(IntegerLiteral n)
        {
            return n.Value;
        }

        protected override object VisitNamespaceDeclaration(NamespaceDeclaration n)
        {
            Visit(n.Block);
            return null;
        }

        protected override object VisitNamespacePropertyCall(NamespacePropertyCall n)
        {
            if (n.CalledNode is NamespacePropertyCall)
                return Visit(n.CalledNode);
            else if (n.CalledNode is FunctionCall)
            {
                object calledObj = CallStack.Peek().GetMember(n.CallerName);

                if (calledObj is ClassSymbol)
                {
                    ClassSymbol calledClass = (ClassSymbol)calledObj;

                    FunctionSymbol func = ((FunctionSymbol)(calledClass.ClassSymbols.LookUpSymbol(((FunctionCall)n.CalledNode).FunctionName)));

                    ActivationRecord ar = new ActivationRecord(func.Name, ActivationRecordType.Function, CallStack.Count == 0 ? 1 : CallStack.Peek().NestingLevel + 1);
                    CallStack.ExtendedPush(calledClass.ClassActivationRecord);
                    CallStack.ExtendedPush(ar);

                    for (int i = 0; i < func.Declaration.Arguments.Count; i++)
                    {
                        ar.SetMember(func.Declaration.Arguments[i].Name, Visit((n.CalledNode as FunctionCall).Arguments[i]));
                    }

                    Visit(func.Declaration.Block);

                    CallStack.Pop();
                    CallStack.Pop();

                    return ar.ReturnValue;
                }

            }

            return null;
            throw new NotImplementedException("This feature has not been implemented yet");
        }

        protected override object VisitNoOperator(NoOperator n)
        {
            return null;
        }

        protected override object VisitProgramElement(ProgramElement n)
        {
            CallStack.ExtendedPush(new ActivationRecord("Program", ActivationRecordType.Program, CallStack.Count == 0 ? 1 : CallStack.Peek().NestingLevel + 1));
            n.Children.ForEach(child => Visit(child));
            LastProgramActivationRecord = CallStack.Pop();
            return null;
        }

        protected override object VisitStringLiteral(StringLiteral n)
        {
            return n.Value;
        }

        protected override object VisitUnaryOperator(UnaryOperator n)
        {
            return null;
        }

        protected override object VisitVariable(Variable n)
        {
            return CallStack.Peek().GetMember(n.Name);
        }

        protected override object VisitReturnStatement(ReturnStatement n)
        {
            return Visit(n.ReturnNode);
        }

        protected override object VisitFunctionForwardDeclaration(FunctionForwardDeclaration n)
        {
            return null;
        }

        protected override object VisitTrueLiteral(TrueLiteral n)
        {
            return true;
        }

        protected override object VisitFalseLiteral(FalseLiteral n)
        {
            return false;
        }

        protected override object VisitNullLiteral(NullLiteral n)
        {
            return null;
        }

        protected override object VisitClassInstanceCreation(ClassInstanceCreation n)
        {
            ActivationRecord ar = new ActivationRecord(n.ClassSymbol.Name, ActivationRecordType.Class, CallStack.Count == 0 ? 1 : CallStack.Peek().NestingLevel + 1);
            CallStack.ExtendedPush(ar);
            n.ClassSymbol.ClassActivationRecord = ar;
            Visit(n.ClassSymbol.Declaration.Block);

            FunctionSymbol constructor = n.ClassSymbol.ClassSymbols.LookUpSymbol(n.ClassSymbol.Name) as FunctionSymbol;
            if (constructor != null)
                Visit(constructor.Declaration.Block);

            CallStack.Pop();

            return n.ClassSymbol;
        }

        protected override object VisitClassDeclaration(ClassDeclaration n)
        {
            return null;
        }
    }
}
