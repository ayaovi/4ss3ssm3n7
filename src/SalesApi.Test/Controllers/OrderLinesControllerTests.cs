using System;
using System.Collections.Generic;
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
  class OrderLinesControllerTests
  {
    [Test]
    public void Get_GivenValidOrderId_ExpectResult()
    {
      //Arrange
      var orderId = new Guid();
      var order = new Order
      {
        Id = orderId
      };
      var orderLines = new List<OrderLine>
      {
        new OrderLine
        {
          Id = new Guid(),
          Order = order
        },
        new OrderLine
        {
          Id = new Guid(),
          Order = order
        }
      };
      var repository = Substitute.For<ISalesRepository>();
      repository.GetOrderLinesByOrderId(orderId).Returns(orderLines);
      var controller = new OrderLinesController(repository);

      //Act
      var result = controller.Get(orderId) as OkObjectResult;
      var value = result?.Value;

      //Assert
      Assert.IsAssignableFrom<List<OrderLine>>(value);
      value.Should().BeEquivalentTo(orderLines);
    }

    [Test]
    public void Get_GivenNonExistingOrderId_ExpectResult()
    {
      //Arrange
      var orderId = Guid.NewGuid();
      var errorMessage = $"There is no order with id {orderId}.";
      var repository = Substitute.For<ISalesRepository>();
      repository.GetOrderLinesByOrderId(Arg.Any<Guid>()).ThrowsForAnyArgs(new Exception());
      var controller = new OrderLinesController(repository);

      //Act
      var result = controller.Get(orderId);
      var objectResult = result as NotFoundObjectResult;

      //Assert
      Assert.IsInstanceOf<NotFoundObjectResult>(result);
      Assert.AreEqual(objectResult?.Value, errorMessage);
    }
  }
}
