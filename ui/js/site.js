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
      $scope.FetchOrderLines(orderId);
      document.getElementById("myModal").style.display = "block";
      document.getElementsByClassName("close")[0].style.display = "block";
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