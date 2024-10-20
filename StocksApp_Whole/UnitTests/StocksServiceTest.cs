using StocksApp_Whole.DTO;
using StocksApp_Whole.Entities;
using Xunit;
using StocksApp_Whole.ServiceContracts;
using StocksApp_Whole.Services;
using Moq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace StocksApp_Whole.UnitTests
{
    public class StocksServiceTest
    {
        private readonly IStocksService _stocksService;
        private readonly ITestOutputHelper _outputHelper;

        public StocksServiceTest(ITestOutputHelper testOutputHelper)
        {
            _stocksService = new StocksService();
            _outputHelper = testOutputHelper;
        }

        #region CreateBuyOrder() test cases

        [Fact]
        public async Task CreateBuyOrder_NullOrder()
        {
            // Arrange
            BuyOrderRequest? request = null;

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _stocksService.CreateBuyOrder(request);
            });
        }

        [Fact]
        public async Task CreateBuyOrder_NullStockSymbol()
        {
            // Arrange
            BuyOrderRequest? request = _stocksService.CreateMockBuyOrderRequest(); ;
            request.StockSymbol = null;

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(request);
            });
        }

        [Fact]
        public async Task CreateBuyOrder_QuantityTooLow()
        {
            BuyOrderRequest buyOrderRequest = _stocksService.CreateMockBuyOrderRequest();

            _outputHelper.WriteLine("Pre : " + buyOrderRequest.Quantity.ToString()); // 20

            buyOrderRequest.Quantity = 0;

            _outputHelper.WriteLine("Post : " + buyOrderRequest.Quantity.ToString()); // 0

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });        
        }

        [Fact]
        public async Task CreateBuyOrder_QuantityTooHigh()
        {
            BuyOrderRequest buyOrderRequest = _stocksService.CreateMockBuyOrderRequest();

            _outputHelper.WriteLine("Pre : " + buyOrderRequest.Quantity.ToString()); // 20

            buyOrderRequest.Quantity = 100000000;

            _outputHelper.WriteLine("Post : " + buyOrderRequest.Quantity.ToString()); // 100000000

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public async Task CreateBuyOrder_PriceTooLow()
        {
            BuyOrderRequest buyOrderRequest = _stocksService.CreateMockBuyOrderRequest();

            _outputHelper.WriteLine("Pre : " + buyOrderRequest.Price.ToString()); // 100

            buyOrderRequest.Price = 0;

            _outputHelper.WriteLine("Post : " + buyOrderRequest.Price.ToString()); // 0

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public async Task CreateBuyOrder_PriceTooHigh()
        {
            BuyOrderRequest buyOrderRequest = _stocksService.CreateMockBuyOrderRequest();

            _outputHelper.WriteLine("Pre : " + buyOrderRequest.Price.ToString()); // 100

            buyOrderRequest.Price = 1000000001;

            _outputHelper.WriteLine("Post : " + buyOrderRequest.Price.ToString()); // 1000000001

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public async Task CreateBuyOrder_InvalidDateandTime()
        {
            BuyOrderRequest buyOrderRequest = _stocksService.CreateMockBuyOrderRequest();

            buyOrderRequest.DateAndTimeOfOrder = DateTime.Parse("1991-02-01");

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public async Task CreateBuyOrder_ValidOrder()
        {
            BuyOrderRequest buyOrderRequest = _stocksService.CreateMockBuyOrderRequest();

            _outputHelper.WriteLine("BORequest: " + buyOrderRequest.ToString());

            BuyOrderResponse actualBuyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);

            _outputHelper.WriteLine("BOResponse: " + actualBuyOrderResponse.ToString());

            if (actualBuyOrderResponse != null)
            {
                Assert.True(actualBuyOrderResponse.BuyOrderID != Guid.Empty);
                // Can't trade nothing so fuck it;
                Assert.True(actualBuyOrderResponse.TradeAmount > 0);
            }           
        }
        #endregion

        #region CreateSellOrder() test cases

        [Fact]
        public async Task CreateSellOrder_NullOrder()
        {
            // Arrange
            SellOrderRequest? request = null;

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _stocksService.CreateSellOrder(request);
            });
        }

        [Fact]
        public async Task CreateSellOrder_NullStockSymbol()
        {
            // Arrange
            SellOrderRequest? request = _stocksService.CreateMockSellOrderRequest(); ;
            request.StockSymbol = null;

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(request);
            });
        }

        [Fact]
        public async Task CreateSellOrder_QuantityTooLow()
        {
            SellOrderRequest sellOrderRequest = _stocksService.CreateMockSellOrderRequest();

            _outputHelper.WriteLine("Pre : " + sellOrderRequest.Quantity.ToString()); // 20

            sellOrderRequest.Quantity = 0;

            _outputHelper.WriteLine("Post : " + sellOrderRequest.Quantity.ToString()); // 0

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public async Task CreateSellOrder_QuantityTooHigh()
        {
            SellOrderRequest sellOrderRequest = _stocksService.CreateMockSellOrderRequest();

            _outputHelper.WriteLine("Pre : " + sellOrderRequest.Quantity.ToString()); // 20

            sellOrderRequest.Quantity = 100000000;

            _outputHelper.WriteLine("Post : " + sellOrderRequest.Quantity.ToString()); // 100000000

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public async Task CreateSellOrder_PriceTooLow()
        {
            SellOrderRequest sellOrderRequest = _stocksService.CreateMockSellOrderRequest();

            _outputHelper.WriteLine("Pre : " + sellOrderRequest.Price.ToString()); // 100

            sellOrderRequest.Price = 0;

            _outputHelper.WriteLine("Post : " + sellOrderRequest.Price.ToString()); // 0

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public async Task CreateSellOrder_PriceTooHigh()
        {
            SellOrderRequest sellOrderRequest = _stocksService.CreateMockSellOrderRequest();

            _outputHelper.WriteLine("Pre : " + sellOrderRequest.Price.ToString()); // 100

            sellOrderRequest.Price = 1000000001;

            _outputHelper.WriteLine("Post : " + sellOrderRequest.Price.ToString()); // 1000000001

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public async Task CreateSellOrder_InvalidDateandTime()
        {
            SellOrderRequest sellOrderRequest = _stocksService.CreateMockSellOrderRequest();

            sellOrderRequest.DateAndTimeOfOrder = DateTime.Parse("1991-02-01");

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public async Task CreateSellOrder_ValidOrder()
        {
            SellOrderRequest sellOrderRequest = _stocksService.CreateMockSellOrderRequest();

            _outputHelper.WriteLine("SORequest: " + sellOrderRequest.ToString());

            SellOrderResponse actualSellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);

            _outputHelper.WriteLine("SOResponse: " + actualSellOrderResponse.ToString());

            if (actualSellOrderResponse != null)
            {
                Assert.True(actualSellOrderResponse.SellOrderID != Guid.Empty);
                // Can't trade nothing so fuck it;
                Assert.True(actualSellOrderResponse.TradeAmount > 0);
            }
        }
        #endregion

        #region GetBuyOrders() test cases

        [Fact]
        public async Task GetBuyOrders_EmptyList()
        {
            // Arrange and Act
            List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetBuyOrders();

            foreach(BuyOrderResponse bor in buyOrderResponses)
            {
                _outputHelper.WriteLine("BuyOrderResponse : " + bor.ToString() + "\n");
            }

            // Assert
            Assert.Empty(buyOrderResponses);
        }

        [Fact]
        public async Task GetBuyOrders_ValidList()
        {
            // Arrange
            List<BuyOrderRequest> buyOrderRequests = _stocksService.CreateManyMockBuyOrderRequests();
            List<BuyOrderResponse> expectedBuyOrderResponses = new List<BuyOrderResponse>();

            // Act
            // Convert each boRequest to boResponse
            foreach(BuyOrderRequest bor in buyOrderRequests)
            {
                expectedBuyOrderResponses.Add(await _stocksService.CreateBuyOrder(bor));
            }

            _outputHelper.WriteLine("Expected:");
            foreach(BuyOrderResponse bor in expectedBuyOrderResponses)
            {
                _outputHelper.WriteLine(bor.ToString() + "\n");
            }

            // Use actual method
            List<BuyOrderResponse> actualBuyOrderResponses = await _stocksService.GetBuyOrders();

            _outputHelper.WriteLine("Actual:");
            foreach (BuyOrderResponse bor in actualBuyOrderResponses)
            {
                _outputHelper.WriteLine(bor.ToString() + "\n");
            }

            // Assert
            foreach(BuyOrderResponse bor in expectedBuyOrderResponses)
            {
                Assert.Contains(bor, actualBuyOrderResponses);
            }

        }

        #endregion

        #region GetSellOrders() test cases

        [Fact]
        public async Task GetSellOrders_EmptyList()
        {
            // Arrange and Act
            List<SellOrderResponse> sellOrderResponses = await _stocksService.GetSellOrders();

            foreach (SellOrderResponse sor in sellOrderResponses)
            {
                _outputHelper.WriteLine("BuyOrderResponse : " + sor.ToString() + "\n");
            }

            // Assert
            Assert.Empty(sellOrderResponses);
        }

        [Fact]
        public async Task GetSellOrders_ValidList()
        {
            // Arrange
            List<SellOrderRequest> sellOrderRequests = _stocksService.CreateManyMockSellOrderRequests();
            List<SellOrderResponse> expectedSellOrderResponses = new List<SellOrderResponse>();

            // Act
            // Convert each boRequest to boResponse
            foreach (SellOrderRequest sor in sellOrderRequests)
            {
                expectedSellOrderResponses.Add(await _stocksService.CreateSellOrder(sor));
            }

            _outputHelper.WriteLine("Expected:");
            foreach (SellOrderResponse sor in expectedSellOrderResponses)
            {
                _outputHelper.WriteLine(sor.ToString() + "\n");
            }

            // Use actual method
            List<SellOrderResponse> actualSellOrderResponses = await _stocksService.GetSellOrders();

            _outputHelper.WriteLine("Actual:");
            foreach (SellOrderResponse sor in actualSellOrderResponses)
            {
                _outputHelper.WriteLine(sor.ToString() + "\n");
            }

            // Assert
            foreach (SellOrderResponse sor in expectedSellOrderResponses)
            {
                Assert.Contains(sor, actualSellOrderResponses);
            }
        }
        #endregion
    }
}
