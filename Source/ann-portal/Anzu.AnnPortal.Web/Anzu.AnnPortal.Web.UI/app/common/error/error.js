(function() {
    'use strict';
    angular.module('annPortalApp')
        .config(function($stateProvider) {
            $stateProvider
                .state('common_error', {
                    url: '/error/:errorCode',
                    templateUrl: 'app/common/error/error.html',
                    controller: 'Common.ErrorCtrl'
                });
        })
        .controller('Common.ErrorCtrl', function($scope, $stateParams) { 
            var vm = $scope.vm = { errorCode: $stateParams.errorCode }; 
            vm.errors = {
                'no-practice-assigned':{
                    title:'No Practice Assigned',
                    description:'There is no Practice assigned to you. Please contact the Administrator.'
                }
            };
        });
})();
