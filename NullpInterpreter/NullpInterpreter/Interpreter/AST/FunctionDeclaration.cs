﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class FunctionDeclaration : ASTNode
    {
        public string FunctionName { get; set; }
        public List<Argument> Arguments { get; set; } = new List<Argument>();
        public Block Block { get; set; }
    }
}
