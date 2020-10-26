using DailyTaskTest.Share;
using Microsoft.Extensions.DependencyInjection;
using Ray.BiliBiliTool.Console;
using Ray.BiliBiliTool.DomainService.Interfaces;
using Xunit;

namespace WatchVideoTest
{
    public class WatchVideo
    {
        [Fact]
        public void Test1()
        {
            Program.PreWorks(new string[] { });

            using (var scope = Program.ServiceProviderRoot.CreateScope())
            {
                var dailyTask = scope.ServiceProvider.GetRequiredService<IVideoDomainService>();

                string aid = dailyTask.GetRandomVideo();
                dailyTask.WatchVideo(aid);

                Assert.True(true);
            }
        }
    }
}
