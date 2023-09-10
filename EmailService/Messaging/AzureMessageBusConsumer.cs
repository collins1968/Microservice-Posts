using Azure.Messaging.ServiceBus;
using EmailService.Models;
using EmailService.Service;
using Newtonsoft.Json;
using System.Text;

namespace EmailService.Messaging
{
    public class AzureMessageBusConsumer : IAzureMessageBusConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly string Connectionstring;
        private readonly string QueueName;
        private readonly ServiceBusProcessor _registrationProcessor;
        private readonly EmailSendService _emailService;
        private readonly Emailservice _saveToDb;
        public AzureMessageBusConsumer(IConfiguration configuration, Emailservice service)
        {

            _configuration = configuration;
            Connectionstring = _configuration.GetSection("ServiceBus:ConnectionString").Get<string>();
            QueueName = _configuration.GetSection("Queues:RegisterUser").Get<string>();
            var serviceBusClient = new ServiceBusClient(Connectionstring);
            _registrationProcessor = serviceBusClient.CreateProcessor(QueueName);
            _emailService = new EmailSendService(_configuration);
            _saveToDb = service;

        }
        public async Task Start()
        {
            //start Processing
            _registrationProcessor.ProcessMessageAsync += OnRegistartion;
            await _registrationProcessor.StartProcessingAsync();

        }

        public async Task Stop()
        {
            await _registrationProcessor.StopProcessingAsync();
            await _registrationProcessor.DisposeAsync();
        }
        private async Task OnRegistartion(ProcessMessageEventArgs arg)
        {
            var message = arg.Message;

            var body = Encoding.UTF8.GetString(message.Body);

            var userMessage = JsonConvert.DeserializeObject<UserMessage>(body);

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<h1> Hello " + userMessage.Name + "</h1>");
                stringBuilder.AppendLine("<br/>Welcome to This Social App ");

                stringBuilder.Append("<br/>");
                stringBuilder.Append('\n');
                stringBuilder.Append("<p> Your registration was successfull</p>");
                var emailLogger = new EmailLoggers()
                {
                    Email = userMessage.Email,
                    Message = stringBuilder.ToString()

                };
                await _saveToDb.SaveData(emailLogger);
                await _emailService.SendEmail(userMessage, stringBuilder.ToString());
                //you can delete the message from the queue
                await arg.CompleteMessageAsync(message);
            }
            catch (Exception ex) {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }

    }
}
