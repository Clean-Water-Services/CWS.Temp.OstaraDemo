using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using OstaraDemo.Models;
using Serilog;

namespace OstaraDemo {

    class Program {

        static async Task Main(string[] args) {

            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            ILogger log = Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(GetLogFolder(), rollingInterval: RollingInterval.Day)
                .CreateLogger();


            string sbConnectionString = config.GetConnectionString("ServiceBus")
                .Replace("{{SHARED_SERVICEBUS_KEY}}", Environment.GetEnvironmentVariable("SHARED_SERVICEBUS_KEY"));

            // The TransportType may need to be set to "ServiceBusTransportType.AmqpWebSockets" depending on your corporate firewall configuration
            var queueClient = new ServiceBusClient(sbConnectionString, new ServiceBusClientOptions{ TransportType = ServiceBusTransportType.AmqpTcp });
            var receiver = queueClient.CreateReceiver(config.GetValue<string>("IncomingQueueName"));
            var sender = queueClient.CreateSender(config.GetValue<string>("OutgoingQueueName"));
            
            log.Information("Receiving...");

            ServiceBusReceivedMessage msg;
            int numberOfBagsReceived = 0;

            do {
                msg = await receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(5));

                if (msg != null) {
                    var bag = JsonSerializer.Deserialize<Bag>(msg.Body.ToString());
                    log.Information("Received bag {0}", bag.BagNumber);

                    // TODO: Process the bag somehow

                    // Once the message is completed, it will be permanently removed from the queue.
                    await receiver.CompleteMessageAsync(msg);
                    numberOfBagsReceived++;
                }

            } while (msg != null);

            log.Information("Received {0} bags.", numberOfBagsReceived);

            // TODO: Do something to get a list of updated bags
            var updatedBags = new Bag[]{};
            int numberOfBagsSent = 0;

            foreach (var bag in updatedBags) {
                log.Information("Sending bag {0}", bag.BagNumber);
                await sender.SendMessageAsync(new ServiceBusMessage(JsonSerializer.Serialize(bag)) { ContentType = "application/json" });
                numberOfBagsSent++;                
            }

            log.Information("Sent {0} bags.", numberOfBagsSent);
        }

        static string GetLogFolder() {
            
            string baseFolder = (Environment.OSVersion.Platform == PlatformID.Win32NT)
                ? Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
                : Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            return String.Join(Path.DirectorySeparatorChar, baseFolder, "OstaraDemo", "log", "OstaraDemo.log");
        }
    }
}
