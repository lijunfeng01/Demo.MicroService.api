﻿using Demo.MicroService.Models;
using Demo.MicroService.IService;
using Microsoft.AspNetCore.Mvc;

/// <summary>
///不需要持久性和数据库。仅在内存中的状态。
///只使用。net Core。不要使用外部框架/程序集, 除非可能用于测试。
///服务需要能够处理大量同时发生的请求
///服务需要能够处理大量客户, 因此要记住复杂性, 特别是频繁操作的时间复杂性。
/// 1.异步处理：使用异步方法和异步操作提高系统的并发处理能力。
/// 2.加入缓存机制 
/// </summary>
[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    /// 更新评分
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="score"></param>
    /// <returns></returns>
    [HttpPost("customer/{customerId}/score/{score}")]
    public async Task<ActionResult<decimal>> UpdateScore(long customerId, decimal score)
    {
        if (score < -1000 || score > 1000)
        {
            return BadRequest("Invalid score. Score must be in the range [-1000, 1000]");
        }

        var updatedScore = await _customerService.UpdateCustomerScoreAsync(customerId, score);
        return updatedScore;
    }

    /// <summary>
    /// 按等级获取客户
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    [HttpGet("leaderboard")]
    public async Task<ActionResult<List<Customer>>> GetLeaderboard(int start, int? end)
    {
        var leaderboard = await _customerService.GetLeaderboardAsync(start, end);
        return leaderboard;
    }

    /// <summary>
    /// 通过customerid获取客户
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("leaderboard/{customerId}")]
    public async Task<ActionResult<List<Customer>>> GetCustomerById(long customerId, int high = 0, int low = 0)
    {
        var customerWithNeighbors = await _customerService.GetCustomerByIdAsync(customerId, high, low);
        return customerWithNeighbors;
    }
}
