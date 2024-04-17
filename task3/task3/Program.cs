using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using task3;

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
        HmacGenerate hmacGenerate = new HmacGenerate();
        var hmacKey = hmacGenerate.GenerateHmacKey();
        int computerMove = random.Next(0, args.Length);
        var compMoveVal = args[computerMove];
        var hmac = hmacGenerate.CalculateHmac(compMoveVal, hmacKey);
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
                HelpTable helpTable = new HelpTable();
                helpTable.GenerateTable(args);
                Console.WriteLine("Table:");
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
                        GameRule gameRule = new GameRule();
                        string winner = gameRule.GetWinner(args, move, computerMove);
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
}
