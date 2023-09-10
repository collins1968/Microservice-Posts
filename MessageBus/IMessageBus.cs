using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMessageBus
{
     public interface IMessageBus
    {
        Task PublishMessage(object message, string topicName);
    }
}
