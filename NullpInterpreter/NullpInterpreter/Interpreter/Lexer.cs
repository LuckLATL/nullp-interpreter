using NullPInterpreter.Interpreter.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter
{
    public class Lexer
    {
        /// <summary>
        /// Current Position of the Lexer
        /// </summary>
        public int Position { get; private set; } = 0;
        public int Line { get; private set; } = 1;
        public int LastPositionLineStart { get; private set; } = 0;
        public int LinePosition { get { return Position - LastPositionLineStart + 1; } }
        /// <summary>
        /// Text the Lexer is processing
        /// </summary>
        private string text;
        /// <summary>
        /// Current Character the Lexer is processing
        /// </summary>
        private char currentCharacter;

        public Lexer(string text)
        {
            if (text.Length == 0)
                throw new Exception("EoF reached before could parse code. Input was empty.");

            this.text = text;
            currentCharacter = text[Position];
        }

        /// <summary>
        /// Advances in the text by one
        /// </summary>
        private void Advance()
        {
            Position++;
            if (Position > text.Length - 1)
                currentCharacter = '\0';
            else
                currentCharacter = text[Position];

            if (currentCharacter == '\n')
            {
                LastPositionLineStart = Position;
                Line++;
            }
        }

        private char Peek()
        {
            int peekPosition = Position + 1;
            if (peekPosition > text.Length - 1)
                return '\0';
            return text[peekPosition];
        }

        private char LookBack()
        {
            int lookBackPosition = Position - 1;
            if (lookBackPosition < 0)
                return '\0';
            return text[lookBackPosition];
        }

        /// <summary>
        /// Advances further as long as current character is a whitespace
        /// </summary>
        private void SkipWhitespace()
        {
            while (char.IsWhiteSpace(currentCharacter))
                Advance();
        }

        private void SkipComment()
        {
            while (currentCharacter != '\n')
            {
                Advance();
            }
            Advance();
        }

        private void SkipBlockComment()
        {
            while (!(LookBack() == '*' && currentCharacter == '/'))
            {
                Advance();
            }
            Advance();
        }

        /// <summary>
        /// Gets an full integer from the current position
        /// </summary>
        /// <returns>The integer as an integer, duh</returns>
        private Token GetNumber()
        {
            string result = "";
            while (currentCharacter != '\0' && char.IsDigit(currentCharacter))
            {
                result += currentCharacter;
                Advance();
            }

            if (currentCharacter == '.')
            {
                result += currentCharacter;
                Advance();

                while (currentCharacter != '\0' && char.IsDigit(currentCharacter))
                {
                    result += currentCharacter;
                    Advance();
                }
            }

            return new Token(TokenType.IntegerLiteral, Convert.ToDouble(result));
        }

        private Token GetWord()
        {
            string result = "";
            while (currentCharacter != '\0' && char.IsLetterOrDigit(currentCharacter))
            {
                result += currentCharacter;
                Advance();
            }

            if (currentCharacter == '(')
            {
                Token t = new Token(TokenType.FunctionCall, result);
                return t;
            }

            if (currentCharacter == '.')
            {
                Token t = new Token(TokenType.NamespacePropertyCall, result);
                return t;
            }

            // If word is reserved keyword, return keyword
            Token token = ReservedKeywords.Keywords.FirstOrDefault(x => (string)x.Value == result);
            if (token == null)
                token = new Token(TokenType.Word, result);
            return token;
        }

        private Token GetStringLiteral()
        {
            string result = "";
            Advance();
            while (currentCharacter != '\0' && LookBack() != '\\' && currentCharacter != '"')
            {
                result += currentCharacter;
                Advance();
            }
            Advance();
            Token token = new Token(TokenType.StringLiteral, result);
            return token;
        }

        /// <summary>
        /// Gets the next token in line
        /// </summary>
        /// <returns>A new token with all the necessary info for the interpreter</returns>
        public Token GetNextToken()
        {
            while (currentCharacter != '\0')
            {
                if (currentCharacter == '/' && Peek() == '/')
                {
                    SkipComment();
                    continue;
                }

                if (currentCharacter == '/' && Peek() == '*')
                {
                    SkipBlockComment();
                    continue;
                }

                // Skip all whitespaces
                if (char.IsWhiteSpace(currentCharacter))
                {
                    SkipWhitespace();
                    continue;
                }

                if (char.IsLetter(currentCharacter))
                    return GetWord();

                // Check if the current character is an digit. If so, return a new integer token
                if (char.IsDigit(currentCharacter))
                    return GetNumber();

                // Select between the different characters for tokenization
                switch (currentCharacter)
                {
                    case '+':
                        Advance();
                        return new Token(TokenType.Plus, '+');
                    case '-':
                        Advance();
                        return new Token(TokenType.Minus, '-');
                    case '*':
                        Advance();
                        return new Token(TokenType.Multiply, '*');
                    case '/':
                        Advance();
                        return new Token(TokenType.Divide, '/');
                    case '(':
                        Advance();
                        return new Token(TokenType.LeftParenthesis, '(');
                    case ')':
                        Advance();
                        return new Token(TokenType.RightParenthesis, ')');
                    case '{':
                        Advance();
                        return new Token(TokenType.BlockOpen, '{');
                    case '}':
                        Advance();
                        return new Token(TokenType.BlockClose, '}');
                    case '=':
                        Advance();
                        if (currentCharacter == '=')
                        {
                            Advance();
                            return new Token(TokenType.Equals, "==");
                        }
                        return new Token(TokenType.Assign, '=');
                    case '!':
                        Advance();
                        if (currentCharacter == '=')
                        {
                            Advance();
                            return new Token(TokenType.NotEquals, "!=");
                        }
                        throw new SyntaxError(Line, LinePosition, "Given comparision operator is not valid.");
                    case ';':
                        Advance();
                        return new Token(TokenType.Semicolon, ';');
                    case '.':
                        Advance();
                        return new Token(TokenType.Dot, '.');
                    case ',':
                        Advance();
                        return new Token(TokenType.Comma, ',');
                    case '"':
                        return GetStringLiteral();
                    default:
                        break;
                }
                // None of the tokens match :(
                throw new SyntaxError(Line, LinePosition, $"Invalid language token.");
            }
            // No more tokens to process => End of File/Line
            return new Token(TokenType.EoF, null);
        }

    }
}
