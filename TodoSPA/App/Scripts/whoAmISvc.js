'use strict';
angular.module('todoApp')
    .factory('whoAmISvc', ['$http', function ($http) {
        return {
            whoAmI: function () {
                return $http.get('/api/WhoAmI');
            }
        };
    }]);