using System.Diagnostics;

class Input16
{
    class Valve
    {
        public int Index;
        public string Name;
        public int Rate;
        public int BitPattern;
        public string[] TunnelsToName;
        public int[] TunnelsToIndex;
    }

    internal static void Run()
    {
        var lines = File.ReadAllLines("input16.txt");
        var input = ReadInput(lines);
        RunPart1(input);
        RunPart2(input);
    }

    private static List<Valve> ReadInput(string[] lines)
    {
        var valves = new List<Valve>();
        var bitPattern = 1;
        var reverseMap = new Dictionary<string, int>();
        var reverseIndex = 0;
        foreach (var line in lines)
        {
            Valve v = new();
            valves.Add(v);

            v.Index = reverseIndex;
            v.Name = line[6..8];
            var pos = line.IndexOf(';', 22);
            v.Rate = int.Parse(line[23..pos]);
            if (v.Rate > 0)
            {
                v.BitPattern = bitPattern;
                bitPattern <<= 1;
            }
            pos = line.IndexOf("valve", pos);
            pos = line.IndexOf(' ', pos) + 1;
            v.TunnelsToName = line[pos..].Split(", ");

            reverseMap.Add(v.Name, reverseIndex);
            reverseIndex++;
        }
        foreach (var valve in valves)
        {
            valve.TunnelsToIndex = new int[valve.TunnelsToName.Length];
            for (int i = 0; i < valve.TunnelsToName.Length; i++)
            {
                valve.TunnelsToIndex[i] = reverseMap[valve.TunnelsToName[i]];
            }
        }
        return valves;
    }

