using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpeedTestApi.Models;
using SpeedTestApi.Services;
using Google.Cloud.PubSub.V1;


namespace SpeedTestApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SpeedTestController : ControllerBase
    {
        private readonly ISpeedTestEvents _eventHub;
        private readonly ISpeedTestEventsGCP _gcp;

        public SpeedTestController(ISpeedTestEvents eventHub, ISpeedTestEventsGCP gcp)
        {
            _eventHub = eventHub;
            _gcp = gcp;
        }

        [Route("ping")]
        [HttpGet]
        public ActionResult<string> Ping()
        {
            return Ok("PONG");
        }

        [HttpPost]
        public async Task<ActionResult<string>> UploadSpeedTest([FromBody] TestResult speedTest)
        {
            var speedTestData = $"Got a TestResult from { speedTest.User } with download { speedTest.Data.Speeds.Download } Mbps.";
            speedTest.Data.Server.Distance = ( (double) ( (int) (speedTest.Data.Server.Distance * 1000.0) ) ) / 1000.0 ;

            Console.WriteLine(speedTestData);

            await _eventHub.PublishSpeedTest(speedTest);
            await _gcp.PublishSpeedTest(speedTest);

            return Ok(speedTestData);
        }

        [HttpPost]
        [Route("azure")]
        public async Task<ActionResult<string>> UploadSpeedTestAzure([FromBody] TestResult speedTest)
        {
            var speedTestData = $"Got a TestResult from { speedTest.User } with download { speedTest.Data.Speeds.Download } Mbps.";

            speedTest.Data.Server.Distance = ((double)((int)(speedTest.Data.Server.Distance * 1000.0))) / 1000.0;

            Console.WriteLine(speedTestData);

            await _eventHub.PublishSpeedTest(speedTest);

            return Ok(speedTestData);
        }

        [Route("gcp")]
        [HttpPost]
        public async Task<ActionResult<string>> UploadSpeedTestGCP([FromBody] TestResult speedTest)
        {
            var speedTestData = $"Got a TestResult from { speedTest.User } with download { speedTest.Data.Speeds.Download } Mbps.";
            speedTest.Data.Server.Distance = ((double)((int)(speedTest.Data.Server.Distance * 1000.0))) / 1000.0;

            Console.WriteLine(speedTestData);

            await _gcp.PublishSpeedTest(speedTest);

            return Ok(speedTestData);
        }
    }
}
