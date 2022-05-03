using NullPInterpreter.Interpreter.AST;
using NullPInterpreter.Interpreter.CallStackManagement;
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

        CallStack CallStack = new CallStack();

        public Interpreter(Parser parser)
        {
            Parser = parser;
        }

        public ProgramElement rootNode;

        public void Prepare()
        {
            rootNode = (ProgramElement)Parser.Parse();
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
                    return leftNode.ToString() + rightNode.ToString();
                }
                throw new InvalidOperationException();
            }
            else
            {
                if (!(leftNode is double && rightNode is double))
                    throw new Exception("bruh, you stupid because only double can be added");

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
                            throw new Exception("Stupid by 0 division");
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

            switch (n.Operator)
            {
                case TokenType.Equals:
                    return leftResult.Equals(rightResult);
                case TokenType.NotEquals:
                    return !leftResult.Equals(rightResult);
            }
            throw new Exception("cannot compare");
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
                ActivationRecord ar = new ActivationRecord(n.FunctionName, ActivationRecordType.Function, 2);
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
            if ((bool)Visit(n.BooleanExpression))
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
            throw new NotImplementedException("This feature has not been implemented yet");
        }

        protected override object VisitNoOperator(NoOperator n)
        {
            return null;
        }

        protected override object VisitProgramElement(ProgramElement n)
        {
            CallStack.ExtendedPush(new ActivationRecord("Program", ActivationRecordType.Program, 1));
            n.Children.ForEach(child => Visit(child));
            CallStack.Pop();
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
    }
}
