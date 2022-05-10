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
    [DebuggerDisplay("[Class Symbol '{ToString(), nq}']")]
    public class ClassSymbol : Symbol
    {
        public ClassDeclaration Declaration { get; set; }

        public ScopedSymbolTable ClassSymbols { get; set; }

        public ActivationRecord ClassActivationRecord { get; set; }

        public override string? ToString()
        {
            string baseString = Declaration.Name;

            ScopedSymbolTable visitingScope = ClassSymbols.EnclosingScope;
            while (visitingScope != null)
            {
                baseString = visitingScope.ScopeName + "." + baseString;
                visitingScope = visitingScope.EnclosingScope;
            }

            return $"<{baseString}>";
        }
    }
}
