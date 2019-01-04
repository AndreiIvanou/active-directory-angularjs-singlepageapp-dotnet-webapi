'use strict';
angular.module('todoApp')
    .factory('whoAmISvc', ['$http', function ($http) {
        return {
            whoAmI: function (group, shouldImpersonate) {
                return $http.get('/api/WhoAmI?group=' + group + '&shouldImpersonate=' + shouldImpersonate);
            }
        };
    }]);