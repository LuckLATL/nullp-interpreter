using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.AST
{
    public class VariableDeclaration : ASTNode
    {
        public Variable Variable { get; set; }

        public ASTNode InitialDefinition { get; set; }

        public VariableDeclaration(Variable variable)
        {
            Variable = variable;
        }
    }
}
