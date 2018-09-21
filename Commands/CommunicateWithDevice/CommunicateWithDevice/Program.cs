using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace CommunicateWithDevice
{
    class Program
    {
        static ServiceClient serviceClient;
        static string connectionString = "";
        private static string deviceId = "test";
        
        static async Task Main(string[] args)
        {
            Console.WriteLine("Send Cloud-to-Device message\n");
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
            //ReceiveFeedbackAsync();
            await SendCloudToDeviceMessageAsync();
            Console.ReadLine();
        }
        
        private static async Task SendCloudToDeviceMessageAsync()
        {
            Console.WriteLine("What to send?");
            var message = Console.ReadLine();
            var commandMessage = new 
                Message(Encoding.ASCII.GetBytes(message));
            //commandMessage.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync(deviceId, commandMessage);
        }
        
        private async static void ReceiveFeedbackAsync()
        {
            var feedbackReceiver = serviceClient.GetFeedbackReceiver();

            Console.WriteLine("\nReceiving c2d feedback from service");
            while (true)
            {
                var feedbackBatch = await feedbackReceiver.ReceiveAsync();
                if (feedbackBatch == null) continue;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Received feedback: {0}", 
                    string.Join(", ", feedbackBatch.Records.Select(f => f.StatusCode)));
                Console.ResetColor();

                await feedbackReceiver.CompleteAsync(feedbackBatch);
            }
        }

    }
}