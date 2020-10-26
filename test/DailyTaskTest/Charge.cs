using System;
using DailyTaskTest.Share;
using Microsoft.Extensions.DependencyInjection;
using Ray.BiliBiliTool.Console;
using Ray.BiliBiliTool.DomainService.Interfaces;
using Xunit;

namespace DailyTaskTest
{
    public class Charge
    {
        [Fact]
        public void Test1()
        {
            var dailyTaskAppService = DailyTaskBuilder.Build();



            Program.PreWorks(new string[] { });

            using (var scope = Program.ServiceProviderRoot.CreateScope())
            {
                var dailyTask = scope.ServiceProvider.GetRequiredService<IChargeDomainService>();
                var accountService = scope.ServiceProvider.GetRequiredService<IAccountDomainService>();

                var userInfo = accountService.LoginByCookie();
                dailyTask.Charge(userInfo);

                Assert.True(true);
            }
        }
    }
}
