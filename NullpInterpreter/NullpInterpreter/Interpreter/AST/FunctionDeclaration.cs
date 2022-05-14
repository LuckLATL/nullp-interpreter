using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    [DebuggerDisplay("[Function Declaration '{FunctionName, nq} (Arguments: {Arguments.Count, nq})']")]
    public class FunctionDeclaration : ASTNode
    {
        public string FunctionName { get; set; }
        public List<Argument> Arguments { get; set; } = new List<Argument>();
        public Block Block { get; set; }

        public override string? ToString()
        {
            string arguments = "";

            foreach (var item in Arguments)
            {
                arguments += $"{item.ToString()}, ";
            }

            if (arguments.Length > 0)
                arguments = arguments.Substring(0, arguments.Length - 2);        

            return $"function {FunctionName}({arguments})";
        }
    }
}
