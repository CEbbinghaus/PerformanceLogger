using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

/// <summary>
/// Author: Christoper-Robin Ebbinghaus, Josh Francis
/// Timer to log and Debug Performance 
/// </summary>
public class PerfTimer
{
	private class LappedWatch
	{
		Stopwatch watch = Stopwatch.StartNew ();
		// Time since the last "lap"
		long lastTime = 0;

		internal long Lap ()
		{
			var diff = watch.ElapsedMilliseconds - lastTime;
			lastTime = watch.ElapsedMilliseconds;
			return diff;
		}

		internal long Finalize ()
		{
			var total = watch.ElapsedMilliseconds;
			watch.Stop ();
			return total;
		}
	}

	
	private List<LappedWatch> stopwatches = new List<LappedWatch> ();
	StringBuilder log = new StringBuilder ();
	private int totalStopwatches => stopwatches.Count;
	private LappedWatch lastWatch => stopwatches[totalStopwatches - 1];
	string TabLevel => new String ('\t', totalStopwatches);

	public void StartTimer (string name)
	{
		log.Append ($"{TabLevel}{name}:\n");
		stopwatches.Add (new LappedWatch ());
	}

	public void StopTimer ()
	{
		if (totalStopwatches <= 0)
#if DEBUG
			throw new ArgumentNullException ("No Timer could be found. Did you call StartTimer first?");
#else
		return;
#endif

		var isLast = totalStopwatches == 1;

		var indenting = isLast ? "" : TabLevel;

		log.Append ($"{indenting}Total: {lastWatch.Finalize()}ms\n");

		stopwatches.RemoveAt (totalStopwatches - 1);

		if (isLast){
			Debug.WriteLine(log);
			Console.WriteLine (log);
		}else
			lastWatch.Lap ();
	}

	public void Log (string name)
	{
		log.Append ($"{TabLevel}{name}: {lastWatch.Lap()}ms\n");
	}

	public void Finish ()
	{
		while (totalStopwatches > 0)
			StopTimer ();
	}
}

public static class StaticTimer
{
	static PerfTimer instance = new PerfTimer();

	public static void StartTimer(string name)
	{
		instance.StartTimer(name);
	}

	public static void StopTimer()
	{
		instance.StopTimer();
	}

	public static void Log(string name)
	{
		instance.Log(name);
	}

	public static void Finish()
	{
		instance.Finish();
	}
}