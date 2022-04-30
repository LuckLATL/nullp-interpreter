using NullPInterpreter.Interpreter;
using NullPInterpreter.Interpreter.AST;
using NullPInterpreter.Interpreter.Exceptions;
using System.Diagnostics;
using System.Text.Json;

namespace NullPInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            string content = File.ReadAllText(@"code-file.nullp");
            Console.WriteLine("============================================");
            Console.WriteLine("Starting Parser...");
            Stopwatch stopwatch = new Stopwatch();
            try
            {
                Interpreter.Interpreter interpreter = new Interpreter.Interpreter(new Parser(new Lexer(content)));
                interpreter.Prepare();
                //interpreter.Interpret();
            }
            catch (SyntaxError ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Syntax Error at {ex.Line}:{ex.Position}: {ex.Message}");
                Console.ResetColor();
            }
            catch (InvalidIdentifierError ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid Identifier Error: {ex.Message}");
                Console.ResetColor();
            }
            catch (DuplicateIdentifierError ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Duplicate Identifier Error: {ex.Message}");
                Console.ResetColor();
            }

            stopwatch.Stop();
            Console.WriteLine($"Finished in {stopwatch.ElapsedMilliseconds}ms");
            //string json = Newtonsoft.Json.JsonConvert.SerializeObject(node, Newtonsoft.Json.Formatting.Indented);
            //File.WriteAllText("output.txt", json);
            Console.ReadLine();

        }
    }
}

