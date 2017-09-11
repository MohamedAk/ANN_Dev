(function () {
    'use strict';
    angular.module('annPortalApp')
        .config(function ($stateProvider) {
            $stateProvider
                .state('settings', {
                    url: '/settings',
                    templateUrl: 'app/settings/settings.html',
                    controller: 'SettingsCtrl'
                });
        })
        .controller('SettingsCtrl', function ($scope, $rootScope, $timeout, toaster, $location, $filter) {
            $scope.Init = function () {
                $scope.identityUrl = IDENTITY_DOMAIN;
            }
        });
})();