using System.Diagnostics;
using VRP;
using VRP.Core.Common.Extractors;
using VRP.Core.CWSaving;
using VRP.Core.SimulateAnnealing;

var inputData = new ClassicTextDataExtractor()
    .Extract(new FileStream("./data/Golden/Golden_9.vrp", FileMode.Open, FileAccess.Read));

List<List<int>> result = new();
Stopwatch sw;

sw = Stopwatch.StartNew();
result = new CWSaving().Process(inputData, SimulateAnnealingParams.CreateDefault());
sw.Stop();
WriteHelper.PrintResult(inputData, result);
Console.WriteLine("Execution SA took: " + sw.ElapsedMilliseconds + "ms");
Console.WriteLine("Execution SA took: " + sw.Elapsed.Seconds + "s");

sw = Stopwatch.StartNew();
result = new HPSaving().Process(inputData, SimulateAnnealingParams.CreateDefault());
sw.Stop();
WriteHelper.PrintResult(inputData, result);
Console.WriteLine("Execution SA took: " + sw.ElapsedMilliseconds + "ms");
Console.WriteLine("Execution SA took: " + sw.Elapsed.Seconds + "s");

sw = Stopwatch.StartNew();
result = new SimulateAnnealing().Process(inputData, SimulateAnnealingParams.CreateDefault());
sw.Stop();
WriteHelper.PrintResult(inputData, result);
Console.WriteLine("Execution SA took: " + sw.ElapsedMilliseconds + "ms");
Console.WriteLine("Execution SA took: " + sw.Elapsed.Seconds + "s");

