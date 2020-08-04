using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using System.IO;

class Loop {
	static void Main(string[] args) {
		int ms = 1000;
		string cmd = "";

		if(args.Length < 1) {
			Console.WriteLine("USAGE: loop [seconds] cmd");
			return;
		}

		double n = 0;
		// check if first arg is the time
		if(double.TryParse(args[0], out n)) {

			if (n <= 0) {
				Console.WriteLine("Seconds should ne under 0");
				return;
			}
			// remove time from array
			args[0] = null;
			ms = Convert.ToInt32(n * 1000);
		}
		for(int i = 0; i < args.Length; i++) {
			cmd = String.Join(" ", args);
		}

		try {
			while(true) {
				using (Process p = new Process()) {
					p.StartInfo.UseShellExecute = false;
					p.StartInfo.FileName = "cmd.exe";
					p.StartInfo.Arguments = "/C " + cmd;
					p.StartInfo.RedirectStandardOutput = true;
					p.StartInfo.RedirectStandardError = true;
					p.Start();
					Console.WriteLine(p.StandardOutput.ReadToEnd());

					// capture error and break loop
					StreamReader errorStream = p.StandardError;
					string error = errorStream.ReadLine();
					if(!string.IsNullOrEmpty( error )) {
						Console.WriteLine(error);
						p.Close();
						return;
					};

					p.WaitForExit();
				}
				Thread.Sleep(ms);
				
			}
		} catch (Exception e) {
			Console.WriteLine(e.Message);
			System.Environment.Exit(-1);
		}

	}
}
