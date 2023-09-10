using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyMessageBus
{
    public class MessageBus : IMessageBus
    {
        public string ConnectionString = "Endpoint=sb://mycommerce.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=2jq1r3DZrcyoJzFPagW11yMEcVMc1UTS7+ASbJosnI0=";
        public async Task PublishMessage(object message, string topicName)
        {
            var serviceBus = new ServiceBusClient(ConnectionString);
            var sender = serviceBus.CreateSender(topicName);

            var messageJson = JsonConvert.SerializeObject(message);

            var theMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageJson))
            {

                CorrelationId = Guid.NewGuid().ToString(),
            };

            await sender.SendMessageAsync(theMessage);
            await serviceBus.DisposeAsync();
            
        }
    }
}
