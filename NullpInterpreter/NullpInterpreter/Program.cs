using NullPInterpreter.Interpreter;
using NullPInterpreter.Interpreter.AST;
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
            stopwatch.Start();
            Parser parser = new Parser(new Lexer(content));

            ASTNode node = parser.Parse();

            stopwatch.Stop();
            Console.WriteLine($"Finished in {stopwatch.ElapsedMilliseconds}ms");
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(node, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("output.txt", json);
            Console.ReadLine();

        }
    }
}

