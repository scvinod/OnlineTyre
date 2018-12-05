using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using System;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;

namespace PubOnlineTyre
{
    class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://onlinetyresbus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=hY9MY75ueEyw7MZZdZf1h9uDPDsyt22fRBrqh6/VEU8=";
        const string TopicName = "tyrestopics";
        static ITopicClient topicClient;

        static void Main(string[] args)
        {
            AddToServiceBus().GetAwaiter().GetResult();
        }

        static async Task AddToServiceBus()
        {
            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after sending all the messages.");
            Console.WriteLine("======================================================");

            OnlineTyreOrder onlTyre = new OnlineTyreOrder()
            {
                TyreBrand = "MRF",
                TyreCount = 2,
                TyreSize = "XL",
                CustomerEMail = "s.c.vinod@hotmail.com"
            };

            string messageBody = JsonConvert.SerializeObject(onlTyre);
            Message message = new Message(Encoding.UTF8.GetBytes(messageBody));
            await topicClient.SendAsync(message);
            Console.WriteLine($"Sent message: {messageBody}");
            Console.ReadKey();
            await topicClient.CloseAsync();
        }

        static async Task AddToCosmosDB()
        {
            try
            {
                DocumentClient client = new DocumentClient(new Uri("https://onlinetyrecosmos.documents.azure.com:443/"), "8pBfpznHAeoBbdRkdgxFneEqcnBkcNGDdDkRLOF8eQWe6spyNWEO7KwGrKouAHDwPsUPh5cewwIk35gnm2uQFA==");
                OnlineTyreProd onlineTyreProd = new OnlineTyreProd()
                {
                    TyreBrand = "MRF",
                    TyreId = 2,
                    TyreSize = "XXL"
                };
                Uri collUri = UriFactory.CreateDocumentCollectionUri("OnlineTyresDB", "OnlineTyreInventory");
                //var options = new RequestOptions { PreTriggerInclude = new[] { "ValidateSupplierName" } };
                ResourceResponse<Document> result = await client.CreateDocumentAsync(collUri, onlineTyreProd);
                var document = result.Resource;
            }

            catch (Exception ex)
            {

            }
        }
    }
}
