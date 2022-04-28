using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter
{
    public static class ReservedKeywords
    {
        public static List<Token> Keywords { get; set; } = new List<Token>()
        {
            new Token(TokenType.KeywordNamespace, "namespace"),
            new Token(TokenType.KeywordIfStatement, "if"),
            new Token(TokenType.KeywordVariable, "var"),
            new Token(TokenType.KeywordFunction, "function")
        };
    }
}
