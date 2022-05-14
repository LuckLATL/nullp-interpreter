using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    [DebuggerDisplay("[Variable Declaration '{Variable.Name, nq} = {InitialDefinition, nq}']" )]
    public class VariableDeclaration : ASTNode
    {
        public Variable Variable { get; set; }

        public ASTNode InitialDefinition { get; set; }

        public VariableDeclaration(Variable variable)
        {
            Variable = variable;
        }

        public override string? ToString()
        {
            return $"var {Variable} = {InitialDefinition};";
        }
    }
}
