using Fclp;
using Icinga;
using Microsoft.Web.Administration;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MonitoringPluginsForWindows
{
    public class check_iis
    {
        private static readonly List<string> listPerfData = new List<string>();
        private static readonly List<string> listSiteOutput = new List<string>();
        private static readonly List<string> listAppPoolOutput = new List<string>();

        private static readonly List<IISInventory> listIISInventory = new List<IISInventory>();
        private static readonly List<AppPool> listAppPoolsOnComputer = new List<AppPool>();
        private static readonly List<WebSite> listSitesOnComputer = new List<WebSite>();
        private static readonly List<string> listStoppedSites = new List<string>();
        private static readonly List<string> listStartedSites = new List<string>();
        private static readonly List<string> listStoppedAppPools = new List<string>();
        private static readonly List<string> listStartedAppPools = new List<string>();

        private static string[] excluded_sites = new string[] { "thisshouldprobablyneverbeoverwrittenbysomething" };
        private static string[] included_sites = new string[] { "thisshouldprobablyneverbeoverwrittenbysomething" };
        private static string[] stopped_sites = new string[] { "thisshouldprobablyneverbeoverwrittenbysomething" };
        private static string[] warn_sites = new string[] { "thisshouldprobablyneverbeoverwrittenbysomething" };

        private static string[] excluded_apppools = new string[] { "thisshouldprobablyneverbeoverwrittenbysomething" };
        private static string[] included_apppools = new string[] { "thisshouldprobablyneverbeoverwrittenbysomething" };
        private static string[] stopped_apppools = new string[] { "thisshouldprobablyneverbeoverwrittenbysomething" };
        private static string[] warn_apppools = new string[] { "thisshouldprobablyneverbeoverwrittenbysomething" };

        private static string outputAppPools = "";
        private static string outputSites = "";
        private static string iisVersion = "unspecified";

        private static bool errorAppPools = false;
        private static bool errorSites = false;

        private static bool do_debug = false;
        private static bool do_verbose = false;
        private static bool do_i2 = false;

        private static bool bDefaultSitesIncluded = false;
        private static bool bDefaultSitesExcluded = false;
        private static bool bDefaultSitesStopped = false;
        private static bool bDefaultSitesWarn = false;

        private static bool bDefaultAppPoolsIncluded = false;
        private static bool bDefaultAppPoolsExcluded = false;
        private static bool bDefaultAppPoolsStopped = false;
        private static bool bDefaultAppPoolsWarn = false;

        private static int iNumberOfAppPools = 0;
        private static int iNumberOfStartedAppPools = 0;
        private static int iNumberOfStoppedAppPools = 0;
        private static int iNumberOfStoppingAppPools = 0;
        private static int iNumberOfStartingAppPools = 0;
        private static int iNumberOfUnknownAppPools = 0;
        private static int iNumberOfCorrectAppPools = 0;
        private static int iNumberOfWrongAppPools = 0;

        private static int iNumberOfSites = 0;
        private static int iNumberOfStartedSites = 0;
        private static int iNumberOfStoppedSites = 0;
        private static int iNumberOfStoppingSites = 0;
        private static int iNumberOfStartingSites = 0;
        private static int iNumberOfUnknownSites = 0;
        private static int iNumberOfCorrectSites = 0;
        private static int iNumberOfWrongSites = 0;

        private static int Main(string[] args)
        {
            int returncode = 0;
            int temp = 3;

            bool do_inventory_sites = false;
            bool do_inventory_apppools = false;
            bool do_sites = false;
            bool do_apppools = false;
            bool do_perfcounter_sites = false;
            bool do_perfcounter_apppools = false;
            bool do_perfdata_mbytes = false;
            bool do_all_running_only = false;
            bool do_skip_empty_vars = false;
            bool do_singluar_check = false;
            bool do_skip_empty_apppools = false;
            bool do_hide_long_output = false;
            bool do_only_autostart_sites = false;

            string inventory_format = "readable";
            string inventory_level = "normal";
            string expected_state = "NotSet";
            string split_by = " ";

            List<string> temp_excluded_sites = new List<string>();
            List<string> temp_included_sites = new List<string>();
            List<string> temp_stopped_sites = new List<string>();
            List<string> temp_warn_sites = new List<string>();

            List<string> temp_excluded_apppools = new List<string>();
            List<string> temp_included_apppools = new List<string>();
            List<string> temp_stopped_apppools = new List<string>();
            List<string> temp_warn_apppools = new List<string>();

            FluentCommandLineParser p = GetP();

            _ = p.Setup<bool>('a', "inventory-websites")
                .WithDescription("Switch to use to provide inventory of Sites")
                .Callback(value => do_inventory_sites = value);

            _ = p.Setup<bool>('A', "inventory-apppools")
                .WithDescription("Switch to use to provide inventory of AppPools")
                .Callback(value => do_inventory_apppools = value);

            _ = p.Setup<bool>('b', "check-websites")
                .WithDescription("Switch to use to check the health status of the local Sites")
                .Callback(value => do_sites = value);

            _ = p.Setup<bool>('B', "check-apppools")
                .WithDescription("Switch to use to check the health status of the local AppPools")
                .Callback(value => do_apppools = value);

            _ = p.Setup<string>('E', "inv-level")
                .Callback(value => inventory_level = value)
                .WithDescription("\tArgument to change the level of output. Default is 'normal', available options are 'normal','full'")
                .SetDefault("normal");

            _ = p.Setup<string>('f', "inv-format")
                .Callback(value => inventory_format = value)
                .WithDescription("\tArgument to provide output of the inventory in other formats, valid options are 'readable', 'i2conf' and 'json'")
                .SetDefault("readable");

            _ = p.Setup<List<string>>('F', "excluded-sites")
                .WithDescription("Excludes Sites from checks and inventory")
                .Callback(items => temp_excluded_sites = items);

            _ = p.Setup<List<string>>('G', "included-sites")
                .WithDescription("Includes Sites to check while all other Sites are excluded, affects both checks and inventory")
                .Callback(items => temp_included_sites = items);

            _ = p.Setup<List<string>>('h', "stopped-sites")
                .WithDescription("\tThe specified Sites are checked that they are stopped")
                .Callback(items => temp_stopped_sites = items);

            _ = p.Setup<List<string>>('H', "warn-sites")
                .WithDescription("\tThese specified Sites will return Warning if they are not in the expected state")
                .Callback(items => temp_warn_sites = items);

            _ = p.Setup<bool>('i', "perfcounter-sites")
                .WithDescription("Switch to use to get perfcounters from Sites")
                .Callback(value => do_perfcounter_sites = value);

            _ = p.Setup<List<string>>('I', "excluded-apppools")
                .WithDescription("Excludes AppPools from checks and inventory")
                .Callback(items => temp_excluded_apppools = items);

            _ = p.Setup<List<string>>('J', "included-apppools")
                .WithDescription("Includes AppPools to check while all other AppPools are excluded, affects both checks and inventory")
                .Callback(items => temp_included_apppools = items);

            _ = p.Setup<List<string>>('k', "stopped-apppools")
                .WithDescription("The specified AppPools are checked that they are stopped")
                .Callback(items => temp_stopped_apppools = items);

            _ = p.Setup<List<string>>('K', "warn-apppools")
                .WithDescription("\tThe specified AppPools will return Warning if they are not in the expected state")
                .Callback(items => temp_warn_apppools = items);

            _ = p.Setup<bool>('l', "perfcounter-apppools")
                .WithDescription("Switch to use to get perfcounters from AppPools")
                .Callback(value => do_perfcounter_apppools = value);

            _ = p.Setup<bool>('L', "skip-empty-apppools")
                .WithDescription("Switch which sets do not check or inventory AppPools which are empty")
                .Callback(value => do_skip_empty_apppools = value);

            _ = p.Setup<bool>('M', "only-autostarted-sites")
                .WithDescription("Switch which sets do only check websites set to autostart")
                .Callback(value => do_only_autostart_sites = value);

            _ = p.Setup<bool>('T', "hide-long-output")
                .WithDescription("Switch to hide the long service output, only prints the summary output and any Sites or AppPools deviating from 'OK'")
                .Callback(value => do_hide_long_output = value);

            _ = p.Setup<string>('u', "expected-state")
                .Callback(value => expected_state = value)
                .WithDescription("Argument to provide the expected State of the AppPool or Site, used together with --single-check");

            _ = p.Setup<bool>('w', "icinga2")
                .WithDescription("\tSwitch used in the Icinga2 CommandDefinition, returns output and perfcounter to the correct class. Do not use via command line.")
                .Callback(value => do_i2 = value);

            _ = p.Setup<bool>('W', "single-check")
                .WithDescription("\tSwitch used together with the Icinga2 Auto Apply rules, this is set when there is a single Site or AppPool to check. Do take great care if you use this outside of the auto apply rules")
                .Callback(value => do_singluar_check = value);

            _ = p.Setup<string>('x', "split-by")
                .WithDescription("\tArgument used to specify what splits all Sites and AppPool arguments. Default is a single space, ' '")
                .Callback(value => split_by = value);

            _ = p.Setup<bool>('X', "inv-hide-empty")
                .WithDescription("Switch to hide empty vars from inventory output")
                .Callback(value => do_skip_empty_vars = value);

            _ = p.Setup<bool>('y', "inv-running-only")
                .WithDescription("Switch to only inventory running Sites and/or AppPools, depending on what has been selected for inventory")
                .Callback(value => do_all_running_only = value);

            _ = p.Setup<bool>('z', "verbose")
                .Callback(value => do_verbose = value)
                .WithDescription("\tSwitch to use when trying to figure out why a Site or an AppPool is not included, excluded or similarly when the returned output is not as expected")
                .SetDefault(false);

            _ = p.Setup<bool>('Z', "debug")
                .Callback(value => do_debug = value)
                .WithDescription("\t\tSwitch to to get maximum verbosity (for debugging)")
                .SetDefault(false);

            _ = p.Setup<bool>('M', "perfdata-mbytes")
                .Callback(value => do_perfdata_mbytes = value)
                .WithDescription("\tArgument used to specify use of Megabytes rather than bytes in perfdata output")
                .SetDefault(false);

            _ = p.SetupHelp("?", "help")
                .Callback(text => Console.WriteLine(text))
                .UseForEmptyArgs()
                .WithHeader(System.AppDomain.CurrentDomain.FriendlyName + " - Windows Service Status plugin for Icinga2, Icinga, Centreon, Shinken, Naemon and other nagios like systems\n\tVersion: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);

            ICommandLineParserResult result = p.Parse(args);

            if ((do_apppools == false) && (do_inventory_apppools == false) && (do_inventory_sites == false) && (do_sites == false))
            {
                // Not going to check Inventory or Checks, no parameters specified for it.
                Console.WriteLine("Neither health check or inventory switches have been specified");
                return (int)ServiceState.ServiceUnknown;
            }

            // Lets see which of the supplied switches changed anything in the include/exclude/stopped context
            HandleArguments(do_sites, do_apppools, do_inventory_sites, do_inventory_apppools, temp_excluded_sites, temp_included_sites, temp_stopped_sites,
                temp_warn_sites, temp_excluded_apppools, temp_included_apppools, temp_stopped_apppools, temp_warn_apppools, split_by);

            // Inventory is blocked from running at the same time as other checks, thus it is run first if specified.
            if (do_inventory_apppools == true || do_inventory_sites == true)
            {
                temp = IisInventory(do_sites, do_apppools, do_inventory_sites, do_inventory_apppools, inventory_format, inventory_level, do_all_running_only,
                    do_skip_empty_vars, do_skip_empty_apppools);
                return temp;
            }

            if (do_apppools == true)
            {
                returncode = CheckAllAppPools(returncode, do_perfcounter_apppools, do_singluar_check, expected_state, do_skip_empty_apppools);
            }
            if (do_sites == true)
            {
                returncode = CheckAllSites(returncode, do_perfcounter_sites, do_singluar_check, do_only_autostart_sites, do_perfdata_mbytes, expected_state);
            }

            returncode = HandleExitText(returncode, do_hide_long_output);

            return returncode;
        }

        private static FluentCommandLineParser GetP()
        {
            return new FluentCommandLineParser();
        }

        private static string[] SplitList(List<string> items, string split_by)
        {
            return split_by == null
                ? throw new ArgumentNullException("split_by")
                : split_by == " "
                    ? items.ToArray()
                    : items.Select(item => item.Split(split_by.ToCharArray()))
                                            .SelectMany(str => str)
                                            .ToArray();
        }

        private static void HandleArguments(bool do_sites, bool do_apppools, bool do_inventory_sites, bool do_inventory_apppools,
            List<string> temp_excluded_sites, List<string> temp_included_sites, List<string> temp_stopped_sites, List<string> temp_warn_sites,
            List<string> temp_excluded_apppools, List<string> temp_included_apppools, List<string> temp_stopped_apppools, List<string> temp_warn_apppools,
            string split_by)
        {
            if (temp_excluded_sites.Count > 0)
            {
                excluded_sites = SplitList(temp_excluded_sites, split_by);
                PrintArray("excluded_sites", excluded_sites);
            }

            if (temp_included_sites.Count > 0)
            {
                included_sites = SplitList(temp_included_sites, split_by);
                PrintArray("included_sites", included_sites);
            }

            if (temp_stopped_sites.Count > 0)
            {
                stopped_sites = SplitList(temp_stopped_sites, split_by);
                PrintArray("stopped_sites", stopped_sites);
            }

            if (temp_warn_sites.Count > 0)
            {
                warn_sites = SplitList(temp_warn_sites, split_by);
                PrintArray("warn_sites", warn_sites);
            }

            if (temp_excluded_apppools.Count > 0)
            {
                excluded_apppools = SplitList(temp_excluded_apppools, split_by);
                PrintArray("excluded_apppools", excluded_apppools);
            }

            if (temp_included_apppools.Count > 0)
            {
                included_apppools = SplitList(temp_included_apppools, split_by);
                PrintArray("included_apppools", included_apppools);
            }

            if (temp_stopped_apppools.Count > 0)
            {
                stopped_apppools = SplitList(temp_stopped_apppools, split_by);
                PrintArray("stopped_apppools", stopped_apppools);
            }

            if (temp_warn_apppools.Count > 0)
            {
                warn_apppools = SplitList(temp_warn_apppools, split_by);
                PrintArray("warn_apppools", warn_apppools);
            }

            if (do_sites || do_inventory_sites)
            {
                if (excluded_sites.Contains("thisshouldprobablyneverbeoverwrittenbysomething"))
                {
                    if (do_verbose == true)
                    {
                        Console.WriteLine("INFO: Default excluded_sites list.");
                    }

                    bDefaultSitesExcluded = true;
                }

                if (included_sites.Contains("thisshouldprobablyneverbeoverwrittenbysomething"))
                {
                    if (do_verbose == true)
                    {
                        Console.WriteLine("INFO: Default included_sites list.");
                    }

                    bDefaultSitesIncluded = true;
                }

                if (stopped_sites.Contains("thisshouldprobablyneverbeoverwrittenbysomething"))
                {
                    if (do_verbose == true)
                    {
                        Console.WriteLine("INFO: Default stopped_sites list.");
                    }

                    bDefaultSitesStopped = true;
                }

                if (warn_sites.Contains("thisshouldprobablyneverbeoverwrittenbysomething"))
                {
                    if (do_verbose == true)
                    {
                        Console.WriteLine("INFO: Default warn_sites list.");
                    }

                    bDefaultSitesWarn = true;
                }
            }

            if (do_apppools || do_inventory_apppools)
            {
                if (excluded_apppools.Contains("thisshouldprobablyneverbeoverwrittenbysomething"))
                {
                    if (do_verbose == true)
                    {
                        Console.WriteLine("INFO: Default excluded_apppools list.");
                    }

                    bDefaultAppPoolsExcluded = true;
                }

                if (included_apppools.Contains("thisshouldprobablyneverbeoverwrittenbysomething"))
                {
                    if (do_verbose == true)
                    {
                        Console.WriteLine("INFO: Default included_apppools list.");
                    }

                    bDefaultAppPoolsIncluded = true;
                }

                if (stopped_apppools.Contains("thisshouldprobablyneverbeoverwrittenbysomething"))
                {
                    if (do_verbose == true)
                    {
                        Console.WriteLine("INFO: Default stopped_apppools list.");
                    }

                    bDefaultAppPoolsStopped = true;
                }

                if (warn_apppools.Contains("thisshouldprobablyneverbeoverwrittenbysomething"))
                {
                    if (do_verbose == true)
                    {
                        Console.WriteLine("INFO: Default warn_apppools list.");
                    }

                    bDefaultAppPoolsWarn = true;
                }
            }
            return;
        }

        private static int HandleExitText(int returncode, bool do_hide_long_output)
        {
            // ORDER the output
            string output = (errorAppPools == true) && (errorSites == false)
                ? outputAppPools + outputSites
                : (errorAppPools == false) && (errorSites == true) ? outputSites + outputAppPools : outputSites + outputAppPools;

            // Handle returncode and exit with proper messages.
            output = returncode == (int)ServiceState.ServiceOK
                ? "OK: " + output
                : returncode == (int)ServiceState.ServiceWarning
                    ? "WARNING: " + output
                    : returncode == (int)ServiceState.ServiceCritical
                                    ? "CRITICAL: " + output
                                    : returncode == (int)ServiceState.ServiceUnknown ? "UNKNOWN: " + output : "UNHANDLED: " + output;

            string outputLong = "";
            if (do_hide_long_output == false)
            {
                int x = 1;
                if (iNumberOfSites > 1)
                {
                    foreach (string outputW in listSiteOutput)
                    {
                        if (x < listSiteOutput.Count)
                        {
                            outputLong = outputLong + outputW + "\n";
                        }
                        else
                        {
                            if (iNumberOfAppPools > 0)
                            {
                                outputLong = outputLong + outputW + "\n";
                            }
                            else
                            {
                                outputLong += outputW;
                            }
                        }
                        x++;
                    }
                }

                int y = 1;
                if (iNumberOfAppPools > 1)
                {
                    foreach (string outputA in listAppPoolOutput)
                    {
                        if (y < listAppPoolOutput.Count)
                        {
                            outputLong = outputLong + outputA + "\n";
                        }
                        else
                        {
                            outputLong += outputA;
                        }
                        y++;
                    }
                }
            }
            string perfdata = "";
            foreach (string outputP in listPerfData)
            {
                perfdata += outputP;
            }

            if (do_i2 == true)
            {
                _ = new CheckResult
                {
                    State = (ServiceState)returncode,
                    Output = do_hide_long_output == false ? output + outputLong : output,

                    PerformanceData = perfdata
                };
            }
            else
            {
                Console.Write(output);
                if ((iNumberOfAppPools > 1 || iNumberOfSites > 1) && do_hide_long_output == false)
                {
                    Console.Write("\n" + outputLong);
                }

                Console.Write(" | " + perfdata);
            }

            return returncode;
        }

        private static void PrintArray(string arrayname, Array array)
        {
            if (do_debug)
            {
                Console.WriteLine("DEBUG - Array: " + arrayname);
                foreach (object row in array)
                {
                    Console.WriteLine("DEBUG - row: " + row);
                }
                Console.WriteLine("DEBUG: End of Array: " + arrayname);
            }
        }

        private static int ReturnCodeMagic(int current_returncode, int suggested_returncode)
        {
            if (do_debug == true)
            {
                Console.WriteLine("DEBUG: Current returncode:\t" + current_returncode);
                Console.WriteLine("DEBUG: Suggested returncode:\t" + suggested_returncode);
            }

            if (current_returncode > suggested_returncode)
            {
                if (do_debug == true)
                {
                    Console.WriteLine("DEBUG: Current returncode is higher");
                }
                return current_returncode;
            }
            else
            {
                if (do_debug == true)
                {
                    Console.WriteLine("DEBUG: Suggested returncode is higher");
                }
                return suggested_returncode;
            }
        }

        private static int IisInventory(bool do_sites, bool do_apppools, bool do_inventory_sites, bool do_inventory_apppools,
            string inventory_format, string inventory_level, bool do_running_only, bool do_skip_empty_vars, bool do_skip_empty_apppools)
        {
            ServerManager iisManager = new ServerManager();

            Array AppPools = new string[] { };
            Array WebSites = new string[] { };

            bool temp = true;
            bool bVerboseInventory = false;

            // Detect if IIS is installed.
            if (IisInstalled() == (int)ServiceState.ServiceUnknown)
            {
                return (int)ServiceState.ServiceUnknown;
            }

            if (inventory_level == "full" && (do_inventory_apppools || do_inventory_sites))
            {
                bVerboseInventory = true;
            }

            if (do_sites)
            {
                if (do_verbose)
                {
                    Console.WriteLine("INFO: Inventory of Sites to check later");
                }

                temp = InventorySites(iisManager, false, false, false);

                if (temp == false)
                {
                    return (int)ServiceState.ServiceUnknown;
                }
            }

            if (do_inventory_sites)
            {
                if (do_verbose)
                {
                    Console.WriteLine("INFO: Inventory of Sites");
                }

                temp = InventorySites(iisManager, bVerboseInventory, do_running_only, true);

                if (temp == false)
                {
                    return (int)ServiceState.ServiceUnknown;
                }
            }

            if (do_apppools)
            {
                if (do_verbose)
                {
                    Console.WriteLine("INFO: Inventory of AppPools to check later");
                }

                temp = InventoryAppPools(iisManager, false, false, false, do_skip_empty_apppools);

                if (temp == false)
                {
                    return (int)ServiceState.ServiceUnknown;
                }
            }

            if (do_inventory_apppools)
            {
                if (do_verbose)
                {
                    Console.WriteLine("INFO: Inventory of AppPools");
                }

                temp = InventoryAppPools(iisManager, bVerboseInventory, do_running_only, true, do_skip_empty_apppools);

                if (temp == false)
                {
                    return (int)ServiceState.ServiceUnknown;
                }
            }

            if (do_inventory_apppools || do_inventory_sites)
            {
                if (do_inventory_apppools)
                {
                    AppPools = listAppPoolsOnComputer.ToArray();
                }

                if (do_inventory_sites)
                {
                    WebSites = listSitesOnComputer.ToArray();
                }

                listIISInventory.Add(new IISInventory(iisVersion, AppPools, WebSites, listStartedAppPools.ToArray(), listStoppedAppPools.ToArray(), listStartedSites.ToArray(), listStoppedSites.ToArray()));

                if (inventory_format == "json")
                {
                    temp = InventoryOutputJSON();
                }

                if (inventory_format == "i2conf")
                {
                    temp = InventoryOutputI2Conf(do_inventory_apppools, do_inventory_sites, do_skip_empty_vars, bVerboseInventory);
                }

                if (inventory_format == "readable")
                {
                    temp = InventoryOutputReadable(do_inventory_apppools, do_inventory_sites, do_skip_empty_vars, bVerboseInventory);
                }

                if (temp == false)
                {
                    return (int)ServiceState.ServiceUnknown;
                }
            }

            return (int)ServiceState.ServiceOK;
        }

        private static bool InventoryOutputJSON()
        {
            string json = JsonConvert.SerializeObject(listIISInventory, Formatting.Indented);
            Console.WriteLine(json);

            return true;
        }

        private static bool InventoryOutputI2Conf(bool do_inventory_apppools, bool do_inventory_sites, bool do_skip_empty_vars, bool bVerboseInventory)
        {
            Dictionary<string, IISInventory> listIISInventoryLocal = listIISInventory.ToDictionary(o => o.IISVersion, o => o);

            foreach (IISInventory IISInventoryLocal in listIISInventory)
            {
                Console.WriteLine("vars.inv.iis.version = \"" + IISInventoryLocal.IISVersion + "\"");
            }
            string i2conf1 = "";
            if (do_inventory_sites == true)
            {
                i2conf1 = IcingaSerializer.Serialize(listStoppedSites.ToArray(), do_skip_empty_vars);
                Console.WriteLine("vars.inv.iis.sites.stopped = " + i2conf1);

                i2conf1 = IcingaSerializer.Serialize(listStartedSites.ToArray(), do_skip_empty_vars);
                Console.WriteLine("vars.inv.iis.sites.started = " + i2conf1);

                foreach (WebSite Site in listSitesOnComputer.ToArray())
                {
                    string i2conf = IcingaSerializer.Serialize(Site, do_skip_empty_vars);
                    Console.WriteLine("vars.inv.iis.site[\"" + Site.Name + "\"] = " + i2conf);
                    Console.WriteLine("");
                }
            }

            if (do_inventory_apppools == true)
            {
                i2conf1 = IcingaSerializer.Serialize(listStoppedAppPools.ToArray(), do_skip_empty_vars);
                Console.WriteLine("vars.inv.iis.apppools.stopped = " + i2conf1);

                i2conf1 = IcingaSerializer.Serialize(listStartedAppPools.ToArray(), do_skip_empty_vars);
                Console.WriteLine("vars.inv.iis.apppools.started = " + i2conf1);

                foreach (AppPool AppPool in listAppPoolsOnComputer.ToArray())
                {
                    string i2conf = IcingaSerializer.Serialize(AppPool, do_skip_empty_vars);
                    Console.WriteLine("vars.inv.iis.apppool[\"" + AppPool.Name + "\"] = " + i2conf);
                    Console.WriteLine("");
                }
            }

            return true;
        }

        private static bool InventoryOutputReadable(bool do_inventory_apppools, bool do_inventory_sites, bool do_skip_empty_vars, bool bVerboseInventory)
        {
            Dictionary<string, IISInventory> listIISInventoryLocal = listIISInventory.ToDictionary(o => o.IISVersion, o => o);
            foreach (IISInventory IISInventoryLocal in listIISInventory)
            {
                Console.WriteLine("IISVersion: " + IISInventoryLocal.IISVersion);
            }

            if (do_inventory_sites == true)
            {
                Console.WriteLine("Sites:");
                foreach (WebSite Site in listSitesOnComputer.ToArray())
                {
                    string readable = ReadableSerializer.Serialize(Site, do_skip_empty_vars);
                    Console.WriteLine(readable);
                }
            }

            if (do_inventory_apppools == true)
            {
                Console.WriteLine("AppPools:");
                foreach (AppPool AppPool in listAppPoolsOnComputer.ToArray())
                {
                    string readable = ReadableSerializer.Serialize(AppPool, do_skip_empty_vars);
                    Console.WriteLine(readable);
                }
            }

            return true;
        }

        public static int IisInstalled()
        {
            try
            {
                using (RegistryKey iisKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\InetStp"))
                {
                    iisVersion = (int)iisKey.GetValue("MajorVersion") + "." + (int)iisKey.GetValue("MinorVersion");

                    return (int)ServiceState.ServiceOK;
                }
            }
            catch
            {
                Console.WriteLine("IIS is not installed.");
                return (int)ServiceState.ServiceUnknown;
            }
        }

        private static bool InventorySites(ServerManager iisManager, bool bVerboseInventory, bool do_running_only, bool do_inventory)
        {
            SiteCollection sites = iisManager.Sites;
            foreach (Site Site in sites)
            {
                string sSiteName = Site.Name.ToString();

                try
                {
                    if (bDefaultSitesExcluded == true)
                    {
                        // No sites to exclude here.
                    }
                    else if (excluded_sites.Contains(sSiteName))
                    {
                        if (do_verbose == true)
                        {
                            Console.WriteLine("INFO: Site in exclude list: " + sSiteName);
                        }

                        continue;
                    }
                    if (bDefaultSitesIncluded == true)
                    {
                        // No sites to skip here.
                    }
                    else if (included_sites.Contains(sSiteName))
                    {
                        if (do_verbose == true)
                        {
                            Console.WriteLine("INFO: Included Site: " + sSiteName);
                        }
                    }
                    else if (bDefaultSitesIncluded == false)
                    {
                        if (do_verbose == true)
                        {
                            Console.WriteLine("INFO: Site not in include list: " + sSiteName);
                        }

                        continue;
                    }

                    if (do_running_only == true)
                    {
                        // Skip any sites (for inventory purposes) that are not started.
                        if (Site.State != ObjectState.Started)
                        {
                            if (do_verbose == true)
                            {
                                Console.WriteLine("INFO: Skipping Site for inventory, it is not running: ");
                            }

                            continue;
                        }
                    }

                    if (do_inventory == true && Site.State == ObjectState.Started)
                    {
                        listStartedSites.Add(sSiteName);
                    }
                    else if (do_inventory == true && Site.State == ObjectState.Stopped)
                    {
                        listStoppedSites.Add(sSiteName);
                    }
                    else if (do_inventory == true)
                    {
                        Console.WriteLine("AppPool in a bad state during inventory!");
                    }

                    Array SiteBindings = InventorySiteBindings(Site, bVerboseInventory);
                    Array SiteApplications = InventorySiteApplications(Site, bVerboseInventory);

                    listSitesOnComputer.Add(new WebSite(Site.Id, Site.Name, Site.ServerAutoStart, Site.IsLocallyStored, Site.State.ToString(), Site.LogFile.Directory,
                        Site.LogFile.Enabled, SiteBindings, SiteApplications));
                }
                catch (Exception e)
                {
                    Console.WriteLine("[UNKNOWN] Error parsing WebSite '" + sSiteName + "' - Valid AppPool? - Message: " + e);
                }
            }

            return true;
        }

        private static bool InventoryAppPools(ServerManager iisManager, bool bVerboseInventory, bool do_running_only, bool do_inventory, bool do_skip_empty_apppools)
        {
            ApplicationPoolCollection appPools = iisManager.ApplicationPools;

            foreach (ApplicationPool AppPool in appPools)
            {
                string sAppPoolName = AppPool.Name.ToString();

                if (bDefaultAppPoolsExcluded == true)
                {
                    // No apppools to exclude here.
                }
                else if (excluded_apppools.Contains(sAppPoolName))
                {
                    if (do_verbose == true)
                    {
                        Console.WriteLine("INFO: AppPool in exclude list: " + sAppPoolName);
                    }

                    continue;
                }

                if (bDefaultAppPoolsIncluded == true)
                {
                    // No apppools to check for include, default include list.
                }
                else if (included_apppools.Contains(sAppPoolName))
                {
                    if (do_verbose == true)
                    {
                        Console.WriteLine("INFO: Included AppPool: " + sAppPoolName);
                    }
                }
                else if (bDefaultAppPoolsIncluded == false)
                {
                    if (do_verbose == true)
                    {
                        Console.WriteLine("INFO: AppPool not in include list: " + sAppPoolName);
                    }

                    continue;
                }

                if (do_running_only == true)
                {
                    // Skip any apppools (for inventory purposes) that are not started.
                    if (AppPool.State != ObjectState.Started)
                    {
                        if (do_verbose)
                        {
                            Console.WriteLine("INFO: Skipping AppPool for inventory, it is not running: " + sAppPoolName);
                        }

                        continue;
                    }
                }

                if (do_skip_empty_apppools == true)
                {
                    int NumberOfTimesUsed = GetNumberApplicationsInAppPool(iisManager, sAppPoolName);
                    if (NumberOfTimesUsed == 0)
                    {
                        if (do_verbose)
                        {
                            Console.WriteLine("INFO: AppPool '" + sAppPoolName + "' is not used by any Applications, this is being skipped due to skip-empty-apppools set");
                        }
                        // Skip this AppPool as it not used by any sites.
                        continue;
                    }
                }

                Array ArrWorkerProcesses = InventoryWorkerProcesses(AppPool, bVerboseInventory);

                if (do_inventory == true && AppPool.State == ObjectState.Started)
                {
                    listStartedAppPools.Add(sAppPoolName);
                }
                else if (do_inventory == true && AppPool.State == ObjectState.Stopped)
                {
                    listStoppedAppPools.Add(sAppPoolName);
                }
                else if (do_inventory == true)
                {
                    Console.WriteLine("AppPool in a bad state during inventory!");
                }

                listAppPoolsOnComputer.Add(new AppPool(AppPool.Name, AppPool.AutoStart, AppPool.Enable32BitAppOnWin64, AppPool.IsLocallyStored,
                    Enum.GetName(typeof(ManagedPipelineMode), AppPool.ManagedPipelineMode), AppPool.ManagedRuntimeVersion, AppPool.QueueLength,
                    Enum.GetName(typeof(ObjectState), AppPool.State), Enum.GetName(typeof(ProcessModelIdentityType), AppPool.ProcessModel.IdentityType),
                    AppPool.ProcessModel.IdleTimeout, AppPool.ProcessModel.LoadUserProfile, AppPool.ProcessModel.MaxProcesses, AppPool.ProcessModel.PingingEnabled,
                    AppPool.ProcessModel.UserName, AppPool.Cpu.SmpAffinitized, Enum.GetName(typeof(LoadBalancerCapabilities), AppPool.Failure.LoadBalancerCapabilities)
                    , ArrWorkerProcesses));
            }
            return true;
        }

        private static int GetNumberApplicationsInAppPool(ServerManager iisManager, string AppPoolName)
        {
            int NumberOfTimesUsed = 0;
            foreach (Site site in iisManager.Sites)
            {
                foreach (Application app in site.Applications)
                {
                    if (app.ApplicationPoolName == AppPoolName)
                    {
                        NumberOfTimesUsed++;
                    }
                }
            }
            return NumberOfTimesUsed;
        }

        private static Array InventoryWorkerProcesses(ApplicationPool AppPool, bool bVerboseInventory)
        {
            List<AppPoolWorkerProcesses> listAppPoolWorkerProcesses = new List<AppPoolWorkerProcesses>();

            // Skip, verbose inventory is not wanted.
            if (bVerboseInventory == false)
            {
                return listAppPoolWorkerProcesses.ToArray();
            }

            foreach (WorkerProcess WorkerProcess in AppPool.WorkerProcesses)
            {
                Array temp_AppPoolWorkerProcessAppDomains = InventoryWPAppDomains(WorkerProcess.ApplicationDomains, AppPool, bVerboseInventory);

                listAppPoolWorkerProcesses.Add(new AppPoolWorkerProcesses(WorkerProcess.AppPoolName, WorkerProcess.IsLocallyStored,
                    WorkerProcess.State.ToString(), temp_AppPoolWorkerProcessAppDomains));
            }

            Array ArrWorkerProcesses = listAppPoolWorkerProcesses.ToArray();
            return ArrWorkerProcesses;
        }

        private static Array InventoryWPAppDomains(ApplicationDomainCollection WPAppDomains, ApplicationPool AppPool, bool bVerboseInventory)
        {
            List<AppPoolWPAppDomains> listAppPoolWPApplicationDomains = new List<AppPoolWPAppDomains>();

            // Skip, verbose inventory is not wanted.
            if (bVerboseInventory == false)
            {
                return listAppPoolWPApplicationDomains.ToArray();
            }

            foreach (ApplicationDomain ApplicationDomain in WPAppDomains)
            {
                //Array ArrWorkerProcesses = InventoryWorkerProcesses(AppPool, bVerboseInventory);
                Array ArrWorkerProcesses = new Array[] { null };
                listAppPoolWPApplicationDomains.Add(new AppPoolWPAppDomains(ApplicationDomain.Id, ApplicationDomain.Idle,
                    ApplicationDomain.IsLocallyStored, ApplicationDomain.PhysicalPath, ApplicationDomain.VirtualPath, ArrWorkerProcesses));
            }

            Array ArrWPAppDomains = listAppPoolWPApplicationDomains.ToArray();
            return ArrWPAppDomains;
        }

        private static Array InventorySiteBindings(Site site, bool bVerboseInventory)
        {
            List<SiteBinding> listSiteBindings = new List<SiteBinding>();

            foreach (Binding Binding in site.Bindings)
            {
                // If there is a returned certificate,  this will not be null
                string certificateHash = null;
                if (Binding.CertificateHash != null)
                {
                    // convert the returned bytestring to hex, so it is easy to check the thumbprint.
                    certificateHash = BitConverter.ToString(Binding.CertificateHash).Replace("-", string.Empty);
                }

                string certificateStore = null;
                if (Binding.CertificateStoreName != null)
                {
                    certificateStore = Binding.CertificateStoreName.ToString();
                }

                string bindingInformation = null;
                if (Binding.BindingInformation.ToString() != null)
                {
                    bindingInformation = Binding.BindingInformation.ToString();
                }

                string host = null;
                if (Binding.Host.ToString() != null)
                {
                    host = Binding.Host.ToString();
                }

                string protocol = null;
                if (Binding.Protocol.ToString() != null)
                {
                    protocol = Binding.Protocol.ToString();
                }

                try
                {
                    listSiteBindings.Add(new SiteBinding(protocol, bindingInformation, host, certificateHash,
                        certificateStore, Binding.UseDsMapper, Binding.IsIPPortHostBinding, Binding.IsLocallyStored));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error in reading sitebinding for site '" + site.Name.ToString() + "', protocol '" + protocol + "', error: " + e);
                }
            }

            Array ArrSiteBindings = listSiteBindings.ToArray();
            return ArrSiteBindings;
        }

        private static Array InventorySiteApplications(Site site, bool bVerboseInventory)
        {
            List<SiteApplications> listSiteApplications = new List<SiteApplications>();

            // Return empty array, verbose inventory is not specified.
            if (bVerboseInventory == false)
            {
                return listSiteApplications.ToArray();
            }

            foreach (Application Application in site.Applications)
            {
                Array VirtualDirectories = InventorySiteAppVirtualDirectories(Application.VirtualDirectories, bVerboseInventory);
                listSiteApplications.Add(new SiteApplications(Application.ApplicationPoolName, Application.EnabledProtocols,
                    Application.IsLocallyStored, Application.Path, VirtualDirectories));
            }

            Array ArrSiteApplications = listSiteApplications.ToArray();
            return ArrSiteApplications;
        }

        private static Array InventorySiteAppVirtualDirectories(VirtualDirectoryCollection Application, bool bVerboseInventory)
        {
            List<SiteAppVirtualDirectories> listVirtualDirectories = new List<SiteAppVirtualDirectories>();

            // Return empty array, verbose inventory is not specified.
            if (bVerboseInventory == false)
            {
                return listVirtualDirectories.ToArray();
            }

            foreach (VirtualDirectory VirtualDirectory in Application)
            {
                listVirtualDirectories.Add(new SiteAppVirtualDirectories(VirtualDirectory.Path, VirtualDirectory.PhysicalPath,
                    VirtualDirectory.IsLocallyStored, VirtualDirectory.LogonMethod.ToString(), VirtualDirectory.UserName));
            }

            Array ArrSiteAppVirtDirs = listVirtualDirectories.ToArray();
            return ArrSiteAppVirtDirs;
        }

        public static int CheckAllSites(int returncode, bool do_perfcounter_sites, bool do_singluar_check, bool do_only_autostart_sites, bool do_perfdata_mbytes, string expected_state)
        {
            ServerManager iisManager = new ServerManager();

            bool temp = false;
            temp = InventorySites(iisManager, false, false, false);
            if (temp == false)
            {
                return (int)ServiceState.ServiceUnknown;
            }

            // If Expected State is set, we use a different logic further down.
            bool bExpectedStateSet = false;
            if (expected_state != "NotSet")
            {
                bExpectedStateSet = true;
            }

            string output;

            Dictionary<string, WebSite> listOverSites = listSitesOnComputer.ToDictionary(o => o.Name, o => o);

            // Loop for each site in sites
            foreach (WebSite site in listOverSites.Values)
            {
                bool bSiteShouldBeStopped = false;
                bool bSiteShouldBeWarned = false;

                string sSiteName = site.Name;

                if (bDefaultSitesStopped == true)
                {
                    // No need to evaluate any further
                }
                else if (stopped_sites.Contains(sSiteName))
                {
                    if (do_verbose == true)
                    {
                        Console.WriteLine("INFO: Site in stopped list: " + sSiteName);
                    }
                    bSiteShouldBeStopped = true;
                }

                if (bDefaultSitesWarn == true)
                {
                    // No need to evaluate any further
                }
                else if (warn_sites.Contains(sSiteName))
                {
                    if (do_verbose == true)
                    {
                        Console.WriteLine("INFO: Site in warn list: " + sSiteName);
                    }
                    bSiteShouldBeWarned = true;
                }

                output = "";
                bool bSiteIsAutostart = site.ServerAutoStart;

                output = bSiteIsAutostart
                    ? "Site '" + sSiteName + "' set to AutoStart and site state is '" + site.State.ToString() + "'"
                    : "Site '" + sSiteName + "' not set to AutoStart and site state is '" + site.State.ToString() + "'";

                // OK
                if (do_singluar_check == true && bExpectedStateSet == true && site.State == expected_state)
                {
                    output += " which is correct";

                    returncode = ReturnCodeMagic(returncode, (int)ServiceState.ServiceOK);

                    CountSiteState(site.State.ToString());
                    iNumberOfCorrectSites++;
                }
                else if (do_singluar_check == true && bExpectedStateSet == true)
                {
                    returncode = bSiteShouldBeWarned == true
                        ? ReturnCodeMagic(returncode, (int)ServiceState.ServiceWarning)
                        : ReturnCodeMagic(returncode, (int)ServiceState.ServiceCritical);

                    outputSites = outputSites + "Site '" + sSiteName + "' with an incorrect state: '" + site.State.ToString() + "'. ";
                    output = output + " when it is set to be '" + expected_state + "'.";

                    CountSiteState(site.State.ToString());
                    iNumberOfWrongSites++;
                    errorSites = true;
                }
                else if ((bSiteShouldBeStopped == true && site.State == ObjectState.Stopped.ToString()) ||
                    (bSiteShouldBeStopped == false && site.State == ObjectState.Started.ToString()) ||
                        (bSiteIsAutostart == false && do_only_autostart_sites == true && site.State == ObjectState.Stopped.ToString()) ||
                        (bSiteIsAutostart == true && do_only_autostart_sites == true && site.State == ObjectState.Started.ToString()) ||
                        (bSiteIsAutostart == true && do_only_autostart_sites == true && site.State == ObjectState.Starting.ToString()))
                {
                    output += " which is correct";

                    returncode = ReturnCodeMagic(returncode, (int)ServiceState.ServiceOK);

                    CountSiteState(site.State.ToString());
                    iNumberOfCorrectSites++;
                }
                else if ((bSiteShouldBeStopped == true && site.State == ObjectState.Started.ToString()) ||
                    (bSiteShouldBeStopped == true && site.State == ObjectState.Starting.ToString()) ||
                    (bSiteShouldBeStopped == true && site.State == ObjectState.Stopping.ToString()) ||
                    (bSiteShouldBeStopped == true && site.State == ObjectState.Unknown.ToString()) ||
                    (bSiteIsAutostart == true && do_only_autostart_sites == true && site.State != ObjectState.Starting.ToString() &&
                    bSiteIsAutostart == true && do_only_autostart_sites == true && site.State != ObjectState.Starting.ToString()))
                {
                    returncode = bSiteShouldBeWarned == true
                        ? ReturnCodeMagic(returncode, (int)ServiceState.ServiceWarning)
                        : ReturnCodeMagic(returncode, (int)ServiceState.ServiceCritical);

                    outputSites = outputSites + "Site '" + sSiteName + "' with an incorrect state: '" + site.State.ToString() + "'. ";
                    output = output + " when it is set to be '" + ObjectState.Stopped.ToString() + "'.";

                    CountSiteState(site.State.ToString());
                    iNumberOfWrongSites++;
                    errorSites = true;
                }
                else if ((bSiteShouldBeStopped == false && site.State == ObjectState.Stopped.ToString()) ||
                    (bSiteShouldBeStopped == false && site.State == ObjectState.Starting.ToString()) ||
                    (bSiteShouldBeStopped == false && site.State == ObjectState.Stopping.ToString()) ||
                    (bSiteShouldBeStopped == false && site.State == ObjectState.Unknown.ToString()))
                {
                    returncode = bSiteShouldBeWarned == true
                        ? ReturnCodeMagic(returncode, (int)ServiceState.ServiceWarning)
                        : ReturnCodeMagic(returncode, (int)ServiceState.ServiceCritical);

                    outputSites = outputSites + "Site '" + sSiteName + "' with an incorrect state: '" + site.State.ToString() + "'. ";
                    output = output + " when it is set to be '" + ObjectState.Started.ToString() + "'. ";

                    CountSiteState(site.State.ToString());
                    iNumberOfWrongSites++;
                    errorSites = true;
                }

                if (do_perfcounter_sites)
                {
                    output = PerfCounterSites(sSiteName, output, do_singluar_check, do_perfdata_mbytes);
                }

                listSiteOutput.Add(output);
                iNumberOfSites++;
            }

            if (errorSites == false)
            {
                if (iNumberOfSites == 0)
                {
                    outputSites = "No Sites matched the filters given, or none exist on this server.";
                    returncode = (int)ServiceState.ServiceUnknown;
                }
                if (iNumberOfSites == 1)
                {
                    string tempOutput = string.Join(",", listSiteOutput.ToArray());
                    outputSites = tempOutput;
                }
                else
                {
                    outputSites = "All Sites are in their correct states. ";
                }
            }

            listPerfData.Add(" 'NumberOfSites'=" + iNumberOfSites);
            listPerfData.Add(" 'NumberOfStartedSites'=" + iNumberOfStartedSites + ";;;0;" + iNumberOfSites);
            listPerfData.Add(" 'NumberOfStoppedSites'=" + iNumberOfStoppedSites + ";;;0;" + iNumberOfSites);
            listPerfData.Add(" 'NumberOfStoppingSites'=" + iNumberOfStoppingSites + ";;;0;" + iNumberOfSites);
            listPerfData.Add(" 'NumberOfStartingSites'=" + iNumberOfStartingSites + ";;;0;" + iNumberOfSites);
            listPerfData.Add(" 'NumberOfUnknownSites'=" + iNumberOfUnknownSites + ";;;0;" + iNumberOfSites);
            listPerfData.Add(" 'NumberOfCorrectSites'=" + iNumberOfCorrectSites + ";;;0;" + iNumberOfSites);
            listPerfData.Add(" 'NumberOfWrongSites'=" + iNumberOfWrongSites + ";;;0;" + iNumberOfSites);

            return returncode;
        }

        public static int CheckAllAppPools(int returncode, bool do_perfcounter_apppools, bool do_singluar_check, string expected_state, bool do_skip_empty_apppools)
        {
            ServerManager iisManager = new ServerManager();

            bool temp = false;
            temp = InventoryAppPools(iisManager, false, false, false, do_skip_empty_apppools);
            if (temp == false)
            {
                return (int)ServiceState.ServiceUnknown;
            }

            // If Expected State is set, we use a different logic further down.
            bool bExpectedStateSet = false;
            if (expected_state != "NotSet")
            {
                bExpectedStateSet = true;
            }

            string output;

            Dictionary<string, AppPool> listOverAppPools = listAppPoolsOnComputer.ToDictionary(o => o.Name, o => o);

            // Loop for each AppPool in appPools list
            foreach (AppPool apppool in listOverAppPools.Values)
            {
                bool bAppPoolShouldBeStopped = false;
                bool bAppPoolShouldBeWarned = false;

                string sAppPoolName = apppool.Name.ToString();

                if (bDefaultAppPoolsStopped == true)
                {
                    // No need to evaluate any further
                }
                else if (stopped_apppools.Contains(sAppPoolName))
                {
                    if (do_verbose == true)
                    {
                        Console.WriteLine("INFO: AppPool in stopped list: " + sAppPoolName);
                    }
                    bAppPoolShouldBeStopped = true;
                }

                if (bDefaultAppPoolsWarn == true)
                {
                    // No need to evaluate any further
                }
                else if (warn_apppools.Contains(sAppPoolName))
                {
                    if (do_verbose == true)
                    {
                        Console.WriteLine("INFO: AppPool in warn list: " + sAppPoolName);
                    }
                    bAppPoolShouldBeWarned = true;
                }

                output = "";

                output = apppool.AutoStart == true
                    ? "AppPool '" + sAppPoolName + "' set to AutoStart and apppool state is '" + apppool.State.ToString() + "'"
                    : "AppPool '" + sAppPoolName + "' not set to AutoStart and apppool state is '" + apppool.State.ToString() + "'";
                // OK
                if (do_singluar_check == true && bExpectedStateSet == true && apppool.State == expected_state)
                {
                    output += " which is correct";

                    returncode = ReturnCodeMagic(returncode, (int)ServiceState.ServiceOK);

                    CountAppPoolState(apppool.State.ToString());
                    iNumberOfCorrectAppPools++;
                }
                else if (do_singluar_check == true && bExpectedStateSet == true)
                {
                    returncode = bAppPoolShouldBeWarned == true
                        ? ReturnCodeMagic(returncode, (int)ServiceState.ServiceWarning)
                        : ReturnCodeMagic(returncode, (int)ServiceState.ServiceCritical);

                    outputAppPools = outputAppPools + "AppPool '" + sAppPoolName + "' with an incorrect state: '" + apppool.State.ToString() + "'. ";
                    output = output + " when it is set to be '" + expected_state + "'.";

                    CountAppPoolState(apppool.State.ToString());
                    iNumberOfWrongAppPools++;
                    errorAppPools = true;
                }
                else if ((bAppPoolShouldBeStopped == true && apppool.State == ObjectState.Stopped.ToString()) ||
                    (bAppPoolShouldBeStopped == false && apppool.State == ObjectState.Started.ToString()))
                {
                    output += " which is correct";

                    returncode = ReturnCodeMagic(returncode, (int)ServiceState.ServiceOK);

                    CountAppPoolState(apppool.State.ToString());
                    iNumberOfCorrectAppPools++;
                }
                else if ((bAppPoolShouldBeStopped == true && apppool.State == ObjectState.Started.ToString()) ||
                    (bAppPoolShouldBeStopped == true && apppool.State == ObjectState.Starting.ToString()) ||
                    (bAppPoolShouldBeStopped == true && apppool.State == ObjectState.Stopping.ToString()) ||
                    (bAppPoolShouldBeStopped == true && apppool.State == ObjectState.Unknown.ToString()))
                {
                    returncode = bAppPoolShouldBeWarned == true
                        ? ReturnCodeMagic(returncode, (int)ServiceState.ServiceWarning)
                        : ReturnCodeMagic(returncode, (int)ServiceState.ServiceCritical);

                    outputAppPools = outputAppPools + "AppPool '" + sAppPoolName + "' with an incorrect state: '" + apppool.State.ToString() + "'. ";
                    output = output + " when it is set to be '" + ObjectState.Stopped.ToString() + "'.";

                    CountAppPoolState(apppool.State.ToString());
                    iNumberOfWrongAppPools++;
                    errorAppPools = true;
                }
                else if ((bAppPoolShouldBeStopped == false && apppool.State == ObjectState.Stopped.ToString()) ||
                    (bAppPoolShouldBeStopped == false && apppool.State == ObjectState.Starting.ToString()) ||
                    (bAppPoolShouldBeStopped == false && apppool.State == ObjectState.Stopping.ToString()) ||
                    (bAppPoolShouldBeStopped == false && apppool.State == ObjectState.Unknown.ToString()))
                {
                    returncode = bAppPoolShouldBeWarned == true
                        ? ReturnCodeMagic(returncode, (int)ServiceState.ServiceWarning)
                        : ReturnCodeMagic(returncode, (int)ServiceState.ServiceCritical);

                    outputAppPools = outputAppPools + "AppPool '" + sAppPoolName + "' with an incorrect state: '" + apppool.State.ToString() + "'. ";
                    output = output + " when it is set to be '" + ObjectState.Started.ToString() + "'.";

                    CountAppPoolState(apppool.State.ToString());
                    iNumberOfWrongAppPools++;
                    errorAppPools = true;
                }

                if (do_perfcounter_apppools)
                {
                    output = PerfCounterAppPools(sAppPoolName, output, do_singluar_check);
                }

                listAppPoolOutput.Add(output);
                iNumberOfAppPools++;
            }

            if (errorAppPools == false)
            {
                if (iNumberOfAppPools == 0)
                {
                    outputAppPools = "No AppPools matched the filters given, or none exist on this server.";
                    returncode = (int)ServiceState.ServiceUnknown;
                }
                if (iNumberOfAppPools == 1)
                {
                    string tempOutput = string.Join(",", listAppPoolOutput.ToArray());
                    outputSites = tempOutput;
                }
                else
                {
                    outputAppPools = "All AppPools are in their correct states. ";
                }
            }

            listPerfData.Add(" 'NumberOfAppPools'=" + iNumberOfAppPools);
            listPerfData.Add(" 'NumberOfStartedAppPools'=" + iNumberOfStartedAppPools + ";;;0;" + iNumberOfAppPools);
            listPerfData.Add(" 'NumberOfStoppedAppPools'=" + iNumberOfStoppedAppPools + ";;;0;" + iNumberOfAppPools);
            listPerfData.Add(" 'NumberOfStoppingAppPools'=" + iNumberOfStoppingAppPools + ";;;0;" + iNumberOfAppPools);
            listPerfData.Add(" 'NumberOfStartingAppPools'=" + iNumberOfStartingAppPools + ";;;0;" + iNumberOfAppPools);
            listPerfData.Add(" 'NumberOfUnknownAppPools'=" + iNumberOfUnknownAppPools + ";;;0;" + iNumberOfAppPools);
            listPerfData.Add(" 'NumberOfCorrectAppPools'=" + iNumberOfCorrectAppPools + ";;;0;" + iNumberOfAppPools);
            listPerfData.Add(" 'NumberOfWrongAppPools'=" + iNumberOfWrongAppPools + ";;;0;" + iNumberOfAppPools);

            return returncode;
        }

        public static void CountSiteState(string status)
        {
            if (status == ObjectState.Started.ToString())
            {
                iNumberOfStartedSites++;
            }
            else if (status == ObjectState.Stopped.ToString())
            {
                iNumberOfStoppedSites++;
            }
            else if (status == ObjectState.Starting.ToString())
            {
                iNumberOfStartingSites++;
            }
            else if (status == ObjectState.Stopping.ToString())
            {
                iNumberOfStoppingSites++;
            }
            else
            {
                iNumberOfUnknownSites++;
            }
        }

        public static void CountAppPoolState(string status)
        {
            if (status == ObjectState.Started.ToString())
            {
                iNumberOfStartedAppPools++;
            }
            else if (status == ObjectState.Stopped.ToString())
            {
                iNumberOfStoppedAppPools++;
            }
            else if (status == ObjectState.Starting.ToString())
            {
                iNumberOfStartingAppPools++;
            }
            else if (status == ObjectState.Stopping.ToString())
            {
                iNumberOfStoppingAppPools++;
            }
            else
            {
                iNumberOfUnknownAppPools++;
            }
        }

        private static string PerfCounterSites(string sSiteName, string output, bool do_singluar_check, bool do_perfdata_mbytes)
        {
            try
            {
                string prefix = "";
                if (!do_singluar_check)
                {
                    prefix = sSiteName + "_";
                }

                PerformanceCounterCategory cat = new PerformanceCounterCategory("Web Service", ".");
                List<PerformanceCounter> counters = new List<PerformanceCounter>();

                PerformanceCounter total_get_requests = new PerformanceCounter("Web Service", "Total Get Requests", sSiteName, ".");
                listPerfData.Add(" '" + prefix + "Total Get Requests'=" + total_get_requests.NextValue() + "c;;;;");

                PerformanceCounter total_post_requests = new PerformanceCounter("Web Service", "Total Post Requests", sSiteName, ".");
                listPerfData.Add(" '" + prefix + "Total Post Requests'=" + total_post_requests.NextValue() + "c;;;;");

                PerformanceCounter total_connection_attempts = new PerformanceCounter("Web Service", "Total Connection Attempts (all instances)", sSiteName, ".");
                listPerfData.Add(" '" + prefix + "Total Connection Attemps'=" + total_connection_attempts.NextValue() + "c;;;;");

                PerformanceCounter total_bytes_sent = new PerformanceCounter("Web Service", "Total Bytes Sent", sSiteName, ".");
                if (do_perfdata_mbytes)
                {
                    listPerfData.Add(" '" + prefix + "Total MBytes Sent'=" + toMbyte(total_bytes_sent.NextValue()) + "MByte;;;;");
                }
                else
                {
                    listPerfData.Add(" '" + prefix + "Total MBytes Sent'=" + total_bytes_sent.NextValue() + ";;;;");
                }

                PerformanceCounter total_bytes_received = new PerformanceCounter("Web Service", "Total Bytes Received", sSiteName, ".");
                if (do_perfdata_mbytes)
                {
                    listPerfData.Add(" '" + prefix + "Total MBytes Received'=" + toMbyte(total_bytes_received.NextValue()) + "MByte;;;;");
                }
                else
                {
                    listPerfData.Add(" '" + prefix + "Total MBytes Received'=" + total_bytes_received.NextValue() + ";;;;");
                }

                PerformanceCounter current_connections = new PerformanceCounter("Web Service", "Current Connections", sSiteName, ".");
                listPerfData.Add(" '" + prefix + "Current Connections'=" + current_connections.NextValue() + "B;;;;");
                output = output + ", current connections: " + current_connections.NextValue();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in getting performance data: " + e);
            }

            return output;
        }

        private static float toMbyte(float b)
        {
            return b / (1024 * 1024);
        }

        private static string PerfCounterAppPools(string sAppPoolName, string output, bool do_singluar_check)
        {
            try
            {
                string prefix = "";
                if (!do_singluar_check)
                {
                    prefix = sAppPoolName + "_";
                }
                PerformanceCounterCategory cat = new PerformanceCounterCategory("Web Service", ".");
                List<PerformanceCounter> counters = new List<PerformanceCounter>();

                PerformanceCounter total_worker_processes_created = new PerformanceCounter("APP_POOL_WAS", "Total Worker Processes Created", sAppPoolName, ".");
                listPerfData.Add(" '" + prefix + "Total Worker Processes Created'=" + total_worker_processes_created.NextValue() + "c;;;;");

                PerformanceCounter total_worker_processes_failures = new PerformanceCounter("APP_POOL_WAS", "Total Worker Process Failures", sAppPoolName, ".");
                listPerfData.Add(" '" + prefix + "Total Worker Process Failures'=" + total_worker_processes_failures.NextValue() + "c;;;;");

                PerformanceCounter apppool_uptime = new PerformanceCounter("APP_POOL_WAS", "Current Application Pool Uptime", sAppPoolName, ".");
                listPerfData.Add(" '" + prefix + "Current Application Pool Uptime'=" + apppool_uptime.NextValue() + "c;;;;");
                output = output + ", application pool uptime: " + apppool_uptime.NextValue();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in getting performance data: " + e);
            }

            return output;
        }
    }
}