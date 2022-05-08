using Microsoft.VisualStudio.TestTools.UnitTesting;
using NullPInterpreter.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreterTests
{
    [TestClass]
    public class ConditionalAndLoopTest
    {
        [TestMethod]
        public void IfStatement()
        {
            string simpleProgram =
            @"
                function TestFunc(executeIf)
                {
	                if (executeIf == true)
	                {
		                return 0;
	                }
	                else
	                {
		                return 1;
	                }
                }

                var result = TestFunc(true);
            ";
            Interpreter interpreter = CodeInterpreter.InterpretProgram(simpleProgram);
            double result = (double)interpreter.LastProgramActivationRecord.GetMember("result");
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void ElseStatement()
        {
            string simpleProgram =
            @"
                function TestFunc(executeIf)
                {
	                if (executeIf == true)
	                {
		                return 0;
	                }
	                else
	                {
		                return 1;
	                }
                }

                var result = TestFunc(false);
            ";
            Interpreter interpreter = CodeInterpreter.InterpretProgram(simpleProgram);
            double result = (double)interpreter.LastProgramActivationRecord.GetMember("result");
            Assert.AreEqual(1, result);
        }
    }
}
