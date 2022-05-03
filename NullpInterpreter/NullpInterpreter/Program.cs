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
            while (true)
            {
                Console.Clear();
                string content = File.ReadAllText(@"code-file.nullp");
                Stopwatch stopwatch = new Stopwatch();
                try
                {
                    Parser p = new Parser(new Lexer(content));
                    Console.WriteLine("Parser");
                    Interpreter.Interpreter interpreter = new Interpreter.Interpreter(p);
                    Console.WriteLine("sem an");
                    interpreter.Prepare();
                    Console.WriteLine("int");
                    interpreter.Interpret();
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
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Unknown Error: {ex.Message}");
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
}

