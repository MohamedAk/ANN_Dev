(function () {
    'use strict';
    angular.module('annPortalApp')
        .config(function (stateHelperProvider) {
            stateHelperProvider
                .state({
                    name: 'admin',
                    url: '/admin',
                    abstract: true,
                    template: '<ui-view/>'
                });
        }).directive('noSpecialChar', function () {
            return {
                require: 'ngModel',
                restrict: 'A',
                link: function (scope, element, attrs, modelCtrl) {
                    modelCtrl.$parsers.push(function (inputValue) {
                        if (inputValue == null) {
                            return '';
                        }

                        var cleanInputValue = inputValue.replace(/[^\w\s~!@$;=#'(),-.:/]/gi, '');
                        // NEW REGEX [^\w\s~!@$;=#'(),-.]
                        // OLD REGEX [^\w\s]/gi
                        if (cleanInputValue != inputValue) {
                            modelCtrl.$setViewValue(cleanInputValue);
                            modelCtrl.$render();
                        }
                        return cleanInputValue;
                    });
                }
            }
        }).directive('bsTooltip', function () {
            return {
                restrict: 'A',
                link: function (scope, element, attrs) {
                    var placement = attrs.placement || 'top';
                    element.tooltip({
                        trigger: 'hover',
                        placement: placement,
                        container: 'body',
                        html: true
                    });
                }
            };
        }).directive('practiceList', [
        function () {
            return {
                restrict: 'A',
                link: function ($scope, element, attr) {
                    var margin = attr.practiceList || 10;
                    var height = ($("body").height() * 1 - margin);
                    $(element).mCustomScrollbar({
                        setHeight: height,
                        theme: "minimal-dark",
                        autoHideScrollbar: false,
                        alwaysShowScrollbar: 0,
                        scrollInertia: 0,
                        advanced: {
                            autoScrollOnFocus: false,
                            updateOnContentResize: true
                        }
                    });

                }
            };
        }
        ]).directive('modal', [
            function () {
                return {
                    template: '<div class="modal fade" data-backdrop="static" data-keyboard="false">' +
                        '<div class="modal-dialog">' +
                          '<div class="modal-content">' +
                            '<div class="modal-header">' +
                              '<h4 class="modal-title"></h4>' +
                            '</div>' +
                            '<div class="modal-body" ng-transclude></div>' +
                          '</div>' +
                        '</div>' +
                      '</div>',
                    restrict: 'E',
                    transclude: true,
                    replace: true,
                    $scope: true,
                    link: function postLink($scope, element, attrs) {
                        $scope.title = attrs.title;
                        $scope.size = attrs.size || "500px";
                        $scope.height = attrs.height || "auto";
                        $scope.custom = attrs.custom;
                        $scope.class = attrs.class || "";

                        //Close modal
                        $scope.closeModal = function () {
                            $(element).modal('hide');
                        };

                        //To prevent modal content from initially showing up
                        $(element).removeClass("hide");

                        $scope.$watch(attrs.visible, function (value) {
                            if (value == true) {
                                $(element).modal('show');
                                $(".modal-dialog").css({
                                    'width': $scope.size,
                                    'height': $scope.height
                                });
                                $(".modal-title").text(attrs.title);
                            }
                            else
                                $(element).modal('hide');
                        });

                        $(element).on('shown.bs.modal', function () {
                            $scope.$apply(function () {
                                $scope.$parent[attrs.visible] = true;
                            });
                        });

                        $(element).on('hidden.bs.modal', function () {
                            $scope.$apply(function () {
                                $scope.$parent[attrs.visible] = false;

                            });

                        });
                    }
                };
            }
        ]).directive('procedureModal', [
            function () {
                return {
                    template: '<div class="modal fade">' +
                        '<div class="modal-dialog custom-proc-modal">' +
                          '<div class="modal-content">' +
                            '<div class="modal-header">' +
                              '<h4 class="modal-title"></h4>' +
                            '</div>' +
                            '<div class="modal-body" ng-transclude></div>' +
                          '</div>' +
                        '</div>' +
                      '</div>',
                    restrict: 'E',
                    transclude: true,
                    replace: true,
                    $scope: true,
                    link: function postLink($scope, element, attrs) {
                        $scope.title = attrs.title;
                        $scope.size = attrs.size || "1000px";
                        $scope.height = attrs.height || "auto";
                        $scope.custom = attrs.custom;
                        $scope.class = attrs.class || "";

                        //Close modal
                        $scope.closeModal = function () {
                            $(element).modal('hide');
                        };

                        //To prevent modal content from initially showing up
                        $(element).removeClass("hide");

                        $scope.$watch(attrs.visible, function (value) {
                            if (value == true) {
                                $(element).modal('show');
                                $(".modal-dialog").css({
                                    'width': $scope.size,
                                    'height': $scope.height
                                });
                                // $(".modal-title").text(attrs.title);
                            }
                            else
                                $(element).modal('hide');
                        });

                        $(element).on('shown.bs.modal', function () {
                            $scope.$apply(function () {
                                $scope.$parent[attrs.visible] = true;
                            });
                        });

                        $(element).on('hidden.bs.modal', function () {
                            $scope.$apply(function () {
                                $scope.$parent[attrs.visible] = false;

                            });

                        });
                    }
                };
            }
        ]);
})();
