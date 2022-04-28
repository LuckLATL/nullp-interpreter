using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter
{
    public class Token
    {
        /// <summary>
        /// Token Type for all kinds of tokens
        /// </summary>
        public TokenType Type { get; set; }
        /// <summary>
        /// Value which the token contains. Also called Lexeme
        /// </summary>
        public object Value { get; set; }

        public Token(TokenType type, object value)
        {
            this.Type = type;
            this.Value = value;
        }

        public override string ToString()
        {
            // Just some fancy output :)
            return $"Token{Type}, {Value}";
        }
    }
}
