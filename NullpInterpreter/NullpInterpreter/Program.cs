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
            Console.Title = "NULLP Interpreter";

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($" NULLP - Interpreter / {DateTime.Now.ToShortDateString()}");
                Console.WriteLine("");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                string content = File.ReadAllText(@"code-file.nullp");
                try
                {
                    Console.Write(" Lexer\t\t\t\t");
                    Lexer lexer = new Lexer(content);
                    WriteStatusToConsole(stopwatch);


                    Console.Write(" Parser\t\t\t\t");
                    Parser p = new Parser(lexer);
                    ASTNode rootNode = p.Parse();
                    WriteStatusToConsole(stopwatch);

                    Interpreter.Interpreter interpreter = new Interpreter.Interpreter(p);

                    Console.Write(" Sematic Analyser\t\t");
                    interpreter.SematicAnalysis(rootNode);
                    WriteStatusToConsole(stopwatch);

                    stopwatch.Stop();
                    Console.WriteLine("");
                    Console.WriteLine(" Starting Program - Good Luck Captain!");
                    Console.WriteLine("");
                    Console.ResetColor();
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

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine();
                Console.WriteLine(" Program Executed - Press Enter To Restart");
                Console.ReadLine();
            }

        }

        private static void WriteStatusToConsole(Stopwatch sw)
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("OK");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"] / ({sw.ElapsedMilliseconds}ms)");
            sw.Restart();
        }
    }
}

