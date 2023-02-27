using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Scanner
{
    class Program
    {
        static Mutex fileMutex = new Mutex();
        static StreamWriter file = null;
        public static void Usage()
        {
            string usageString = @"
Syntax for Scanner:
    scanner.exe hosts=[DNS name or IP Address, using comma to seperate computer IDs] ports=[each ports, reconfigured port] [timeout=] [outfile=]
Argument Keys:
    hosts   - Required. Comma separated list of hosts. This can be computer name or IP address.
    ports   - Required. Comma separated list of ports, or one of the following reconfigured port lists:
                admin - 135, 139, 445, 3389, 5985, 5986
                web - 21, 23, 25, 80, 443, 8080
                top20 - 21, 22, 23, 25, 53, 80, 110, 111, 135, 139, 143, 443, 445, 993, 995, 1723, 3306, 3389, 5900, 8080
                server-common - 7, 9, 13, 17, 19, 25, 42, 80, 88, 110, 111, 119, 135, 149, 389, 443, 445, 465, 563, 587, 636, 
                                808, 993, 995, 1433, 1688, 1801, 3268, 3269, 3387, 3388, 3389, 4044, 6516, 6881, 8391, 8443, 8530, 8531, 9389
    timeout - Optional. Length of time in milliseconds for scanner to wait for a response. EX: 5000 = 5 seconds.
                Note: Lowest value is 500 milliseconds which it will default to if no value is given.
    outfile - Optional. File to write results out to on disk. Writes to current folder if none provided. Slows scanning.
                If no file is specified, output will be written to the console.​
Example Usage:​
        PortScanner.exe hosts=127.0.0.1,google.com ports=21,22,23 timeout=5000
        PortScanner.exe hosts=127.0.0.1 ports=admin";
            Console.WriteLine(usageString);
        }

        static Dictionary<string, object> results = new Dictionary<string, object>();
        public static Dictionary<string, object> ArgParser(string[] args)
        {
            string[] keys = { "timeout", "hosts", "ports", "outfile", "infile" };
            string[] requiredKeys = { "hosts", "ports" };
            results.Add("timeout", 500);

            foreach (string arg in args)
            {
                string[] parts = arg.Split('=');
                if (parts.Length != 2)
                {
                    Console.WriteLine("[X] Invalid argument passed: {0}", arg);
                    Usage();
                    Environment.Exit(1);
                }
                if (!keys.Contains(parts[0]))
                {
                    Console.WriteLine("[X] Unknown argument passed: {0}", parts[0]);
                    continue;
                }
                switch (parts[0])
                {
                    case "hosts":
                        var hostArray = parts[1].Split(',');
                        var val = hostArray;
                        results.Add("hosts", val);
                        break;
                    case "timeout":
                        var val2 = parts[1];
                        if (Convert.ToInt32(val2) < 500)
                        {
                            Console.WriteLine("[-] Error: Timeout will be set to the minimum at 500 milliseconds.\n");
                        }
                        results.Remove("timeout");
                        results.Add("timeout", val2);
                        break;
                    case "outfile":
                        var val3 = parts[1];
                        results.Add("outfile", val3);
                        Console.WriteLine("[+] Results will be written out to " + val3 + ".\n");
                        break;
                    case "ports":
                        if (parts[1] == "admin")
                        {
                            Console.WriteLine("[+] The following ports will be scanned: 135, 139, 445, 3389, 5985, 5986. \n");
                            results["ports"] = new object[] { 135, 139, 445, 3389, 5985, 5986 };
                        }
                        else if (parts[1] == "web")
                        {
                            Console.WriteLine("[+] The following ports will be scanned: 21, 23, 25, 80, 443, 8080. \n");
                            results["ports"] = new object[] { 21, 23, 25, 80, 443, 8080 };
                        }
                        else if (parts[1] == "top20")
                        {
                            Console.WriteLine("[+] The following ports will be scanned: 21, 22, 23, 25, 53, 80, 110, 111, 135, 139, 143, 443, 445, 993, 995, 1723, 3306, 3389, 5900, 8080. \n");
                            results["ports"] = new object[] { 21, 22, 23, 25, 53, 80, 110, 111, 135, 139, 143, 443, 445, 993, 995, 1723, 3306, 3389, 5900, 8080 };
                        }
                        else if (parts[1] == "server-common")
                        {
                            Console.WriteLine("[+] The following ports will be scanned: 7, 9, 13, 17, 19, 25, 42, 80, 88, 110, 111, 119, 135, 149, 389, 443, 445, 465, 563, 587, 636, 808, 993, " +
                                "995, 1433, 1688, 1801, 3268, 3269, 3387, 3388, 3389, 4044, 6516, 6881, 8391, 8443, 8530, 8531, 9389 \n");
                            results["ports"] = new object[] { 7, 9, 13, 17, 19, 25, 42, 80, 88, 110, 111, 119, 135, 149, 389, 443, 445, 465, 563, 587, 636, 808, 993, 995, 1433, 1688, 1801, 3268, 3269, 3387, 3388, 3389, 4044, 6516, 6881, 8391, 8443, 8530, 8531, 9389 };
                        }
                        else
                        {
                            var portArray = parts[1].Split(',');
                            var val5 = portArray;
                            results.Add("ports", val5);
                        }
                        break;
                    default:
                        Console.WriteLine("[X] Unknown parameter passed: {0}", parts[0]);
                        break;
                }
            }
            foreach (string requiredKey in requiredKeys)
            {
                if (!results.ContainsKey(requiredKey))
                {
                    Console.WriteLine("[X] Missing required parameter: {0}", requiredKey);
                    Usage();
                    Environment.Exit(1);
                }
            }
            return results;
        }
        public static bool IsPortOpen(string host, object ports1)
        {
            bool bRet;
            try
            {
                Socket s = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
                int Time = Convert.ToInt32(results["timeout"]);
                s.ReceiveTimeout = Time;
                s.Connect(host, Convert.ToInt32(ports1));
                bRet = true;
            }
            catch (Exception)
            {
                bRet = false;
            }
            return bRet;
        }

        public static void PortScan(string host, object outfileName)
        {
            string formatString = "{0} : port {1} is {2}.";
            Parallel.ForEach((object[])results["ports"], ports =>
            {
                string content = String.Format(formatString, host, ports, IsPortOpen(host, ports) ? "open" : "closed");
                if (file != null)
                {
                    fileMutex.WaitOne();
                    try
                    {
                        file.WriteLine(content, "a");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[X] Exception occurred while writing to file: {0}\n{1}", ex.Message, ex.StackTrace);
                    }
                    finally
                    {
                        fileMutex.ReleaseMutex();
                    }
                }
                Console.WriteLine(String.Format(formatString, host, ports, IsPortOpen(host, ports) ? "open" : "closed"));
            });
        }

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Usage();
            }
            else
            {
                ArgParser(args);
                string outfileName = "";
                if (results.TryGetValue("outfile", out object value))
                    outfileName = value.ToString();
                if (outfileName != "")
                    file = new StreamWriter(outfileName, true);
                foreach (object host in (object[])results["hosts"])
                {
                    PortScan(host.ToString(), outfileName);
                    Console.WriteLine("[+] Scanner complete.");
                }
                if (file != null)
                    file.Close();
            }
        }
    }
}
