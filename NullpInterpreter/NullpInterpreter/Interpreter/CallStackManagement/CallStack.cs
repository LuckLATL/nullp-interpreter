using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.CallStackManagement
{
    public class CallStack : Stack<ActivationRecord>
    {
        public void ExtendedPush(ActivationRecord record)
        {
            if (this.Count != 0)
                record.PreviousRecord = this.Peek();
            Debug.WriteLine("Callstack Enter: " + record.Name);
            this.Push(record);
        }
    }
}
