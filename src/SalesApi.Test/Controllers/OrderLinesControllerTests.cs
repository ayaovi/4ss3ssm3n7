using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using SalesApi.Controllers;
using SalesApi.Models;
using SalesApi.Persistence;

namespace SalesApi.Test.Controllers
{
  [TestFixture]
  public class OrderLinesControllerTests
  {
    [Test]
    public async Task Get_GivenValidOrderId_ExpectResult()
    {
      //Arrange
      var orderId = Guid.NewGuid();
      var order = new Order
      {
        Id = orderId
      };
      var orderLines = new List<OrderLine>
      {
        new OrderLine
        {
          Id = Guid.NewGuid(),
          Order = order
        },
        new OrderLine
        {
          Id = Guid.NewGuid(),
          Order = order
        }
      };
      var repository = Substitute.For<ISalesRepository>();
      repository.GetOrderLinesByOrderIdAsync(orderId).Returns(orderLines);
      var controller = new OrderLinesController(repository);

      //Act
      var result = await controller.Get(orderId) as OkObjectResult;
      var value = result?.Value;

      //Assert
      Assert.IsAssignableFrom<List<OrderLine>>(value);
      value.Should().BeEquivalentTo(orderLines);
    }

    [Test]
    public async Task Get_GivenNonExistingOrderId_ExpectResult()
    {
      //Arrange
      var orderId = Guid.NewGuid();
      var errorMessage = $"There is no order with id {orderId}.";
      var repository = Substitute.For<ISalesRepository>();
      repository.GetOrderLinesByOrderIdAsync(Arg.Any<Guid>()).ThrowsForAnyArgs(new Exception());
      var controller = new OrderLinesController(repository);

      //Act
      var result = await controller.Get(orderId);
      var objectResult = result as NotFoundObjectResult;

      //Assert
      Assert.IsInstanceOf<NotFoundObjectResult>(result);
      Assert.AreEqual(objectResult?.Value, errorMessage);
    }
  }
}
