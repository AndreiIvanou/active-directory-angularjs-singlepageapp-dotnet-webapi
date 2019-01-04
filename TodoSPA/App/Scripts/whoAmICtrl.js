'use strict';
angular.module('todoApp')
    .controller('whoAmICtrl', ['$scope', '$location', 'whoAmISvc', 'adalAuthenticationService', function ($scope, $location, whoAmISvc, adalService) {
        $scope.group = "ANIVN_AzureADSQLUsers";
        $scope.shouldImpersonate = true;

        $scope.error = "";
        $scope.loadingMessage = "";
        $scope.me = "";
        $scope.currentUserToken = "";
        $scope.tokenForSql = "";               
             
        $scope.populate = function () {            
            $scope.loadingMessage = "Loading...";
            $scope.error = "";            
            $scope.me = "";
            $scope.currentUserToken = "";
            $scope.tokenForSql = "";

            whoAmISvc.whoAmI($scope.group, $scope.shouldImpersonate).success(function (result) {
                $scope.me = result.me;
                $scope.currentUserToken = result.currentUserToken;
                $scope.tokenForSql = result.tokenForSql;
                $scope.loadingMessage = "";
            }).error(function (err) {
                $scope.error = err;
                $scope.loadingMessage = "";
            })
        };

    }]);