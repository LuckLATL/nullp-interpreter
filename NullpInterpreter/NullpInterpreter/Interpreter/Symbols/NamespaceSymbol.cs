using NullPInterpreter.Interpreter.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.Symbols
{
    public class NamespaceSymbol : Symbol
    {
        public NamespaceDeclaration Declaration { get; set; }
    }
}