    private static byte[,] CalculateDistances(List<Valve> valves)
    {
        int count = valves.Count;
        var distances = new byte[count, count];
        for (int i = 0; i < distances.GetLength(0); i++)
        {
            for (int j = 0; j < distances.GetLength(1); j++)
            {
                if (i == j)
                {
                    distances[i, j] = 0;
                }
                else
                {
                    distances[i, j] = 255;
                }
            }
        }

        for (int i = 0; i < count; i++)
        {
            var valve = valves[i];
            for (int j = 0; j < valve.TunnelsToIndex.Length; j++)
            {
                distances[i, valve.TunnelsToIndex[j]] = 1;
            }
        }

        for (int k = 0; k < count; k++)
        {
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (distances[i, j] > distances[i, k] + distances[k, j])
                    {
                        distances[i, j] = (byte)(distances[i, k] + distances[k, j]);
                    }
                }
            }
        }

        return distances;
    }

    private static void RunPart1(List<Valve> valves)
    {
        var distances = CalculateDistances(valves);
        var valvesToOpen = valves.Where(v => v.Rate > 0).ToArray();
        var startPos = valves.FindIndex(v => v.Name == "AA");
        var pressureRelased = CalculateMaxPressureRelease(startPos, 0, 30);
        System.Console.WriteLine(pressureRelased);

        int CalculateMaxPressureRelease(int currentPos, int openValves, int timeLeft)
        {
            var maxSoFar = 0;
            for (int i = 0; i < valvesToOpen.Length; i++)
            {
                var valve = valvesToOpen[i];
                if ((openValves & valve.BitPattern) == 0)
                {
                    var timeAfterThisValve = timeLeft - 1 - distances[currentPos, valve.Index];
                    if (timeAfterThisValve > 0)
                    {
                        var released = timeAfterThisValve * valve.Rate +
                            CalculateMaxPressureRelease(valve.Index,
                                openValves | valve.BitPattern,
                                timeAfterThisValve);
                        if (released > maxSoFar)
                        {
                            maxSoFar = released;
                        }
                    }

                }
            }
            return maxSoFar;
        }
    }

    private static void RunPart2(List<Valve> valves)
    {
        var distances = CalculateDistances(valves);
        var valvesToOpen = valves.Where(v => v.Rate > 0).ToArray();
        var startPos = valves.FindIndex(v => v.Name == "AA");
        var pressureRelased = CalculateMaxPressureRelease(startPos, startPos, 0, 26, 26);
        System.Console.WriteLine(pressureRelased);

        /*
         // esse metodo demorou quase 1,5h por conta do aumento exponencial do
         // espaco de pesquisa com o segundo for loop recursivo

        int CalculateMaxPressureRelease(int currentPosA, int currentPosB,
            int openValves, int timeLeftA, int timeleftB)
        {
            var maxSoFar = 0;

            for (int i = 0; i < valvesToOpen.Length; i++)
            {
                var valve = valvesToOpen[i];
                if ((openValves & valve.BitPattern) == 0)
                {
                    var timeAfterThisValve = timeLeftA - 1 - distances[currentPosA, valve.Index];
                    if (timeAfterThisValve > 0)
                    {
                        var released = timeAfterThisValve * valve.Rate +
                            CalculateMaxPressureRelease(valve.Index, currentPosB,
                                openValves | valve.BitPattern,
                                timeAfterThisValve, timeleftB);
                        if (released > maxSoFar)
                        {
                            maxSoFar = released;
                        }
                    }

                }
            }

            for (int i = 0; i < valvesToOpen.Length; i++)
            {
                var valve = valvesToOpen[i];
                if ((openValves & valve.BitPattern) == 0)
                {
                    var timeAfterThisValve = timeleftB - 1 - distances[currentPosB, valve.Index];
                    if (timeAfterThisValve > 0)
                    {
                        var released = timeAfterThisValve * valve.Rate +
                            CalculateMaxPressureRelease(currentPosA, valve.Index,
                                openValves | valve.BitPattern,
                                timeLeftA, timeAfterThisValve);
                        if (released > maxSoFar)
                        {
                            maxSoFar = released;
                        }
                    }

                }
            }

            return maxSoFar;
        }
        */

        int CalculateMaxPressureRelease(int currentPosA, int currentPosB,
            int openValves, int timeLeftA, int timeLeftB)
        {
            var maxSoFar = 0;

            for (int i = 0; i < valvesToOpen.Length; i++)
            {
                var valve = valvesToOpen[i];
                if ((openValves & valve.BitPattern) == 0)
                {
                    int timeAfterThisValve;
                    if (timeLeftA >= timeLeftB)
                        timeAfterThisValve = timeLeftA - 1 - distances[currentPosA, valve.Index];
                    else
                        timeAfterThisValve = timeLeftB - 1 - distances[currentPosB, valve.Index];

                    if (timeAfterThisValve > 0)
                    {
                        var released = timeAfterThisValve * valve.Rate;
                        if (timeLeftA >= timeLeftB)
                        {
                            released += CalculateMaxPressureRelease(valve.Index, currentPosB,
                                  openValves | valve.BitPattern,
                                  timeAfterThisValve, timeLeftB);
                        }
                        else
                        {
                            released += CalculateMaxPressureRelease(currentPosA, valve.Index,
                                openValves | valve.BitPattern,
                                timeLeftA, timeAfterThisValve);

                        }

                        if (released > maxSoFar)
                        {
                            maxSoFar = released;
                        }
                    }

                }
            }
            // }
            // else
            // {
            //     for (int i = 0; i < valvesToOpen.Length; i++)
            //     {
            //         var valve = valvesToOpen[i];
            //         if ((openValves & valve.BitPattern) == 0)
            //         {
            //             var timeAfterThisValve = timeleftB - 1 - distances[currentPosB, valve.Index];
            //             if (timeAfterThisValve > 0)
            //             {
            //                 var released = timeAfterThisValve * valve.Rate +
            //                     CalculateMaxPressureRelease(currentPosA, valve.Index,
            //                         openValves | valve.BitPattern,
            //                         timeLeftA, timeAfterThisValve);
            //                 if (released > maxSoFar)
            //                 {
            //                     maxSoFar = released;
            //                 }
            //             }

            //         }
            //     }
            // }

            return maxSoFar;
        }

    }
}
