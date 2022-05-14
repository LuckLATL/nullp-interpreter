using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class Indexer : ASTNode
    {
        public Variable Variable { get; set; }
        public int Start { get; set; }
        public int End { get; set; }

        public override string? ToString()
        {
            return $"{Variable}[{Start}..{End}]";
        }
    }
}
