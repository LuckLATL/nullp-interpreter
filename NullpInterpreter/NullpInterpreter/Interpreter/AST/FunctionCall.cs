using NullPInterpreter.Interpreter.Symbols;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    [DebuggerDisplay("[Function Call '{FunctionName, nq}(Arguments: {Arguments.Count})']")]
    public class FunctionCall : ASTNode
    {
        public string FunctionName { get; set; }
        public List<ASTNode> Arguments { get; set; } = new List<ASTNode>();
        public FunctionSymbol FunctionSymbol { get; set; }

        public FunctionCall(string functionName)
        {
            FunctionName = functionName;
        }

        public override string? ToString()
        {
            string arguments = "";

            foreach (var item in Arguments)
            {
                arguments += $"{item.ToString()}, ";
            }

            if (arguments.Length > 0)
                arguments = arguments.Substring(0, arguments.Length - 2);

            return $"{FunctionName}({arguments})";
        }
    }
}
