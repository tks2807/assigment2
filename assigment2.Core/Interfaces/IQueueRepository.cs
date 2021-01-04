using System;
using System.Threading.Tasks;
using assigment2.Core.Models;

namespace assigment2.Core.Interfaces
{
    public interface IQueueRepository
    {
        Task<bool> AddMessage(Message message);
        Task<bool> SetHandled(Guid messageId);
        Task<Message> UnhandledEmail();
        Task<Message> UnhandledLog();
    } 
}
