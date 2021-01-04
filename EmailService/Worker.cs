using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace EmailService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEmailSender _sender;

        public Worker(ILogger<Worker> logger, IEmailSender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("http://localhost:31313/api/queue/retrive/email");
                    string result = response.Content.ReadAsStringAsync().Result;
                    _logger.LogInformation(result);
                    try
                    {
                        var email = JsonConvert.DeserializeObject<MessageDTO>(result);
                        if (email != null)
                        {
                            var msg = new Message(new string[] { "jewandfriends@gmail.com" }, "Email Serivce", email.JsonContent, "");
                            await _sender.SendEmail(msg);
                            await client.GetAsync("http://localhost:31313/api/queue/retrive/email" + email.Id);
                        }
                    } catch (Exception ex)
                    {
                        _logger.LogInformation(ex.Message);
                    }
                }
                    await Task.Delay(30000, stoppingToken);
            }
        }
    }
}
