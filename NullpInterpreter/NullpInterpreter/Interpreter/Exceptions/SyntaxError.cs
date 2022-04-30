using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.Exceptions
{
    public class SyntaxError : Exception
    {
        public int Line { get; set; }
        public int Position { get; set; }

        public SyntaxError(int line, int position, string message = "No further information provided.") : base(message)
        {
            Line = line;
            Position = position;
        }
    }
}
