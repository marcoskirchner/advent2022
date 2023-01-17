using System.Diagnostics;

class Input15
{
    struct Reading
    {
        public int SensorX;
        public int SensorY;
        public int BeaconX;
        public int BeaconY;
        public int Distance;
    }

    internal static void Run()
    {
        var lines = File.ReadAllLines("input15.txt");
        var readings = ReadInput(lines);
        var rowToCheck = 10;
        var maxCoords = 20;
        if (readings[0].SensorX > 100)
        {
            rowToCheck = 2_000_000;
            maxCoords = 4_000_000;
        }
        RunPart1(readings, rowToCheck);
        RunPart2(readings, maxCoords);
    }

    private static List<Reading> ReadInput(string[] lines)
    {
        var readings = new List<Reading>();
        foreach (var line in lines)
        {
            Reading r = new();

            var p1 = line.IndexOf('=') + 1;
            var p2 = line.IndexOf(',', p1);
            r.SensorX = int.Parse(line[p1..p2]);

            p1 = line.IndexOf('=', p2) + 1;
            p2 = line.IndexOf(':', p1);
            r.SensorY = int.Parse(line[p1..p2]);

            p1 = line.IndexOf('=', p2) + 1;
            p2 = line.IndexOf(',', p1);
            r.BeaconX = int.Parse(line[p1..p2]);

            p1 = line.IndexOf('=', p2) + 1;
            r.BeaconY = int.Parse(line[p1..]);

            r.Distance = Math.Abs(r.SensorX - r.BeaconX) + Math.Abs(r.SensorY - r.BeaconY);
            readings.Add(r);
        }
        return readings;
    }

    private static void RunPart1(List<Reading> readings, int rowToCheck)
    {
        var coveredRanges = new List<(int start, int end)>();
        foreach (var reading in readings)
        {
            var spare = reading.Distance - Math.Abs(rowToCheck - reading.SensorY);
            if (spare > 0)
            {
                coveredRanges.Add((reading.SensorX - spare, reading.SensorX + spare));
            }
        }

        coveredRanges.Sort();
        var coveredPoses = 0;
        var lastEnd = int.MinValue;
        foreach (var range in coveredRanges)
        {
            var start = range.start;
            if (start <= lastEnd)
            {
                start = lastEnd + 1;
            }
            if (range.end > lastEnd)
            {
                lastEnd = range.end;
            }

            coveredPoses += lastEnd - start + 1;
        }
        var beaconsOnRow = readings.Where(r => r.BeaconY == rowToCheck).DistinctBy(r => r.BeaconX).Count();
        System.Console.WriteLine(coveredPoses - beaconsOnRow);
    }

    private static void RunPart2(List<Reading> readings, int maxCoords)
    {
        for (int rowToCheck = 0; rowToCheck <= maxCoords; rowToCheck++)
        {
            var coveredRanges = new List<(int start, int end)>();
            foreach (var reading in readings)
            {
                var spare = reading.Distance - Math.Abs(rowToCheck - reading.SensorY);
                if (spare > 0)
                {
                    coveredRanges.Add((reading.SensorX - spare, reading.SensorX + spare));
                }
            }

            coveredRanges.Sort();
            var minUncovered = 0;
            foreach (var range in coveredRanges)
            {
                if (range.start <= minUncovered && range.end >= minUncovered)
                {
                    minUncovered = range.end + 1;
                }
            }
            if (minUncovered <= maxCoords)
            {
                System.Console.WriteLine(minUncovered * 4_000_000L + rowToCheck);
                break;
            }
        }

    }
}
