using NullPInterpreter.Interpreter.AST;
using NullPInterpreter.Interpreter.CallStackManagement;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.Symbols
{
    [DebuggerDisplay("[Namespace Symbol '{Name, nq}']")]
    public class NamespaceSymbol : Symbol
    {
        public ScopedSymbolTable NamespaceSymbols { get; set; }
        public ActivationRecord NamespaceActivationRecord { get; set; }
    }
}
