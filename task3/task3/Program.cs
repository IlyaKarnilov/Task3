using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 3 || args.Length % 2 == 0)
        {
            Console.WriteLine("Error: Incorrect number of arguments.");
            Console.WriteLine("Usage: Program.exe arg1 arg2 arg3 ...");
            Console.WriteLine("Please provide an odd number of unique strings as arguments.");
            return;
        }
        else if (args.Distinct().Count() < args.Length)
        {
            Console.WriteLine("Error: Duplicate arguments found.");
            Console.WriteLine("Please provide a set of unique strings as arguments.");
            return;
        }

        Random random = new Random();
        var hmacKey = GenerateHmacKey();
        int computerMove = random.Next(0, args.Length);
        var compMoveVal = args[computerMove];
        var hmac = CalculateHmac(compMoveVal, hmacKey);
        Console.WriteLine($"HMAC: {hmac}");

        while(true){
            Console.WriteLine("Available moves: ");
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine($"{i + 1} - {args[i]}");
            }
            Console.WriteLine("0 - exit");
            Console.WriteLine("? - help");
            Console.WriteLine("Enter your move: ");
            string input = Console.ReadLine();
            if (input == "?")
            {
                var table = GenerateTable(args);
                Console.WriteLine("Table:");
                foreach (var row in table)
                {
                    Console.WriteLine(string.Join("\t", row));
                }
            }
            else if (input == "0")
            {
                break;
            }
            else
            {
                int move;
                if (int.TryParse(input, out move))
                {
                    move = move - 1;
                    if (move >= 0 && move <= args.Length)
                    {
                        string winner = GetWinner(args, move, computerMove);
                        Console.WriteLine($"Your move: {args[move]}");
                        Console.WriteLine($"Computer move: {args[computerMove]}");
                        Console.WriteLine(winner);
                        Console.WriteLine($"HMAC key: {hmacKey}");
                        Console.WriteLine("you can check the HMAC on the website: https://www.freeformatter.com/hmac-generator.html#before-output");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("incorrect input. Out of range");
                    }
                }
                else
                {
                    Console.WriteLine("incorrect input");
                }
            }
        }
        Console.ReadLine();
    }

    public static string GenerateHmacKey()
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] key = new byte[32];
            rng.GetBytes(key);
            string hexKey = BitConverter.ToString(key).Replace("-", "").ToLower();
            return hexKey;
        }
    }

    public static string CalculateHmac(string move, string key)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
        {
            byte[] bytes = Encoding.UTF8.GetBytes(move);
            byte[] hash = hmac.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }

    public static string GetWinner(string[] args, int userMove, int computerMove)
    {
        int n = args.Length;
        int range = n / 2;

        if (userMove == computerMove)
        {
            return "Draw";
        }
        else if (userMove > range)
        {
            int distance = userMove - range;
            if (computerMove < distance || computerMove > userMove)
            {
                return "You Win!";
            }
            else
            {
                return "You Lose!";
            }
        }
        else if ((userMove + 1) % n <= computerMove && computerMove <= (userMove + range) % n ||
                 (userMove + 1 - n) <= computerMove && computerMove <= (userMove + range - n))
        {
            return "You Win!";
        }
        else
        {
            return "You Lose!";
        }
    }

    public static List<List<string>> GenerateTable(string[] args)
    {
        var table = new List<List<string>>();
        int columnWidth = Math.Max(10, args.Max(s => s.Length) + 2);

        var header = new List<string> { "Moves".PadRight(columnWidth) };
        foreach (var move in args)
        {
            header.Add(move.PadRight(columnWidth));
        }
        table.Add(header);

        for (int i = 0; i < args.Length; i++)
        {
            var row = new List<string> { args[i].PadRight(columnWidth) };
            for (int j = 0; j < args.Length; j++)
            {
                string winner = GetWinner(args, i, j);
                row.Add(winner.PadRight(columnWidth));
            }
            table.Add(row);
        }
        return table;
    }
}
