using NullPInterpreter.Interpreter.AST;
using NullPInterpreter.Interpreter.CallStackManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.Symbols
{
    public class NamespaceSymbol : Symbol
    {
        public ScopedSymbolTable NamespaceSymbols { get; set; }
        public ActivationRecord NamespaceActivationRecord { get; set; }
    }
}
