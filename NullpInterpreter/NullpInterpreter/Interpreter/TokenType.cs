using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter
{
    public enum TokenType
    {
        KeywordVariable,
        StringLiteral,
        IntegerLiteral,
        Plus,                               // The plus operator '+'
        Minus,                              // The minus operator '-'
        Multiply,                           // The star operator '*'
        Divide,                             // The slash operator '/'
        EoF,                                // Indicator for the End of a File
        LeftParenthesis,                    // Opening bracket left '('
        RightParenthesis,                   // Closing bracket right ')'
        Assign,                             // Assignment operator '='
        Semicolon,                          // Semicolon ';'
        Dot,                                // Dot '.'
        Word,                               // Variable
        BlockOpen,                          // Block open '{'
        BlockClose,                         // Block close '}'
        KeywordNamespace,                   // Program header
        KeywordClass,
        KeywordIfStatement,
        KeywordElseStatement,
        KeywordWhile,
        Equals,
        NotEquals,
        KeywordFunction,
        NamespacePropertyCall,
        Comma,
        KeywordReturn,
        KeywordTrue,
        KeywordFalse,
        KeywordNull,
        KeywordNew,
        LeftSquareBracket,
        RightSquareBracket,
        FunctionCall                        // Calling a function
    }

    public class TokenTypeExtension
    {
        public static string TokenTypeToReadableString(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.KeywordVariable:
                    return "var";
                case TokenType.StringLiteral:
                    return "string";
                case TokenType.IntegerLiteral:
                    return "integer";
                case TokenType.Plus:
                    return "+";
                case TokenType.Minus:
                    return "-";
                case TokenType.Multiply:
                    return "*";
                case TokenType.Divide:
                    return "/";
                case TokenType.EoF:
                    return "End of File";
                case TokenType.LeftParenthesis:
                    return "(";
                case TokenType.RightParenthesis:
                    return ")";
                case TokenType.Assign:
                    return "=";
                case TokenType.Semicolon:
                    return ";";
                case TokenType.Dot:
                    return ".";
                case TokenType.Word:
                    return "word";
                case TokenType.BlockOpen:
                    return "{";
                case TokenType.BlockClose:
                    return "}";
                case TokenType.KeywordNamespace:
                    return "namespace";
                case TokenType.KeywordIfStatement:
                    return "if";
                case TokenType.KeywordElseStatement:
                    return "else";
                case TokenType.Equals:
                    return "==";
                case TokenType.NotEquals:
                    return "!=";
                case TokenType.KeywordFunction:
                    return "function";
                case TokenType.NamespacePropertyCall:
                    return "namespace property";
                case TokenType.Comma:
                    return ",";
                case TokenType.KeywordReturn:
                    return "return";
                case TokenType.KeywordNull:
                    return "null";
                case TokenType.KeywordTrue:
                    return "true";
                case TokenType.KeywordFalse:
                    return "false";
                case TokenType.KeywordClass:
                    return "class";
                case TokenType.KeywordNew:
                    return "new";
                case TokenType.LeftSquareBracket:
                    return "[";
                case TokenType.RightSquareBracket:
                    return "]";
                case TokenType.KeywordWhile:
                    return "while";
                case TokenType.FunctionCall:
                    return "function call";
            }
            return "[not found]";
        }
    }
}
