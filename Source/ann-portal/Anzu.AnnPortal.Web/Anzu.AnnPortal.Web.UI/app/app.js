(function () {
    'use strict';
    angular.module('annPortalApp', [
      'ngCookies',
      'ngResource',
      'ngSanitize',
      'kendo.directives',
      'ui.router',
      'ui.router.stateHelper',
      'ngMaterial',
      'ngAria',
      'ui.router.stateHelper',
      'afklStickyElement',
      'ngAnimate',
      'ui.bootstrap',
      'toaster'
    ]).config(function ($httpProvider) {

        $httpProvider.interceptors.push(function ($q) {

            return {

                'responseError': function (rejection) {

                    var defer = $q.defer();

                    if (rejection.status == 401) {
                        window.location.assign('/Portal/Errors/401.html');
                    }

                    defer.reject(rejection);
                    console.log(defer.promise);
                    return defer.promise;

                }
            };
        });
    }).config(function ($stateProvider, $urlRouterProvider, $locationProvider, $httpProvider, $ariaProvider) {
        // main router
        if (USER_ROLE == 'ADMINISTRATOR' || USER_ROLE == 'SUPER_ADMIN') {
            let tempPath = window.location.hash.toString();
            if (tempPath.includes("/dashboard/")) {
                $urlRouterProvider.otherwise('/dashboard/ANN%20Monitor');
            }
            else {
                $urlRouterProvider.otherwise('/admin/practice-management');
            }
        } else if (USER_ROLE == 'ASAPS_USER') {
            $urlRouterProvider.otherwise('/dashboard/');
        } else if (USER_ROLE == 'PRACTICE_USER') {
            $urlRouterProvider.otherwise('/dashboard/ANN%20Monitor');
        } else {
            // or redirect to identity
            $urlRouterProvider.otherwise(IDENTITY_DOMAIN + 'Login/Login');
        }

        // $locationProvider.html5Mode(false);
        //$httpProvider.interceptors.push('authInterceptor');
        $httpProvider.defaults.withCredentials = true;
        $httpProvider.defaults.headers.post['Content-Type'] = 'application/json; charset=utf-8';
        //$httpProvider.interceptors.push('apiInterceptor');
        $ariaProvider.config({
            tabindex: false,
            ariaInvalid: false,
            ariaLabel: false
        });
    })
     .config(function ($mdThemingProvider) {
         var primary = $mdThemingProvider.extendPalette('indigo', {
             '500': '#008C69',
             'contrastDefaultColor': 'light'
         });

         $mdThemingProvider.definePalette('primary', primary);

         $mdThemingProvider.theme('default')
          .primaryPalette('primary');

     })
     .factory('authInterceptor', ['$rootScope', '$q', '$cookieStore', '$location', function ($rootScope, $q, $cookieStore, $location) {
         return {
             request: function (config) {
                 config.headers = config.headers || {};
                 if ($cookieStore.get('isAuthenticated')) {
                     config.headers.Authorization = 'Bearer ' + $cookieStore.get('isAuthenticated');
                 }
                 return config;
             },

             responseError: function (response) {
                 if (response.status === 401) {
                     //$location.path('/');

                     // remove any stale tokens
                     $cookieStore.remove('token');
                     return $q.reject(response);
                 } else {
                     return $q.reject(response);
                 }
             }
         };
     }])
     .run(function ($rootScope, $location, Auth, $window, $cookieStore, $http) {
         var localhost = $window.location.host.indexOf('localhost') >= 0;
         // Redirect to login if route requires auth and you're not logged in

         // Authorize and validate current route with the logged in user
         var url = '/Portal/api/Metadata/AuthorizeRouting?p=' + $location.path();
         $http.get(url).then(function (res) {
             if (res) {
                 if (!res.data.isValid) {
                     $location.path(res.data.redirectUrl);
                 }
             }
         });

         $rootScope.$on('$stateChangeStart', function (event, next) {
             if (!$cookieStore.get('isAuthenticated')) {
                 //$location.path('/');
             }
         });
         window.iframeLoaded = function () {
             //$scope.$apply(function () {
             $rootScope.$emit('iframe-loaded');
             //});
         };
     })
     .factory('Auth', function () {
         return {
             isLoggedInAsync: function (cb) {
                 cb(true);
             }
         };
     }).factory('$httpMock', function ($q) {
         return {
             get: function () {
                 return {
                     respond: function (data) {
                         return $q(function (resolve) {
                             setTimeout(function () {
                                 resolve({
                                     data: data
                                 });
                             });
                         });
                     }
                 };
             },
             post: function () {
                 return {
                     respond: function (data) {
                         return $q(function (resolve) {
                             setTimeout(function () {
                                 resolve({
                                     data: data
                                 });
                             });
                         });
                     }
                 };
             }
         };
     });
})();