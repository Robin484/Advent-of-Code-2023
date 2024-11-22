using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            foreach (string line in File.ReadAllLines(input))
            {
                if (!string.IsNullOrEmpty(line))
                {
                    var matches = ScanAndConvertNumbers(line);
                    if (matches.Any())
                    {
                        calibrationValue.Add(int.Parse($"{matches.First()}{matches.Last()}"));
                    }
                }
            }

            Console.WriteLine($"Sum of all calibration values: {calibrationValue.Sum()}");
        }

        public static string ConvertToNumbers(string line)
        {
            return line.Replace("zero",  "0")
                       .Replace("one",   "1")
                       .Replace("two",   "2")
                       .Replace("three", "3")
                       .Replace("four",  "4")
                       .Replace("five",  "5")
                       .Replace("six",   "6")
                       .Replace("seven", "7")
                       .Replace("eight", "8")
                       .Replace("nine",  "9");
        }

        public static List<int> ScanAndConvertNumbers(string line)
        {
            List<int> digits = new List<int>();
            Dictionary<string, int> numbers = new Dictionary<string, int>() {
                { "zoro", 0 },
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
                { "six", 6 },
                { "seven", 7 },
                { "eight", 8 },
                { "nine", 9 }
            };

            //line = line.ToLower();

            for (int i = 0; i < line.Length; i++)
            {
                // if the character is a digit add it
                if (char.IsDigit(line[i]))
                {
                    digits.Add(int.Parse(line[i].ToString()));
                }

                // Otherwise check if the character is a number (written as a word)
                var subString = line.Substring(i);
                foreach (string number in numbers.Keys)
                {
                    if (subString.StartsWith(number))
                    {
                        digits.Add(numbers[number]);
                    }
                }
            }
            return digits;
        }
    }
}