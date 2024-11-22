using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 4: Scratchcards");
            Console.WriteLine();

            Part1("test");
            Part1("input");
            Part2("test");
            Part2("input");
        }

        public static void Part1(string input)
        {
            List<int> cards = new List<int>();
            foreach (string line in File.ReadAllLines(input))
            {

                // Each empty line
                if (!string.IsNullOrEmpty(line))
                {
                    string[] parts = line.Split(new char[] { ':', '|' });
                    if (parts.Length != 3)
                        continue;

                    List<int> winning = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(c => int.Parse(c)).ToList<int>();
                    List<int> numbers = parts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(c => int.Parse(c)).ToList<int>();
                    int matches = numbers.Count(c => winning.Contains(c));

                    if (matches > 0)
                    {
                        int points = 0;
                        for (int i = 0; i < matches; i++)
                        {
                            points = (points == 0 ? 1 : points * 2);
                        }
                        cards.Add(points);
                    }
                }
            }

            Console.WriteLine($"Sum {cards.Sum()}");
        }

        public static void Part2(string input)
        {
            Dictionary<int, Card> cards = new Dictionary<int, Card>();
            foreach (string line in File.ReadAllLines(input))
            {

                // Each empty line
                if (!string.IsNullOrEmpty(line))
                {
                    Card card = new Card(line);
                    cards[card.Id] = card;
                }
            }

            // Duplicate cards based on the matches
            foreach(Card card in cards.Values)
            {
                for(int n = 1; n <= card.Matches; n++)
                {
                    if(cards.ContainsKey(card.Id +n ))
                        cards[card.Id + n].Duplicate(card.Copies);
                }
            }

            Console.WriteLine($"Total Cards {cards.Values.Select(c => c.Copies).Sum()}");
        }
    }

    public class Card
    {
        public Card(string line)
        {
            string[] parts = line.Split(new char[] { ':', '|' });
            if (parts.Length == 3)
            {
                Id = int.Parse(parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);
                Winning = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(c => int.Parse(c)).ToList<int>();
                Numbers = parts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(c => int.Parse(c)).ToList<int>();
                Matches = Numbers.Count(c => Winning.Contains(c));
            }
        }

        public void Duplicate(int copies = 1)
        {
            Copies += copies;
        }

        public int Id { get; set; }
        public List<int> Winning { get; set; }
        public List<int> Numbers { get; set; }
        public int Matches { get; set; }
        public int Copies { get; private set; } = 1;
        public bool Processed { get; private set; } = false;
    }
}
