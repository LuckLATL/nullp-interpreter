using NullPInterpreter.Interpreter;
using NullPInterpreter.Interpreter.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreterTests
{
    static class CodeInterpreter
    {
        public static Interpreter InterpretProgram(string code)
        {
            Lexer lexer = new Lexer(code);
            Parser p = new Parser(lexer);
            ASTNode rootNode = p.Parse();
            Interpreter interpreter = new Interpreter(p);
            interpreter.SematicAnalysis(rootNode);
            interpreter.Interpret();

            return interpreter;
        }
    }
}
