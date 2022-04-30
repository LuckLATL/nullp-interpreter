using NullPInterpreter.Interpreter.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.Symbols
{
    [DebuggerDisplay("(ScopedSymbolTable) {ScopeName,np} [{ScopeLevel,np}]")]
    public class ScopedSymbolTable
    {
        Dictionary<string, Symbol> symbols = new Dictionary<string, Symbol>();

        public string ScopeName { get; set; }
        public int ScopeLevel { get; set; }
        public ScopedSymbolTable EnclosingScope { get; set; }

        public ScopedSymbolTable(string scopeName, int scopeLevel, ScopedSymbolTable enclosingScope = null)
        {
            ScopeName = scopeName;
            ScopeLevel = scopeLevel;
            EnclosingScope = enclosingScope;
        }

        public void InitializeBuiltIns()
        {
            symbols.Add("WriteLine", new Symbol() { Name = "WriteLine", Type = SymbolType.Function });
            symbols.Add("ReadLine", new Symbol() { Name = "ReadLine", Type = SymbolType.Function });
        }

        public void AddSymbol(Symbol symbol)
        {
            if (symbols.ContainsKey(symbol.Name))
                throw new DuplicateIdentifierError($"Symbol with the name '{symbol.Name}' has already been declared.");
            symbols[symbol.Name] = symbol;
        }

        public Symbol LookUpSymbol(string symbolName)
        {
            if (!symbols.ContainsKey(symbolName))
            {
                if (EnclosingScope == null)
                {
                    throw new InvalidIdentifierError($"Symbol with the name '{symbolName}' has not been declared.");
                }
                else
                {
                    return EnclosingScope.LookUpSymbol(symbolName);
                }
            }
            return symbols[symbolName];
        }

        public void RemoveSymbol(Symbol symbol)
        {
            symbols.Remove(symbol.Name);
        }

        public override string? ToString()
        {
            string returnResult = $"===================== {ScopeName} =========================\n";
            foreach (Symbol symbol in symbols.Values)
            {
                for (int i = 0; i < ScopeLevel; i++)
                {
                    returnResult += "\t";
                }

                returnResult += $"[{symbol.Type}]\t{symbol.Name} \n";
            }
            return returnResult;
        }
    }
}
