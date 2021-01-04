using System;
using System.Linq;
using System.Threading.Tasks;
using assgiment2.Infrastructure.context;
using assigment2.Core.Interfaces;
using assigment2.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace assgiment2.Infrastructure.Repositories
{
    public class QueueRepository : IQueueRepository
    {
        private readonly DataContext _context;

        public QueueRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> AddMessage(Message message)
        {
            _context.Messages.Add(message);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Message> UnhandledEmail()
        {
            var message = await _context.Messages.Where(x => !x.Handled && x.Type == "email").FirstOrDefaultAsync();
            return message;
        }

        public async Task<Message> UnhandledLog()
        {
            var message = await _context.Messages.Where(x => !x.Handled && x.Type == "log").FirstOrDefaultAsync();
            return message;
        }

        public async Task<bool> SetHandled(Guid messageId)
        {
            var message = _context.Messages.Where(x => x.Id == messageId).FirstOrDefault();
            if (message == null)
            {
                return false;
            }
            message.Handled = true;
            message.HandleTime = DateTime.Now;
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
