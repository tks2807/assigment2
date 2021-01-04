using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using assigment2.Core.DTOs;
using assigment2.Core.Interfaces;
using assigment2.Core.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace assigment2
{
    [Route("api/queue")]
    public class QueueController : ControllerBase
    {
        private readonly IQueueRepository _queueRepository;

        public QueueController(IQueueRepository queueRepository)
        {
            _queueRepository = queueRepository;
        }

        [HttpPost("add")]
        public async Task<ActionResult> Add(MessageDTO msg)
        {
            Message m = new Message()
            {
                Id = Guid.NewGuid(),
                Type = msg.Type,
                Handled = false,
                JsonContent = msg.JsonContent,
                AddTime = DateTime.Now
            };

            if (await _queueRepository.AddMessage(m))
            {
                return Ok("New message added");
            }

            return BadRequest("ERROR!");

        }

        [HttpGet("handled/{messageId}")]
        public async Task<ActionResult> SetHandled(Guid msgId)
        {
            if (await _queueRepository.SetHandled(msgId))
            {
                return Ok("New message added");
            }

            return BadRequest("ERROR!");
        }

        [HttpGet("retrieve/email")]
        public async Task<ActionResult> UnhandledEmail()
        {
            var msg = await _queueRepository.UnhandledEmail();
            return Ok(msg);
        }

        [HttpGet("retrieve/log")]
        public async Task<ActionResult> UnhandledLog()
        {
            var msg = await _queueRepository.UnhandledLog();
            return Ok(msg);
        }
    }
}
