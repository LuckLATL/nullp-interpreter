using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Special
{
    public class NullObject
    {
        public override string? ToString()
        {
            return "<NULL>";
        }

        public override bool Equals(object? obj)
        {
            return obj is NullObject;
        }
    }
}
