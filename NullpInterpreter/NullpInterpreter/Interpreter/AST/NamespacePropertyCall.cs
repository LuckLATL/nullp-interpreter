using NullPInterpreter.Interpreter.Symbols;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    [DebuggerDisplay("[Property Call '{CallerName, nq} ➜ {CalledNode, nq}']")]
    public class NamespacePropertyCall : ASTNode
    {
        public string CallerName { get; set; }
        public ASTNode CalledNode { get; set; }
        public Symbol SourceSymbol { get; set; }

        public override string? ToString()
        {
            return $"{CallerName}.{CalledNode}";
        }
    }
}
