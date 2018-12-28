'use strict';
angular.module('todoApp')
    .controller('whoAmICtrl', ['$scope', '$location', 'whoAmISvc', 'adalAuthenticationService', function ($scope, $location, whoAmISvc, adalService) {
        $scope.error = "";
        $scope.loadingMessage = "Loading...";
        $scope.me = "";
             
        $scope.populate = function () {
            whoAmISvc.whoAmI().success(function (result) {
                $scope.me = result;
                $scope.loadingMessage = "";
            }).error(function (err) {
                $scope.error = err;
                $scope.loadingMessage = "";
            })
        };

    }]);