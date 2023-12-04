using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 1: Trebuchet?!");
            Console.WriteLine();

            Part1();
            Part2("test2");
            Part2("input");
        }

        public static void Part1()
        {
            List<int> calibrationValue = new List<int>();

            Regex regex = new Regex(@"\d");
            foreach (string line in File.ReadAllLines("input"))
            {

                // Each empty line is a new elf
                if (!string.IsNullOrEmpty(line))
                {
                    var matches = regex.Matches(line);
                    if(matches.Any())
                    {
                        calibrationValue.Add(int.Parse($"{matches.First()}{matches.Last()}"));
                    }
                }
            }

            Console.WriteLine($"Sum of all calibration values: {calibrationValue.Sum()}");
        }

        public static void Part2(string input)
        {
            List<int> calibrationValue = new List<int>();

            Regex regex = new Regex(@"(one)|(two)|(three)|(four)|(five)|(six)|(seven)|(eight)|(nine)|\d");
            foreach (string line in File.ReadAllLines(input))
            {

                // Each empty line is a new elf
                if (!string.IsNullOrEmpty(line))
                {
                    var matches = regex.Matches(line);
                    if (matches.Any())
                    {
                        calibrationValue.Add(int.Parse(ConvertToNumbers($"{matches.First()}{matches.Last()}")));
                    }
                }
            }

            Console.WriteLine($"Sum of all calibration values: {calibrationValue.Sum()}");
        }

        public static string ConvertToNumbers(string line)
        {
            return line.Replace("one",   "1")
                       .Replace("two",   "2")
                       .Replace("three", "3")
                       .Replace("four",  "4")
                       .Replace("five",  "5")
                       .Replace("six",   "6")
                       .Replace("seven", "7")
                       .Replace("eight", "8")
                       .Replace("nine",  "9");
        }
    }
}
