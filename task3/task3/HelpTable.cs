using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;

namespace task3
{
    public class HelpTable
    {
        public void GenerateTable(string[] args)
        {
            GameRule gameRule = new GameRule();
            var table = new ConsoleTable("v You\\PC >");
            table.AddColumn(args);

            for (int i = 0; i < args.Length; i++)
            {
                var row = new List<string> { args[i] };
                for (int j = 0; j < args.Length; j++)
                {
                    string winner = gameRule.GetWinner(args, i, j);
                    row.Add(winner);
                }
                table.AddRow(row.ToArray());
            }
            table.Write();
        }
    }
}
