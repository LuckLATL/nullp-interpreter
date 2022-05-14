﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.CallStackManagement
{
    public enum ActivationRecordType
    {
        Program,
        Namespace,
        Class,
        Function,
        Block
    }
}
