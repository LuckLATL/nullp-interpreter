using NullPInterpreter.Interpreter.AST;
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
            throw new NotImplementedException();
        }

        protected override object VisitBinaryOperator(BinaryOperator n)
        {
            throw new NotImplementedException();
        }

        protected override object VisitBlock(Block n)
        {
            throw new NotImplementedException();
        }

        protected override object VisitBooleanExpression(BooleanExpression n)
        {
            throw new NotImplementedException();
        }

        protected override object VisitVariableDeclaration(VariableDeclaration n)
        {
            throw new NotImplementedException();
        }

        protected override object VisitFunctionCall(FunctionCall n)
        {
            throw new NotImplementedException();
        }

        protected override object VisitFunctionDeclaration(FunctionDeclaration n)
        {
            throw new NotImplementedException();
        }

        protected override object VisitIfStatement(IfStatement n)
        {
            throw new NotImplementedException();
        }

        protected override object VisitIntegerLiteral(IntegerLiteral n)
        {
            return n.Value;
        }

        protected override object VisitNamespaceDeclaration(NamespaceDeclaration n)
        {
            throw new NotImplementedException();
        }

        protected override object VisitNamespacePropertyCall(NamespacePropertyCall n)
        {
            throw new NotImplementedException();
        }

        protected override object VisitNoOperator(NoOperator n)
        {
            throw new NotImplementedException();
        }

        protected override object VisitProgramElement(ProgramElement n)
        {
            throw new NotImplementedException();
        }

        protected override object VisitStringLiteral(StringLiteral n)
        {
            return n.Value;
        }

        protected override object VisitUnaryOperator(UnaryOperator n)
        {
            throw new NotImplementedException();
        }

        protected override object VisitVariable(Variable n)
        {
            throw new NotImplementedException();
        }
    }
}
