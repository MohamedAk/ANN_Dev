(function () {
    'use strict';
    angular.module('annPortalApp')
        .config(function (stateHelperProvider) {
            stateHelperProvider
                .state({
                    name: 'admin.asap-user-management',
                    url: '/asap-user-management',
                    templateUrl: 'app/admin/asap-user-management/asap-user-management.html',
                    controller: 'Admin.AsapUserManagementCtrl',
                    abstract: true,
                    children: [{
                        name: 'index',
                        url: '',
                        templateUrl: 'app/admin/asap-user-management/asap-user-management.index.html',
                        controller: 'Admin.AsapUserManagementCtrl.IndexCtrl'
                    }, {
                        name: 'edit',
                        url: '/:userId/edit',
                        templateUrl: 'app/admin/asap-user-management/asap-user-management.edit.html',
                        controller: 'Admin.AsapUserManagementCtrl.EditCtrl'
                    }, ]
                });
        })
        .controller('Admin.AsapUserManagementCtrl', function ($scope, toaster, $rootScope) {
            var vm = $scope.vm = {};
            $rootScope.$on('$stateChangeSuccess', function () {
                setSubtitle();
            });
            setSubtitle();

            function setSubtitle() {
                if (window.location.href.indexOf('/new') > 0) {
                    vm.subTitle = 'New ASAP User';
                    vm.showCreateBtn = false;
                } else if (window.location.href.indexOf('/edit') > 0) {
                    vm.subTitle = 'Edit ASAP User';
                    vm.showCreateBtn = false;
                } else {
                    vm.subTitle = undefined;
                    vm.showCreateBtn = true;
                }
            }

        })
        .controller('Admin.AsapUserManagementCtrl.IndexCtrl', function ($scope, toaster, PracticeService) {
            //           var vm = $scope.vm = {};

            $scope.asapUsersDataSource = {
                transport: {
                    read: {
                        url: '/Portal/api/Practice/GetASAPUsers',
                    }
                }
            };

            $("#listView").kendoListView({
                dataSource: $scope.asapUsersDataSource,
                pageable: true,
                template: kendo.template($("#template").html()),
            });

            $scope.cancel = function () {
                $location.path('/admin/asap-user-management');
                vm.practice = {};

            };

            $scope.deactiveateAsap = function (userId) {
                var externalUser = { id: userId };
                PracticeService.deactivateASAPUser(externalUser).success(function (data) {
                    console.log(":::DEACTIVATE ASAPUSER :::");
                    $("#listView").data("kendoListView").dataSource.read();
                    $("#listView").data("kendoListView").refresh();
                    if (data) {
                        toaster.success({ title: "", body: "ASAP User Deleted successfully." });
                    }
                }).error(function (error, status) {
                    if (status == -1) {
                        toaster.error({ title: "", body: "Cannot perform the function due to network failure." });
                    } else {
                        toaster.error({ title: "", body: "Cannot perform the function Error occured." });
                    }
                });
            }
        })
        .controller('Admin.AsapUserManagementCtrl.EditCtrl', function ($scope, $timeout, $stateParams, toaster, $filter, $location, PracticeService) {
            var vm = $scope.vm = {};

            //start confirmation when form dirty
            $scope.showModal = false;
            $scope.confirmResponded = false;
            $scope.leavePage = false;
            $scope.nextLocation = "#";
            $scope.isSave = false;

            $scope.$watch('confirmResponded', function () {
                if (!$scope.isSave) {
                    if ($scope.confirmResponded) {
                        if ($scope.leavePage) {
                            $timeout(function () {
                                $location.url($scope.nextLocation.split('#')[1]);
                            }, 500);
                        }
                        else {
                            $scope.confirmResponded = false;
                        }
                    }
                }
            });

            $scope.$on('$locationChangeStart', function (event, next, current) {
                if (!$scope.confirmResponded) {
                    if ($scope.practiceForm) {
                        if ($scope.practiceForm.$dirty) {
                            $scope.showModal = true;
                            $scope.nextLocation = next;
                            event.preventDefault();
                        }
                    }
                }
                else {
                    $scope.showModal = false;
                }
            });

            $scope.confirmNo = function () {
                $scope.leavePage = false;
                $scope.showModal = false;
                $scope.confirmResponded = true;
            }

            $scope.confirmYes = function () {
                $scope.leavePage = true;
                $scope.showModal = false;
                $scope.confirmResponded = true;
            }
            //end confirmation when form dirty

            //load states
            $scope.init = function () {
                //userSearchDatasource
                $scope.userSearchDatasource = {
                    type: "webapi",
                    serverFiltering: true,
                    transport: {
                        read: {
                            url: function () {
                                return "api/Practice/SearchUsers";
                            },

                        },
                        parameterMap: function (data, action) {
                            if (Object.keys(data).length != 0) {
                                var newParams = {
                                    filter: data.filter.filters[0].value
                                };
                                return newParams;
                            }
                        }

                    }
                }
            }

            $scope.save = function (form) {
                vm.isExsistingASAPUser = false;
                vm.isUserAlreadyExsist = false;
                if (form.$valid) {
                    PracticeService.createASAPUser(vm.asapUser).success(function (data) {
                        if (data != null && data.errorMessage === 'User is already an ASAP user.') {
                            vm.isExsistingASAPUser = true;
                        } else if (data.errorMessage === 'User already assigned to practice.') {
                            vm.isUserAlreadyExsist = true;
                        }
                        else {
                            console.log(":::SAVE PRACTICE :::");
                            toaster.success({ title: "", body: "New ASAP user added successfully." });
                            $scope.confirmResponded = true;
                            $scope.leavePage = true;
                            $scope.isSave = true;
                            $location.path('/admin/asap-user-management');
                        }

                    }).error(function (error, status) {

                        if (status == -1) {
                            toaster.error({ title: "", body: "Cannot perform the function due to network failure." });
                        } else {
                            toaster.error({ title: "", body: "Cannot perform the function Error occured." });
                        }
                    });
                }

            };

            $scope.cancel = function () {
                $location.path('/admin/asap-user-management');
                vm.practice = {};

            };

        });
})();
function asapUserDeactivate(str) {
    angular.element(document.getElementById('asapUserList')).scope().deactiveateAsap(str);
}