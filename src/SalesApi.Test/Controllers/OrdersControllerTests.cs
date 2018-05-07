using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SalesApi.Controllers;
using SalesApi.Models;
using SalesApi.Persistence;

namespace SalesApi.Test.Controllers
{
  [TestFixture]
  public class OrdersControllerTests
  {
    [Test]
    public async Task Get_GivenExistingOrders_ExpectResult()
    {
      //Arrange
      var guid1 = new Guid();
      var guid2 = new Guid();
      var orders = new List<Order>
      {
        new Order
        {
          Client = new Client{Id = 1, FirstName = "John", LastName = "Smith"},
          Id = guid1,
          OrderLines = new List<OrderLine>
          {
            new OrderLine
            {
              Id = guid2,
              Item = new Item
              {
                Description = "some item",
                Id = 1,
                Material = new Material()
              }
            }
          }
        }
      };

      var repository = Substitute.For<ISalesRepository>();
      repository.GetOrdersAsync().Returns(orders);
      var controller = new OrdersController(repository);

      //Act
      var result = await controller.Get() as OkObjectResult;
      var value = result?.Value;

      //Assert
      Assert.IsAssignableFrom<List<Order>>(value);
      value.Should().BeEquivalentTo(orders);
    }

    [Test]
    public async Task Post_GivenValidOrderRequest_ExpectOrderToBeAdded()
    {
      //Arrange
      var repository = Substitute.For<ISalesRepository>();
      repository.When(x => x.AddOrderAsync(Arg.Any<OrderRequest>())).Do(x => {});
      var controller = new OrdersController(repository);

      //Act
      var result = await controller.Post(new OrderRequest());
    
      //Assert
      Assert.IsInstanceOf<OkResult>(result);
    }

    [Test]
    public async Task Post_GivenNonExistingClientIdInOrderRequest_ExpectNotFound()
    {
      //Arrange
      const string errorMessage = "Client with 1 does not exist.";
      var repository = Substitute.For<ISalesRepository>();
      repository.When(x => x.AddOrderAsync(Arg.Any<OrderRequest>())).Throw(new Exception(errorMessage));
      var controller = new OrdersController(repository);

      //Act
      var result = await controller.Post(new OrderRequest());
      var objectResult = result as NotFoundObjectResult;

      //Assert
      Assert.IsInstanceOf<NotFoundObjectResult>(result);
      Assert.AreEqual(objectResult?.Value, errorMessage);
    }

    [Test]
    public async Task Post_GivenNonExistingItemInOrderRequest_ExpectNotFound()
    {
      //Arrange
      const string errorMessage = "Some of the specified item(s) does not exist.";
      var repository = Substitute.For<ISalesRepository>();
      repository.When(x => x.AddOrderAsync(Arg.Any<OrderRequest>())).Throw(new Exception(errorMessage));
      var controller = new OrdersController(repository);

      //Act
      var result = await controller.Post(new OrderRequest());
      var objectResult = result as NotFoundObjectResult;

      //Assert
      Assert.IsInstanceOf<NotFoundObjectResult>(result);
      Assert.AreEqual(objectResult?.Value, errorMessage);
    }
  }
}