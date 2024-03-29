﻿using NullPInterpreter.Interpreter.AST;
using NullPInterpreter.Interpreter.BuiltIn;
using NullPInterpreter.Interpreter.CallStackManagement;
using NullPInterpreter.Interpreter.Symbols;
using NullPInterpreter.Runtime;
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
            object rightNode = Visit(n.RightNode);

            if (n.LeftNode is Variable v)
                CallStack.Peek().SetMember(v.Name, rightNode);
            else if (n.LeftNode is Indexer i)
            {
                object variableValue = CallStack.Peek().GetMember(i.Variable.Name);
                if (variableValue is List<object> l)
                {
                    if (i.Start < 0 || i.End < 0)
                        throw new IndexOutOfRangeException($"Index '{i.Start}-{i.End}' was out of range for list for index '{l.Count}'");

                    if (i.Start >= l.Count || i.End >= l.Count)
                    {
                        int max = i.Start < i.End ? i.End : i.Start;

                        while (l.Count <= max)
                        {
                            l.Add(null);
                        }
                    }


                    for (int ii = i.Start; ii <= i.End; ii++)
                    {
                        l[ii] = rightNode;
                    }
                }
                else if (variableValue is string s)
                {
                    if (i.Start < 0 || i.Start >= s.Length || i.End < 0 || i.End >= s.Length)
                        throw new IndexOutOfRangeException($"Index '{i.Start}-{i.End}' was out of range for string for index '{s.Length}'");

                    string first = s.Substring(0, i.Start);
                    string second = s.Substring(i.End + 1, s.Length-i.End-1);
                    CallStack.Peek().SetMember(i.Variable.Name, first + rightNode.ToString() + second);
                }
            }
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
                if (CallStack.Peek().ShouldReturn)
                    break;

                if (child is ReturnStatement)
                {
                    CallStack.Peek().ReturnValue = Visit(child);
                    CallStack.Peek().ShouldReturn = true;
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

            if (leftResult is bool && rightResult == null)
                return leftResult;

            switch (n.Operator)
            {
                case TokenType.Equals:

                    if ((leftResult is NullObject && rightResult is not NullObject) || (leftResult is not NullObject && rightResult is NullObject))
                        return false;

                    if (leftResult is NullObject && rightResult is NullObject)
                        return true;

                    return leftResult.Equals(rightResult);
                case TokenType.NotEquals:

                    if ((leftResult is NullObject && rightResult is not NullObject) || (leftResult is not NullObject && rightResult is NullObject))
                        return true;

                    if (leftResult is NullObject && rightResult is NullObject)
                        return false;

                    return !leftResult.Equals(rightResult);
            }

            if (!(leftResult is double && rightResult is double))
                throw new Exception("Both values must be a number value");

            double leftDouble = Convert.ToDouble(leftResult);
            double rightDouble = Convert.ToDouble(rightResult);

            switch (n.Operator)
            {
                case TokenType.GreaterEquals:
                    return leftDouble >= rightDouble;
                case TokenType.SmallerEquals:
                    return leftDouble <= rightDouble;
                case TokenType.Greater:
                    return leftDouble > rightDouble;
                case TokenType.Smaller:
                    return leftDouble < rightDouble;
            }

            throw new Exception($"Invalid operator ('{TokenTypeExtension.TokenTypeToReadableString(n.Operator)}') for boolean expression found.");
        }

        protected override object VisitVariableDeclaration(VariableDeclaration n)
        {
            object initialValue = new NullObject();
            if (n.InitialDefinition != null)
            {
                initialValue = Visit(n.InitialDefinition);
            }
            CallStack.Peek().SetMember(n.Variable.Name, initialValue);
            return null;
        }

        protected override object VisitFunctionCall(FunctionCall n)
        {
            object returnValue;
            
            ActivationRecord ar = new ActivationRecord(n.FunctionName, ActivationRecordType.Function, CallStack.Count == 0 ? 1 : CallStack.Peek().NestingLevel + 1);
            CallStack.ExtendedPush(ar);

            for (int i = 0; i < n.Arguments.Count; i++)
            {
                ar.SetMember(n.FunctionSymbol.Declaration.Arguments[i].Name, Visit(n.Arguments[i]));
            }

            Visit(n.FunctionSymbol.Declaration.Block);
            returnValue = ar.ReturnValue;
            CallStack.Pop();
            

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
            ActivationRecord ar = new ActivationRecord(n.Name, ActivationRecordType.Namespace, CallStack.Count == 0 ? 1 : CallStack.Peek().NestingLevel + 1);
            n.SourceSymbol.NamespaceActivationRecord = ar;
            CallStack.Peek().SetMember(n.Name, n.SourceSymbol);
            CallStack.ExtendedPush(ar);
            Visit(n.Block);
            CallStack.Pop();
            return null;
        }

        protected override object VisitNamespacePropertyCall(NamespacePropertyCall n)
        {
            ASTNode temp = n.CalledNode;
            object returnValue = null;
            int scopeCounter = 0;

            object callerSymbol = n.SourceSymbol;
            if (callerSymbol == null)
                callerSymbol = CallStack.Peek().GetMember(n.CallerName);

            ScopedSymbolTable latestSymbol = null;

            switch (callerSymbol)
            {
                case NamespaceSymbol ns:

                    ActivationRecord nsAr = ns.NamespaceActivationRecord;
                    if (ns is BuiltInNamespaceSymbol bnss)
                        nsAr = new ActivationRecord(bnss.Name, ActivationRecordType.Namespace, CallStack.Peek().NestingLevel + 1) { Members = nsAr.Members};

                    CallStack.ExtendedPush(nsAr);
                    latestSymbol = ns.NamespaceSymbols;
                    scopeCounter++;
                    break;
                case ClassSymbol cs:
                    CallStack.ExtendedPush(cs.ClassActivationRecord);
                    latestSymbol = cs.ClassSymbols;
                    scopeCounter++;
                    break;
                case VariableSymbol vs:

                    object variableDef = CallStack.Peek().GetMember(vs.Name);

                    switch (variableDef)
                    {
                        case NamespaceSymbol ns:
                            latestSymbol = ns.NamespaceSymbols;

                            ActivationRecord snsAr = ns.NamespaceActivationRecord;
                            if (ns is BuiltInNamespaceSymbol sbnss)
                                snsAr = new ActivationRecord(sbnss.Name, ActivationRecordType.Namespace, CallStack.Peek().NestingLevel + 1) { Members = snsAr.Members };

                            CallStack.ExtendedPush(snsAr);
                            scopeCounter++;
                            break;
                        case ClassSymbol cs:
                            latestSymbol = cs.ClassSymbols;
                            CallStack.ExtendedPush(cs.ClassActivationRecord);
                            scopeCounter++;
                            break;
                    }
                    
                    break;
            }

            while (temp is NamespacePropertyCall nspc)
            {
                object calledObj = CallStack.Peek().GetMember(nspc.CallerName);

                if (calledObj is NamespaceSymbol nsym)
                {
                    ActivationRecord snsAr = nsym.NamespaceActivationRecord;
                    if (nsym is BuiltInNamespaceSymbol sbnss)
                        snsAr = new ActivationRecord(sbnss.Name, ActivationRecordType.Namespace, CallStack.Peek().NestingLevel + 1) { Members = snsAr.Members };

                    CallStack.ExtendedPush(snsAr);
                    latestSymbol = nsym.NamespaceSymbols;
                    scopeCounter++;
                }
                else if (calledObj is ClassSymbol classSym)
                {
                    CallStack.ExtendedPush(classSym.ClassActivationRecord);
                    latestSymbol = classSym.ClassSymbols;
                    scopeCounter++;
                }

                temp = nspc.CalledNode;
            }

            if (temp is FunctionCall fcall)
            {
                ActivationRecord ar = new ActivationRecord(fcall.FunctionName, ActivationRecordType.Function, CallStack.Count == 0 ? 1 : CallStack.Peek().NestingLevel + 1);
                FunctionSymbol funcSym = fcall.FunctionSymbol;

                if (funcSym == null && latestSymbol != null)
                    funcSym = (FunctionSymbol)latestSymbol.LookUpSymbol(fcall.FunctionName);

                for (int i = 0; i < funcSym.Declaration.Arguments.Count; i++)
                {
                    ar.SetMember(funcSym.Declaration.Arguments[i].Name, Visit(fcall.Arguments[i]));
                }

                if (funcSym.Declaration.Block.Children.Count != 0 && funcSym.Declaration.Block.Children[0] is AST.BuiltIn)
                {
                    (funcSym.Declaration.Block.Children[0] as AST.BuiltIn).Arguments.Clear();

                    for (int i = 0; i < fcall.Arguments.Count; i++)
                        (funcSym.Declaration.Block.Children[0] as AST.BuiltIn).Arguments.Add(Visit(fcall.Arguments[i]));
                }

                CallStack.ExtendedPush(ar);
                Visit(funcSym.Declaration.Block);
                returnValue = CallStack.Peek().ReturnValue;
                CallStack.Pop();
            }
            else if (temp is ClassInstanceCreation cic)
            {
                returnValue = Visit(cic);
            }
            else if (temp is Variable v)
            {
                returnValue = CallStack.Peek().GetMember(v.Name);
            }


            for (int i = 0; i < scopeCounter; i++)
            {
                CallStack.Pop();
            }

            return returnValue;

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
            return new NullObject();
        }

        protected override object VisitClassInstanceCreation(ClassInstanceCreation n)
        {
            ActivationRecord ar = new ActivationRecord(n.ClassSymbol.Name, ActivationRecordType.Class, CallStack.Count == 0 ? 1 : CallStack.Peek().NestingLevel + 1);

            ClassSymbol newInstance = new ClassSymbol() { Name = n.ClassSymbol.Name, Type = SymbolType.Class, ClassActivationRecord = ar, ClassSymbols = n.ClassSymbol.ClassSymbols, Declaration = n.ClassSymbol.Declaration };
            
            if (n.ClassSymbol is BuiltInClassSymbol builtInInstance)
            {
                newInstance = builtInInstance.New();
                newInstance.ClassActivationRecord = ar;
            }

            CallStack.ExtendedPush(ar);
            Visit(newInstance.Declaration.Block);

            FunctionSymbol constructor = newInstance.ClassSymbols.LookUpSymbol(newInstance.Name, true) as FunctionSymbol;
            if (constructor != null)
            {
                ActivationRecord constructorAr = new ActivationRecord(constructor.Name, ActivationRecordType.Function, CallStack.Count == 0 ? 1 : CallStack.Peek().NestingLevel + 1);

                ASTNode subNodes = n.ClassToCreate;

                while (subNodes is NamespacePropertyCall npc)
                {
                    subNodes = npc.CalledNode;
                }

                FunctionCall fcall = (FunctionCall)subNodes;

                for (int i = 0; i < constructor.Declaration.Arguments.Count; i++)
                {
                    constructorAr.SetMember(constructor.Declaration.Arguments[i].Name, Visit(fcall.Arguments[i]));
                }
                CallStack.ExtendedPush(constructorAr);

                Visit(constructor.Declaration.Block);

                CallStack.Pop();
            }

            CallStack.Pop();

            return newInstance;
        }

        protected override object VisitClassDeclaration(ClassDeclaration n)
        {
            return null;
        }

        protected override object VisitList(List n)
        {
            List<object> list = new List<object>();
            foreach (var item in n.Items)
            {
                list.Add(Visit(item));
            }

            return list;
        }

        protected override object VisitIndexer(Indexer n)
        {
            object evaluatedObj = Visit(n.Variable);

            if (evaluatedObj is string s)
            {
                if (n.Start < 0 || n.Start > s.Length-1 || n.End < 0 || n.End > s.Length-1)
                    throw new IndexOutOfRangeException($"Index [{n.Start}-{n.End}] is not in bounds [0-{s.Length-1}]");

                if (n.Start == n.End)
                    return s[n.Start];
                else
                {
                    return s.Substring(n.Start, n.End - n.Start);
                }
            }
            else if (evaluatedObj is List<object> l)
            {
                if (n.Start < 0 || n.Start > l.Count-1 || n.End < 0 || n.End > l.Count-1)
                    throw new IndexOutOfRangeException($"Index [{n.Start}-{n.End}] is not in bounds [0-{l.Count-1}]");

                if (n.Start == n.End)
                    return l[n.Start];
                else
                {
                    List<object> tmpList = new List<object>();
                    for (int i = n.Start; i <= n.End; i++)
                    {
                        tmpList.Add(l[i]);
                    }
                    return tmpList;
                }
            }

            throw new Exception($"'{n.Variable.Name}' is not indexable.");
        }

        protected override object VisitWhileStatement(WhileStatement n)
        {
            while ((bool)Visit(n.BooleanExpression))
            {
                Visit(n.Block);
                if (CallStack.Peek().ShouldReturn)
                    break;
            }

            return null;
        }

        protected override object VisitClassForwardDeclaration(ClassForwardDeclaration n)
        {
            return null;
        }

        protected override object VisitBooleanExpressionCombination(BooleanExpressionCombination n)
        {
            bool left = (bool)Visit(n.Left);
            bool right = (bool)Visit(n.Right);

            switch (n.Operator)
            {
                case TokenType.Pipe:
                    return  left || right;
                case TokenType.Ampersand:
                    return left && right;
            }
            return null;
        }

        protected override object VisitBuiltIn(AST.BuiltIn n)
        {
            object ret = n.Function.Invoke(n.Arguments);
            n.Arguments.Clear();
            CallStack.Peek().ReturnValue = ret;
            CallStack.Peek().ShouldReturn = true;
            return ret;
        }

        protected override object VisitForStatement(ForStatement n)
        {
            CallStack.ExtendedPush(new ActivationRecord("for", ActivationRecordType.Block, CallStack.Peek().NestingLevel + 1) { });

            Visit(n.VariableDeclaration);
            while ((bool)Visit(n.BooleanExpression))
            {
                Visit(n.Block);
                Visit(n.Statement);
            }

            CallStack.Pop();

            return null;
        }
    }
}
