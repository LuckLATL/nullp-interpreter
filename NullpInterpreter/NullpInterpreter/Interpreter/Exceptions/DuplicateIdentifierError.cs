using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.Exceptions
{
    public class DuplicateIdentifierError : Exception
    {
        public DuplicateIdentifierError(string? message) : base(message) { }
    }
}
