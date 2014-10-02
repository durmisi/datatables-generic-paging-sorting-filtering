
var MyApp = angular.module('MyApp', [
    'ngRoute',
    'ngTouch',
    'ui.router',
    'ui.bootstrap',
    'datatables',
    'MyApp.filters',
    'MyApp.services',
    'MyApp.directives',
    'MyApp.controllers'
]);

angular.module('MyApp.filters', []);
angular.module('MyApp.services', []);
angular.module('MyApp.directives', []);
angular.module('MyApp.controllers', []);


angular.module('MyApp.directives', []);
angular.module('MyApp.controllers', []);

MyApp.config([
    '$routeProvider', '$locationProvider',
    function($routeProvider, $locationProvider) {
        $.fn.dataTable.ext.legacy.ajax = false;
        $routeProvider.when('/home', { templateUrl: 'partials/home.html', controller: 'homeController', });
        $routeProvider.otherwise({ redirectTo: '/home' });
    }
]);