using System.Diagnostics;

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
            Lexer lexer = new Lexer(content);

            Token currentToken = lexer.GetNextToken();
            while (currentToken.Type != TokenType.EoF)
            {
                Console.WriteLine(currentToken.Value + $"\t[{currentToken.Type}]");
                currentToken = lexer.GetNextToken();
            }

            stopwatch.Stop();
            Console.WriteLine($"Finished in {stopwatch.ElapsedMilliseconds}ms");

            Console.ReadLine();

        }
    }
}

