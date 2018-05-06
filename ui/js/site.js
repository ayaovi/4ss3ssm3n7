(function () {
  const app = angular.module("myApp", []);
  const apiBaseUrl = 'http://localhost:50032'
  const errorMessage = function (data, status) {
    return `Error: ${status}${data.Message !== undefined ? (` ${data.Message}`) : ""}`;
  };

  app.controller("myCtrl", ["$http", "$scope", function ($http, $scope) {
    $scope.orders = [{
      id: "1",
      clientId: "1",
      orderLines: [
        {
          id: "1",
        },
        {
          id: "2"
        }
      ]
    }];

    $scope.viewOrders = function () {

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