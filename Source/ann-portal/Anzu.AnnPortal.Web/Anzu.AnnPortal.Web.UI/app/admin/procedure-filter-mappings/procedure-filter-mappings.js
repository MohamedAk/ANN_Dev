 (function () {
    'use strict';
    angular.module('annPortalApp')
        .config(function (stateHelperProvider) {
            stateHelperProvider
                .state({
                    name: 'admin.procedure-filter-mappings',
                    url: '/procedure-filter-mappings',
                    templateUrl: 'app/admin/procedure-filter-mappings/procedure-filter-mappings.html',
                    controller: 'Admin.ProcedureFilterMappingsCtrl',
                    abstract: true,
                    children: [{
                        name: 'index',
                        url: '',
                        templateUrl: 'app/admin/procedure-filter-mappings/procedure-filter-mappings.index.html',
                        controller: 'Admin.ProcedureFilterMappingsCtrl.IndexCtrl'
                    }, {
                        name: 'edit',
                        url: '/:userId/edit',
                        templateUrl: 'app/admin/procedure-filter-mappings/procedure-filter-mappings.edit.html',
                        controller: 'Admin.ProcedureFilterMappingsCtrl.EditCtrl'
                    }, , {
                        name: 'editNew',
                        url: '/:userId/editnew',
                        templateUrl: 'app/admin/procedure-filter-mappings/procedure-filter-mappings.editnew.html',
                        controller: 'Admin.ProcedureFilterMappingsCtrl.EditNewCtrl'
                    }, {
                        name: 'editRelated',
                        url: '/:userId/editRelated',
                        templateUrl: 'app/admin/procedure-filter-mappings/procedure-filter-mappings-related.edit.html',
                        controller: 'Admin.ProcedureFilterMappingsCtrl.Related.EditCtrl'
                    }]
                });
        })
        .controller('Admin.ProcedureFilterMappingsCtrl', function ($scope, toaster, $rootScope) {
            var vm = $scope.vm = {};
            $rootScope.$on('$stateChangeSuccess', function () {
                setSubtitle();
            });
            setSubtitle();

            function setSubtitle() {
                if (window.location.href.indexOf('/new') > 0) {
                    vm.subTitle = 'New Filter Mapping';
                    vm.showCreateBtn = false;
                } else if (window.location.href.indexOf('/edit') > 0) {
                    vm.subTitle = 'Edit Filter Mapping';
                    vm.showCreateBtn = false;
                } else if (window.location.href.indexOf('/procedure-filter-mappings') > 0) {
                    vm.subTitle = 'Procedure Filter Mapping';
                    vm.showCreateBtn = true;
                } else {
                    vm.subTitle = undefined;
                    vm.showCreateBtn = true;
                }
            }

        })
        .controller('Admin.ProcedureFilterMappingsCtrl.IndexCtrl', function ($scope, $rootScope, ProcedureLevelService, BreastImplantService, $q, toaster) {

            var vm = $scope.vm = { newItem: { level1: {}, level2: {}, level3: {}, level4: {}, breastImplant: {} } };
            $scope.curTab = 1;
            vm.isDupplicateSequence = false;
            vm.isNotCompletedSequence = false;

            $scope.searchOption = "1";
            $scope.sortOption = "4";
            $scope.searchText = "";

            $scope.cubeProcessQueue = [];
            $scope.isCubeProcessing = false;
            $scope.showOkModal = false;

            //ProcedureLevelService.getAllProcedureLevels()
            //    .then(function (res) {
            //        vm.procedurelevels = res.data;

            //    })
            //    .catch(function (err) {
            //        console.log(err);
            //    });

            gridInitialize();

            $scope.getJobRunningStatus = function () {
                // get job running status
                $scope.cubeProcessQueue = [];
                ProcedureLevelService.getJobRunningStatus().success(function (res) {
                    _.each(res, function (obj) {
                        $scope.cubeProcessQueue.push(obj.emrId);
                    });
                    $scope.isCubeProcessing = res.length > 0;
                    $rootScope.isCubeProcessing = res.length > 0;
                    $scope.showOkModal = isCubeProcessing;
                    // showOkModal
                });
            };

            $scope.getJobRunningStatus();

            function gridInitialize() {
                $scope.mainGridOptions =
                {
                    dataSource: {
                        type: "json",
                        transport: {
                            read: {
                                url: function () {
                                    return "/Portal/api/Metadata/GetProcedureLevels/" + $scope.sortOption + "/"
                                },
                            }
                        },
                        pageSize: 10,
                        serverPaging: true,
                        serverSorting: true,
                        serverFiltering: true,
                        serverGrouping: true,
                        serverAggregates: true,
                        schema: {
                            data: "data",
                            total: "total",
                        },
                    },
                    selectable: false,
                    sortable: false,
                    pageable: true,
                    //scrollable: {
                    //    virtual: true
                    //},
                    //selectable: "row",
                    columns: [{
                        field: "name",
                        title: "Procedure Name",
                        width: "16%"
                    }, {
                        field: "procedureLevel1",
                        title: "Level 1",
                        width: "11%",
                        template: "{{dataItem.procedureLevel1.name}}"
                    },
                    {
                        field: "procedureLevel1",
                        title: "Level 2",
                        width: "11%",
                        template: "{{dataItem.procedureLevel2.name}}"
                    },
                    {
                        field: "procedureLevel1",
                        title: "Level 3",
                        width: "11%",
                        template: "{{dataItem.procedureLevel3.name}}"
                    }

                    , {
                        field: "procedureLevel1",
                        title: "Level 4",
                        width: "11%",
                        template: "{{dataItem.procedureLevel4.name}}"
                    }
                    , {
                        field: "BreastImplantUse",
                        title: "Breast Implant Use",
                        width: "11%",
                        template: "<input type=\"checkbox\" ng-disabled=\"true\" ng-model=\"dataItem.practiceProductFlag\"  value=\"dataItem.practiceProductFlag\" class=\"custom-left-margin\">"
                    }
                    , {
                        field: "RelatedProcedure",
                        title: "Related Procedure",
                        width: "11%",
                        template: "<input type=\"checkbox\" ng-disabled=\"true\" ng-model=\"dataItem.isRelatedProcedureExists\"  value=\"dataItem.isRelatedProcedureExists\" class=\"custom-left-margin\">"
                    }
                    , {
                        field: "",
                        title: "",
                        width: "auto",
                        template: "<a class=\"button\" href=\"\\#/admin/procedure-filter-mappings/#=id#/editRelated\" style=\"box-sizing: border-box;\"><i class=\"fa fa-pencil\" aria-hidden=\"true\"></i> Edit</a>"
                    }
                    , {
                        field: "",
                        title: "",
                        width: "6%",
                        template: "<button title=\"Remove\" class=\"icon-no-border\" aria-label=\"Settings\" ng-click=\"showConfirmPopup(#=id#)\" ng-disabled=\"isCubeProcessing\"><i class=\"fa fa-minus\"></i></button>"
                    }],
                    dataBound: function (e) {
                        if (!e.sender.dataSource.view().length) {
                            var colspan = e.sender.thead.find("th:visible").length;
                            var emptyRow = "<tr><td colspan=\"8\"><div class=\"no-data-alert\"><div>No data available for the given search criteria</div></div></td></tr>";
                            var gridWrapper = e.sender.wrapper;
                            var gridDataTable = e.sender.table;
                            var gridDataArea = gridDataTable.closest(".k-grid-content");
                            e.sender.tbody.end().html(emptyRow);
                        }
                    }
                };
            }

            $scope.addBreastImplant = function (breastImplantItem) {
                if (!breastImplantItem.Name) {
                    return;
                }
                if (_.some(vm.breastImplants, function (item) {
                        return item.Name && item.Name.toLowerCase() === breastImplantItem.Name.toLowerCase();
                })) {
                    return;
                }
                vm.breastImplants.push({ Name: breastImplantItem.Name });
                breastImplantItem.Name = '';
            };
            $scope.removeBreastImplant = function (breastImplantItem) {
                _.remove(vm.breastImplants, breastImplantItem);
            };

            // Search & Sort
            $scope.onSearchInputKeyPress = function (event) {
                if (event.charCode == 13) {
                    $scope.search();
                }
            }

            $scope.search = function () {
                let filterText = "NA";
                if (!$scope.searchOption || !$scope.searchText) {
                    $scope.searchOption = "1";
                    $scope.searchText = "";
                }

                filterText = ($scope.searchText != "") ? $scope.searchText.replace(/\//gi, "_!").replace(/:/gi, "_~") : "NA";

                $scope.myGrid.dataSource.options.transport.read.url = '/Portal/api/Metadata/GetProcedureLevels/' + $scope.sortOption + '/' + $scope.searchOption + '/' + encodeURIComponent(filterText.trim()) + '/';
                // $scope.myGrid.dataSource.read();
                $scope.myGrid.dataSource.page(1);
            };

            $scope.resetSearch = function () {
                $scope.searchOption = "1";
                $scope.sortOption = "4";
                $scope.searchText = "";

                $scope.myGrid.dataSource.options.transport.read.url = '/Portal/api/Metadata/GetProcedureLevels/' + $scope.sortOption + '/';
                // $scope.myGrid.dataSource.read();
                $scope.myGrid.dataSource.page(1);
            };

            $scope.removeProcedure = function (index) {
                $scope.yesClicked = false;
                ProcedureLevelService.deleteProcedure(index)
                  .then(function (res) {
                      $scope.tempProcedureId = "";
                      toaster.success({ title: "", body: "Procedure removed successfully." });
                      // $scope.myGrid.dataSource.read();
                      $scope.myGrid.dataSource.page(1);
                      $("#confirmPopup").removeClass('showPopup');
                  })
                  .catch(function () {
                      toaster.error({ title: "", body: "Error removing procedure." });
                      $("#confirmPopup").removeClass('showPopup');
                  });
            };

            // Show Confirm popup
            $scope.showConfirmPopup = function (id) {
                $scope.showModal = false;
                $scope.tempProcedureId = id;
                $("#popText").html('Are you sure you want to remove this procedure?');
                $("#confirmPopup").addClass('showPopup');
            };

            // Confirm popup
            $scope.confirmYes = function () {
                $scope.yesClicked = true;
                $scope.removeProcedure($scope.tempProcedureId);
            }

            $scope.confirmNo = function () {
                $("#confirmPopup").removeClass('showPopup');
                $scope.showOkModal = false;
            }
        })
        .controller('Admin.ProcedureFilterMappingsCtrl.EditNewCtrl', function ($scope, $timeout, $stateParams, toaster, $filter, $location, ProcedureLevelService, $q) {
            $scope.editcontollermsg = "editnewctrlmsg!!!!!!!!!!!";

            var vm = $scope.vm = {};

            ProcedureLevelService.getProcedureById($stateParams.userId)
                    .then(function (res) {
                        vm.procedure = res.data.name;
                        vm.procedureLevel1 = res.data.procedureLevel1.name;
                        vm.procedureLevel2 = res.data.procedureLevel2.name;
                        vm.procedureLevel3 = res.data.procedureLevel3.name;
                        vm.procedureLevel4 = res.data.procedureLevel4.name;
                        vm.practiceProductFlag = res.data.practiceProductFlag;
                    });

            $scope.level1Datasource = {
                type: "webapi",
                serverFiltering: true,
                transport: {
                    read: {
                        url: function () {
                            return "api/Metadata/ProcedureLevelFilterByText/1";
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
            };

        })
        .controller('Admin.ProcedureFilterMappingsCtrl.Related.EditCtrl', function ($scope, $timeout, $stateParams, toaster, $filter, $location, ProcedureLevelService, $q, $uibModal) {
            var vm = $scope.vm = {};
            //vm.isDupplicateSequence = false;
            //vm.isNotCompletedSequence = false;

            //start confirmation when form dirty
            $scope.showModal = false;
            $scope.showProcModal = false;
            $scope.confirmResponded = false;
            $scope.leavePage = false;
            $scope.nextLocation = "#";
            $scope.isSave = false;
            $scope.tempRelatedProc = [];

            //$scope.selectedProceduresList = [];
            $scope.selectedProceduresList = [{ procedure: {}, procedureLevel1: {}, procedureLevel2: {}, procedureLevel3: {}, procedureLevel4: {} }];

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
                    if ($scope.filterMappingForm) {
                        if ($scope.filterMappingForm.$dirty) {
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
                $scope.showProcModal = false;
                $scope.confirmResponded = true;
            }

            $scope.confirmYes = function () {
                $scope.leavePage = true;
                $scope.showModal = false;
                $scope.confirmResponded = true;
            }

            $scope.confirmProcYes = function () {
                $scope.leavePage = false;
                $scope.showProcModal = false;
                $scope.confirmResponded = true;
                $('#procGrid').data('kendoGrid').dataSource.page($scope.nextPage);
                $scope.pageChanging = false;
            }

            //end confirmation when form dirty

            //load states
            $scope.init = function () {
                vm.newProcedureLevels = [{ procedure: {}, procedureLevel1: {}, procedureLevel2: {}, procedureLevel3: {}, procedureLevel4: {} }];
                vm.newProcedureLevels = vm.newProcedureLevels || [];
                vm.procedurePatterns = vm.procedurePatterns || [];

                vm.RelatedProcedureLevels = [{ procedure: {}, procedureLevel1: {}, procedureLevel2: {}, procedureLevel3: {}, procedureLevel4: {} }];
                vm.selectedProceduresList = [{ procedure: {}, procedureLevel1: {}, procedureLevel2: {}, procedureLevel3: {}, procedureLevel4: {} }];

                //ADDED MA
                //productsDataSoruce list
                $scope.procedureDataSoruce = {
                    transport: {
                        read: {
                            url: 'api/Practice/GetProcedures',
                        }
                    }
                };

                //companyDataSoruce list
                $scope.companyDataSoruce = {
                    transport: {
                        read: {
                            url: 'api/Metadata/GetCompanies',
                        }
                    }
                };

                //productsDataSoruce list
                $scope.productsDataSoruce = {
                    transport: {
                        read: {
                            url: 'api/Metadata/GetProducts',
                        }
                    }
                };

                $scope.existingProcedure;
                $scope.existingLevel1;
                $scope.existingLevel2;
                $scope.existingLevel3;
                $scope.existingLevel4;
                $scope.existingBreastImplant;


                if ($stateParams.userId === 'new') {

                } else {
                    ProcedureLevelService.getProcedureById($stateParams.userId)
                  .then(function (res) {
                      vm.newProcedureLevels[0] = res.data;

                      $scope.existingProcedure = vm.newProcedureLevels[0].name;
                      //$scope.existingLevel1 = vm.newProcedureLevels[0].procedureLevel1.name;
                      //$scope.existingLevel2 = vm.newProcedureLevels[0].procedureLevel2.name;
                      //$scope.existingLevel3 = vm.newProcedureLevels[0].procedureLevel3.name;
                      //$scope.existingLevel4 = vm.newProcedureLevels[0].  procedureLevel4.name;
                      //$scope.existingBreastImplant = vm.newProcedureLevels[0].practiceProductFlag;

                      //vm.newProcedureLevels.push(res.data);
                      //vm.practice.practiceUserList = res.data.practiceUserList;
                      //vm.practice.brestImplants = res.data.brestImplants;

                      //if (vm.practice.brestImplants != []) {
                      //    angular.forEach(vm.practice.brestImplants, function (data) {

                      //        var range = {
                      //            start: new Date(data.fromDate),
                      //            end: new Date(data.toDate)
                      //        };
                      //        vm.dateRange.push(range);
                      //    });
                      //}
                  })
                  .catch(function () {

                  });

                    ProcedureLevelService.getRelatedProcedureByProcedureId($stateParams.userId)
                  .then(function (res) {
                      vm.RelatedProcedureLevels = res.data;
                      //vm.newProcedureLevels.push(res.data);
                      //vm.practice.practiceUserList = res.data.practiceUserList;
                      //vm.practice.brestImplants = res.data.brestImplants;

                      //if (vm.practice.brestImplants != []) {
                      //    angular.forEach(vm.practice.brestImplants, function (data) {

                      //        var range = {
                      //            start: new Date(data.fromDate),
                      //            end: new Date(data.toDate)
                      //        };
                      //        vm.dateRange.push(range);
                      //    });
                      //}
                  })
                  .catch(function () {

                  });



                }

                //END ADDED MA


            }

            //$scope.save = function (form) {

            //};

            $scope.cancel = function () {
                $location.path('/admin/procedure-filter-mappings');
            };

            //$scope.procedureDatasource = {
            //    type: "webapi",
            //    serverFiltering: true,
            //    transport: {
            //        read: {
            //            url: function () {
            //                return "api/Metadata/ProcedureFilterByText";
            //            },
            //        },
            //        parameterMap: function (data, action) {
            //            if (Object.keys(data).length != 0) {
            //                var newParams = {
            //                    filter: data.filter.filters[0].value
            //                };
            //                return newParams;
            //            }
            //        }
            //    }
            //};

            $scope.level1Datasource = {
                type: "webapi",
                serverFiltering: true,
                transport: {
                    read: {
                        url: function () {
                            //return "api/Metadata/ProcedureLevelFilterByText/1";
                            return "api/Metadata/ProcedureLevelFilterByText/1/" + $stateParams.userId;
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
            };

            $scope.level2Datasource = {
                type: "webapi",
                serverFiltering: true,
                transport: {
                    read: {
                        url: function () {
                            //return "api/Metadata/ProcedureLevelFilterByText/2";
                            return "api/Metadata/ProcedureLevelFilterByText/2/" + $stateParams.userId;
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
            };

            $scope.level3Datasource = {
                type: "webapi",
                serverFiltering: true,
                transport: {
                    read: {
                        url: function () {
                            //return "api/Metadata/ProcedureLevelFilterByText/3/";
                            return "api/Metadata/ProcedureLevelFilterByText/3/" + $stateParams.userId;
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
            };

            $scope.level4Datasource = {
                type: "webapi",
                serverFiltering: true,
                transport: {
                    read: {
                        url: function () {
                            //return "api/Metadata/ProcedureLevelFilterByText/4/";
                            return "api/Metadata/ProcedureLevelFilterByText/4/" + $stateParams.userId;
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
            };

            //ADDED MA

            $scope.searchProc = function () {
                $scope.procFilter = $("#filterSearch").val();

                let typedText = $("#filterSearch").val();

                if (typedText == "") {
                    typedText = "NA";
                }
                typedText = typedText.replace(/\//gi, "_!").replace(/:/gi, "_~");
                // $scope.procFilter.trim()

                $('#procGrid').data('kendoGrid').dataSource.options.transport.read.url = '/Portal/api/Metadata/GetProcedureLevels/4/1/' + encodeURIComponent(typedText) + '/' + '?procedureId=' + $stateParams.userId;
                // $('#procGrid').data('kendoGrid').dataSource.read();
                $('#procGrid').data('kendoGrid').dataSource.page(1);
            }

            $scope.enterPress = function ($event) {
                if ($event.which === 13) {
                    $scope.searchProc();
                }
            }

            $scope.selectProcedure = function (dataitem, triggered) {
                $scope.filterMappingForm.$dirty = true;
                //$scope.enableSaveBtn(dataitem);
                $scope.dataitem = dataitem;
                // console.log($scope.dataitem);
                openProcModal();
                return dataitem.procedure;



                //if ($scope.dataitem && ($scope.dataitem.productTypeId != null || $scope.dataitem.procedureLevel1 != null)) {
                //    var item = $scope.dataitem;
                //    $scope.dataitem = null;
                //    return item;
                //} else {
                //    $scope.filterMappingForm.$dirty = true;
                //    //$scope.enableSaveBtn(dataitem);
                //    $scope.dataitem = dataitem;
                //    // console.log($scope.dataitem);
                //    openProcModal();
                //    return dataitem.procedure;
                //}
            };

            function openProcModal() {
                gridInitializeProcedures();

                $scope.modalInstance = $uibModal.open({
                    templateUrl: 'procModalTemplate',
                    appendTo: $('body'),
                    scope: $scope,
                    windowClass: 'newClass',
                    backdrop: 'static',
                    size: 'lg'
                });

                /*$('#procModal').modal('show');
                $(".modal-dialog").css({
                    'width': "1000px",
                    'height': "auto"
                });*/

                $scope.procModalOpened = true;
            };

            var checkedIds = {};

            function gridInitializeProcedures() {
                $scope.selectStatusText = "Select All"
                $scope.selectStatus = false;
                $scope.procFilter = !$scope.procFilter ? "" : $scope.procFilter;
                if (!$("#filterSearch").val()) {
                    $scope.procFilter = "";
                }
                $scope.mainGridOptions = {
                    dataSource: {
                        type: "json",
                        transport: {
                            read: {
                                url: function () {
                                    return '/Portal/api/Metadata/GetProcedureLevels/4/1/NA/?procedureId=' + $stateParams.userId
                                },
                            }
                        },
                        requestStart: function (e) {
                            // console.log(e);
                            kendo.ui.progress($("#emrMapping"), true);

                            // let gridData = $("#procGrid").data("kendoGrid").dataSource.data();
                            let gridData = e.sender.data();
                            let selectedList = gridData.filter(function (row) {
                                return (row.isSelect && row.isSelect === true);
                            });

                            if (!$scope.pageChanging && selectedList.length > 0) {
                                $scope.statusText = "Are you sure you want to leave this page?";
                                $scope.showProcModal = true;
                                $scope.nextPage = e.sender._page;
                                $scope.pageChanging = true;
                                $scope.$apply();
                                e.preventDefault();
                            }
                            else {
                                $scope.selectStatusText = "Select All"
                                $scope.selectStatus = false;
                                $scope.showProcModal = false;
                            }
                        },
                        pageSize: 8,
                        serverPaging: true,
                        serverSorting: true,
                        serverFiltering: true,
                        serverGrouping: true,
                        serverAggregates: true,
                        schema: {
                            data: "data",
                            total: "total",
                        },
                    },
                    selectable: false,
                    sortable: false,
                    pageable: true,
                    scrollable: true,
                    columns: [
                     {
                         field: "Id",
                         title: "Id",
                         width: "20%",
                         template: "{{dataItem.id}}"
                     }
                     , {
                         field: "name",
                         title: "Procedure Name",
                         width: "20%",
                         template: "<span bs-tooltip data-original-title='{{dataItem.name}}'>{{dataItem.name}}</span>"
                     }, {
                         field: "procedureLevel1",
                         title: "Level 1",
                         width: "20%",
                         template: "<span bs-tooltip data-original-title='{{dataItem.procedureLevel1.name}}'>{{dataItem.procedureLevel1.name}}</span>"
                     }, {
                         field: "procedureLevel1",
                         title: "Level 2",
                         width: "20%",
                         template: "<span bs-tooltip data-original-title='{{dataItem.procedureLevel2.name}}'>{{dataItem.procedureLevel2.name}}</span>"
                     }, {
                         field: "procedureLevel1",
                         title: "Level 3",
                         width: "20%",
                         template: "<span bs-tooltip data-original-title='{{dataItem.procedureLevel3.name}}'>{{dataItem.procedureLevel3.name}}</span>"
                     }
                     , {
                         field: "procedureLevel1",
                         title: "Level 4",
                         width: "20%",
                         template: "<span bs-tooltip data-original-title='{{dataItem.procedureLevel4.name}}'>{{dataItem.procedureLevel4.name}}</span>"
                     }
                     , {
                         field: "select",
                         title: "Select",
                         headerTemplate: "<button class=\"btn btn-primary\" type=\"button\" ng-click=\"selectAll()\"><small>{{selectStatusText}}</small></button>",
                         width: "20%",
                         template: "<input class=\"checkbox custom-margin-left\" type=\"checkbox\" ng-disabled=\"false\" ng-model=\"dataItem.isSelect\"  value=\"dataItem.isSelect\">"
                     }
                    ]
                };
            }

            $scope.selectAll = function () {
                if (!$scope.selectStatus) {
                    $scope.selectStatusText = "Unselect All"
                    var gridData = $("#procGrid").data("kendoGrid").dataSource.data();
                    _.each(gridData, function (item) { item.isSelect = true; });
                }
                else {
                    $scope.selectStatusText = "Select All";
                    var gridData = $("#procGrid").data("kendoGrid").dataSource.data();
                    _.each(gridData, function (item) { item.isSelect = false });
                }
                $scope.selectStatus = !$scope.selectStatus;
            };

            $scope.cancelModal = function () {
                $scope.modalInstance.close();
            }

            $scope.addSelected = function () {
                var gridData = $("#procGrid").data("kendoGrid").dataSource.data();
                vm.selectedProceduresList = gridData.filter(function (row) {
                    return (row.isSelect && row.isSelect === true);
                });

                _.each(vm.selectedProceduresList, function (e) {
                    let idList = _.map(vm.RelatedProcedureLevels, function (d) { return d.id });
                    if (idList.indexOf(e.id) === -1) {
                        vm.RelatedProcedureLevels.push(e);
                    }
                });

                //vm.RelatedProcedureLevels.concat(vm.selectedProceduresList);

                //vm.RelatedProcedureLevels = gridData.filter(function (row) {
                //    return (row.isSelect && row.isSelect === true);
                //});

                $scope.cancelModal();
            };

            //END ADDED MA

            $scope.checkStatus = function (level, item, index) {
                switch (level) {
                    case 1:
                        if (!item.name || index != 0) {
                            item.procedureLevel1 = null;
                            item.procedureLevel2 = null;
                            item.procedureLevel3 = null;
                            item.procedureLevel4 = null;

                            item.procedureLevelOne = "";
                            item.procedureLevelTwo = "";
                            item.procedureLevelThree = "";
                            item.procedureLevelFour = "";
                            return true;
                        }
                        else {
                            return false;
                        }
                    case 2:
                        if (!item.procedureLevelOne || index != 0) {
                            item.procedureLevel2 = null;
                            item.procedureLevel3 = null;
                            item.procedureLevel4 = null;

                            item.procedureLevelTwo = "";
                            item.procedureLevelThree = "";
                            item.procedureLevelFour = "";
                            return true;
                        }
                        else {
                            return false;
                        }
                    case 3:
                        if (!item.procedureLevelTwo || index != 0) {
                            item.procedureLevel3 = null;
                            item.procedureLevel4 = null;

                            item.procedureLevelThree = "";
                            item.procedureLevelFour = "";
                            return true;
                        }
                        else {
                            return false;
                        }
                    case 4:
                        if (!item.procedureLevelThree || index != 0) {
                            item.procedureLevel4 = null;

                            item.procedureLevelFour = "";
                            return true;
                        }
                        else {
                            return false;
                        }
                    default:
                        break;
                }
            }

            $scope.addNewProcedureLevel = function () {

                if (vm.newProcedureLevels.length > 0) {
                    if (isValidProcedureLevelSequence()) {
                        if (!isAlreadyExists()) {
                            var promise = isDuplicateProcedureLevelSequence(vm.newProcedureLevels[0].name, vm.newProcedureLevels[0].procedureLevel1, vm.newProcedureLevels[0].procedureLevel2, vm.newProcedureLevels[0].procedureLevel3, vm.newProcedureLevels[0].procedureLevel4);
                            $q.all([promise]).then(function (d) {
                                if (!vm.isDupplicateSequence) {
                                    vm.isNotCompletedSequence = false;
                                    vm.newProcedureLevels.unshift({ id: 0, name: null, procedureLevel1: null, procedureLevel2: null, procedureLevel3: null, procedureLevel4: null });
                                }
                            });
                        }

                    } else {
                        vm.isNotCompletedSequence = true;
                    }
                } else {
                    if (vm.newProcedureLevels.length == 0) {
                        vm.newProcedureLevels = [{ id: 0, name: null, procedureLevel1: null, procedureLevel2: null, procedureLevel3: null, procedureLevel4: null }];
                    }
                }
            };

            $scope.saveProcedureLevelSequences = function (sequences, procId) {
                //vm.isNotCompletedSequence = false;
                //vm.isDupplicateSequence = false;
                if (sequences[0].name != undefined) {

                    if ($scope.existingProcedure === sequences[0].name) {

                        if (sequences[0].procedureLevel1 == null || sequences[0].procedureLevel2 == null) {
                            vm.isNotCompletedSequence = true;
                            return false;
                        }

                        if ((sequences[0].procedureLevel1 != null && sequences[0].procedureLevel1.id == "") ||
                        (sequences[0].procedureLevel2 != null && sequences[0].procedureLevel2.id == "")) {
                            vm.isNotCompletedSequence = true;
                            return false;
                        }

                        sequences[0].relatedProcedureId = _.map(vm.RelatedProcedureLevels, function (thisProcedure) {
                            return thisProcedure.id;
                        });

                        ProcedureLevelService.procedureRelatedProcedure(sequences[0])
                                 .then(function (res) {
                                     var test = res.data;
                                     vm.isNotCompletedSequence = false;
                                     vm.isDupplicateSequence = false;
                                     toaster.success({ title: "", body: "Procedure added successfully." });
                                     $scope.confirmResponded = true;
                                     $scope.leavePage = true;
                                     $scope.isSave = true;

                                     $scope.cancel();
                                     // gridInitialize();
                                     //_.forEach(sequences, function (item) {
                                     //    vm.procedurelevels.unshift(item);
                                     //});
                                     vm.newProcedureLevels = [{ id: 0, name: null, procedureLevel1: null, procedureLevel2: null, procedureLevel3: null, procedureLevel4: null }];

                                 })
                                 .catch(function (err) {
                                     console.log(err);
                                 });
                    }
                    else if (isValidProcedureLevelSequence() || (!vm.isDupplicateSequence && vm.isDupplicateSequence != undefined)) {

                        if (sequences[0].procedureLevel1 == null || sequences[0].procedureLevel2 == null) {
                            vm.isNotCompletedSequence = true;
                            return false;
                        }

                        if (!isAlreadyExists()) {

                            var promise = isDuplicateProcedureLevelSequence(vm.newProcedureLevels[0].name, vm.newProcedureLevels[0].procedureLevel1, vm.newProcedureLevels[0].procedureLevel2, vm.newProcedureLevels[0].procedureLevel3, vm.newProcedureLevels[0].procedureLevel4);


                            sequences[0].relatedProcedureId = _.map(vm.RelatedProcedureLevels, function (thisProcedure) {
                                return thisProcedure.id;
                            });

                            $q.all([promise]).then(function (d) {

                                if (!vm.isDupplicateSequence) {

                                    ProcedureLevelService.procedureRelatedProcedure(sequences[0])
                                            .then(function (res) {
                                                var test = res.data;
                                                vm.isNotCompletedSequence = undefined;
                                                vm.isDupplicateSequence = undefined;
                                                toaster.success({ title: "", body: "Procedure added successfully." });
                                                $scope.confirmResponded = true;
                                                $scope.leavePage = true;
                                                $scope.isSave = true;

                                                $scope.cancel();
                                                // gridInitialize();
                                                //_.forEach(sequences, function (item) {
                                                //    vm.procedurelevels.unshift(item);
                                                //});
                                                vm.newProcedureLevels = [{ id: 0, name: null, procedureLevel1: null, procedureLevel2: null, procedureLevel3: null, procedureLevel4: null }];

                                            })
                                            .catch(function (err) {
                                                console.log(err);
                                            });
                                } else {
                                    vm.isNotCompletedSequence = undefined;
                                    vm.isDupplicateSequence = true;
                                }
                            });
                        }
                    } else {
                        vm.isNotCompletedSequence = true;
                    }
                } else {
                    vm.isNotCompletedSequence = true;
                }
            };

            $scope.removeRelatedProcedureLevel = function (index) {
                vm.RelatedProcedureLevels.splice(index, 1);
            };

            $scope.removeNewProcedureLevel = function (index) {
                var level1 = vm.newProcedureLevels[index].procedureLevel1 != null ? vm.newProcedureLevels[index].procedureLevel1.id : "";
                var level2 = vm.newProcedureLevels[index].procedureLevel2 != null ? vm.newProcedureLevels[index].procedureLevel2.id : "";
                var level3 = vm.newProcedureLevels[index].procedureLevel3 != null ? vm.newProcedureLevels[index].procedureLevel3.id : "";
                var level4 = vm.newProcedureLevels[index].procedureLevel4 != null ? vm.newProcedureLevels[index].procedureLevel4.id : "";

                var pattern = "" + level1 + "" + level2 + "" + level3 + "" + level4 + "";

                var patternIndex = vm.procedurePatterns.indexOf(pattern);
                if (patternIndex != -1) {
                    vm.procedurePatterns.splice(patternIndex, 1);
                }
                vm.newProcedureLevels.splice(index, 1);
            };

            function isDuplicateProcedureLevelSequence(name, procedureLevel1, procedureLevel2, procedureLevel3, procedureLevel4) {
                var deferred = $q.defer();

                var level1 = procedureLevel1 != null ? procedureLevel1.id : null;
                var level2 = procedureLevel2 != null ? procedureLevel2.id : null;
                var level3 = procedureLevel3 != null ? procedureLevel3.id : null;
                var level4 = procedureLevel4 != null ? procedureLevel4.id : null;

                ProcedureLevelService.isDuplicateProcedureLevelSequence(name, level1, level2, level3, level4)
                 .then(function (res) {
                     vm.isDupplicateSequence = res.data;
                     deferred.resolve(res.data);
                 })
                 .catch(function (error, status) {
                     console.log(error);
                     if (status == -1) {
                         toaster.error({ title: "", body: "Cannot perform the function due to network failure." });
                     } else {
                         toaster.error({ title: "", body: "Cannot perform the function Error occured." });
                     }
                 });
                return deferred.promise;
            }

            function isAlreadyExists() {
                var name = vm.newProcedureLevels[0].name != null ? vm.newProcedureLevels[0].name : "";
                var level1 = vm.newProcedureLevels[0].procedureLevel1 != null ? vm.newProcedureLevels[0].procedureLevel1.id : "";
                var level2 = vm.newProcedureLevels[0].procedureLevel2 != null ? vm.newProcedureLevels[0].procedureLevel2.id : "";
                var level3 = vm.newProcedureLevels[0].procedureLevel3 != null ? vm.newProcedureLevels[0].procedureLevel3.id : "";
                var level4 = vm.newProcedureLevels[0].procedureLevel4 != null ? vm.newProcedureLevels[0].procedureLevel4.id : "";

                var pattern = "" + name + "" + level1 + "" + level2 + "" + level3 + "" + level4 + "";

                if (-1 != vm.procedurePatterns.indexOf(pattern)) {
                    vm.isDupplicateSequence = true;
                    return true;
                } else {
                    vm.isDupplicateSequence = undefined;
                    vm.procedurePatterns.unshift(pattern);
                    return false;
                }
            }

            function isValidProcedureLevelSequence() {
                if ((vm.newProcedureLevels[0].name == null || vm.newProcedureLevels[0].name == "" || vm.newProcedureLevels[0].procedureLevel1 == null) ||
                    (vm.newProcedureLevels[0].procedureLevel1 != null && vm.newProcedureLevels[0].procedureLevel1.id == "") ||
                    (vm.newProcedureLevels[0].procedureLevel2 != null && vm.newProcedureLevels[0].procedureLevel2.id == "") ||
                    (vm.newProcedureLevels[0].procedureLevel3 != null && vm.newProcedureLevels[0].procedureLevel3.id == "") ||
                    (vm.newProcedureLevels[0].procedureLevel4 != null && vm.newProcedureLevels[0].procedureLevel4.id == "")) {
                    return false;

                } else {
                    if ((vm.newProcedureLevels[0].name != null || vm.newProcedureLevels[0].name != "")
                        && (vm.newProcedureLevels[0].procedureLevel1 != null
                        && (vm.newProcedureLevels[0].procedureLevel1.id == ""
                        || vm.newProcedureLevels[0].procedureLevel1.id == undefined))
                        || (vm.newProcedureLevels[0].procedureLevel2 != null
                        && (vm.newProcedureLevels[0].procedureLevel2.id == ""
                        || vm.newProcedureLevels[0].procedureLevel2.id == undefined))) {
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            }
        })
            .controller('Admin.ProcedureFilterMappingsCtrl.EditCtrl', function ($scope, $timeout, $stateParams, toaster, $filter, $location, ProcedureLevelService, $q) {
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
                        if ($scope.filterMappingForm) {
                            if ($scope.filterMappingForm.$dirty) {
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
                    vm.newProcedureLevels = [{ procedure: {}, procedureLevel1: {}, procedureLevel2: {}, procedureLevel3: {}, procedureLevel4: {} }];
                    vm.newProcedureLevels = vm.newProcedureLevels || [];
                    vm.procedurePatterns = vm.procedurePatterns || [];
                }

                $scope.save = function (form) {


                };

                $scope.cancel = function () {
                    $location.path('/admin/procedure-filter-mappings');
                };


                //$scope.procedureDatasource = {
                //    type: "webapi",
                //    serverFiltering: true,
                //    transport: {
                //        read: {
                //            url: function () {
                //                return "api/Metadata/ProcedureFilterByText";
                //            },
                //        },
                //        parameterMap: function (data, action) {
                //            if (Object.keys(data).length != 0) {
                //                var newParams = {
                //                    filter: data.filter.filters[0].value
                //                };
                //                return newParams;
                //            }
                //        }
                //    }
                //};

                $scope.level1Datasource = {
                    type: "webapi",
                    serverFiltering: true,
                    transport: {
                        read: {
                            url: function () {
                                return "api/Metadata/ProcedureLevelFilterByText/1";
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
                };

                $scope.level2Datasource = {
                    type: "webapi",
                    serverFiltering: true,
                    transport: {
                        read: {
                            url: function () {
                                return "api/Metadata/ProcedureLevelFilterByText/2";
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
                };

                $scope.level3Datasource = {
                    type: "webapi",
                    serverFiltering: true,
                    transport: {
                        read: {
                            url: function () {
                                return "api/Metadata/ProcedureLevelFilterByText/3";
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
                };

                $scope.level4Datasource = {
                    type: "webapi",
                    serverFiltering: true,
                    transport: {
                        read: {
                            url: function () {
                                return "api/Metadata/ProcedureLevelFilterByText/4";
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
                };

                $scope.addNewProcedureLevel = function () {

                    if (vm.newProcedureLevels.length > 0) {
                        if (isValidProcedureLevelSequence()) {

                            if (!isAlreadyExists()) {
                                var promise = isDuplicateProcedureLevelSequence(vm.newProcedureLevels[0].name, vm.newProcedureLevels[0].procedureLevel1, vm.newProcedureLevels[0].procedureLevel2, vm.newProcedureLevels[0].procedureLevel3, vm.newProcedureLevels[0].procedureLevel4);
                                $q.all([promise]).then(function (d) {
                                    if (!vm.isDupplicateSequence) {
                                        vm.isNotCompletedSequence = false;
                                        vm.newProcedureLevels.unshift({ id: 0, name: null, procedureLevel1: null, procedureLevel2: null, procedureLevel3: null, procedureLevel4: null });
                                    }

                                });
                            }

                        } else {
                            vm.isNotCompletedSequence = true;
                        }
                    } else {
                        if (vm.newProcedureLevels.length == 0) {
                            vm.newProcedureLevels = [{ id: 0, name: null, procedureLevel1: null, procedureLevel2: null, procedureLevel3: null, procedureLevel4: null }];
                        }
                    }

                };

                $scope.saveProcedureLevelSequences = function (sequences) {
                    if (isValidProcedureLevelSequence() || (!vm.isDupplicateSequence && vm.isDupplicateSequence != undefined)) {
                        if (!isAlreadyExists()) {

                            var promise = isDuplicateProcedureLevelSequence(vm.newProcedureLevels[0].name, vm.newProcedureLevels[0].procedureLevel1, vm.newProcedureLevels[0].procedureLevel2, vm.newProcedureLevels[0].procedureLevel3, vm.newProcedureLevels[0].procedureLevel4);

                            $q.all([promise]).then(function (d) {
                                if (!vm.isDupplicateSequence) {
                                    ProcedureLevelService.saveProcedureLevelSequences(sequences)
                                 .then(function (res) {
                                     var test = res.data;
                                     vm.isNotCompletedSequence = false;
                                     vm.isDupplicateSequence = false;
                                     toaster.success({ title: "", body: "Procedure level sequence added successfully." });
                                     $scope.confirmResponded = true;
                                     $scope.leavePage = true;
                                     $scope.isSave = true;

                                     $scope.cancel();
                                     gridInitialize();
                                     _.forEach(sequences, function (item) {
                                         vm.procedurelevels.unshift(item);
                                     });
                                     vm.newProcedureLevels = [{ id: 0, name: null, procedureLevel1: null, procedureLevel2: null, procedureLevel3: null, procedureLevel4: null }];


                                 })
                                 .catch(function (err) {
                                     console.log(err);
                                 });
                                } else {
                                    vm.isNotCompletedSequence = true;
                                    vm.isDupplicateSequence = false;
                                }
                            });
                        }
                    } else {
                        vm.isNotCompletedSequence = true;
                    }

                };

                $scope.removeNewProcedureLevel = function (index) {
                    var level1 = vm.newProcedureLevels[index].procedureLevel1 != null ? vm.newProcedureLevels[index].procedureLevel1.id : "";
                    var level2 = vm.newProcedureLevels[index].procedureLevel2 != null ? vm.newProcedureLevels[index].procedureLevel2.id : "";
                    var level3 = vm.newProcedureLevels[index].procedureLevel3 != null ? vm.newProcedureLevels[index].procedureLevel3.id : "";
                    var level4 = vm.newProcedureLevels[index].procedureLevel4 != null ? vm.newProcedureLevels[index].procedureLevel4.id : "";

                    var pattern = "" + level1 + "" + level2 + "" + level3 + "" + level4 + "";

                    var patternIndex = vm.procedurePatterns.indexOf(pattern);
                    if (patternIndex != -1) {
                        vm.procedurePatterns.splice(patternIndex, 1);
                    }
                    vm.newProcedureLevels.splice(index, 1);
                };

                function isDuplicateProcedureLevelSequence(name, procedureLevel1, procedureLevel2, procedureLevel3, procedureLevel4) {
                    var deferred = $q.defer();

                    var level1 = procedureLevel1 != null ? procedureLevel1.id : null;
                    var level2 = procedureLevel2 != null ? procedureLevel2.id : null;
                    var level3 = procedureLevel3 != null ? procedureLevel3.id : null;
                    var level4 = procedureLevel4 != null ? procedureLevel4.id : null;

                    ProcedureLevelService.isDuplicateProcedureLevelSequence(name, level1, level2, level3, level4)
                     .then(function (res) {

                         vm.isDupplicateSequence = res.data;
                         deferred.resolve(res.data);
                     })
                     .catch(function (error, status) {
                         console.log(error);
                         if (status == -1) {
                             toaster.error({ title: "", body: "Cannot perform the function due to network failure." });
                         } else {
                             toaster.error({ title: "", body: "Cannot perform the function Error occured." });
                         }
                     });
                    return deferred.promise;
                }

                function isAlreadyExists() {

                    var level1 = vm.newProcedureLevels[0].procedureLevel1 != null ? vm.newProcedureLevels[0].procedureLevel1.id : "";
                    var level2 = vm.newProcedureLevels[0].procedureLevel2 != null ? vm.newProcedureLevels[0].procedureLevel2.id : "";
                    var level3 = vm.newProcedureLevels[0].procedureLevel3 != null ? vm.newProcedureLevels[0].procedureLevel3.id : "";
                    var level4 = vm.newProcedureLevels[0].procedureLevel4 != null ? vm.newProcedureLevels[0].procedureLevel4.id : "";

                    var pattern = "" + level1 + "" + level2 + "" + level3 + "" + level4 + "";

                    if (-1 != vm.procedurePatterns.indexOf(pattern)) {
                        vm.isDupplicateSequence = true;
                        return true;
                    } else {
                        vm.isDupplicateSequence = false;
                        vm.procedurePatterns.unshift(pattern);
                        return false;
                    }

                }

                function isValidProcedureLevelSequence() {

                    if ((vm.newProcedureLevels[0].name == null || vm.newProcedureLevels[0].name == "" || vm.newProcedureLevels[0].procedureLevel1 == null) ||
                        (vm.newProcedureLevels[0].procedureLevel1 != null && vm.newProcedureLevels[0].procedureLevel1.id == "") ||
                        (vm.newProcedureLevels[0].procedureLevel2 != null && vm.newProcedureLevels[0].procedureLevel2.id == "") ||
                        (vm.newProcedureLevels[0].procedureLevel3 != null && vm.newProcedureLevels[0].procedureLevel3.id == "") ||
                        (vm.newProcedureLevels[0].procedureLevel4 != null && vm.newProcedureLevels[0].procedureLevel4.id == "")) {
                        return false;

                    } else {
                        if ((vm.newProcedureLevels[0].name != null || vm.newProcedureLevels[0].name != "") && (vm.newProcedureLevels[0].procedureLevel1 != null && (vm.newProcedureLevels[0].procedureLevel1.id == "" || vm.newProcedureLevels[0].procedureLevel1.id == undefined))) {
                            return false;
                        }
                        return true;
                    }
                }

            });
})();
