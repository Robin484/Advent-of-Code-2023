using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day2
{
    class Bag
    {
        int red = 0;
        public int Red {
            get { return red; }
            set
            {
                red = Math.Max(red, value);
            }
        }

        int blue = 0;
        public int Blue
        {
            get { return blue; }
            set
            {
                blue = Math.Max(blue, value);
            }
        }

        int green = 0;
        public int Green
        {
            get { return green; }
            set
            {
                green = Math.Max(green, value);
            }
        }

        public int Power
        {
            get { return Red * Green * Blue; }
        }

        public bool PossibleDraw(Bag bag)
        {
            if (Red <= bag.Red && Green <= bag.Green && Blue <= bag.Blue)
                return true;
            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 2: Cube Conundrum");
            Console.WriteLine();

            Bag match = new Bag() { Red = 12, Green = 13, Blue = 14 };
            Part1("test", match);
            Part1("input", match);
            Part2("test");
            Part2("input");
        }

        public static void Part1(string input, Bag match)
        {
            Regex reGame = new Regex(@"(Game) (\d+)");
            Regex reCube = new Regex(@"(\d+) ((red)|(green)|(blue))");
            List<int> games = new List<int>();

            foreach (string line in File.ReadAllLines(input))
            {

                // Each empty line
                if (!string.IsNullOrEmpty(line))
                {
                    // Split into [Game N:] [draws]
                    string[] parts = line.Split(':');
                    if(parts.Length == 2)
                    {
                        var gm = reGame.Matches(parts[0]);
                        int game = int.Parse(gm[0].Groups[2].Value);
                        Bag bag = new Bag();
                        foreach (string draw in parts[1].Split(';'))
                        {
                            foreach (string cubes in draw.Split(','))
                            {
                                var cube = reCube.Matches(cubes.Trim());
                                string colour = (string)cube[0].Groups[2].Value;
                                int count = int.Parse(cube[0].Groups[1].Value);
                                switch (colour)
                                {
                                    case "red":
                                        bag.Red = count;
                                        break;
                                    case "green":
                                        bag.Green = count;
                                        break;
                                    case "blue":
                                        bag.Blue = count;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        if (bag.PossibleDraw(match))
                            games.Add(game);
                    }
                }
            }

            Console.WriteLine($"Matched with {games.Count}, summed to {games.Sum()}");
        }

        public static void Part2(string input)
        {
            Regex reGame = new Regex(@"(Game) (\d+)");
            Regex reCube = new Regex(@"(\d+) ((red)|(green)|(blue))");
            int power = 0;

            foreach (string line in File.ReadAllLines(input))
            {

                // Each empty line
                if (!string.IsNullOrEmpty(line))
                {
                    // Split into [Game N:] [draws]
                    string[] parts = line.Split(':');
                    if (parts.Length == 2)
                    {
                        var gm = reGame.Matches(parts[0]);
                        int game = int.Parse(gm[0].Groups[2].Value);
                        Bag bag = new Bag();
                        foreach (string draw in parts[1].Split(';'))
                        {
                            foreach (string cubes in draw.Split(','))
                            {
                                var cube = reCube.Matches(cubes.Trim());
                                string colour = (string)cube[0].Groups[2].Value;
                                int count = int.Parse(cube[0].Groups[1].Value);
                                switch (colour)
                                {
                                    case "red":
                                        bag.Red = count;
                                        break;
                                    case "green":
                                        bag.Green = count;
                                        break;
                                    case "blue":
                                        bag.Blue = count;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        power += bag.Power;
                    }
                }
            }

            Console.WriteLine($"Sum power of {power}");
        }
    }
}
