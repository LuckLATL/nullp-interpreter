using Microsoft.VisualStudio.TestTools.UnitTesting;
using NullPInterpreter.Interpreter;
using NullPInterpreter.Interpreter.AST;

namespace NullPInterpreterTests
{
    [TestClass]
    public class VariableTest
    {
        [TestMethod]
        public void VariableDeclaration()
        {
            string simpleProgram =
            @"
                // Define string
                var name = ""Hello World"";
                name = ""My Name"";
                
                // Define number
                var number;
                number = 2;
                number = 5.0;

                // Define boolean
                var boolean = false;
                boolean = true;
                
                // Null as a value
                name = null;
            ";
            CodeInterpreter.InterpretProgram(simpleProgram);                
        }

        [TestMethod]
        public void NumberOperation()
        {
            string simpleProgram =
            @"
                var newNumber = (5 * 10) + 1;
                newNumber = newNumber + 10 / 2;
            ";
            Interpreter interpreter = CodeInterpreter.InterpretProgram(simpleProgram);
            double newNumber = (double)interpreter.LastProgramActivationRecord.GetMember("newNumber");
            Assert.AreEqual(56, newNumber);
        }

        [TestMethod]
        public void StringAddition()
        {
            string simpleProgram =
            @"
                var newString = ""Hello"";
                newString = newString + "" World"";
                var newStringWithNumber = newString + "" "" + 10 + "" times"";
            ";
            Interpreter interpreter = CodeInterpreter.InterpretProgram(simpleProgram);
            string newString = (string)interpreter.LastProgramActivationRecord.GetMember("newString");
            string newStringWithNumber = (string)interpreter.LastProgramActivationRecord.GetMember("newStringWithNumber");
            Assert.AreEqual("Hello World", newString);
            Assert.AreEqual("Hello World 10 times", newStringWithNumber);
        }
    }
}