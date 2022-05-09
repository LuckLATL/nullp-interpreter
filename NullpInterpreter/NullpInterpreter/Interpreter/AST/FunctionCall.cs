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
    }
}
