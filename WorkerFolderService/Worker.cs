using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WorkerFolderService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private FileSystemWatcher watcher;
        private readonly string path = @"/Users/imac/Desktop/folder";  

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Changed += OnChanged;
            return base.StartAsync(cancellationToken);
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation("New file created at: {time}", DateTimeOffset.Now);
            SendMessage(e.FullPath);
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation("File deleted at: {time}", DateTimeOffset.Now);
            SendMessage(e.FullPath);
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            _logger.LogInformation("File renamed at: {time}", DateTimeOffset.Now);
            SendMessage(e.FullPath);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation("File changed at: {time}", DateTimeOffset.Now);
            SendMessage(e.FullPath);
        }

        public async Task SendMessage(string name)
        {
            var message = new
            {
                Type = "email",
                JsonContent = "File" + name + "added to queue"
            };

            var js = JsonConvert.SerializeObject(name);
            var data = new StringContent(js, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("http://localhost:31313/api/queue/add", data);
                string result = response.Content.ReadAsStringAsync().Result;
                _logger.LogInformation(result);
            }
        }
       

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                watcher.EnableRaisingEvents = true;
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
