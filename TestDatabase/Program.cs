using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Subscribing;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Extensions.ManagedClient;

namespace TestDatabase
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("Test_application")
                .WithTcpServer("eu1.cloud.thethings.network",8883)
                .WithCredentials("project-software-engineering@ttn", "NNSXS.DTT4HTNBXEQDZ4QYU6SG73Q2OXCERCZ6574RVXI.CQE6IG6FYNJOO2MOFMXZVWZE4GXTCC2YXNQNFDLQL4APZMWU6ZGA")
                .WithTls()
                .WithCleanSession()
                .Build();

            await mqttClient.ConnectAsync(options, CancellationToken.None);
            await mqttClient.SubscribeAsync(new MqttClientSubscribeOptionsBuilder().WithTopicFilter("v3/project-software-engineering@ttn/devices/py-saxion/up").Build());
            
            
            Console.WriteLine("Hello World!");

            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();
            });

            // Here to make program run until user stops it
            // changed.
            Console.ReadLine();
        }
    }
}
