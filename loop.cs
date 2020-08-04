using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using System.IO;

class Loop
{

    static void printHelp(string additional = "")
    {
        Console.WriteLine("{0}\nUSAGE: loop [seconds] cmd", additional);
    }
    static void Main(string[] args)
    {

        // create List to work better with arguments
        List<string> argsList = new List<string>(args);

        int ms = 1000;
        string cmd = "";

        if (argsList.Count < 1)
        {
            printHelp();
            return;
        }

        double n = 0;
        // check if first arg is the time
        if (double.TryParse(argsList[0], out n))
        {

            if (n <= 0)
            {
                Console.WriteLine("Seconds should ne under 0");
                return;
            }
            // remove time from array
            argsList.RemoveAt(0);
            ms = Convert.ToInt32(n * 1000);
        }

        // join arguments
        for (int i = 0; i < argsList.Count; i++)
        {
            cmd = String.Join(" ", argsList);
        }

        // check if command is empty
        if (String.IsNullOrEmpty(cmd))
        {
            printHelp("No Command");
            return;
        }

        try
        {
            while (true)
            {
                using (Process p = new Process())
                {
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
                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine(error);
                        p.Close();
                        return;
                    };

                    p.WaitForExit();
                }
                Thread.Sleep(ms);

            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            System.Environment.Exit(-1);
        }

    }
}
