'use strict';
angular.module('todoApp', ['ngRoute','AdalAngular'])
.config(['$routeProvider', '$httpProvider', 'adalAuthenticationServiceProvider', function ($routeProvider, $httpProvider, adalProvider) {

    $routeProvider.when("/Home", {
        controller: "homeCtrl",
        templateUrl: "/App/Views/Home.html",
    }).when("/WhoAmI", {
        controller: "whoAmICtrl",
        templateUrl: "/App/Views/WhoAmI.html",
        requireADLogin: true,
    }).when("/TodoList", {
        controller: "todoListCtrl",
        templateUrl: "/App/Views/TodoList.html",
        requireADLogin: true,
    }).when("/ToGoList", {
        controller: "toGoListCtrl",
        templateUrl: "/App/Views/ToGoList.html",
        requireADLogin: true,
    }).when("/UserData", {
        controller: "userDataCtrl",
        templateUrl: "/App/Views/UserData.html",
        }).otherwise({ redirectTo: "/Home" });



    var endpoints = {

        // Map the location of a request to an API to a the identifier of the associated resource
        "https://togoapi.azurewebsites.net/": "https://EPAM.onmicrosoft.com/ANIVN-ToGoAPI"
    };

    adalProvider.init(
        {
            instance: 'https://login.microsoftonline.com/',
            tenant: 'EPAM.onmicrosoft.com',
            clientId: 'ab4df43c-22f8-41b6-80fe-67dd32489591',
            extraQueryParameter: 'nux=1',
            endpoints: endpoints,
            //cacheLocation: 'localStorage', // enable this for IE, as sessionStorage does not work for localhost.  
            // Also, token acquisition for the To Go API will fail in IE when running on localhost, due to IE security restrictions.
        },
        $httpProvider
        );
   
}]);
