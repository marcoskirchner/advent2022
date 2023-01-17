﻿using System.Text.RegularExpressions;

internal class Input19
{
    const int NEW_ORE_ROBOT = 1 << 0;
    const int NEW_CLAY_ROBOT = 1 << 8;
    const int NEW_OBSIDIAN_ROBOT = 1 << 16;
    const int NEW_GEODE_ROBOT = 1 << 24;

    static int FromOre(byte value) => value << 0;
    static int FromClay(byte value) => value << 8;
    static int FromObsidian(byte value) => value << 16;
    static byte OreValue(int value) => (byte)(value >> 0);
    static byte ClayValue(int value) => (byte)(value >> 8);
    static byte ObsidianValue(int value) => (byte)(value >> 16);
    static byte GeodeValue(int value) => (byte)(value >> 24);

    sealed record Blueprint(
        int ID,
        int OreRobotCost,
        int ClayRobotCost,
        int ObsidianRobotCost,
        int GeodeRobotCost
    );

    internal static void Run()
    {
        var lines = File.ReadAllLines("..\\..\\..\\input19.txt");
        var input = ReadInput(lines).Take(2);
        RunPart1(input);
        RunPart2(input);
    }

    private static Blueprint[] ReadInput(string[] lines)
    {
        var r = new Regex("Blueprint (\\d+): Each ore robot costs (\\d+) ore. Each clay robot costs (\\d+) ore. Each obsidian robot costs (\\d+) ore and (\\d+) clay. Each geode robot costs (\\d+) ore and (\\d+) obsidian.");
        var input = new Blueprint[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            var m = r.Match(lines[i]).Groups.Values
                .Skip(1)
                .Select(v => byte.Parse(v.Value))
                .ToArray();
            input[i] = new(m[0],
                FromOre(m[1]),
                FromOre(m[2]),
                FromOre(m[3]) + FromClay(m[4]),
                FromOre(m[5]) + FromObsidian(m[6]));
        }
        return input;
    }

    /*
    1 - 9
    2 - 12

    33
    Run for 1.060.371 ms

    1 - 9
    2 - 0
    3 - 1
    4 - 9
    5 - 0
    6 - 2
    7 - 8
    8 - 1
    9 - 0
    10 - 0
    11 - 6
    12 - 2
    13 - 2
    14 - 0
    15 - 2
    16 - 0
    17 - 0
    18 - 13
    19 - 0
    20 - 7
    21 - 6
    22 - 0
    23 - 15
    24 - 1
    25 - 3
    26 - 0
    27 - 4
    28 - 13
    29 - 6
    30 - 10

    2160
    Run for 3.810.866 ms
    */

    private static void RunPart1(IEnumerable<Blueprint> input)
    {
        var sum = 0;
        Parallel.ForEach(input, new ParallelOptions() { MaxDegreeOfParallelism = 4 }, blueprint =>
        {
            var n = MaxNumberOfGeodes(blueprint, 24);
            sum += blueprint.ID * n;
            Console.WriteLine(blueprint.ID + " - " + n);
        });
        Console.WriteLine(sum);
    }

    private static void RunPart2(IEnumerable<Blueprint> input)
    {
        var product = 1;
        Parallel.ForEach(input.Take(3), new ParallelOptions() { MaxDegreeOfParallelism = 4 }, blueprint =>
        {
            var n = MaxNumberOfGeodes(blueprint, 32);
            product *= n;
            Console.WriteLine(blueprint.ID + " - " + n);
        });
        Console.WriteLine(product);
    }

    static int MaxNumberOfGeodes(Blueprint bp, int minutes)
    {
        var maxOre = new[]
        {
                OreValue(bp.OreRobotCost),
                OreValue(bp.ClayRobotCost),
                OreValue(bp.ObsidianRobotCost),
                OreValue(bp.GeodeRobotCost),
            }.Max();
        return MaxGeodes(minutes, 0, FromOre(1));

        int MaxGeodes(int minutesLeft, int resources, int robots)
        {
            int localMax;
            int maxSoFar = GeodeValue(resources);
            if (minutesLeft == 0)
                return maxSoFar;
            minutesLeft--;

            var newResourcesValue = resources + robots;

            if (OreValue(resources) >= OreValue(bp.OreRobotCost) && OreValue(robots) < maxOre)
            {
                localMax = MaxGeodes(minutesLeft,
                    newResourcesValue - bp.OreRobotCost,
                    robots + NEW_ORE_ROBOT);
                if (localMax > maxSoFar)
                    maxSoFar = localMax;
            }
            if (OreValue(resources) >= OreValue(bp.ClayRobotCost) && ClayValue(robots) < ClayValue(bp.ObsidianRobotCost))
            {
                localMax = MaxGeodes(minutesLeft,
                    newResourcesValue - bp.ClayRobotCost,
                    robots + NEW_CLAY_ROBOT);
                if (localMax > maxSoFar)
                    maxSoFar = localMax;
            }

            if (OreValue(resources) >= OreValue(bp.ObsidianRobotCost)
                && ClayValue(resources) >= ClayValue(bp.ObsidianRobotCost)
                && ObsidianValue(robots) < ObsidianValue(bp.GeodeRobotCost))
            {
                localMax = MaxGeodes(minutesLeft,
                    newResourcesValue - bp.ObsidianRobotCost,
                    robots + NEW_OBSIDIAN_ROBOT);
                if (localMax > maxSoFar)
                    maxSoFar = localMax;
            }
            if (OreValue(resources) >= OreValue(bp.GeodeRobotCost)
                && ObsidianValue(resources) >= ObsidianValue(bp.GeodeRobotCost))
            {
                localMax = MaxGeodes(minutesLeft,
                    newResourcesValue - bp.GeodeRobotCost,
                    robots + NEW_GEODE_ROBOT);
                if (localMax > maxSoFar)
                    maxSoFar = localMax;
            }

            localMax = MaxGeodes(minutesLeft, newResourcesValue, robots);
            if (localMax > maxSoFar)
                maxSoFar = localMax;

            return maxSoFar;
        }
    }
}
