using NullPInterpreter.Interpreter.AST;
using NullPInterpreter.Interpreter.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.BuiltIn
{
    public class BuiltInClassSymbol : ClassSymbol
    {
        public Dictionary<string, Func<List<object>, object>> Functions { get; set; } = new Dictionary<string, Func<List<object>, object>>();

        public static ClassDeclaration GetClass()
        {
            Block block = new Block();
            AST.BuiltIn builtIn = new AST.BuiltIn();
            builtIn.Function = (args) => { return null; };
            block.Children.Add(builtIn);
            ClassDeclaration classDeclaration = new ClassDeclaration(block, "Unknown");
            return classDeclaration;
        }

        public ScopedSymbolTable GetScope()
        {
            ScopedSymbolTable scopedSymbolTable = new ScopedSymbolTable(Name, 0, null);

            foreach (var item in Functions)
            {
                FunctionDeclaration functionDeclaration = new FunctionDeclaration() { FunctionName = item.Key };
                functionDeclaration.Block = new Block();
                AST.BuiltIn builtIn = new AST.BuiltIn();
                builtIn.Function = item.Value;
                functionDeclaration.Block.Children.Add(builtIn);
                scopedSymbolTable.AddSymbol(new BuiltInFunctionSymbol() { Name = item.Key, Type = SymbolType.Function, Declaration = functionDeclaration });
            }

            return scopedSymbolTable;
        }

        public virtual BuiltInClassSymbol New() { return new BuiltInClassSymbol(); }
    }
}
