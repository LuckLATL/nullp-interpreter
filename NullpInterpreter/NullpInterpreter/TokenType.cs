using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter
{
    public enum TokenType
    {
        KeywordVariable,                            
        StringLiteral,                            
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
        KeywordIfStatement,
        KeywordElseStatement,
        Equals,
        NotEquals,
        KeywordFunction,
        FunctionCall                        // Calling a function
    }
}
