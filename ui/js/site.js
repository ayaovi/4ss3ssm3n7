(function () {
  const app = angular.module("myApp", []);
  const apiBaseUrl = 'http://localhost:55497/';
  const ordersUri = "api/orders";
  const orderlinesUri = "api/orderlines";
  const errorMessage = function (data, status) {
    return `Error: ${status}${data.Message !== undefined ? (` ${data.Message}`) : ""}`;
  };

  app.controller("myCtrl", ["$http", "$scope", function ($http, $scope) {
    $scope.orders = [{
      id: 1,
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
    }

    $scope.getActivePlayers = function () {
      $http.get(`${apiBaseUrl}/${usersUri}/all`)
        .success(function (data, _) {
          $scope.playersOnline = data.filter(player => player.Name !== $("#gamer-name").val());
        })
        .error(function (data, status) {
          $scope.errorToSearch = errorMessage(data, status);
        });
    };

    $scope.setupPvP = function () {
      $http.get(`${apiBaseUrl}/${agniKaiUri}/initiate`).then(response => {
        $scope.agnikaiTicket = response.data;
        gameHubProxy.server.agniKaiStartNotification($scope.agnikaiTicket, $scope.selectedPlayer.Name);
        const req = {
          method: "POST",
          url: `${apiBaseUrl}/${usersUri}/submit`,
          data: {
            token: $scope.gameToken.Value,
            ticket: $scope.agnikaiTicket
          }
        }
        $http(req).then(response => {
          $scope.indicator = util.fieldToIndicator(response.data);
		      console.log(`${$("#gamer-name").val()}'s indicator is ${$scope.indicator}`);
        });
        gameHubProxy.server.joinAgniKai($scope.agnikaiTicket);
      });
    }
  }]);
})();