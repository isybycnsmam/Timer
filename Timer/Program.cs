using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

namespace Timer {
	class Program {
		public class WorkEvent {
			public TimeSpan Elapsed { get; set; }
			public DateTime Start = DateTime.Now;

			public override string ToString() => $"Start: {Start,-24} Elapsed: {Elapsed.ToString(@"hh\:mm\:ss")}\n";
		}

		public static int GeneralMenu() {
			var items = new[] { "Start", "View last 10", "Quit" };

			for (int i = 0; i < items.Length; i++)
				Console.WriteLine($"{i + 1}. {items[i]}");

			int choice;
			if (int.TryParse(Console.ReadLine(), out choice) || choice < 1 || items.Length > 4)
				return GeneralMenu();
			else
				return choice;
		}
		static void Main(string[] args) {
			for (; ; ) {
				switch (GeneralMenu()) {
					case 1:
						Start();
						break;
					case 2:
						ViewLastTen();
						break;
					case 3://quit
						return;
				}

				Console.Title = "Press 'c' to continue";
				while (Console.ReadKey().KeyChar != 'c')
					;

				Console.Clear();
			}
		}

		public static void Start() {
			var work = new WorkEvent();

			var stopwatch = new Stopwatch();
			stopwatch.Start();

			new Thread(() => {
				while (stopwatch.IsRunning) {
					Console.Title = stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
					Thread.Sleep(1000);
				}
			}).Start();

			for (; ; ) {
				Console.Clear();
				Console.Write("To stop please type 'stop': ");
				if (Console.ReadLine() == "stop")
					break;
			}
			stopwatch.Stop();
			work.Elapsed = stopwatch.Elapsed;

			Save(work);
		}
		public static void Save(WorkEvent work) {
			Console.WriteLine();

			try {
				File.AppendAllText("work.txt", work.ToString());
				Console.WriteLine("Saved!");
			}
			catch {
				Console.WriteLine("Failed to save");
			}

			Console.WriteLine(work.ToString());
		}
		public static void ViewLastTen() {
			Console.Clear();
			if (!File.Exists("work.txt")) {
				Console.WriteLine("File does not exists!");
				return;
			}

			var lastWorks = File.ReadAllLines("work.txt").Reverse().Take(10).ToList();
			for (int i = 0; i < lastWorks.Count; i++)
				Console.WriteLine($"{i + 1}. {lastWorks[i]}");
		}

	}
}