(function () {
    'use strict';
    angular.module('annPortalApp')
        .controller('Components.SlideMenuCtrl', function ($scope, $location, $rootScope, $mdSidenav, PracticeService) {
            var vm = this;

            $rootScope.$on('slide-bar.toggle', function () {
                $mdSidenav('main-slide-menu').toggle();
            });
            $rootScope.$on('slide-bar.open', function () {
                $mdSidenav('main-slide-menu').open();
            });
            $rootScope.$on('slide-bar.close', function () {
                $mdSidenav('main-slide-menu').close();
            });
            $scope.isActiveRoute = function (route) {
                return $location.path() === route;
            };
            $scope.closeMenu = function () {
                $mdSidenav('main-slide-menu').close();
            };
            $scope.appVersion = APP_VERSION;
            $scope.firstName = FIRST_NAME;
            $scope.lastName = LAST_NAME;
            $scope.logoffLink = IDENTITY_DOMAIN + '/Login/Logoff';

            if (USER_ROLE == "PRACTICE_USER" || USER_ROLE == "SUPER_ADMIN") {
                vm.dashboards = [{
                    icon: 'fa-line-chart',
                    id: 'ANN Monitor',
                    name: 'ANN Monitor'
                }, {
                    icon: 'fa-retweet',
                    id: 'Return Patients',
                    name: 'Return Patients'
                }, {
                    icon: 'fa-repeat',
                    id: 'Repeat Procedures',
                    name: 'Repeat Procedures'
                }, {
                    icon: 'fa-random',
                    id: 'Conversion',
                    name: 'Conversion'
                }];

            } else {
                vm.dashboards = [{
                    icon: 'fa-line-chart',
                    id: 'ANN Monitor Admin',
                    name: 'ANN Monitor'
                }, {
                    icon: 'fa-retweet',
                    id: 'Return Patients Admin',
                    name: 'Return Patients'
                }, {
                    icon: 'fa-repeat',
                    id: 'Repeat Procedures Admin',
                    name: 'Repeat Procedures'
                }, {
                    icon: 'fa-bar-chart',
                    id: 'Products Sold Admin',
                    name: 'Products Sold'
                }, {
                    icon: 'fa-random',
                    id: 'Conversion Admin',
                    name: 'Conversion'
                }];

            }


            function getUser() {
                PracticeService.getUserByUserName(USER_ID)
                  .then(function (res) {
                      vm.user = res.data;
                      // console.log(vm.user);
                  })
                  .catch(function () {

                  });
            }

            getUser();

            vm.isAdmin = function () {
                let tempPath = window.location.hash.toString();

                if (tempPath.includes("#/settings")) {
                    return ("ADMINISTRATOR" === USER_ROLE || "SUPER_ADMIN" === USER_ROLE);
                }
                if (("ADMINISTRATOR" === USER_ROLE || "SUPER_ADMIN" === USER_ROLE) && (tempPath.includes("/admin/"))) {        // used to check the permissions when loading the side menu
                    return true;
                } else {
                    return false;
                }
            }

            $scope.onSelect = function (e) {
                var message = $.map(e.files, function (file) { return file.name; }).join(", ");
                console.log("event :: select (" + message + ")");

                $.each(e.files, function (index, value) {
                    var imgTypeOk = value.extension == ".JPG"
                             || value.extension == ".JPEG"
                             || value.extension == ".jpg"
                             || value.extension == ".jpeg"
                            || value.extension == ".bmp"
                            || value.extension == ".BMP"
                            || value.extension == ".png"
                            || value.extension == ".PNG"
                            || value.extension == ".gif"
                            || value.extension == ".GIF";

                    if (!imgTypeOk) {
                        e.preventDefault();
                        showError("Invalid file type '" + value.extension + "' Please upload a standard image file type! (jpg, bmp, png, gif)", "");
                        $scope.$apply();
                    }
                });

            }

            $scope.onSuccess = function onSuccess(e) {
                getUser();
                $scope.filename = e.files[0].name;
            };

            $scope.windowOptions = {
                draggable: false,
                modal: true,
                center: true
            };

            $("#profileImage").click(function () {
                var wnd = $("#profileImagewindow").kendoWindow({
                    visible: false
                }).data("kendoWindow");
                wnd.center().open();
            });
        });
})();
