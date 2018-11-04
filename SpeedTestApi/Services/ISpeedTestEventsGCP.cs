using System.Threading.Tasks;
using SpeedTestApi.Models;

namespace SpeedTestApi.Services
{
    public interface ISpeedTestEventsGCP
    {
        Task PublishSpeedTest(TestResult SpeedTest);
    }
}