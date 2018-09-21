using System;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;

namespace BeHappy
{
    class Program
    {
        static string DeviceConnectionString = "";
        static DeviceClient Client = null;
        
        static void Main(string[] args)
        {
            try
            {
                InitClient();
                BeHappy();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error in sample: {0}", ex.Message);
            }
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }

        private static async void InitClient()
        {
            try
            {
                Console.WriteLine("Connecting to hub");
                Client = DeviceClient.CreateFromConnectionString(DeviceConnectionString, 
                    TransportType.Mqtt);
                Console.WriteLine("Retrieving twin");
                await Client.GetTwinAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error in sample: {0}", ex.Message);
            }
        }

        private static async void BeHappy()
        {
            try
            {
                Console.WriteLine("Reported property: mood set to happy");

                var reportedProperties = new TwinCollection();
                var personalInfo = new TwinCollection();
                personalInfo["mood"] = "happy";
                reportedProperties["personalInfo"] = personalInfo;
                await Client.UpdateReportedPropertiesAsync(reportedProperties);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error in sample: {0}", ex.Message);
            }
        }
    }
}