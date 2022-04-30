using NullPInterpreter.Interpreter;
using NullPInterpreter.Interpreter.AST;
using NullPInterpreter.Interpreter.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter
{
    public class Parser
    {
        /// <summary>
        /// The Lexer used by the interpreter
        /// </summary>
        private Lexer lexer = null;
        /// <summary>
        /// The currently active token
        /// </summary>
        private Token currentToken = null;

        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
            // Get the next token to process
            currentToken = lexer.GetNextToken();
        }

        /// <summary>
        /// Consumes (eats) the current token if it matches the expected token type.
        /// Assigns the next token as the current token if the statement above is true.
        /// </summary>
        /// <param name="expectedTokenType">The type the current token should have for the token to be consumed</param>
        private void ConsumeCurrentToken(TokenType expectedTokenType)
        {
            if (currentToken.Type == expectedTokenType)
                currentToken = lexer.GetNextToken();
            else
                // Throw an exception as the input didn't match the output
                throw new SyntaxError(lexer.Line, lexer.LinePosition, $"Read token '{currentToken.Value}' did not match expected token '{TokenTypeExtension.TokenTypeToReadableString(expectedTokenType)}'.");
        }

        private List<ASTNode> StatementList()
        {
            List<ASTNode> results = new List<ASTNode>() { };

            while (currentToken.Type != TokenType.BlockClose && currentToken.Type != TokenType.EoF)
            {
                ASTNode nodeToAdd = Statement();
                results.Add(nodeToAdd);
            }

            return results;
        }

        /// <summary>
        /// Processes the next expression from the current token.
        /// </summary>
        /// <returns></returns>
        public ASTNode Expression()
        {
            // Get the first term
            ASTNode result = Term();

            // Every following plus or minus can 
            while (currentToken.Type == TokenType.Plus || currentToken.Type == TokenType.Minus)
            {
                Token token = currentToken;
                ConsumeCurrentToken(currentToken.Type);
                result = new BinaryOperator(result, token, Term());
            }

            return result;
        }

        /// <summary>
        /// Processes the topmost priority expressions after the factor is known.
        /// </summary>
        /// <returns>The result of the term itself</returns>
        private ASTNode Term()
        {
            ASTNode result = Factor();
            while (currentToken.Type == TokenType.Divide || currentToken.Type == TokenType.Multiply)
            {
                Token token = currentToken;
                ConsumeCurrentToken(token.Type);
                result = new BinaryOperator(result, token, Factor());
            }

            return result;
        }

        /// <summary>
        /// Processes the lowest possible expressions like integer values and brackets.
        /// </summary>
        /// <returns>Value of the expression given</returns>
        private ASTNode Factor()
        {
            Token token = currentToken;

            if (token.Type == TokenType.Plus)
            {
                ConsumeCurrentToken(TokenType.Plus);
                ASTNode node = new UnaryOperator(token, Factor());
                return node;
            }
            else if (token.Type == TokenType.Minus)
            {
                ConsumeCurrentToken(TokenType.Minus);
                ASTNode node = new UnaryOperator(token, Factor());
                return node;
            }
            else if (token.Type == TokenType.IntegerLiteral)
            {
                ConsumeCurrentToken(TokenType.IntegerLiteral);
                return new IntegerLiteral(Double.Parse(token.Value.ToString()));
            }
            else if (token.Type == TokenType.LeftParenthesis)
            {
                ConsumeCurrentToken(TokenType.LeftParenthesis);
                ASTNode result = Expression();
                ConsumeCurrentToken(TokenType.RightParenthesis);
                return result;
            }
            else if (token.Type == TokenType.Word)
            {
                ASTNode result = Variable();
                return result;
            }
            else if (token.Type == TokenType.StringLiteral)
            {
                ASTNode result = StringLiteral();
                return result;
            }
            else if (token.Type == TokenType.FunctionCall)
            {
                ASTNode result = FunctionCall();
                return result;
            }

            throw new SyntaxError(lexer.Line, lexer.LinePosition, "Expression does not validate to known possible operations.");
        }

        private ASTNode Variable()
        {
            ASTNode node = new Variable(currentToken, currentToken.Value.ToString());
            ConsumeCurrentToken(TokenType.Word);
            return node;
        }

        private ASTNode Argument()
        {
            ASTNode node = new Argument(currentToken, currentToken.Value.ToString());
            ConsumeCurrentToken(TokenType.Word);
            return node;
        }

        public ASTNode StringLiteral()
        {
            ASTNode node = new StringLiteral(currentToken.Value.ToString());
            ConsumeCurrentToken(TokenType.StringLiteral);
            return node;
        }


        private ASTNode Statement()
        {
            ASTNode node;
            if (currentToken.Type == TokenType.BlockOpen)
            {
                node = Block();
            }
            else if (currentToken.Type == TokenType.Word)
            {
                node = AssigmentStatement();
                ConsumeCurrentToken(TokenType.Semicolon);
            }
            else if (currentToken.Type == TokenType.KeywordVariable)
            {
                node = VariableDeclaration();
                ConsumeCurrentToken(TokenType.Semicolon);
            }
            else if (currentToken.Type == TokenType.FunctionCall)
            {
                node = FunctionCall();
                ConsumeCurrentToken(TokenType.Semicolon);
            }
            else if (currentToken.Type == TokenType.KeywordNamespace)
            {
                node = NamespaceDeclaration();
            }
            else if (currentToken.Type == TokenType.KeywordFunction)
            {
                node = FunctionDeclaration();
            }
            else if (currentToken.Type == TokenType.NamespacePropertyCall)
            {
                node = NamespacePropertyCall();
                ConsumeCurrentToken(TokenType.Semicolon);
            }
            else if (currentToken.Type == TokenType.KeywordIfStatement)
            {
                node = IfStatement();
            }
            else
                node = new NoOperator();

            return node;
        }

        private ASTNode FunctionDeclaration()
        {
            ConsumeCurrentToken(TokenType.KeywordFunction);
            FunctionDeclaration node = new FunctionDeclaration();
            node.FunctionName = currentToken.Value.ToString();
            ConsumeCurrentToken(TokenType.FunctionCall);
            ConsumeCurrentToken(TokenType.LeftParenthesis);

            while (currentToken.Type != TokenType.RightParenthesis)
            {
                node.Arguments.Add((Argument)Argument());

                if (currentToken.Type != TokenType.RightParenthesis)
                    ConsumeCurrentToken(TokenType.Comma);
            }
            ConsumeCurrentToken(TokenType.RightParenthesis);
            node.Block = (Block)Block();

            return node;
        }

        private ASTNode IfStatement()
        {
            IfStatement node = new IfStatement();
            ConsumeCurrentToken(TokenType.KeywordIfStatement);
            ConsumeCurrentToken(TokenType.LeftParenthesis);
            node.BooleanExpression = BooleanExpression();
            ConsumeCurrentToken(TokenType.RightParenthesis);
            node.Block = (Block)Block();

            if (currentToken.Type == TokenType.KeywordElseStatement)
            {
                ConsumeCurrentToken(TokenType.KeywordElseStatement);
                node.ElseBlock = (Block)Block();
            }

            return node;
        }

        private ASTNode BooleanExpression()
        {
            BooleanExpression node = new BooleanExpression();
            node.Left = Expression();
            node.Operator = currentToken.Type;
            if (currentToken.Type == TokenType.Equals)
                ConsumeCurrentToken(TokenType.Equals);
            else if (currentToken.Type == TokenType.NotEquals)
                ConsumeCurrentToken(TokenType.NotEquals);
            node.Right = Expression();
            return node;
        }

        private ASTNode Block()
        {
            ConsumeCurrentToken(TokenType.BlockOpen);
            List<ASTNode> nodes = StatementList();
            ConsumeCurrentToken(TokenType.BlockClose);

            Block root = new Block();
            foreach (ASTNode subNode in nodes)
            {
                root.Children.Add(subNode);
            }
            return root;
        }

        private ASTNode AssigmentStatement()
        {
            ASTNode left = Variable();
            Token token = currentToken;
            ConsumeCurrentToken(TokenType.Assign);
            ASTNode right;
            right = Expression();

            ASTNode node = new AssignmentOperator(left, token, right);
            return node;
        }

        private VariableDeclaration VariableDeclaration()
        {
            ConsumeCurrentToken(TokenType.KeywordVariable);
            Variable variable = new Variable(currentToken, currentToken.Value.ToString());
            ConsumeCurrentToken(TokenType.Word);
            VariableDeclaration declaration = new VariableDeclaration(variable);

            if (currentToken.Type == TokenType.Assign)
            {
                ConsumeCurrentToken(TokenType.Assign);
                declaration.InitialDefinition = Expression();
            }
            
            return declaration;
        }

        private ASTNode FunctionCall()
        {
            FunctionCall call = new FunctionCall(currentToken.Value.ToString());
            ConsumeCurrentToken(TokenType.FunctionCall);
            ConsumeCurrentToken(TokenType.LeftParenthesis);

            while (currentToken.Type != TokenType.RightParenthesis)
            {
                call.Arguments.Add(Expression());

                if (currentToken.Type != TokenType.RightParenthesis)
                    ConsumeCurrentToken(TokenType.Comma);
            }
            ConsumeCurrentToken(TokenType.RightParenthesis);
            return call;
        }
        private ASTNode NamespacePropertyCall()
        {
            NamespacePropertyCall call = new NamespacePropertyCall() { CallerName = currentToken.Value.ToString() };
            ConsumeCurrentToken(TokenType.NamespacePropertyCall);
            ConsumeCurrentToken(TokenType.Dot);
            
            if (currentToken.Type == TokenType.NamespacePropertyCall)
            {
                call.CalledNode = NamespacePropertyCall();
            }
            else if (currentToken.Type == TokenType.FunctionCall)
            {
                call.CalledNode = FunctionCall();
            }
            else if (currentToken.Type == TokenType.Word)
            {
                call.CalledNode = Variable();
            }
            return call;
        }

        private ASTNode NamespaceDeclaration()
        {
            ConsumeCurrentToken(TokenType.KeywordNamespace);
            string namespaceName = currentToken.Value.ToString();
            ConsumeCurrentToken(TokenType.Word);
            NamespaceDeclaration namespaceNode = new NamespaceDeclaration((Block)Block(), namespaceName);
            return namespaceNode;
        }


        public ASTNode Parse()
        {
            List<ASTNode> nodes = StatementList();

            if (currentToken.Type != TokenType.EoF)
            {
                throw new SyntaxError(lexer.Line, lexer.LinePosition, "Programm didn't terminate correctly.");
            }

            ASTNode root = new ProgramElement() { Children = nodes };
            return root;
        }
    }
}
