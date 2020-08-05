using Engine.Processing;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceChecks
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<TimeSpan> func = () => TestICP(ReadFromString(Properties.Resources.bunny2), ReadFromString(Properties.Resources.bunny4));
            
            CheckPerformance(3, func);
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        static TimeSpan RunFunction<T>(Func<T> func)
        {
            GC.Collect();
            var sw = Stopwatch.StartNew();
            func();
            sw.Stop();
            GC.Collect();
            return sw.Elapsed;
        }

        static List<Vector3> ReadFromString(string str)
        {
            var result = new List<Vector3>();
            var lines = str.Split('\n');
            foreach (var line in lines)
            {
                var coords = line.Split(' ').Select(v => float.TryParse(v, out float f) ? f : 0).ToArray();
                if (coords.Length == 3)
                {
                    result.Add(new Vector3(coords[0], coords[1], coords[2]));
                }
            }
            return result;
        }

        static List<Vector3> ReadFromFile(string file)
        {
            var result = new List<Vector3>();
            using (var fs = new StreamReader(file))
            {
                while (fs.Peek() >= 0)
                {
                    var line = fs.ReadLine();
                    var coords = line.Split(' ').Select(v=>float.TryParse(v, out float f)? f : 0).ToArray();
                    if (coords.Length == 3)
                    {
                        result.Add(new Vector3(coords[0], coords[1], coords[2]));
                    }
                    
                }
                fs.Close();
            }
            return result;
        }


        static TimeSpan TestICP(List<Vector3> v1, List<Vector3> v2)
        {
            var icp = new ICP(v1, v2);
            return RunFunction(() => icp.Align(50));
        }

        static void CheckPerformance(int count, Func<TimeSpan> test)
        {
            Console.WriteLine("Checking Function " + test.Method.Name);
            var AverageTime = Enumerable.Range(0, count).Select(_ => test()).Average(t=>t.TotalMilliseconds);

            Console.WriteLine("Average Time in ms to Compute Function " + AverageTime);
        }
    }
}
