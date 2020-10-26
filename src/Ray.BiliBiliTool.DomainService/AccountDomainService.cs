﻿using System;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Ray.BiliBiliTool.Agent.Dtos;
using Ray.BiliBiliTool.Agent.Interfaces;
using Ray.BiliBiliTool.DomainService.Interfaces;

namespace Ray.BiliBiliTool.DomainService
{
    public class AccountDomainService : IAccountDomainService
    {
        private readonly ILogger<AccountDomainService> _logger;
        private readonly IDailyTaskApi _dailyTaskApi;

        public AccountDomainService(ILogger<AccountDomainService> logger,
            IDailyTaskApi dailyTaskApi)
        {
            _logger = logger;
            _dailyTaskApi = dailyTaskApi;
        }

        public UseInfo LoginByCookie()
        {
            var apiResponse = _dailyTaskApi.LoginByCookie().Result;

            if (apiResponse.Code != 0 || !apiResponse.Data.IsLogin)
            {
                _logger.LogWarning("登录异常，Cookies可能失效了,请仔细检查Github Secrets中DEDEUSERID SESSDATA BILI_JCT三项的值是否正确");
                return null;
            }

            _logger.LogInformation("登录成功");

            UseInfo useInfo = apiResponse.Data;

            //用户名模糊处理
            _logger.LogInformation("用户名称: {0}", useInfo.GetFuzzyUname());
            _logger.LogInformation("硬币余额: " + useInfo.Money);

            if (useInfo.Level_info.Current_level < 6)
            {
                _logger.LogInformation("距离升级到Lv{0}还有: {1}天",
                    useInfo.Level_info.Current_level + 1,
                    (useInfo.Level_info.Next_exp - useInfo.Level_info.Current_exp) / 65);
            }
            else
            {
                _logger.LogInformation("当前等级Lv6，经验值为：" + useInfo.Level_info.Current_exp);
            }

            return useInfo;
        }

        /// <summary>
        /// 获取每日任务完成情况
        /// </summary>
        /// <returns></returns>
        public DailyTaskInfo GetDailyTaskStatus()
        {
            var apiResponse = _dailyTaskApi.GetDailyTaskRewardInfo().Result;
            if (apiResponse.Code == 0)
            {
                _logger.LogInformation("请求本日任务完成状态成功");
                //desp.appendDesp("请求本日任务完成状态成功");
                return apiResponse.Data;
            }
            else
            {
                _logger.LogDebug(JsonSerializer.Serialize(apiResponse));
                return _dailyTaskApi.GetDailyTaskRewardInfo().Result.Data;
                //todo:偶发性请求失败，再请求一次
            }
        }
    }
}
