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
            Interpreter.Interpreter interpreter = new Interpreter.Interpreter(new Parser(new Lexer(content)));
            interpreter.Prepare();
            //interpreter.Interpret();
            stopwatch.Stop();
            Console.WriteLine($"Finished in {stopwatch.ElapsedMilliseconds}ms");
            //string json = Newtonsoft.Json.JsonConvert.SerializeObject(node, Newtonsoft.Json.Formatting.Indented);
            //File.WriteAllText("output.txt", json);
            Console.ReadLine();

        }
    }
}

