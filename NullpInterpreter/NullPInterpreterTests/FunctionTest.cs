using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreterTests
{
    [TestClass]
    public class FunctionTest
    {
        [TestMethod]
        public void FunctionDeclaration()
        {
            string simpleProgram =
            @"
                function TestFunc(par1, par2)
                {
                    var test = par1 + par2;
                    test = test + 1;
                    return test;
                }
                
                var calcStuff = TestFunc(5, 3);
                WriteLine(calcStuff);
            ";
            CodeInterpreter.InterpretProgram(simpleProgram);
        }

        [TestMethod]
        public void FunctionForwardDeclaration()
        {
            string simpleProgram =
            @"
                function TestFunc;
                var calcStuff = TestFunc(5, 3);
                function TestFunc(par1, par2) {}
            ";
            CodeInterpreter.InterpretProgram(simpleProgram);
        }
    }
}
