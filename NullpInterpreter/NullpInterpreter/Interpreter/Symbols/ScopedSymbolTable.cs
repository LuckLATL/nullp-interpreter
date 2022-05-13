using NullPInterpreter.Interpreter.BuiltIn;
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
            foreach (var item in BuiltInManager.BuiltIns)
            {
                AddBuiltIn(item, this);
            }

            //symbols.Add("WriteLine", new FunctionSymbol() { Name = "WriteLine", Type = SymbolType.Function });
            //symbols.Add("ReadLine", new FunctionSymbol() { Name = "ReadLine", Type = SymbolType.Function });
            
            symbols.Add("DateTime", new NullPDateTime());

        }

        private void AddBuiltIn(BuiltInBase builtIn, ScopedSymbolTable parent)
        {
            switch (builtIn)
            {
                case BuiltInFunction builtInFunction:
                    FunctionSymbol f = new FunctionSymbol() { Name = builtInFunction.Name, Type = SymbolType.Function };
                    
                    f.Declaration = new AST.FunctionDeclaration() { Block = new AST.Block(), FunctionName = f.Name };
                    f.Declaration.Block.Children.Add(new AST.BuiltIn() { Function = builtInFunction.Function });
                    parent.AddSymbol(f);
                    break;
                case BuiltInNamespace builtInNamespace:
                    NamespaceSymbol s = new NamespaceSymbol() { Name = builtInNamespace.Name, Type = SymbolType.Namespace };
                    s.NamespaceActivationRecord = new CallStackManagement.ActivationRecord(builtInNamespace.Name, CallStackManagement.ActivationRecordType.Namespace, parent.ScopeLevel + 1);
                    s.NamespaceSymbols = new ScopedSymbolTable(builtInNamespace.Name, parent.ScopeLevel + 1, parent);
                    parent.AddSymbol(s);

                    foreach (var item in builtInNamespace.Children)
                    {
                        AddBuiltIn(item, s.NamespaceSymbols);
                    }

                    break;
                case BuiltInClass builtInClass:
                    break;
            }
        }

        public void AddSymbol(Symbol symbol)
        {
            if (symbols.ContainsKey(symbol.Name))
            {
                if (symbol.Type == SymbolType.Function && symbols[symbol.Name].Type == SymbolType.Function && (symbols[symbol.Name] as FunctionSymbol).Declaration == null)
                    ((FunctionSymbol)symbols[symbol.Name]).Declaration = ((FunctionSymbol)symbol).Declaration;
                else if (symbol.Type == SymbolType.Class && symbols[symbol.Name].Type == SymbolType.Class && (symbols[symbol.Name] as ClassSymbol).Declaration == null)
                {
                    ((ClassSymbol)symbols[symbol.Name]).Declaration = ((ClassSymbol)symbol).Declaration;
                    ((ClassSymbol)symbols[symbol.Name]).ClassSymbols = ((ClassSymbol)symbol).ClassSymbols;
                }
                else
                    throw new DuplicateIdentifierError($"Symbol with the name '{symbol.Name}' has already been declared.");
            }
            symbols[symbol.Name] = symbol;
        }

        public Symbol LookUpSymbol(string symbolName, bool returnNullOnMiss = false)
        {
            if (!symbols.ContainsKey(symbolName))
            {
                if (EnclosingScope == null)
                {
                    if (returnNullOnMiss)
                        return null;

                    throw new InvalidIdentifierError($"Symbol with the name '{symbolName}' has not been declared.");
                }
                else
                {
                    return EnclosingScope.LookUpSymbol(symbolName, returnNullOnMiss);
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
