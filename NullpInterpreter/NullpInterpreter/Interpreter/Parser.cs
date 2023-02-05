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

        private Token previousToken = null;

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
            {
                previousToken = currentToken;
                currentToken = lexer.GetNextToken();
            }
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
                result = new BinaryOperator(result, token, Term()) { Line = lexer.Line, Character = lexer.LinePosition };
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
                result = new BinaryOperator(result, token, Factor()) { Line = lexer.Line, Character = lexer.LinePosition };
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
                ASTNode node = new UnaryOperator(token, Factor()) { Line = lexer.Line, Character = lexer.LinePosition };
                return node;
            }
            else if (token.Type == TokenType.Minus)
            {
                ConsumeCurrentToken(TokenType.Minus);
                ASTNode node = new UnaryOperator(token, Factor()) { Line = lexer.Line, Character = lexer.LinePosition };
                return node;
            }
            else if (token.Type == TokenType.IntegerLiteral)
            {
                ConsumeCurrentToken(TokenType.IntegerLiteral);
                return new IntegerLiteral(Double.Parse(token.Value.ToString())) { Line = lexer.Line, Character = lexer.LinePosition };
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
            else if (token.Type == TokenType.KeywordFalse)
            {
                ASTNode result = FalseLiteral();
                return result;
            }
            else if (token.Type == TokenType.KeywordTrue)
            {
                ASTNode result = TrueLiteral();
                return result;
            }
            else if (token.Type == TokenType.KeywordNull)
            {
                ASTNode result = NullLiteral();
                return result;
            }
            else if (token.Type == TokenType.KeywordNew)
            {
                ASTNode result = ClassInstanceCreation();
                return result;
            }
            else if (token.Type == TokenType.NamespacePropertyCall)
            {
                ASTNode result = NamespacePropertyCall();
                return result;
            }
            else if (token.Type == TokenType.LeftSquareBracket)
            {
                ASTNode result = List();
                return result;
            }
            else if (token.Type == TokenType.Pipe || token.Type == TokenType.Ampersand)
            {
                return null;
            }

            throw new SyntaxError(lexer.Line, lexer.LinePosition, "Expression does not validate to known possible operations.");
        }

        private ASTNode FalseLiteral()
        {
            ConsumeCurrentToken(TokenType.KeywordFalse);
            return new FalseLiteral();
        }

        private ASTNode TrueLiteral()
        {
            ConsumeCurrentToken(TokenType.KeywordTrue);
            return new TrueLiteral();
        }

        private ASTNode List()
        {
            AST.List list = new AST.List() { Line = lexer.Line, Character = lexer.LinePosition };
            ConsumeCurrentToken(TokenType.LeftSquareBracket);

            if (currentToken.Type != TokenType.RightSquareBracket)
            {
                while (true)
                {
                    list.Items.Add(Expression());

                    if (currentToken.Type != TokenType.Comma)
                        break;
                    ConsumeCurrentToken(TokenType.Comma);
                }
            }

            ConsumeCurrentToken(TokenType.RightSquareBracket);
            return list;
        }

        private ASTNode NullLiteral()
        {
            ConsumeCurrentToken(TokenType.KeywordNull);
            return new NullLiteral() { Line = lexer.Line, Character = lexer.LinePosition };
        }

        private ASTNode Variable()
        {
            ASTNode node = new Variable(currentToken, currentToken.Value.ToString()) { Line = lexer.Line, Character = lexer.LinePosition };
            ConsumeCurrentToken(TokenType.Word);

            if (currentToken.Type == TokenType.LeftSquareBracket)
            {
                Indexer i = (Indexer)Indexer();
                i.Variable = (Variable)node;
                node = i;
            }

            return node;
        }

        private ASTNode Indexer()
        {
            Indexer indexer = new Indexer() { Line = lexer.Line, Character = lexer.LinePosition };
            ConsumeCurrentToken(TokenType.LeftSquareBracket);

            indexer.Start = Convert.ToInt32(currentToken.Value);
            ConsumeCurrentToken(TokenType.IntegerLiteral);

            if (currentToken.Type == TokenType.Dot) // Ranged indexer
            {
                ConsumeCurrentToken(TokenType.Dot);
                ConsumeCurrentToken(TokenType.Dot);
                indexer.End = Convert.ToInt32(currentToken.Value);
                ConsumeCurrentToken(TokenType.IntegerLiteral);
            }
            else
            {
                indexer.End = indexer.Start;
            }

            ConsumeCurrentToken(TokenType.RightSquareBracket);
            return indexer;
        }

        private ASTNode Argument()
        {
            ASTNode node = new Argument(currentToken, currentToken.Value.ToString()) { Line = lexer.Line, Character = lexer.LinePosition };
            ConsumeCurrentToken(TokenType.Word);
            return node;
        }

        public ASTNode StringLiteral()
        {
            ASTNode node = new StringLiteral(currentToken.Value.ToString()) { Line = lexer.Line, Character = lexer.LinePosition };
            ConsumeCurrentToken(TokenType.StringLiteral);
            return node;
        }


        private ASTNode Statement(bool ignoreSemicolon = false)
        {
            ASTNode node;
            if (currentToken.Type == TokenType.BlockOpen)
            {
                node = Block();
            }
            else if (currentToken.Type == TokenType.Word)
            {
                node = AssigmentStatement();
                if (!ignoreSemicolon)
                    ConsumeCurrentToken(TokenType.Semicolon);
            }
            else if (currentToken.Type == TokenType.KeywordVariable)
            {
                node = VariableDeclaration();
                if (!ignoreSemicolon)
                    ConsumeCurrentToken(TokenType.Semicolon);
            }
            else if (currentToken.Type == TokenType.FunctionCall)
            {
                node = FunctionCall();
                if (!ignoreSemicolon)
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
                if (!ignoreSemicolon)
                    ConsumeCurrentToken(TokenType.Semicolon);
            }
            else if (currentToken.Type == TokenType.KeywordIfStatement)
            {
                node = IfStatement();
            }
            else if (currentToken.Type == TokenType.KeywordWhile)
            {
                node = WhileStatement();
            }
            else if (currentToken.Type == TokenType.KeywordFor)
            {
                node = ForStatement();
            }
            else if (currentToken.Type == TokenType.KeywordReturn)
            {
                node = ReturnStatement();
                if (!ignoreSemicolon)
                    ConsumeCurrentToken(TokenType.Semicolon);
            }
            else if (currentToken.Type == TokenType.KeywordClass)
            {
                node = ClassDeclaration();
            }
            else if (currentToken.Type == TokenType.Semicolon)
            {
                node = new NoOperator() { Line = lexer.Line, Character = lexer.LinePosition };
                if (!ignoreSemicolon)
                    ConsumeCurrentToken(TokenType.Semicolon);
            }
            else if (currentToken.Type == TokenType.KeywordNew)
            {
                node = ClassInstanceCreation();
                if (!ignoreSemicolon)
                    ConsumeCurrentToken(TokenType.Semicolon);
            }
            else
                throw new SyntaxError(lexer.Line, lexer.LinePosition, $"Unexpected token '{TokenTypeExtension.TokenTypeToReadableString(currentToken.Type)}' found.");

            return node;
        }

        private ASTNode WhileStatement()
        {
            WhileStatement node = new WhileStatement() { Line = lexer.Line, Character = lexer.LinePosition };
            ConsumeCurrentToken(TokenType.KeywordWhile);
            ConsumeCurrentToken(TokenType.LeftParenthesis);
            node.BooleanExpression = BooleanExpression();
            ConsumeCurrentToken(TokenType.RightParenthesis);
            node.Block = (Block)Block();

            return node;
        }

        private ASTNode ForStatement()
        {
            ForStatement node = new ForStatement() { Line = lexer.Line, Character = lexer.LinePosition };
            ConsumeCurrentToken(TokenType.KeywordFor);
            ConsumeCurrentToken(TokenType.LeftParenthesis);

            node.VariableDeclaration = VariableDeclaration();
            ConsumeCurrentToken(TokenType.Semicolon);

            node.BooleanExpression = (BooleanExpression)BooleanExpression();
            ConsumeCurrentToken(TokenType.Semicolon);

            node.Statement = Statement(true);

            ConsumeCurrentToken(TokenType.RightParenthesis);

            node.Block = (Block)Block();

            return node;
        }

        private ASTNode ClassDeclaration()
        {
            ConsumeCurrentToken(TokenType.KeywordClass);
            string className = currentToken.Value.ToString();
            ConsumeCurrentToken(TokenType.Word);

            ASTNode classNode = null;
            if (currentToken.Type == TokenType.Semicolon)
            {
                classNode = new ClassForwardDeclaration(className) { Line = lexer.Line, Character = lexer.LinePosition };
            }
            else
            {
                int line = lexer.Line;
                classNode = new ClassDeclaration((Block)Block(), className) { Line = line, Character = lexer.LinePosition };
            }

            
            return classNode;
        }

        private ASTNode ClassInstanceCreation()
        {
            ConsumeCurrentToken(TokenType.KeywordNew);

            ClassInstanceCreation node = new ClassInstanceCreation() { Line = lexer.Line, Character = lexer.LinePosition };

            if (currentToken.Type == TokenType.NamespacePropertyCall)
                node.ClassToCreate = NamespacePropertyCall();
            else if (currentToken.Type == TokenType.FunctionCall)
                node.ClassToCreate = FunctionCall();

            return node;
        }

        private ASTNode FunctionDeclaration()
        {
            ConsumeCurrentToken(TokenType.KeywordFunction);

            if (currentToken.Type == TokenType.Word) // Function forward declaration
            {
                FunctionForwardDeclaration node = new FunctionForwardDeclaration() { Line = lexer.Line, Character = lexer.LinePosition };
                node.Name = currentToken.Value.ToString();
                ConsumeCurrentToken(TokenType.Word);
                ConsumeCurrentToken(TokenType.Semicolon);
                return node;
            }
            else
            {
                FunctionDeclaration node = new FunctionDeclaration() { Line = lexer.Line, Character = lexer.LinePosition };
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

            throw new SyntaxError(lexer.Line, lexer.LinePosition, $"Unexpected token '{currentToken.Type}' found on function declaration.");
        }

        private ASTNode IfStatement()
        {
            IfStatement node = new IfStatement() { Line = lexer.Line, Character = lexer.LinePosition };
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
            BooleanExpression node = new BooleanExpression() { Line = lexer.Line, Character = lexer.LinePosition };

            node.Left = Expression();
            node.Operator = currentToken.Type;
            bool isStandaloneExpressnion = false; // Means there is only 'true' or 'false' in the expression
            if (currentToken.Type == TokenType.Equals)
                ConsumeCurrentToken(TokenType.Equals);
            else if (currentToken.Type == TokenType.NotEquals)
                ConsumeCurrentToken(TokenType.NotEquals);
            else if (currentToken.Type == TokenType.Smaller)
                ConsumeCurrentToken(TokenType.Smaller);
            else if (currentToken.Type == TokenType.Greater)
                ConsumeCurrentToken(TokenType.Greater);
            else if (currentToken.Type == TokenType.SmallerEquals)
                ConsumeCurrentToken(TokenType.SmallerEquals);
            else if (currentToken.Type == TokenType.GreaterEquals)
                ConsumeCurrentToken(TokenType.GreaterEquals);
            else if (currentToken.Type == TokenType.RightParenthesis)
                isStandaloneExpressnion = true;

            if (!isStandaloneExpressnion)
                node.Right = Expression();

            if (currentToken.Type == TokenType.Pipe || currentToken.Type == TokenType.Ampersand)
            {
                BooleanExpressionCombination combination = new BooleanExpressionCombination() { Left = node, Line = lexer.Line };

                TokenType savedType = currentToken.Type;
                ConsumeCurrentToken(savedType);
                ConsumeCurrentToken(savedType);

                combination.Right = BooleanExpression();
                combination.Operator = savedType;
                return combination;
            }

            return node;
        }

        private ASTNode ReturnStatement()
        {
            ReturnStatement node = new ReturnStatement() { Line = lexer.Line, Character = lexer.LinePosition };
            ConsumeCurrentToken(TokenType.KeywordReturn);
            node.ReturnNode = Expression();
            return node;
        }

        private ASTNode Block()
        {
            ConsumeCurrentToken(TokenType.BlockOpen);
            List<ASTNode> nodes = StatementList();
            ConsumeCurrentToken(TokenType.BlockClose);

            Block root = new Block() { Line = lexer.Line, Character = lexer.LinePosition };
            foreach (ASTNode subNode in nodes)
            {
                root.Children.Add(subNode);
            }
            return root;
        }

        private ASTNode AssigmentStatement()
        {
            ASTNode left = Variable();
            ASTNode node = null;
            ASTNode right = null;
            Token token = new Token(TokenType.Assign, '=');

            // Assign like normal
            if (currentToken.Type == TokenType.Assign)
            {
                ConsumeCurrentToken(TokenType.Assign);
                right = Expression();
            }
            // Assign variable to itself + 1
            else if (currentToken.Type == TokenType.Plus)
            {
                ConsumeCurrentToken(TokenType.Plus);
                ConsumeCurrentToken(TokenType.Plus);

                right = new BinaryOperator(left, new Token(TokenType.Plus, '+'), new IntegerLiteral(1)) { Line = lexer.Line, Character = lexer.LinePosition };
            }
            else if (currentToken.Type == TokenType.Minus)
            {
                ConsumeCurrentToken(TokenType.Minus);
                ConsumeCurrentToken(TokenType.Minus);

                right = new BinaryOperator(left, new Token(TokenType.Minus, '-'), new IntegerLiteral(1)) { Line = lexer.Line, Character = lexer.LinePosition };
            }

            node = new AssignmentOperator(left, token, right) { Line = lexer.Line, Character = lexer.LinePosition };
            return node;
        }

        private VariableDeclaration VariableDeclaration()
        {
            ConsumeCurrentToken(TokenType.KeywordVariable);
            Variable variable = new Variable(currentToken, currentToken.Value.ToString()) { Line = lexer.Line, Character = lexer.LinePosition };
            ConsumeCurrentToken(TokenType.Word);
            VariableDeclaration declaration = new VariableDeclaration(variable) { Line = lexer.Line, Character = lexer.LinePosition };

            if (currentToken.Type == TokenType.Assign)
            {
                ConsumeCurrentToken(TokenType.Assign);
                declaration.InitialDefinition = Expression();
            }
            
            return declaration;
        }

        private ASTNode FunctionCall()
        {
            FunctionCall call = new FunctionCall(currentToken.Value.ToString()) { Line = lexer.Line, Character = lexer.LinePosition };
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
            NamespacePropertyCall call = new NamespacePropertyCall() { CallerName = currentToken.Value.ToString(), Line = lexer.Line, Character = lexer.LinePosition };
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
            NamespaceDeclaration namespaceNode = new NamespaceDeclaration((Block)Block(), namespaceName) { Line = lexer.Line, Character = lexer.LinePosition };
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
