(function () {
    'use strict';
    angular.module('annPortalApp')
        .config(function ($stateProvider) {
            $stateProvider
                .state('dashboard', {
                    url: '/dashboard/:dashboardId',
                    templateUrl: 'app/dashboard/dashboard.html',
                    controller: 'DashboardCtrl'
                });
        })
        .controller('DashboardCtrl', function ($scope, $rootScope, $timeout, $stateParams, $sce, $mdSidenav, $http, $mdToast) {
            var vm = $scope.vm = { selectedDashboardId: $stateParams.dashboardId };
            $scope.etlRunning = false;
            //var current_user = CURRENT_USER;
            $scope.practiceId = PRACTICE_ID;
            $scope.user_role = USER_ROLE;
            // vm.dashboardId = "";
            console.log(USER_ROLE);

            checkEtlStatus();

            $(window).resize(function () {
                $scope.$apply(function () {
                    // setIframe();
                    // autoResize()
                    checkEtlStatus();   // reverting changes
                });
            });
            $rootScope.$on('dashboard-filters.open', function () {
                $scope.$apply(function () {
                    $mdSidenav('dashboard-filters-slide-menu').open();
                });
            });
            $rootScope.$on('dashboard-filters.close', function () {
                $scope.$apply(function () {
                    $mdSidenav('dashboard-filters-slide-menu').close();
                });
            });
            $rootScope.$on('dashboard-filters.toggle', function () {
                $mdSidenav('dashboard-filters-slide-menu').toggle();
            });
            window.$rootScope = $rootScope;
            $rootScope.$on('iframe-loaded', function () {
                // $scope.$apply(function () {
                // });
            });
            $rootScope.$on('iframe-first-widget-loaded', function () {
                $scope.$apply(function () {
                    $scope.iframeLoaded = true;
                });
            });
            $rootScope.$on('iframe-dashboard-loaded', function () {
                // console.log(8899999999999);
            });
            $rootScope.$on('iframe-dashboard-end-callback', function () {
                // autoResize();
                console.log('end');
            });

            /// DO NOT REMOVE
            //function autoResize() {
            //    // Finding dragging splitter
            //    var tempTable = $("#iframe1").contents().find(".dx-dashboard-viewer > table.dx-viewer-item-table")[0];
            //    var tableRow = $(tempTable).contents().find("td.dx-dashboard-splitter-h-separator");

            //    if (tempTable) {
            //        // Get rendered height values
            //        var outerH = $(window).height();
            //        var tableH = $(tempTable).height();
            //        var parentH = $($("#iframe1").contents().find("#viewerContainer")[0]).height();

            //        var selectorId = 1;
            //        if ($stateParams.dashboardId === "Return Patients Admin"
            //                || $stateParams.dashboardId === "ANN Monitor Admin"
            //                || $stateParams.dashboardId === "Repeat Procedures Admin") {
            //            selectorId = 2;
            //        }
            //        console.log(parentH - tableH);

            //        /// OPTION DEFAULT
            //        // Drag splitter manually
            //        var splitter = $(tableRow[tableRow.length - selectorId]);
            //        splitter.attr("draggable", true);
            //        $(splitter).mousedown();
            //        $(splitter).simulate("drag-n-drop", {
            //            dy: (parentH - tableH)
            //        });
            //        $(splitter).mouseup();

            //        // Resize filter widgets
            //        if ($stateParams.dashboardId === "ANN Monitor Admin") {
            //            // age group filter widget
            //            var ageGroupWidget = $("#iframe1").contents().find("div[data-layout-item-name = treeViewDashboardItem4]")[0];
            //            $(ageGroupWidget).css('min-height', '30vh').css('overflow', 'hidden');
            //            var ageGroupWidgetTable = $(ageGroupWidget).find(".dx-dashboard-item")[0];
            //            $(ageGroupWidgetTable).css('min-height', '25vh');

            //            // gender selector widget
            //            var genderWidget = $("#iframe1").contents().find("div[data-layout-item-name = treeViewDashboardItem5]")[0];
            //            $(genderWidget).css('min-height', '30vh').css('overflow', 'hidden');
            //            var genderWidgetTable = $(genderWidget).find(".dx-dashboard-item")[0];
            //            $(genderWidgetTable).css('min-height', '25vh');
            //        }
            //    }
            //}

            function checkEtlStatus() {
                $scope.iframeLoaded = false;
                var url = '/Portal/api/Metadata/GetSqlJobStatus';
                $http.get(url).then(function (res) {
                    var statusData = res.data;
                    if (statusData) {
                        $scope.etlRunning = statusData.status;
                        console.log(statusData.message);
                        if (statusData.status) {
                            $timeout(checkEtlStatus, 5000);
                        }
                        else {
                            // load dashboards
                            setIframe();
                        }
                    }
                    else {
                        // load dashboards
                        setIframe();
                    }
                });
            }

            function setIframe() {
                $scope.iframeLoaded = false;
                $timeout(function () {
                    $scope.iframeLoaded = true;

                    window.addEventListener("resize", function () {
                        document.getElementById('iframe1').contentWindow.location.reload(true);
                    }, true);

                }, 8000);
                if ($scope.user_role == 'PRACTICE_USER') {
                    //console.log($stateParams.dashboardId == "");
                    var host = window.location.host;
                    if (host.indexOf('localhost') >= 0) {
                        host = 'localhost';
                    }
                    if ($stateParams.dashboardId == "") {
                        $stateParams.dashboardId = "ANN Monitor";
                    }
                    var url = '//' + host + '/BiPortal/UI/IFrameEdition/viewer.aspx?xID=' + $stateParams.dashboardId + '&xType=DSH&xPractice=' + $scope.practiceId;
                    /*if (window.innerWidth <= 800) {
                        url = '//'+host + '/BiPortal/UI/IFrameEdition/viewer.aspx?xID=' + $stateParams.dashboardId + ' Mobile&xType=DSH';
                    }*/
                    if (vm.iframeUrlUnsecure === url) {
                        $scope.iframeLoaded = true;
                    }
                    vm.iframeUrlUnsecure = url;
                    vm.iframeUrl = $sce.trustAsResourceUrl(url);
                } else {

                    if ($stateParams.dashboardId == "") {
                        $stateParams.dashboardId = "ANN Monitor Admin";
                    }

                    var host = window.location.host;
                    if (host.indexOf('localhost') >= 0) {
                        host = 'localhost';
                    }
                    var url = '//' + host + '/BiPortal/UI/IFrameEdition/viewer.aspx?xID=' + $stateParams.dashboardId + '&xType=DSH&xPractice=' + $scope.practiceId;
                    /*if (window.innerWidth <= 800) {
                        url = '//'+host + '/BiPortal/UI/IFrameEdition/viewer.aspx?xID=' + $stateParams.dashboardId + ' Mobile&xType=DSH';
                    }*/
                    if (vm.iframeUrlUnsecure === url) {
                        $scope.iframeLoaded = true;
                    }
                    vm.iframeUrlUnsecure = url;
                    vm.iframeUrl = $sce.trustAsResourceUrl(url);
                }
            }
            $rootScope.$on('iframe-dashboard-title-tooltip', function (e, data) {
                $('.module-dashboard md-sidenav').append(data);
            });
        });
})();