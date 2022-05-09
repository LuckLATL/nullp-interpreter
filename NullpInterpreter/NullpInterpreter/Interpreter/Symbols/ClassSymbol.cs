using NullPInterpreter.Interpreter.AST;
using NullPInterpreter.Interpreter.CallStackManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.Symbols
{
    public class ClassSymbol : Symbol
    {
        public ClassDeclaration Declaration { get; set; }

        public ScopedSymbolTable ClassSymbols { get; set; }

        public ActivationRecord ClassActivationRecord { get; set; }
    }
}
