using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class BuiltIn : ASTNode
    {
        public Func<List<object>, object> Function;
        public List<object> Arguments = new List<object>();
    }
}
