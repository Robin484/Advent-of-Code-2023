using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 3: Gear Ratios");
            Console.WriteLine();

            Part1("test");
            Part1("input");
            new Part2("test").Run();
            new Part2("input").Run();
        }

        public static void Part1(string input)
        {

            var data = LoadData(input);
            //PrintData(data);

            List<int> numbers = new List<int>();
            for(int row=0; row < data.Count; row++)
            {
                numbers.AddRange(ScanRow(data, row));
            }

            Console.WriteLine($"Sum {numbers.Sum()}");
        }

        public static List<int> ScanRow(List<List<char>> data, int row)
        {
            List<int> numbers = new List<int>();

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
                        // Check for any adjacent symbols
                        if(CheckForAdjacentSymbols(data, row, startCol, endCol))
                        {
                            numbers.Add(int.Parse(number));
                        }
                        number = string.Empty;
                    }
                }
            }
            return numbers;
        }

        public static bool CheckForAdjacentSymbols(List<List<char>> data, int row, int startCol, int endCol)
        {
            // Check for any sumbols in row above
            if (SubRow(data, row - 1, startCol - 1, endCol + 1).Any(c => IsSymbol(c)))
                return true;

            // Check for any sumbols in row below
            if (SubRow(data, row + 1, startCol - 1, endCol + 1).Any(c => IsSymbol(c)))
                return true;

            // Check character to the left and right
            return IsSymbol(GetData(data, row, startCol - 1)) || IsSymbol(GetData(data, row, endCol + 1));
        }

        public static bool IsSymbol(char? c)
        {
            if (!c.HasValue)
                return false;

            return (!char.IsDigit(c.Value) && c.Value != '.');
        }

        /// <summary>
        /// Gets a sub select of a row
        /// </summary>
        /// <param name="data">teh data grid</param>
        /// <param name="row">the row</param>
        /// <param name="startCol">start column</param>
        /// <param name="endCol">end column</param>
        /// <returns>Sub selection</returns>
        public static List<char> SubRow(List<List<char>> data, int row, int startCol, int endCol)
        {
            // Check for out of range row
            if (row < 0 || row >= data.Count())
                return new List<char>();

            startCol = Math.Clamp(startCol, 0, data[row].Count - 1);
            endCol = Math.Clamp(endCol, 0, data[row].Count - 1);

            // We might have been able to use GetRange here but to d
            return data[row].GetRange(startCol, (endCol - startCol + 1));
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

        public static char? GetData(List<List<char>> data, int row, int col)
        {
            if (row >= 0 && row < data.Count && col >= 0 && col < data[row].Count)
                return data[row][col];

            return null;
        }

        public static void PrintData(List<List<char>> data)
        {
            for(int row = 0; row < data.Count; row++)
            {
                for (int col = 0; col < data[row].Count; col++)
                {
                    Console.Write(data[row][col]);
                }
                Console.WriteLine();
            }
        }
    }
}
