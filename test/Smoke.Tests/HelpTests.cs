using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Smoke.Tests {
    public class HelpTests {
        
        [SetUp]
        public void Setup() {

        }

        [Test]
        public async Task show_help() {
            var scriptLines = await File.ReadAllLinesAsync("./scripts/RunConsoleApp.ps1");

            var stringBuilder = new StringBuilder();

            using (PowerShell powerShell = PowerShell.Create()) {
                //foreach (var scriptLine in scriptLines) {
                //    powerShell.AddScript(scriptLine);
                //}

                powerShell.AddCommand("echo test");

                powerShell.Streams.Progress.DataAdded += handleProgressRecord;
                powerShell.Streams.Debug.DataAdded += handleProgressRecord;
                powerShell.Streams.Error.DataAdded += handleErrorRecord;
                powerShell.Streams.Information.DataAdded += handleInformationRecord;
                powerShell.Streams.Verbose.DataAdded += handleProgressRecord;
                powerShell.Streams.Warning.DataAdded += handleProgressRecord;
                //powerShell.Streams.

                powerShell.AddParameters(new Dictionary<string, string> {
                    { "ConsoleAppTestPath", "aa" }
                });
                try {
                    var pipelineObjects = await powerShell.InvokeAsync();

                } catch (Exception e) {
                    Console.WriteLine(e);
                    throw;
                }
            }

        }

        static void handleProgressRecord(object sender, DataAddedEventArgs e) {
            ProgressRecord newRecord = ((PSDataCollection<ProgressRecord>)sender)[e.Index];
            if (newRecord.PercentComplete != -1) {
                Console.Clear();
                Console.WriteLine("Progress updated: {0}", newRecord.PercentComplete);
            }
        }

        static void handleErrorRecord(object sender, DataAddedEventArgs e) {
            ErrorRecord newRecord = ((PSDataCollection<ErrorRecord>)sender)[e.Index];
            //if (newRecord. != -1)
            //{
            //    Console.Clear();
            //    Console.WriteLine("Progress updated: {0}", newRecord.PercentComplete);
            //}
        }

        static void handleInformationRecord(object sender, DataAddedEventArgs e) {
            InformationRecord newRecord = ((PSDataCollection<InformationRecord>)sender)[e.Index];
            //if (newRecord. != -1)
            //{
            //    Console.Clear();
            //    Console.WriteLine("Progress updated: {0}", newRecord.PercentComplete);
            //}
        }
    }
}