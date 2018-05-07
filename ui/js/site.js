(function () {
  const app = angular.module("myApp", []);
  const apiBaseUrl = 'http://localhost:55497/';
  const ordersUri = "api/orders";
  const orderlinesUri = "api/orderlines";
  const errorMessage = function (data, status) {
    return `Error: ${status}${data.Message !== undefined ? (` ${data.Message}`) : ""}`;
  };

  app.controller("myCtrl", ["$http", "$scope", function ($http, $scope) {
    let today = new Date();
    $scope.orders = [{
      id: 1,
      date: today.getDate(),
      client: {
        id: 1
      },
      orderLines: [
        {
          id: 1,
        },
        {
          id: 2
        }
      ]
    }];
    
    $scope.orderlines = [{
      id: 1,
      quantity: 2,
      item: {
        id: 1,
        description: "An item"
      },
      order: {
        id: 1
      }
    }];
    
    let counter = 1;
    
    $scope.FetchOrders = async function () {
//      let result = await $scope.FetchData(`${apiBaseUrl}${ordersUri}`);
//      if (result != undefined) {
//        $scope.orders = result;
//      }
      $http.get(`${apiBaseUrl}/${ordersUri}`)
        .success(function (data, _) {
          $scope.orders = data;
        })
        .error(function (data, status) {
          $scope.errorToSearch = errorMessage(data, status);
        });
    }
    
    $scope.FetchOrderLines = function (orderId) {
      $http.get(`${apiBaseUrl}/${orderlinesUri}/${orderId}`)
        .success(function (data, _) {
          $scope.orderlines = data;
        })
        .error(function (data, status) {
          $scope.errorToSearch = errorMessage(data, status);
        });
    }

//    $scope.FetchData = function (url) {
//      $http.get(url)
//        .success(function (data, _) {
//          return data;
//        })
//        .error(function (data, status) {
//          //TODO log error.
//          return undefined;
//        });
//    }
    
    $scope.ShowOrderLines = function (orderId) {
      currentOrderId = orderId;
      $scope.FetchOrderLines(orderId);
      document.getElementById("myModal").style.display = "block";
      document.getElementsByClassName("close")[0].style.display = "block";
    }
    
    $scope.AddOrderLineRow = function () {
      let tableRef = document.getElementById('ordLineTbl').getElementsByTagName('tbody')[0];
      
      let newRow   = tableRef.insertRow(tableRef.rows.length); /* Insert a row in the table at the last row */
      
      let orderLineId = newRow.insertCell(0);
      let itemId = newRow.insertCell(1);
      let orderId = newRow.insertCell(2);
      let qty = newRow.insertCell(3);
      
      orderLineId.innerHTML = "----";
      itemId.innerHTML = `<input id=\"itemId${counter}\" type=\"number\" placeholder=\"1\"/>`;
      orderId.innerHTML = `${$scope.orderlines[0].order.id}`;
      qty.innerHTML = `<input id=\"quantity${counter}\" type=\"number\" placeholder=\"1\"/>`;
      counter = counter + 1;  /* increment counter for next row to be added. */
    }
    
    $scope.SaveOrderEdit = function () {
      let table = document.getElementById("ordLineTbl");  /* our table from the DOM. */
      
      /* this is the order request we are making. */
      let order = {
        ClientId: $scope.orders[0].client.id,
        OrderId: $scope.orderlines[0].order.id,
        OrderLineRequests: []
      }
      
      for (var i = 0; i < counter; i++) {
        let itemId = document.getElementById(`itemId${i}`);
        let quantity = document.getElementById(`quantity${i}`);
        
        let orderline = {
          Item: {
            Id: itemId.value,
          },
          Quantity: quantity.value
        };
        
        if (i < Object.keys($scope.orderlines).length) {
          orderline.OrderLineId = $scope.orderlines[i].id;
        }
        order.OrderLineRequests.push(orderline);
      }
      
      $http.put(`${apiBaseUrl}/${ordersUri}`, JSON.stringify(order))
        .success(function (_, _) { $scope.CloseOrderLines(); })
        .error(function (data, status) {
          $scope.errorToSearch = errorMessage(data, status);
        });
      
      counter = 1;  /* reset counter */
    }
    
    $scope.AddOrder = function () {
      
    }
    
    $scope.DeleteOrder = function (orderId) {
      $http.delete(`${apiBaseUrl}/${ordersUri}/${orderId}`)
        .success(function (data, _) {
          /* do we need to relead the page. */
        })
        .error(function (data, status) {
          $scope.errorToSearch = errorMessage(data, status);
        });
    }
    
    $scope.CloseOrderLines = function () {
      // When the user clicks on <span> (x), close the modal
      document.getElementsByClassName("close")[0].style.display = "none";
      document.getElementById("myModal").style.display = "none";
    }
    
    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
      if (event.target === document.getElementById("myModal")) {
        document.getElementById("myModal").style.display = "none";
      }
    }
  }]);
})();