using NullPInterpreter.Interpreter.AST;
using NullPInterpreter.Interpreter.CallStackManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.Events
{
    public class BreakpointHitEventArgs : EventArgs
    {
        public ASTNode CurrentNode { get; }
        public CallStack Callstack { get; }

        public BreakpointHitEventArgs(ASTNode currentNode, CallStack callstack)
        {
            CurrentNode = currentNode;
            Callstack = callstack;
        }
    }
}
