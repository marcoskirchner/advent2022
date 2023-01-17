class Input1
{
    internal static void Run()
    {
        var lines = File.ReadAllLines("input1.txt");
        
        var top_calories = new List<int>();
        var sum = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            var l = lines[i];
            if (l.Length > 0)
            {
                sum += int.Parse(l);
            }
            else
            {
                top_calories.Add(sum);
                sum = 0;
            }
        }

        top_calories.Sort();
        top_calories.Reverse();
        System.Console.WriteLine(top_calories[0]);
        System.Console.WriteLine(top_calories.Take(3).Sum());
    }
}
