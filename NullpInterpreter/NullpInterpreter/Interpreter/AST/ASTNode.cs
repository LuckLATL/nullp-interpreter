using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public abstract class ASTNode 
    {
        public int Line { get; set; }
        public int Character { get; set; }
    }
}
