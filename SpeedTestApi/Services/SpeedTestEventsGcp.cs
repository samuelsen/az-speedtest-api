using System;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.PubSub.V1;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using SpeedTestApi.Models;

namespace SpeedTestApi.Services
{
    public class SpeedTestEventsGcp : ISpeedTestEventsGCP, IDisposable
    {
        private readonly PublisherClient _client;

        public SpeedTestEventsGcp(string projectId, string topicId)
        {
            PublisherServiceApiClient publisherClient = PublisherServiceApiClient.Create();
            PublisherClient publisher = PublisherClient.CreateAsync(
                new TopicName(projectId, topicId)).GetAwaiter().GetResult();

            _client = publisher;
        }

        public async Task PublishSpeedTest(TestResult speedTest)
        {
            var message = JsonConvert.SerializeObject(speedTest);
            await _client.PublishAsync(message);
        }

        public void Dispose()
        {
            //_client.ShutdownAsync().GetAwaiter().GetResult();
        }
    }
}