using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day3
{
    public class Part2
    {
        private string file;
        List<Gear> gears = new List<Gear>();

        public Part2(string file)
        {
            this.file = file;
        }

        public void Run()
        {
            var data = LoadData(file);

            // Extract gears
            for (int row = 0; row < data.Count; row++)
            {
                for (int col = 0; col < data[row].Count; col++)
                {
                    if (data[row][col] == '*')
                        gears.Add(new Gear(row, col));
                }
            }

            // Scan for numbers, add any numbers that are adjacent to a gear
            for(int row = 0; row < data.Count; row++)
            {
                ScanRowForNumbers(data, row);
            }

            Console.WriteLine($"Sum of gear ratios: {gears.Sum(g => g.Ratio)}");
        }

        /// <summary>
        /// Helper method to load data into a matrix
        /// </summary>
        /// <param name="input">Input filename</param>
        /// <returns>Matrix</returns>
        public static List<List<char>> LoadData(string input)
        {
            List<List<char>> data = new List<List<char>>();
            foreach (string line in File.ReadAllLines(input))
            {
                if (!string.IsNullOrEmpty(line))
                {
                    List<char> row = new List<char>();
                    foreach (char c in line)
                    {
                        row.Add(c);
                    }

                    if (data.Count > 0 && data.First().Count != row.Count)
                        throw new Exception($"Unexpected data, row is {(row.Count < data.Count ? "less" : "greater")} than the first row");

                    data.Add(row);
                }
            }

            return data;
        }

        public void ScanRowForNumbers(List<List<char>> data, int row)
        {
            if (row < data.Count)
            {
                string number = string.Empty;
                int startCol = 0;
                int endCol = 0;
                for (int c = 0; c < data[row].Count; c++)
                {
                    char current = data[row][c];
                    // Found a number
                    if (char.IsDigit(current))
                    {
                        // If this is the start of a number, remember the
                        // starting col
                        if (number.Length == 0)
                        {
                            startCol = c;
                        }
                        number += current;
                        endCol = c;
                    }

                    // Check for the end of a number, current char is not a
                    // digit or we've reached the end of the row
                    if (number.Length > 0 && (!char.IsDigit(current) || c == data[row].Count - 1))
                    {
                        // Check for any adjacent gears
                        foreach (Gear adjacent in gears.Where(g => IsAdjacent(g, row, startCol, endCol)))
                        {
                            adjacent.AddNumber(int.Parse(number));
                        }
                        number = string.Empty;
                    }
                }
            }
        }

        public bool IsAdjacent(Gear gear, int row, int startCol, int endCol)
        {
            return ((gear.Row >= (row - 1) && gear.Row <= (row + 1)) && (gear.Col >= (startCol - 1) && gear.Col <= (endCol + 1)));
        }
    }

    public class Gear
    {
        public Gear(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public int Row { get; private set; }
        public int Col { get; private set; }

        private List<int> numbers = new List<int>();

        public int Ratio
        {
            get
            {
                if (numbers.Count == 2)
                    return numbers[0] * numbers[1];
                return 0;
            }
        }

        public void AddNumber(int number)
        {
            numbers.Add(number);
        }
    }
}
