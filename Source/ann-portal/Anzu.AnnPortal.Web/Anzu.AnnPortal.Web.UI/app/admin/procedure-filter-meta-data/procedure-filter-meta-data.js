(function () {
    'use strict';
    angular.module('annPortalApp')
        .config(function (stateHelperProvider) {
            stateHelperProvider
                .state({
                    name: 'admin.procedure-filter-meta-data',
                    url: '/procedure-filter-meta-data',
                    templateUrl: 'app/admin/procedure-filter-meta-data/procedure-filter-meta-data.html',
                    controller: 'Admin.ProcedureFilterMetaDataCtrl',
                });
        })
        .controller('Admin.ProcedureFilterMetaDataCtrl', function ($scope, $timeout, ProcedureLevelService, BreastImplantService, toaster, $location) {
            var vm = $scope.vm = { newItem: { level1: {}, level2: {}, level3: {}, level4: {}, breastImplant: {}, productsSold: {}, productCompanie: {}, isItemExist: false, isItemEmpty: false } };

            $scope.levelSubmitted = false;
            $scope.breastSubmitted = false;
            $scope.productSubmitted = false;
            $scope.companySubmitted = false;

            //start confirmation when form dirty

            $scope.showModal = false;
            $scope.confirmResponded = false;
            $scope.leavePage = false;
            $scope.nextLocation = "#";
            $scope.$watch('confirmResponded', function () {
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
            });

            $scope.$on('$locationChangeStart', function (event, next, current) {
                if (!$scope.confirmResponded) {
                    if ($scope.filterForm) {
                        if ($scope.filterForm.$dirty) {
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

            $scope.curTab = 1;
            ProcedureLevelService.getProcedureLevel($scope.curTab)
                .then(function (res) {
                    vm.items = res.data;
                    vm.levels = {
                        level1: [],
                        level2: [],
                        level3: [],
                        level4: [],
                    };

                    _.forEach(vm.items, function (item) {
                        vm.levels['level' + item.levelNumber].unshift(item);
                    });

                })
                .catch(function (err) {
                    console.log(err);
                });

            $scope.saveLevel = function () {

                var isDupplicateResult = false;
                for (var i = 0; i < vm.levels["level" + $scope.curTab].length; i++) {
                    var result = $scope.isDupplicateLevel(i);
                    if (result) {
                        isDupplicateResult = true;
                    }
                }

                if (isDupplicateResult) {
                    $scope.vm.isItemExist = true;
                } else {
                    $scope.levelSubmitted = true;
                    $scope.vm.isItemExist = false;

                    // Check for empty items
                    var nullItems = $scope.emptyFieldValidation();

                    if (nullItems.length == 0) {
                        $scope.vm.isItemEmpty = false;
                        // get tab list and update
                        ProcedureLevelService.saveProcedureLevels(vm.levels["level" + $scope.curTab])
                           .then(function (res) {
                               toaster.success({ title: "", body: "Procedure Level " + $scope.curTab + " has been successfully updated." });
                               vm.items = res.data;
                               vm.levels['level' + $scope.curTab] = [];
                               _.forEach(vm.items, function (item) {
                                   vm.levels['level' + item.levelNumber].push(item);
                               });

                               if ($scope.filterForm) {
                                   $scope.filterForm.$dirty = false;
                               }
                               //  $scope.levelSubmitted = false;
                           })
                           .catch(function (err) {
                               console.log(err);
                               $scope.levelSubmitted = false;
                           });
                    }
                    else {
                        $scope.vm.isItemEmpty = true;
                    }
                }
            };

            $scope.emptyFieldValidation = function () {
                // Check for empty items
                var nullItems = vm.levels["level" + $scope.curTab].filter(function (level) {
                    return level.name == null || level.name.trim() == "";
                });
                return nullItems;
            }

            $scope.emptyFieldValidationBreastImplants = function () {
                var nullItems = vm.breastImplants.filter(function (level) {
                    return level.name == null || level.name.trim() == "";
                });
                return nullItems;
            }

            $scope.emptyFieldValidationProductsSold = function () {
                var nullItems = vm.productsSolds.filter(function (level) {
                    return level.name == null || level.name.trim() == "";
                });
                return nullItems;
            }

            $scope.emptyFieldValidationProductCompanies = function () {
                var nullItems = vm.productCompanies.filter(function (level) {
                    return level.name == null || level.name.trim() == "";
                });
                return nullItems;
            }

            $scope.changeTabLevel = function (tabLevelId) {
                $scope.searchText = '';
                $scope.curTab = tabLevelId;
                $scope.vm.isItemExist = false;
                $scope.vm.isItemEmpty = false;
                if ($scope.filterForm) {
                    $scope.filterForm.$dirty = false;
                }
                if (vm.levels["level" + tabLevelId].length == 0 && $scope.curTab > 0 && $scope.curTab < 5) {
                    getProcedureLevel($scope.curTab);
                }
            };

            $scope.addToLevel = function () {
                $scope.searchText = '';
                $scope.vm.isItemExist = false;
                $scope.levelSubmitted = false;
                if (vm.levels['level' + $scope.curTab].length == 0 || vm.levels['level' + $scope.curTab][0].Id != 0) {
                    if ($scope.filterForm) {
                        $scope.filterForm.$setDirty();
                    }
                    vm.levels['level' + $scope.curTab].unshift({
                        Id: 0,
                        name: null,
                        LevelNumber: $scope.curTab,
                        IsProduct: null,
                        RecordStatusId: 1
                    });
                }
                else {
                    if (vm.levels['level' + $scope.curTab][0].Id == 0 && vm.levels['level' + $scope.curTab][0].name == null) {
                        //Check for empty items
                    } else {
                        if ($scope.isDupplicateLevel(0) && vm.levels['level' + $scope.curTab][0].RecordStatusId != 2) {
                            $scope.vm.isItemExist = true;

                        } else {
                            if ($scope.filterForm) {
                                $scope.filterForm.$setDirty();
                            }
                            vm.levels['level' + $scope.curTab].unshift({
                                Id: 0,
                                name: null,
                                LevelNumber: $scope.curTab,
                                IsProduct: null,
                                RecordStatusId: null
                            });
                        }
                    }
                }
            };

            $scope.removeFromLevel = function (list, index) {
                if (index > -1) {
                    if ($scope.filterForm) {
                        $scope.filterForm.$setDirty();
                    }
                    if (list[index].id > 0) {
                        list[index].RecordStatusId = 2;
                        $scope.levelSubmitted = false;
                    } else {

                        list.splice(index, 1);
                    }
                }
            };

            BreastImplantService.getBreastImplants()
               .then(function (res) {
                   vm.breastImplants = res.data;

               })
               .catch(function (err) {
                   console.log(err);
               });

            $scope.removeBreastImplant = function (id) {
                let index = _.findIndex(vm.breastImplants, { id: id });
                if (index > -1) {
                    if ($scope.filterForm) {
                        $scope.filterForm.$setDirty();
                    }
                    if (vm.breastImplants[index].id > 0) {
                        vm.breastImplants[index].recordStatusId = 2;
                        $scope.breastSubmitted = false;
                    } else {
                        vm.breastImplants.splice(index, 1);
                    }
                }
            };

            $scope.addToBreastImplants = function () {
                $scope.searchText = '';
                $scope.vm.isItemExist = false;
                $scope.breastSubmitted = false;
                if (vm.breastImplants.length == 0 || vm.breastImplants[0].id != null) {
                    if ($scope.filterForm) {
                        $scope.filterForm.$setDirty();
                    }
                    vm.breastImplants.unshift({
                        Id: 0,
                        name: null
                    });
                } else {
                    if (vm.breastImplants[0].Id == 0 && vm.breastImplants[0].name == null) {
                        //Check for empty items
                    } else {
                        if ($scope.isDupplicateBreastImplant(0) && vm.breastImplants[0].recordStatusId != 2) {
                            $scope.vm.isItemExist = true;

                        } else {
                            if ($scope.filterForm) {
                                $scope.filterForm.$setDirty();
                            }
                            vm.breastImplants.unshift({
                                Id: 0,
                                name: null
                            });
                        }
                    }
                }

            };

            $scope.saveBreastImplants = function () {

                var isDupplicateResult = false;
                for (var i = 0; i < vm.breastImplants.length; i++) {
                    var result = $scope.isDupplicateBreastImplant(i);
                    if (result) {
                        isDupplicateResult = true;
                    }

                }

                if (isDupplicateResult) {
                    $scope.vm.isItemExist = true;

                } else {
                    $scope.breastSubmitted = true;
                    $scope.vm.isItemExist = false;

                    var nullItems = $scope.emptyFieldValidationBreastImplants();

                    if (nullItems == 0) {
                        $scope.vm.isItemEmpty = false;
                        BreastImplantService.saveBreastImplants(vm.breastImplants)
                           .then(function (res) {

                               toaster.success({ title: "", body: "Breast Implants have been successfully updated." });
                               vm.breastImplants = res.data;

                               if ($scope.filterForm) {
                                   $scope.filterForm.$dirty = false;
                               }
                               //vm.levels['level' + $scope.curTab] = [];
                               //_.forEach(vm.items, function (item) {
                               //    vm.levels['level' + item.levelNumber].unshift(item);
                               //});
                               //$scope.breastSubmitted = false;
                           })
                           .catch(function (err) {
                               console.log(err);
                               $scope.breastSubmitted = false;
                           });
                    }
                    else {
                        $scope.vm.isItemEmpty = true;
                    }
                }
            }

            function getProcedureLevel(levelId) {
                ProcedureLevelService.getProcedureLevel(levelId)
                  .then(function (res) {
                      vm.levels['level' + levelId] = [];
                      _.forEach(res.data, function (item) {
                          vm.levels['level' + item.levelNumber].unshift(item);
                      });

                  })
                  .catch(function (err) {
                      console.log(err);
                  });
            }

            BreastImplantService.getProducts()
               .then(function (res) {
                   vm.productsSolds = res.data;

               })
               .catch(function (err) {
                   console.log(err);
               });

            $scope.addToProdcutsSold = function () {
                $scope.searchText = '';
                $scope.vm.isItemExist = false;
                $scope.productSubmitted = false;
                if (vm.productsSolds.length == 0 || vm.productsSolds[0].id != null) {
                    if ($scope.filterForm) {
                        $scope.filterForm.$setDirty();
                    }
                    vm.productsSolds.unshift({
                        Id: 0,
                        name: null
                    });
                } else {
                    if (vm.productsSolds[0].Id == 0 && vm.productsSolds[0].name == null) {
                        //Check for empty items
                    } else {
                        if ($scope.isDupplicateProductsSold(0) && vm.productsSolds[0].recordStatusId != 2) {
                            $scope.vm.isItemExist = true;

                        } else {
                            if ($scope.filterForm) {
                                $scope.filterForm.$setDirty();
                            }
                            vm.productsSolds.unshift({
                                Id: 0,
                                name: null
                            });
                        }
                    }
                }

            };

            $scope.saveProdcutsSolds = function () {

                var isDupplicateResult = false;
                for (var i = 0; i < vm.productsSolds.length; i++) {
                    var result = $scope.isDupplicateProductsSold(i);
                    if (result) {
                        isDupplicateResult = true;
                    }

                }

                if (isDupplicateResult) {
                    $scope.vm.isItemExist = true;

                } else {
                    $scope.productSubmitted = true;
                    $scope.vm.isItemExist = false;

                    var nullItems = $scope.emptyFieldValidationProductsSold();

                    if (nullItems == 0) {
                        $scope.vm.isItemEmpty = false;
                        BreastImplantService.saveProducts(vm.productsSolds)
                       .then(function (res) {

                           toaster.success({ title: "", body: "Product Sold have been successfully updated." });
                           vm.productsSolds = res.data;

                           if ($scope.filterForm) {
                               $scope.filterForm.$dirty = false;
                           }
                           //vm.levels['level' + $scope.curTab] = [];
                           //_.forEach(vm.items, function (item) {
                           //    vm.levels['level' + item.levelNumber].unshift(item);
                           //});
                           //  $scope.productSubmitted = false;
                       })
                       .catch(function (err) {
                           console.log(err);
                           $scope.productSubmitted = false;
                       });
                    }
                    else {
                        $scope.vm.isItemEmpty = true;
                    }


                }
            }

            $scope.removeProdcutsSold = function (id) {
                let index = _.findIndex(vm.productsSolds, { id: id });
                if (index > -1) {
                    if ($scope.filterForm) {
                        $scope.filterForm.$setDirty();
                    }
                    if (vm.productsSolds[index].id > 0) {
                        vm.productsSolds[index].recordStatusId = 2;
                        $scope.productSubmitted = false;
                    } else {
                        vm.productsSolds.splice(index, 1);
                    }
                }
            };

            BreastImplantService.getCompanies()
               .then(function (res) {
                   vm.productCompanies = res.data;

               })
               .catch(function (err) {
                   console.log(err);
               });

            $scope.addToProductCompanie = function () {
                $scope.searchText = '';
                // $scope.vm.isItemExist = false;
                $scope.companySubmitted = false;
                if (vm.productCompanies.length == 0 || vm.productCompanies[0].id != null) {
                    if ($scope.filterForm) {
                        $scope.filterForm.$setDirty();
                    }
                    vm.productCompanies.unshift({
                        Id: 0,
                        name: null
                    });
                } else {
                    if (vm.productCompanies[0].Id == 0 && vm.productCompanies[0].name == null) {
                        //Check for empty items
                    } else {
                        if ($scope.isDupplicateProductsCompanies(0) && vm.productCompanies[0].recordStatusId != 2) {
                            $scope.vm.isItemExist = true;

                        } else {
                            $scope.vm.productCompanies[0].isError = false;
                            if ($scope.filterForm) {
                                $scope.filterForm.$setDirty();
                            }
                            vm.productCompanies.unshift({
                                Id: 0,
                                name: null
                            });
                        }
                    }
                }
            };

            $scope.cancel = function () {
                $location.path('/admin/practice-management');
            };

            $scope.saveProductCompanies = function () {
                var isDupplicateResult = false;
                for (var i = 0; i < vm.productCompanies.length; i++) {
                    var result = $scope.isDupplicateProductsCompanies(i);
                    if (result) {
                        isDupplicateResult = true;
                    }

                }

                if (isDupplicateResult) {
                    $scope.vm.isItemExist = true;

                } else {
                    $scope.companySubmitted = true;
                    $scope.vm.isItemExist = false;

                    var nullItems = $scope.emptyFieldValidationProductCompanies();

                    if (nullItems == 0) {
                        $scope.vm.isItemEmpty = false;
                        BreastImplantService.saveCompanies(vm.productCompanies)
                       .then(function (res) {

                           toaster.success({ title: "", body: "Product Companies have been successfully updated." });
                           vm.productCompanies = res.data;

                           if ($scope.filterForm) {
                               $scope.filterForm.$dirty = false;
                           }
                           //vm.levels['level' + $scope.curTab] = [];
                           //_.forEach(vm.items, function (item) {
                           //    vm.levels['level' + item.levelNumber].unshift(item);
                           //});
                           // $scope.companySubmitted = false;
                       })
                       .catch(function (err) {
                           console.log(err);
                           $scope.companySubmitted = false;
                       });
                    }
                    else {
                        $scope.vm.isItemEmpty = true;
                    }
                }
            }

            $scope.removeProductCompanies = function (id) {
                let index = _.findIndex(vm.productCompanies, { id: id });
                if (index > -1) {
                    if ($scope.filterForm) {
                        $scope.filterForm.$setDirty();
                    }

                    if (vm.productCompanies[index].id > 0) {
                        vm.productCompanies[index].recordStatusId = 2;
                        $scope.companySubmitted = false;
                    } else {
                        vm.productCompanies.splice(index, 1);
                    }
                }
            };

            $scope.isDupplicateLevel = function (index) {
                var existingItems = vm.levels['level' + $scope.curTab].filter(function (item) {
                    if (item.name != null && vm.levels['level' + $scope.curTab][index].name != undefined && vm.levels['level' + $scope.curTab][index].name != null) {
                        return item.name.toLowerCase() == vm.levels['level' + $scope.curTab][index].name.toLowerCase();
                    }
                });

                if ((existingItems != null && existingItems.length > 1)) {
                    $scope.vm.isItemExist = true;
                    vm.levels['level' + $scope.curTab][index].isError = true;
                    return true;

                } else if (vm.levels['level' + $scope.curTab][0].name == '') {
                    return false;
                } else {
                    $scope.vm.isItemExist = false;
                    vm.levels['level' + $scope.curTab][index].isError = false;
                    return false;

                    //if (vm.levels['level' + $scope.curTab][0].Id == 0 && vm.levels['level' + $scope.curTab][0].name != null && vm.levels['level' + $scope.curTab][0].name != "") {
                    //    ProcedureLevelService.isProcedureLevelUnique($scope.curTab, vm.levels['level' + $scope.curTab][0].name)
                    //   .then(function (res) {
                    //       if (res.data == true) {
                    //           return true;
                    //       } else {
                    //           return false;
                    //       }
                    //   })
                    //   .catch(function (err) {
                    //       console.log(err);
                    //   });

                    //}
                }
            };

            $scope.isDupplicateBreastImplant = function (index) {
                var existingItems = vm.breastImplants.filter(function (item) {

                    if (item.name != null && vm.breastImplants[index] != null && vm.breastImplants[index].name != null) {
                        return item.name.toLowerCase() == vm.breastImplants[index].name.toLowerCase()
                    }
                });

                if (existingItems != null && existingItems.length > 1) {
                    $scope.vm.isItemExist = true;
                    $scope.vm.breastImplants[index].isError = true;
                    return true;

                } else if (vm.breastImplants[0].name == '') {
                    return false;
                } else {
                    $scope.vm.isItemExist = false;
                    $scope.vm.breastImplants[index].isError = false;
                    return false;
                    //if (vm.breastImplants[0].Id == 0 && vm.breastImplants[0].name != null && vm.breastImplants[0].name != "") {
                    //    ProcedureLevelService.isBreastImplantUnique(vm.breastImplants[0].name)
                    //   .then(function (res) {
                    //       if (res.data == true) {
                    //           return true;

                    //       } else {
                    //           return false;
                    //       }
                    //   })
                    //   .catch(function (err) {
                    //       console.log(err);

                    //   });

                    //}
                }
            };

            $scope.isDupplicateProductsSold = function (index) {
                var existingItems = vm.productsSolds.filter(function (item) {
                    if (item.name != null && vm.productsSolds[index] != null && vm.productsSolds[index].name != null) {
                        return item.name.toLowerCase() == vm.productsSolds[index].name.toLowerCase()
                    }
                });

                if ((existingItems != null && existingItems.length > 1)) {
                    $scope.vm.isItemExist = true;
                    $scope.vm.productsSolds[index].isError = true;
                    return true;

                }
                else if (vm.productsSolds[0].name == '') {
                    return false;
                }
                else {
                    $scope.vm.isItemExist = false;
                    $scope.vm.productsSolds[index].isError = false;
                    return false;
                    //if (vm.productsSolds[0].Id == 0 && vm.productsSolds[0].name != null && vm.productsSolds[0].name != "") {
                    //    ProcedureLevelService.isProductTypeUnique(vm.productsSolds[0].name)
                    //   .then(function (res) {
                    //       if (res.data == true) {
                    //           return true;
                    //       } else {
                    //           return false;
                    //       }
                    //   })
                    //   .catch(function (err) {
                    //       console.log(err);
                    //   });

                    //}
                }
            }

            $scope.isDupplicateProductsCompanies = function (index) {

                var existingItems = vm.productCompanies.filter(function (item) {
                    if (item.name != null && vm.productCompanies[index].name != null) {
                        return item.name.toLowerCase() == vm.productCompanies[index].name.toLowerCase()
                    }
                });

                if (existingItems != null && existingItems.length > 1) {
                    $scope.vm.isItemExist = true;
                    $scope.vm.productCompanies[index].isError = true;
                    return true;

                } else if (vm.productCompanies[0].name == '') {
                    return false;
                }
                else {
                    $scope.vm.isItemExist = false;
                    $scope.vm.productCompanies[index].isError = false;
                    return false;
                    //if (vm.productCompanies[0].Id == 0 && vm.productCompanies[0].name != null && vm.productCompanies[0].name != "") {
                    //    ProcedureLevelService.isCompanyUnique(vm.productCompanies[0].name)
                    //   .then(function (res) {
                    //       if (res.data == true) {
                    //          // $scope.vm.isItemExist = true;
                    //           return true;

                    //       } else {
                    //          // $scope.vm.isItemExist = false;
                    //           return false;
                    //       }
                    //   })
                    //   .catch(function (err) {
                    //       console.log(err);

                    //   });

                    //} else {
                    //    $scope.vm.isItemExist = false;
                    //}
                }
            };

            $scope.companyChange = function () {
                $scope.vm.isItemExist = false;
                $scope.companySubmitted = false;
            };

            $scope.productChange = function () {
                $scope.productSubmitted = false;
            };

            $scope.breastImplantChange = function () {
                $scope.breastSubmitted = false;
            };

            $scope.levelChange = function () {
                $scope.levelSubmitted = false;
            };

        });
})();
