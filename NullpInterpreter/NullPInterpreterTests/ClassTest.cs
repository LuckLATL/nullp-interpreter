using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreterTests
{
    [TestClass]
    public class ClassTest
    {
        [TestMethod]
        public void ClassDeclaration()
        {
            string simpleProgram =
            @"
                class A
				{
					function A_Func(par1)
					{
						Console.WriteLine(""A_Func"");
					}
				}

				var aInstance = new A();
				aInstance.A_Func(0);
            ";
            CodeInterpreter.InterpretProgram(simpleProgram);
        }

		[TestMethod]
		public void ClassDeclarationWithConstructor()
		{
			string simpleProgram =
			@"
                class A
				{
					function A(par1)
					{
						Console.WriteLine(par1);
					}

					function A_Func(par1)
					{
						Console.WriteLine(""A_Func"");
					}
				}

				var aInstance = new A(""parameter1"");
				aInstance.A_Func(0);
            ";
			CodeInterpreter.InterpretProgram(simpleProgram);
		}

		[TestMethod]
        public void StackImplementationAsClass()
        {
            string simpleProgram =
			@"
            class Stack;

			var stack = new Stack();

			stack.Push(""item1"");

			stack.Push(""item2"");
			stack.Push(""item3"");
			stack.Push(""item4"");

			while (stack.count > 0)
			{
				stack.Pop();
				stack.Peek();
			}

			class Stack
					{
						var count = 0;

						class Item
						{
							var next;
							var value;

							function Item(n, v)
							{
								next = n;
								value = v;
							}
						}

						var topMost = null;

						function Push(item)
						{
							count++;
							topMost = new Item(topMost, item);
						}

						function Pop()
						{
							if (topMost == null)
							{
								return null;
							}

							count--;
							var retVal = topMost.value;
							topMost = topMost.next;
							return retVal;
						}

						function Peek()
						{
							return topMost.value;
						}

					}
            ";
            CodeInterpreter.InterpretProgram(simpleProgram);
        }
    }
}
