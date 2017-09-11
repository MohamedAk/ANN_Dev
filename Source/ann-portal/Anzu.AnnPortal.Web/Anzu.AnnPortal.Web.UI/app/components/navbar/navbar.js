(function () {
    'use strict';
    angular.module('annPortalApp')
        .controller('Components.NavbarCtrl', function ($scope, $rootScope, $mdDialog, $stateParams, NavBarService) {
            var vm = $scope.vm = {};
            $scope.emrId = EMR_ID;
            $scope.UserRole = USER_ROLE;
            NavBarService.getNavigationBarInformation().success(function (data) {
                if (data != null) {
                    vm.totalProcedures = data.totalProcedures;
                    vm.totalRevenue = data.totalRevenue;
                }
            }).error(function (error, status) {

            });

            vm.dashboardTitle = $stateParams.dashboardId;
            $scope.toggleSlideMenu = function () {
                $rootScope.$emit('slide-bar.toggle');
            };
            $scope.toggleFilters = function () {
                if ($('iframe')[0]) {
                    $('iframe')[0].contentWindow.toggleFilters();
                }
            };
            $scope.toggleFiltersList = function () {
                $rootScope.$emit('dashboard-filters.toggle');
            };
            $scope.showLogoutDialog = function (ev) {
                var confirm = $mdDialog.confirm()
                    .title('Do you need to Log Out?')
                    .ariaLabel('')
                    .targetEvent(ev)
                    .ok('Yes')
                    .cancel('Cancel')
                    .closeTo(document.body);
                $mdDialog.show(confirm).then(function () {
                }, function () {
                });
            };

            $scope.isAdmin = function () {

                if (USER_ROLE == 'ADMINISTRATOR' || USER_ROLE == 'SUPER_ADMIN') {
                    let tempPath = window.location.hash.toString();
                    if (tempPath.includes("/dashboard/")) {
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            }

            $('md-toolbar .filters-list,md-toolbar .filters-list i').on('mouseenter', function () {
                $rootScope.$emit('dashboard-filters.open');
            });
            $('md-toolbar .filters-list,md-toolbar .filters-list i').on('mouseleave', function () {
                $rootScope.$emit('dashboard-filters.close');
            });


        });
})();
