using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace Service
{
    class Program
    {
        static RegistryManager registryManager;
        static string connectionString = "";
        private static string deviceId = "";
        
        static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            AddTagsAndQuery().Wait();
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
        
        public static async Task AddTagsAndQuery()
        {
            var twin = await registryManager.GetTwinAsync(deviceId);
            var patch =
                @"{
                    tags: {
                        conference: {
                            name: 'DDDEastAnglia',
                            location: 'Cambridge'
                        }
                    }
                }";
            await registryManager.UpdateTwinAsync(twin.DeviceId, patch, twin.ETag);

            var query = registryManager.CreateQuery(
                "SELECT * FROM devices WHERE tags.conference.name = 'DDDEastAnglia'", 100);
            var twinsInRedmond43 = await query.GetNextAsTwinAsync();
            Console.WriteLine("Devices on DDDEastAnglia: {0}", 
                string.Join(", ", twinsInRedmond43.Select(t => t.DeviceId)));

            query = registryManager.CreateQuery("SELECT * FROM devices WHERE tags.conference.name = 'DDDEastAnglia'" +
                                                " AND properties.reported.personalInfo.mood = 'happy'", 100);
            var twinsInRedmond43UsingCellular = await query.GetNextAsTwinAsync();
            Console.WriteLine("Happy devices on DDDEastAnglia: {0}", 
                string.Join(", ", twinsInRedmond43UsingCellular.Select(t => t.DeviceId)));
        }
    }
}