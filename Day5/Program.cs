using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Day5.SeedRange;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 5: If You Give A Seed A Fertilizer");
            Console.WriteLine();

            Part1("test");
            Part1("input");
            Part2("test");
            Part2("input");
        }

        public static void Part1(string input)
        {

            Almanac almanac = new Almanac(input);

            Dictionary<long, long> seedLocations = almanac.SeedsToLocations();
            long location = seedLocations.Values.OrderBy(v => v).First();

            Console.WriteLine($"Smallest location {location}");
        }

        public static void Part2(string input)
        {
            AlmanacPart2 almanac = new AlmanacPart2(input);

            long smallest = almanac.SeedsToSmallestLocation();

            Console.WriteLine($"Smallest location {smallest}");
        }
    }

    public class AlmanacPart2 : Almanac
    {
        List<SeedRange> seeds = new List<SeedRange>();
        public AlmanacPart2(string input) : base(input)
        {
            AlmanacMap current = null;
            foreach (string line in File.ReadAllLines(input))
            {
                if (!string.IsNullOrEmpty(line))
                {
                    // Parse the seeds line
                    if (line.StartsWith("seeds:"))
                    {
                        seeds = ParseSeeds(line);
                    }
                    // Parse a new map line
                    else if (line.Trim().EndsWith("map:"))
                    {
                        if (current != null)
                        {
                            Maps.Add(current);
                        }
                        current = new AlmanacMap(line);
                    }
                    // Add a range to the current mapping
                    else
                    {
                        current.AddRange(line);
                    }
                }
            }

            // Add the last mapping
            if (current != null)
            {
                Maps.Add(current);
            }
        }

        public List<SeedRange> ParseSeeds(string line)
        {
            List<SeedRange> seeds = new List<SeedRange>();
            List<long> seedLine = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select<string, long>(s => long.Parse(s)).ToList();
            for (int i = 0; i < seedLine.Count; i += 2)
            {
                if (i + 1 >= seedLine.Count)
                    throw new Exception("Unexpected seed, not enough entries");

                seeds.Add(new SeedRange(seedLine[i], seedLine[i + 1]));
            }
            return seeds;
        }

        /// <summary>
        /// Finds the smallest mapped location
        /// </summary>
        /// <returns></returns>
        public long SeedsToSmallestLocation()
        {
            var seedLocations = new ConcurrentBag<long>();
            // For each through building up the mapping from seed to location then build up a dictionary
            Console.WriteLine($"Processing {seeds.Count}");
            Parallel.ForEach(seeds, seed =>
            {
                Console.WriteLine($"Seed starting {seed.Start}");
                long location = long.MaxValue;
                for (long i = seed.Start; i < seed.Start + seed.Length; i++)
                {
                    location = Math.Min(SeedToLocation(i), location);
                }
                seedLocations.Add(location);
                Console.WriteLine($"Seed finished {seed.Start}");
            });

            return seedLocations.OrderBy(l => l).First();
        }
    }

    public class Almanac
    {
        public List<long> Seeds { get; internal set; } = new List<long>();
        public List<AlmanacMap> Maps { get; private set; } = new List<AlmanacMap>();

        public Almanac(string input)
        {
            AlmanacMap current = null;
            foreach (string line in File.ReadAllLines(input))
            {
                if (!string.IsNullOrEmpty(line))
                {
                    // Parse the seeds line
                    if(line.StartsWith("seeds:"))
                    {
                        Seeds = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select<string, long>(s => long.Parse(s)).ToList();
                    }
                    // Parse a new map line
                    else if(line.Trim().EndsWith("map:"))
                    {
                        if (current != null)
                        {
                            Maps.Add(current);
                        }
                        current = new AlmanacMap(line);
                    }
                    // Add a range to the current mapping
                    else
                    {
                        current.AddRange(line);
                    }
                }
            }

            // Add the last mapping
            if (current != null)
            {
                Maps.Add(current);
            }
        }

        /// <summary>
        /// Get a location for a given seed
        /// </summary>
        /// <param name="seed">The seed</param>
        /// <returns>Mapped location</returns>
        public long SeedToLocation(long seed)
        {
            string current = "seed";
            long mappedValue = seed;
            foreach (AlmanacMap map in Maps)
            {
                if (!map.Source.Equals(current))
                    throw new Exception($"Unexpected source '{current}' does not equal '{map.Source}'");
                current = map.Destination;

                mappedValue = map.MapSource(mappedValue);

                if (map.Destination.Equals("location"))
                    return mappedValue;
            }

            throw new Exception($"Could not find Location for seed '{seed}'");
        }

        /// <summary>
        /// Maps seeds to locations
        /// </summary>
        /// <returns></returns>
        public Dictionary<long, long> SeedsToLocations()
        {
            Dictionary<long, long> mapping = new Dictionary<long, long>();
            foreach(long seed in Seeds)
            {
                mapping.Add(seed, SeedToLocation(seed));
            }

            return mapping;
        }
    }

    public class AlmanacMap
    {
        public string Source { get; private set; }
        public string Destination { get; private set; }

        List<MapRange> ranges = new List<MapRange>();

        public AlmanacMap(string mappingStr)
        {
            string[] parts = mappingStr.ToLower().Replace("map:", "").TrimEnd().Split('-');
            if (parts.Count() != 3)
                throw new Exception($"Unexpected mapping '{mappingStr}'");

            Source = parts[0];
            Destination = parts[2];
        }

        /// <summary>
        /// Maps a Source value to a Destination
        /// </summary>
        /// <param name="source"source value></param>
        /// <returns>destination value</returns>
        public long MapSource(long source)
        {
            MapRange range = ranges.FirstOrDefault(r => r.WithinRange(source));
            if (range != null)
                return range.MapSource(source);

            return source;
        }

        /// <summary>
        /// Add a range, the range is defined '5 10 2' where the first number is
        /// the source value, second is the destination and third is the length
        /// of the range
        /// </summary>
        /// <param name="range">Range string</param>
        public void AddRange(string range)
        {
            string[] parts = range.Split();
            long source = long.Parse(parts[1]);
            long destination = long.Parse(parts[0]);
            long length = long.Parse(parts[2]);

            // add the source -> destination mapping
            ranges.Add(new MapRange(source, destination, length));
        }
    }

    public class SeedRange
    {
        public long Start { get; private set; }
        public long Length { get; private set; }

        public SeedRange(long start, long length)
        {
            Start = start;
            Length = length;
        }
    }

    public class MapRange
    {
        public long Source { get; private set; }
        public long Destination { get; private set; }
        public long Length { get; private set; }

        public MapRange(long source, long destination, long length)
        {
            Source = source;
            Destination = destination;
            Length = length;
        }

        public bool WithinRange(long value)
        {
            if (value >= Source && value <= Source + Length)
                return true;

            return false;
        }

        /// <summary>
        /// Map Source to a Destination
        /// </summary>
        /// <param name="source">Source value</param>
        /// <returns>Destination</returns>
        public long MapSource(long source)
        {
            if (WithinRange(source))
                return Destination + (source - Source);
            return source;
        }
    }
}
