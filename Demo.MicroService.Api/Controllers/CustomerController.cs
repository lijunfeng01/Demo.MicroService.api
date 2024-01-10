using Demo.MicroService.Models;
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
        // 分数范围的内联验证
        if (score < -1000 || score > 1000)
        {
            return BadRequest("无效的分数。分数必须在[- 1000,1000]范围内");
        }

        // 判断customerId的有效性
        if (customerId <= 0)
        {
            return BadRequest("无效的客户ID");
        }

        var updatedScore = await _customerService.UpdateCustomerScoreAsync(customerId, score);
        return Ok(updatedScore);
    }

    /// <summary>
    /// 按等级获取客户
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    [HttpGet("leaderboard")]
    public async Task<ActionResult<List<Customer>>> GetLeaderboard(int? start, int? end)
    {
        // 对排行榜范围的内联验证
        if (start.HasValue && start.Value < 1)
        {
            return BadRequest("起始位置必须大于0");
        }
        if (end.HasValue && end.Value < start)
        {
            return BadRequest("结束位置必须大于或等于开始位置");
        }

        var leaderboard = await _customerService.GetLeaderboardAsync(start, end);
        return Ok(leaderboard);
    }

    /// <summary>
    /// 通过customerid获取客户
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("leaderboard/{customerId}")]
    public async Task<ActionResult<List<Customer>>> GetCustomerById([FromRoute] long customerId,[FromQuery] int high = 0,[FromQuery] int low = 0)
    {
        // 判断customerId的有效性
        if (customerId <= 0)
        {
            return BadRequest("无效的客户ID");
        }
        
        if (high < 0)
        {
            return BadRequest("高邻居计数不能为负");
        }
        if (low < 0)
        {
            return BadRequest("低邻居计数不能为负");
        }
        var customerWithNeighbors = await _customerService.GetCustomerByIdAsync(customerId, high, low);
        return customerWithNeighbors;
    }
}
