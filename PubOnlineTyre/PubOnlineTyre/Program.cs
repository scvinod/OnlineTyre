using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using System;
using Newtonsoft.Json;

namespace PubOnlineTyre
{
    class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://onlinetyresbus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=hY9MY75ueEyw7MZZdZf1h9uDPDsyt22fRBrqh6/VEU8=";
        const string TopicName = "tyrestopics";
        static ITopicClient topicClient;

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after sending all the messages.");
            Console.WriteLine("======================================================");


            OnlineTyre onlTyre = new OnlineTyre()
            {
                TyreBrand = "MRF",
                TyreCount = 2,
                TyreSize = "XL"
            };

            string messageBody = JsonConvert.SerializeObject(onlTyre);

            Message message = new Message(Encoding.UTF8.GetBytes(messageBody));
            
            await topicClient.SendAsync(message);

            Console.WriteLine($"Sent message: {messageBody}");

            Console.ReadKey();

            await topicClient.CloseAsync();
            
        }

    }
}
