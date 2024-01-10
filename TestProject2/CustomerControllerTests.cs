using Moq;
using Microsoft.AspNetCore.Mvc;
using Demo.MicroService.IService;
using Demo.MicroService.Models;

namespace TestProject2
{
    public class CustomerControllerTests
    {
        /// <summary>
        /// ��Ч����ʱ�����������Ƿ񷵻�BadRequest
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateScore_WithInvalidScore_ShouldReturnBadRequest()
        {
            // Arrange
            var mockService = new Mock<ICustomerService>();
            var controller = new CustomerController(mockService.Object);

            // Act
            var result = await controller.UpdateScore(1, 2000); // Score outside valid range

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        /// <summary>
        /// ��������Ч����ʱ�����Ǽ������������Ƿ񷵻ؾ�����ȷֵ��OkObjectResult
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateScore_WithValidScore_ShouldReturnUpdatedScore()
        {
            // Arrange
            var mockService = new Mock<ICustomerService>();
            decimal scoreToUpdate = 40;
            mockService.Setup(s => s.UpdateCustomerScoreAsync(It.IsAny<long>(), It.IsAny<decimal>()))
                .ReturnsAsync(scoreToUpdate);

            var controller = new CustomerController(mockService.Object);

            // Act
            var result = await controller.UpdateScore(38819, scoreToUpdate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(scoreToUpdate, okResult.Value);
        }

        // ʹ����Ч��Χ����GetLeaderboard
        [Fact]
        public async Task GetLeaderboard_WithValidRange_ShouldReturnLeaderboard()
        {
            // Arrange
            var mockService = new Mock<ICustomerService>();
            var leaderboard = new List<Customer>
            {
                new Customer { CustomerId = 76786448, Score = 78, Rank = 41 },
                new Customer { CustomerId = 254814111, Score = 65, Rank = 42 },
                new Customer { CustomerId = 53274324, Score = 64, Rank = 43 },
                new Customer { CustomerId = 6144320, Score = 32, Rank = 44 },
            };
            mockService.Setup(s => s.GetLeaderboardAsync(It.IsAny<int?>(), It.IsAny<int?>()))
                       .ReturnsAsync(leaderboard);
            var controller = new CustomerController(mockService.Object);

            // Act
            var result = await controller.GetLeaderboard(41, 44);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedLeaderboard = Assert.IsType<List<Customer>>(okResult.Value);
            Assert.Equal(leaderboard.Count, returnedLeaderboard.Count);
        }

        // ʹ����Ч����ʼ��������GetLeaderboard
        [Fact]
        public async Task GetLeaderboard_WithInvalidStart_ShouldReturnBadRequest()
        {
            // Arrange
            var mockService = new Mock<ICustomerService>();
            var controller = new CustomerController(mockService.Object);

            // Act
            var result = await controller.GetLeaderboard(-1, 10); // ������Ч

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        // ʹ��һ�����ڵĿͻ�����GetCustomerById
        [Fact]
        public async Task GetCustomerById_WithExistingCustomer_ShouldReturnCustomerWithNeighbors()
        {
            // Arrange
            var mockService = new Mock<ICustomerService>();
            long customerId = 6144320;
            int high = 2;
            int low = 2;
            var customerWithNeighbors = new List<Customer>
            {
                // Add customers that would represent neighbors
            };

            mockService.Setup(s => s.GetCustomerByIdAsync(customerId, high, low))
                       .ReturnsAsync(customerWithNeighbors);

            var controller = new CustomerController(mockService.Object);

            // Act
            var result = await controller.GetCustomerById(customerId, high, low);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCustomerWithNeighbors = Assert.IsType<List<Customer>>(okResult.Value);
            Assert.Equal(customerWithNeighbors.Count, returnedCustomerWithNeighbors.Count);
        }
    }
}