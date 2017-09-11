(function () {
    'use strict';
    angular.module('annPortalApp')
     .config(function (stateHelperProvider) {
         stateHelperProvider
          .state({
              name: 'admin.practice-management',
              url: '/practice-management',
              templateUrl: 'app/admin/practice-management/practice-management.html',
              controller: 'Admin.PracticeManagementCtrl',
              abstract: true,
              children: [{
                  name: 'index',
                  url: '',
                  templateUrl: 'app/admin/practice-management/practice-management.index.html',
                  controller: 'Admin.PracticeManagement.IndexCtrl'
              }, {
                  name: 'edit',
                  url: '/:practiceId/edit',
                  templateUrl: 'app/admin/practice-management/practice-management.edit.html',
                  controller: 'Admin.PracticeManagement.EditCtrl'
              }, {
                  name: 'procedure-mapping',
                  url: '/:emrId/emr-mapping',
                  templateUrl: 'app/admin/practice-management/practice-management.emr-mapping.html',
                  controller: 'Admin.PracticeManagement.EmrMappingCtrl'
              }, ]
          });
     })
     .controller('Admin.PracticeManagementCtrl', function ($scope, toaster, $rootScope, PracticeService) {
         var vm = $scope.vm = {};
         $rootScope.$on('$stateChangeSuccess', function () {
             setSubtitle();
         });
         setSubtitle();

         function setSubtitle() {
             vm.showEmrId = false;
             if (window.location.href.indexOf('/new') > 0) {
                 vm.subTitle = 'New Practice';
                 vm.showCreateBtn = false;
                 vm.showCreateMappingBtn = false;
             } else if (window.location.href.indexOf('/edit') > 0) {
                 vm.subTitle = 'Edit Practice';
                 vm.showCreateBtn = false;
                 vm.showCreateMappingBtn = false;
             } else if (window.location.href.indexOf('/emr-mapping') > 0) {
                 vm.subTitle = 'EMR Mappings';
                 vm.showEmrId = true;
                 vm.showCreateBtn = false;
                 vm.showCreateMappingBtn = false;
             } else {
                 vm.subTitle = undefined;
                 vm.showCreateBtn = true;
                 vm.showCreateMappingBtn = false;
             }
         }

         $scope.addMapping = function () {
             angular.element(document.getElementById('emr-mapping')).scope().addMapping();
         }
     })
     .controller('Admin.PracticeManagement.IndexCtrl', function ($scope, $q, $window, $filter, $interval, $location, toaster, PracticeService) {
         $scope.showModal = false;
         $scope.showPopupOk = false;
         $scope.isCancelAllowed = true;
         $scope.hasData = true;
         // var vm = $scope.vm = {};
         $scope.showCreateBtn = true;
         // alert(USER_ROLE)
         $scope.createNewPractice = function () {
             $scope.selectedPractice = {};
         };

         $scope.searchOption = 'practiceId';
         $scope.searchText = '';
         // load practice list      
         $scope.practiceDataSource = {
             transport: {
                 read: {
                     url: '/Portal/api/Practice/Practices',
                 }
             }
         };

         $scope.cubeProcessQueue = [];
         $scope.cubeProcessDisplayQueue = [];
         $scope.isItemsSelected = ($scope.cubeProcessQueue.length > 0);
         $scope.isCubeProcessing = false;

         $scope.dataDeleteOption = false;
         $scope.processCubeOption = "0";
         $scope.practiceStatus = false;
         $scope.showCubeOptions = false;
         $scope.popupType = 0;

         $scope.init = function () {
             $scope.getJobRunningStatus();
             $interval(function () {
                 if ($scope.isCubeProcessing) {
                     $scope.getJobRunningStatus();
                 }
             }, 60000);

             PracticeService.getPractices().success(function (res) {
                 $scope.allPracticeList = res;
             });

         }

         $scope.getJobRunningStatus = function () {
             // get job running status
             $scope.cubeProcessQueue = [];
             PracticeService.getJobRunningStatus().success(function (res) {
                 _.each(res, function (obj) {
                     $scope.isCancelAllowed = (obj.canCancel == 0);
                     $scope.cubeProcessQueue.push(obj.emrId);
                 });
                 $scope.isCubeProcessing = res.length > 0;
                 $scope.cancelButtonStatus();
             });
         }

         $scope.goToUrl = function (link) {
             $location.url(link);
         }

         // view practice
         $scope.viewPractice = function (practiceId, emrId) {
             PracticeService.practiceHasDataCheck(emrId).success(function (res) {
                 if (res) {
                     // update cookie
                     PracticeService.updateDashboradIdForPreview(practiceId)
                         .success(function (res) {
                             // let w = window.outerWidth;
                             // let h = window.outerHeight;
                             // 'Preview Dashboard', 'width=' + w + ',height=' + h
                             window.open('/Portal/#/dashboard/ANN%20Monitor', '_blank');
                         })
                         .error(function (error, status) {
                             $("#confirmPopup").removeClass('showPopup');
                             if (status == -1) {
                                 toaster.error({
                                     title: "",
                                     body: "Cannot perform the function due to network failure."
                                 });
                             } else {
                                 toaster.error({
                                     title: "",
                                     body: "Cannot perform the function Error occured."
                                 });
                             }
                         });
                 }
                 else {
                     let text = "No data available for the selected practice";
                     $scope.showPopupOk = true;
                     showPopup(2, text);
                 }
             });
         }

         $scope.onSearchInputKeyPress = function (event) {
             if (event.charCode == 13) {
                 $scope.search();
             }
         }

         $scope.search = function () {
             $scope.hasData = true;
             let filterText = "NA";
             if (!$scope.searchOption || !$scope.searchText) {
                 $scope.searchOption = "practiceId";
                 $scope.searchText = "";
             }

             filterText = ($scope.searchText != "") ? $scope.searchText.replace(/\//gi, "_!").replace(/:/gi, "_~") : "NA";

             $scope.listView.dataSource.options.transport.read.url = '/Portal/api/Practice/Practices/' + $scope.searchOption + '/' + encodeURIComponent(filterText.trim()) + '/';

             var promise = $scope.listView.dataSource.read();
             $q.all([promise]).then(function (res) {
                 $scope.hasData = ($scope.listView.dataSource.view().length > 0);
             });
         }

         $scope.clearSearch = function () {
             $scope.searchOption = "";
             $scope.searchText = "";
             $scope.listView.dataSource.options.transport.read.url = '/Portal/api/Practice/Practices/';
             var promise = $scope.listView.dataSource.read();
             $q.all([promise]).then(function (res) {
                 $scope.hasData = ($scope.listView.dataSource.view().length > 0);
             });
             $scope.searchOption = 'practiceId';
         }

         // hasmappedprodcut with date range  and  emr mapping ?

         var showPopup = function (type, text) {
             $scope.tempPopupType = type;
             $("#popText").html(text);
             $("#confirmPopup").addClass('showPopup');
         }

         $scope.addToRefreshProcess = function (emrId) {
             $scope.popupType = 0;
             // validate changes
             PracticeService.validateCubeRefreshByPractice(emrId).then(function (res) {
                 // NEW CODE
                 if (res.data) {
                     // has changes to process
                     // add to refresh queue
                     $scope.cubeProcessQueue.push(emrId);
                     $scope.cubeProcessDisplayQueue.push({
                         emrId: emrId, name: $scope.getEmrNameById(emrId)
                     });
                     $scope.isItemsSelected = ($scope.cubeProcessQueue.length > 0);
                 }
                 else {
                     // doesn't have changes to process
                     // show validation popup
                     let text = 'There are no changes in order to refresh this cube';
                     $scope.showPopupOk = true;
                     showPopup(2, text);
                 }

                 /// OLD CODE - DO NOT REMOVE
                 // confirm popup type 2
                 //$scope.tempPracticeId = practiceId;
                 //let text = 'Are you sure you want to set this cube to refresh?';
                 //showPopup(2, text);
                 /// ------------------------
             });
         }

         $scope.processAllCubes = function () {
             // confirm popup type 4
             $scope.popupType = 0;
             $scope.cubeProcessDisplayQueue = [];

             // get valid practice list
             var emrList = _.map($scope.allPracticeList, 'emrId');

             PracticeService.validatePracticeList(emrList).then(function (res) {
                 console.log(res);
                 if (res.data.length > 0) {
                     // has changes to process
                     // add to refresh queue
                     $scope.cubeProcessQueue = res.data;
                     $scope.isItemsSelected = ($scope.cubeProcessQueue.length > 0);
                 }

                 if ($scope.isItemsSelected) {
                     let text = 'Are you sure you want to refresh all cubes?';
                     $scope.showPopupOk = false;
                     showPopup(4, text);
                 }
                 else {
                     $scope.cubeProcessQueue = [];
                     let text = 'There are no changes in order to refresh all cubes';
                     $scope.showPopupOk = true;
                     showPopup(2, text);
                 }
             });
         }

         $scope.startRefreshProcess = function () {
             $scope.popupType = 0;
             // confirm popup type 3
             let text = 'Data will be refreshed for following practices. Do you want to continue?';
             showPopup(3, text);
         }

         $scope.cancelRefreshProcess = function () {
             // $("[id*='refresh-btn-']").removeAttr('disabled');
             if ($scope.isCubeProcessing) {
                 PracticeService.cancelJob().success(function (res) {
                     if (res) {
                         $scope.cubeProcessQueue = [];
                         $scope.cubeProcessDisplayQueue = [];
                         $scope.isItemsSelected = false;
                         $scope.isCubeProcessing = false;
                         $scope.isCancelAllowed = true;

                         toaster.success({
                             title: "",
                             body: "Cube Refresh Cancelled Successfully"
                         });
                     }
                     else {
                         toaster.error({
                             title: "",
                             body: "Error cancelling "
                         });
                     }
                     $scope.getJobRunningStatus();
                 });
             }
             else {
                 $scope.cubeProcessQueue = [];
                 $scope.cubeProcessDisplayQueue = [];
                 $scope.isItemsSelected = false;
             }
         }

         $scope.getEmrNameById = function (emrId) {
             if (_.filter($scope.allPracticeList, { emrId: emrId })[0]) {
                 return _.filter($scope.allPracticeList, { emrId: emrId })[0].name;
             }
             else {
                 return "";
             }
         }

         $scope.deactiveate = function (practiceId, status, emrId) {
             // confirm popup type 1
             // status = false if deactivate, true if activate
             $scope.practiceStatus = !status;
             $scope.tempPracticeId = practiceId;
             $scope.tempEmrId = emrId;
             $scope.processCubeOption = "0";
             if (status) {
                 // Reactivate practice chcek
                 //PracticeService.practiceReActivatedCheck(emrId).success(function (res) {
                 //    $scope.reActive = res;
                 //    let statusCheck = status ? "Activate" : "Deactivate";
                 //    let text = 'Are you sure you want to <strong>' + statusCheck + '</strong> this practice?';
                 //    $scope.popupType = 1;
                 //    showPopup(1, text);
                 //});

                 let statusCheck = status ? "Activate" : "Deactivate";
                 if (statusCheck == "Activate") {
                     PracticeService.isPracticeContainImplants(practiceId).success(function (data) {
                         let text = '';
                         if (data == false) {
                             text = 'Are you sure you want to <strong>' + statusCheck + '</strong> this practice?' + '<br/>There are no Breast Implant Information attached. This will be added to unspecified category';
                         }
                         else {
                             text = 'Are you sure you want to <strong>' + statusCheck + '</strong> this practice?';
                         }
                         $scope.popupType = 1;
                         showPopup(1, text);
                     });
                 }
                 else {
                     let text = 'Are you sure you want to <strong>' + statusCheck + '</strong> this practice?';
                     $scope.popupType = 1;
                     showPopup(1, text);
                 }
             }
             else {
                 let statusCheck = status ? "Activate" : "Deactivate";
                 let text = 'Are you sure you want to <strong>' + statusCheck + '</strong> this practice?';
                 $scope.popupType = 1;
                 showPopup(1, text);
             }
         }

         $scope.updateDataDeleteStatus = function (option) {
             $scope.dataDeleteOption = option;
         }

         $scope.updateCubeRefreshStatus = function (option) {
             $scope.processCubeOption = (option == "1") ? true : false;
         }

         $scope.confirmYes = function () {
             switch ($scope.tempPopupType) {
                 case 1:
                     // activate / deactivate function
                     let practice = {
                         id: $scope.tempPracticeId,
                         deleteData: $scope.dataDeleteOption,
                         refreshCube: $scope.processCubeOption,
                     };
                     PracticeService.deactivatePractice(practice).success(function (data) {
                         console.log(":::DEACTIVATE PRACTICE :::");

                         // cube process now
                         if ($scope.processCubeOption == "1") {
                             $scope.cubeProcessQueue.push($scope.tempEmrId);
                             $scope.processCube(0);
                         }

                         $("#confirmPopup").removeClass('showPopup');
                         if (data) {
                             toaster.success({
                                 title: "",
                                 body: "Practice Activated successfully."
                             });
                         } else {
                             toaster.success({
                                 title: "",
                                 body: "Practice Deactivated successfully."
                             });
                         }

                         $scope.dataDeleteOption = false;
                         $scope.processCubeOption = false;
                         $scope.practiceStatus = false;
                         $scope.showCubeOptions = false;

                         $("#listView").data("kendoListView").dataSource.read();
                     }).error(function (error, status) {
                         $("#confirmPopup").removeClass('showPopup');
                         if (status == -1) {
                             toaster.error({
                                 title: "",
                                 body: "Cannot perform the function due to network failure."
                             });
                         } else {
                             toaster.error({
                                 title: "",
                                 body: "Cannot perform the function Error occured."
                             });
                         }
                     });
                     break;
                 case 2:
                     // add to refresh queue
                     // $scope.cubeProcessQueue.push($scope.tempPracticeId);

                     // disable btn
                     // $("#refresh-btn-" + $scope.tempPracticeId).attr('disabled', true);
                     // $scope.isItemsSelected = ($scope.cubeProcessQueue.length > 0);

                     $("#confirmPopup").removeClass('showPopup');
                     break;
                 case 3:
                     // start to process queue
                     $("#confirmPopup").removeClass('showPopup');

                     // start processing
                     $scope.isCubeProcessing = true;
                     // Pass data to backend
                     // processCube
                     $scope.processCube(0);
                     break;
                 case 4:
                     // start to process queue
                     $("#confirmPopup").removeClass('showPopup');

                     // start processing
                     $scope.isCubeProcessing = true;
                     $scope.processCube(1);
                     break;
                 case 5:
                     break;
                 default:
                     break;
             }
         }

         $scope.processCube = function (type) {
             PracticeService.processCube($scope.cubeProcessQueue, type).success(function (data) {
                 $scope.getJobRunningStatus();
             });
         }

         $scope.checkProcessStatus = function (practiceId) {
             return !(_.indexOf($scope.cubeProcessQueue, practiceId) == -1);
         }

         $scope.isSelected = function (practiceId) {
             return !(_.indexOf($scope.cubeProcessQueue, practiceId) == -1);
         }

         $scope.confirmOK = function () {
             $scope.tempPracticeId = undefined;
             $scope.tempEmrId = undefined;
             $scope.showPopupOk = false;
             $("#confirmPopup").removeClass('showPopup');
         }

         $scope.confirmNo = function () {
             $scope.tempPopupType = undefined;
             $scope.tempPracticeId = undefined;
             $scope.tempEmrId = undefined;

             $scope.dataDeleteOption = false;
             $scope.processCubeOption = "0";

             $scope.cubeProcessDisplayQueue = [];
             $scope.cubeProcessQueue = [];
             $scope.isItemsSelected = false;
             $("#confirmPopup").removeClass('showPopup');
         }

         $scope.cancelButtonStatus = function () {
             if ($scope.isCubeProcessing) {
                 return $scope.isCancelAllowed;
             }
             else {
                 return !($scope.isItemsSelected);
             }
         }
     })
     .controller('Admin.PracticeManagement.EditCtrl', function ($scope, $timeout, $stateParams, toaster, PracticeService, $filter, $location, $q) {
         var vm = $scope.vm = {};

         //start confirmation when form dirty
         $scope.cubeProcessQueue = [];
         $scope.isCubeProcessing = false;
         $scope.showOkModal = false;
         $scope.showModal = false;
         $scope.confirmResponded = false;
         $scope.leavePage = false;
         $scope.nextLocation = "#";
         $scope.isSave = false;
         $scope.isSaveClicked = false;
         $scope.isAddClicked = false;

         $scope.$watch('confirmResponded', function () {
             if (!$scope.isSave) {
                 if ($scope.confirmResponded) {
                     if ($scope.leavePage) {
                         $timeout(function () {
                             $location.url($scope.nextLocation.split('#')[1]);
                         }, 500);
                     } else {
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
             } else {
                 $scope.showModal = false;
             }
         });

         $scope.confirmNo = function () {
             $scope.leavePage = false;
             $scope.showModal = false;
             $scope.showOkModal = false;
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

             vm.practiceUser = {
                 contactNumber: {
                     required: true,
                     minlength: 10
                 }
             };
             vm.isValidDate = false;
             vm.isValidContactNumber = false;
             vm.isProdcutEmpty = false;
             vm.isBreastImplantDateRangeSelected = false;
             vm.isValidProdcutWithRange = false;
             vm.isPracticeNameUnique = false;
             vm.isUserAlreadyExsist = false;
             vm.isUserASAPUser = false;
             //state list
             $scope.stateDataSource = {
                 transport: {
                     read: {
                         url: 'api/Practice/States',
                     }
                 }
             };

             //zip codes
             $scope.zipCodeDatasource = {
                 type: "webapi",
                 serverFiltering: true,
                 transport: {
                     read: {
                         url: function () {
                             return "api/Practice/ZipCodeFilterByText";
                         },
                     },
                     parameterMap: function (data, action) {
                         if (Object.keys(data).length != 0 && data.filter.filters[0] != null) {
                             var newParams = {
                                 filter: data.filter.filters[0].value
                             };
                             return newParams;
                         }
                     }

                 }
             }

             $scope.productListDataSource = {
                 transport: {
                     read: {
                         url: 'api/Practice/GetProductList',
                     }
                 }
             };

             //userSearchDatasource
             $scope.userSearchDatasource = {
                 type: "webapi",
                 serverFiltering: true,
                 transport: {
                     read: {
                         url: function () {
                             return "api/Practice/GetPracticeUsers";
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

             if ($stateParams.practiceId === 'new') {

                 PracticeService.getNextEmrId().success(function (data) {
                     console.log(":::GET NEXT PRACTICE EMRID :::");
                     vm.practice = {
                         emrId: data
                     };

                     vm.practice.practiceUserList = vm.practice.practiceUserList || [];
                     vm.practice.brestImplants = vm.practice.brestImplants || [];
                     vm.dateRange = vm.dateRange || [];

                 }).error(function (error, status) {
                     vm.practice = vm.practice;
                     if (status == -1) {
                         toaster.error({
                             title: "",
                             body: "Cannot perform the function due to network failure."
                         });
                     } else {
                         toaster.error({
                             title: "",
                             body: "Cannot perform the function Error occured."
                         });
                     }
                 });

                 vm.breastImplant = {
                     fromDate: null,
                     toDate: null
                 };

             } else {

                 vm.dateRange = vm.dateRange || [];

                 PracticeService.getPractice($stateParams.practiceId)
                  .then(function (res) {
                      vm.practice = res.data;
                      vm.practice.practiceUserList = res.data.practiceUserList;
                      vm.practice.brestImplants = res.data.brestImplants;

                      if (vm.practice.brestImplants != []) {
                          angular.forEach(vm.practice.brestImplants, function (data) {

                              var range = {
                                  start: new Date(data.fromDate),
                                  end: new Date(data.toDate)
                              };
                              vm.dateRange.push(range);
                          });
                      }
                  })
                  .catch(function () {

                  });
             }

             $scope.getJobRunningStatus();
         }

         //add Practice Users to Practice
         $scope.addPracticeUser = function (practiceUser) {
             if (practiceUser.userName != undefined) {
                 vm.practice.practiceUserList = vm.practice.practiceUserList || [];
                 var promise = $scope.chkUserAlreadyAssignToPractice(practiceUser);
                 $q.all([promise]).then(function (d) {
                     if (!vm.isUserAlreadyExsist && !($scope.containsObject(practiceUser.userName, vm.practice.practiceUserList))) {
                         vm.practice.practiceUserList.unshift(practiceUser);
                     } else {
                         var selectedItem = $scope.getItem(practiceUser.userName, vm.practice.practiceUserList);
                         if (selectedItem && selectedItem.RecordStatusId === 2) {
                             selectedItem.RecordStatusId = 1;
                             vm.isUserAlreadyExsist = false;
                         }
                     }
                 });
             }
         };

         Array.prototype.pushIfNotExist = function (element, comparer) {
             if (!this.inArray(comparer)) {
                 this.push(element);
             }
         };

         $scope.getItem = function (key, list) {
             return list.find(x => x.userName === key);
         }

         //remove Practice User from the Practice
         $scope.removePracticeUser = function (index) {
             if (index > -1) {
                 vm.practice.practiceUserList[index].RecordStatusId = 2;
             }
         }

         function findWithAttr(array, attr, value) {
             for (var i = 0; i < array.length; i += 1) {
                 // date validation
                 let recordStatusId = array[i]["RecordStatusId"] || array[i]["recordStatusId"];
                 let attrVal = array[i][attr];
                 let d1 = new Date(attrVal).setHours(12, 0, 0, 0);
                 let d2 = new Date(value[attr]).setHours(12, 0, 0, 0);

                 // array[i].brestImplant.name === value.brestImplant.name &&
                 if (d1 === d2 && recordStatusId != 2) {
                     return i;
                 }
             }
             return -1;
         }

         //add BreastImplant Prodcut to Practice
         $scope.addBreastImplant = function (breastImplant) {

             breastImplant.recordStatusId = 1;
             if (!$scope.isValidBreastInPlant(breastImplant)) {
                 vm.isProdcutEmpty = false;
                 vm.dateRange = vm.dateRange || [];
                 //var range = {
                 //    start: new Date(breastImplant.fromDate),
                 //    end: breastImplant.toDate != null ? new Date(breastImplant.toDate) : null
                 //};
                 //console.log(breastImplant.brestImplant.name);
                 //console.log(vm.practice.brestImplants[0].brestImplant.name);

                 vm.practice.brestImplants = vm.practice.brestImplants || [];
                 //if (vm.practice.brestImplants.length == 0) {
                 //    vm.practice.brestImplants.unshift(breastImplant);
                 //    vm.isValidDate = false;
                 //    vm.breastImplant = { fromDate: null, toDate: null };
                 //} else {
                 //    var previousDate = new Date(vm.practice.brestImplants[0].fromDate);
                 //    var selectedDate = new Date(breastImplant.fromDate);
                 //    var isPreviousDateInvalid = false;
                 //    if (vm.practice.brestImplants[0].RecordStatusId == 2 && previousDate > selectedDate) {
                 //        isPreviousDateInvalid = true;
                 //    }

                 //    if (findWithAttr(vm.practice.brestImplants, 'fromDate', breastImplant) == -1) {
                 //        vm.isValidProdcutWithRange = false;
                 //        vm.practice.brestImplants = vm.practice.brestImplants || [];
                 //        vm.practice.brestImplants.unshift(breastImplant);
                 //        vm.isValidDate = false;
                 //        vm.breastImplant = { fromDate: null, toDate: null };
                 //    } else {
                 //        vm.isValidProdcutWithRange = true;
                 //        // vm.dateRange.splice(vm.dateRange.length - 1 , 1);
                 //    }
                 //}

                 if (findWithAttr(vm.practice.brestImplants, 'fromDate', breastImplant) == -1) {
                     vm.isValidProdcutWithRange = false;
                     vm.practice.brestImplants = vm.practice.brestImplants || [];
                     vm.practice.brestImplants.unshift(breastImplant);
                     vm.isValidDate = false;
                     vm.breastImplant = { fromDate: null, toDate: null };
                 } else {
                     vm.isValidProdcutWithRange = true;
                     // vm.dateRange.splice(vm.dateRange.length - 1 , 1);
                 }
             }
             else {
                 vm.isProdcutEmpty = false;
             }
         };

         //remove BreastImplant Prodcut from Practice
         $scope.removeBreastImplant = function (index) {
             if (index > -1) {
                 vm.practice.brestImplants[index].RecordStatusId = 2;
                 // vm.practice.BrestImplants.splice(index, 1);
             }
         }

         $scope.createNewPractice = function () {
             vm.selectedPractice = {
             };
         };

         $scope.save = function (form) {

             if (form.$valid && !vm.isPracticeNameUnique && !vm.isValidContactNumber && !$scope.isEmpty(vm.practice.state) && !$scope.isEmpty(vm.practice.zipCode)) {
                 if ($stateParams.practiceId === 'new') {
                     PracticeService.createPractice(vm.practice).success(function (data) {
                         console.log(":::SAVE PRACTICE :::");
                         if (data) {
                             // update Identity DB
                             var userIdList = [];
                             for (var i = 0; i < vm.practice.practiceUserList.length; i++) {
                                 var tempUser = vm.practice.practiceUserList[i];
                                 userIdList.push(tempUser.rrUserId);
                             }
                             // console.log(userIdList);
                             console.log(data);
                             var postData = JSON.stringify({ userIdList: userIdList, practiceName: data.emrId, practiceId: data.id });
                             PracticeService.updateIdentityUsers(postData).success(function (resData) {
                                 toaster.success({
                                     title: "",
                                     body: "New Practice added successfully."
                                 });
                                 $scope.confirmResponded = true;
                                 $scope.leavePage = true;
                                 $scope.isSave = true;

                                 $location.path('/admin/practice-management');
                             });
                         }
                     }).error(function (error, status) {
                         if (status == -1) {
                             toaster.error({
                                 title: "",
                                 body: "Cannot perform the function due to network failure."
                             });
                         } else {
                             toaster.error({
                                 title: "",
                                 body: "Cannot perform the function Error occured."
                             });
                         }
                     });
                 } else {
                     PracticeService.updatePractice(vm.practice).success(function (data) {

                         console.log(":::UPDATE PRACTICE :::");
                         vm.practice = data;
                         vm.practice.practiceUserList = data.practiceUserList;
                         vm.practice.brestImplants = data.brestImplants;

                         // update Identity DB
                         var userIdList = [];
                         for (var i = 0; i < vm.practice.practiceUserList.length; i++) {
                             var tempUser = vm.practice.practiceUserList[i];
                             userIdList.push(tempUser.rrUserId);
                         }
                         // console.log(userIdList);
                         var postData = JSON.stringify({ userIdList: userIdList, practiceName: data.emrId, practiceId: data.id });
                         PracticeService.updateIdentityUsers(postData).success(function (resData) {
                             toaster.success({
                                 title: "",
                                 body: "Practice information updated."
                             });
                             $scope.confirmResponded = true;
                             $scope.leavePage = true;
                             $scope.isSave = true;

                             $location.path('/admin/practice-management');
                         });

                     }).error(function (error, status) {
                         vm.practice = vm.practice;
                         if (status == -1) {
                             toaster.error({
                                 title: "",
                                 body: "Cannot perform the function due to network failure."
                             });
                         } else {
                             toaster.error({
                                 title: "",
                                 body: "Cannot perform the function Error occured."
                             });
                         }
                     });
                 }
             }
         };

         $scope.cancel = function () {
             $location.path('/admin/practice-management');
             if ($scope.leavePage) {
                 vm.practice = {};
             }

         };

         $scope.isEmpty = function (obj) {
             for (var i in obj) {
                 if (obj.hasOwnProperty(i))
                     return false;
             }
             return true;
         };

         $scope.isValidFromAndToDate = function (fromDate, toDate) {

             vm.isValidProdcutWithRange = false;
             if ((Date.parse(toDate) - Date.parse(fromDate)) < 0) {
                 vm.isValidDate = true;

             } else {
                 vm.isValidDate = false;
             }
         };

         $scope.isValidContactNo = function (number) {
             if (number != null && (number.indexOf("_") + 1) != 0) {
                 vm.isValidContactNumber = true;
             } else {
                 vm.isValidContactNumber = false;
             }
         };

         $scope.isValidBreastInPlant = function (breastImplant) {
             if ($scope.isValidProdcut(breastImplant.brestImplant) || breastImplant.fromDate == null) {
                 vm.isBreastImplantDateRangeSelected = true;
                 return true;
             } else {
                 vm.isBreastImplantDateRangeSelected = false;
                 return false;
             }
         }

         $scope.isValidProdcut = function (breastImplant) {
             if ($scope.isEmpty(breastImplant)) {
                 vm.isProdcutEmpty = true;
                 //alert(true);
             } else {
                 vm.isProdcutEmpty = false;
                 //alert(false);
             }

         };

         $scope.chkUserAlreadyAssignToPractice = function (practiceUser) {
             var deferred = $q.defer();

             //PracticeService.isUserAlreadyAdded(practiceUser).success(function (data) {
             //    vm.isUserAlreadyExsist = data;
             //    deferred.resolve(data);
             //});
             vm.isUserAlreadyExsist = false;
             vm.isUserASAPUser = false;
             PracticeService.isUserAlreadyAssigned(practiceUser).success(function (data) {
                 if (data != null) {
                     vm.isUserAlreadyExsist = data.isUserAlreadyExsistInPractice;
                     vm.isUserASAPUser = data.isUserASAPUser;
                 }

                 deferred.resolve(data);
             });
             return deferred.promise;
         }

         $scope.chkPracticeNameUnique = function (practiceName) {
             PracticeService.isPracticeNameUnique(practiceName).success(function (data) {
                 vm.isPracticeNameUnique = data;
             });
         };

         $scope.containsObject = function (userName, list) {
             return list.some(function (el) {
                 return vm.isUserAlreadyExsist = (el.userName === userName);
             });
         }

         function isOverlap(dateRanges) {
             var sortedRanges = dateRanges.sort((previous, current) => {

                 // get the start date from previous and current
                 var previousTime = previous.start.getTime();
                 var currentTime = current.start.getTime();
                 // if the previous is earlier than the current
                 if (previousTime < currentTime) {
                     return -1;
                 }
                 // if the previous time is the same as the current time
                 if (previousTime === currentTime) {
                     return 0;
                 }
                 // if the previous time is later than the current time
                 return 1;
             });

             var result = sortedRanges.reduce((result, current, idx, arr) => {
                 // get the previous range
                 if (idx === 0) {
                     return result;
                 }
                 var previous = arr[idx - 1];
                 // check for any overlap
                 var previousEnd = previous.end.getTime();
                 var currentStart = current.start.getTime();
                 var overlap = (previousEnd >= currentStart);
                 // store the result
                 if (overlap) {
                     // yes, there is overlap
                     result.overlap = true;
                     result.ovelapIndx = idx;

                     // store the specific ranges that overlap
                     result.ranges.push({
                         previous: previous,
                         current: current
                     })
                 }
                 return result;
                 // seed the reduce  
             }, {
                 overlap: false,
                 ranges: []
             });

             // return the final results  
             return result;
         }

         $scope.toggleModal = function (isCreate) {
             if (isCreate) {
                 $scope.modalTitle = "Create New Practice User";
                 $scope.errorCount = 0;
                 $scope.disableSave = false;
                 $scope.isEditMode = false;
                 $scope.submitted = false;
                 $scope.Roles = null;
                 $scope.User = {
                     Id: null,
                     UserId: null,
                     FirstName: null,
                     LastName: null,
                     OrganizationId: 0,
                     UserDesignation: null,
                     Email: null,
                     DesignationId: 0,
                     SecondaryHubIds: [],
                     Roles: [],
                     RoleId: '60f463da-0e90-4092-bb04-9d01ec763643'
                 };

                 //if ($scope.isUserHubAdmin == 'True' && $scope.isUserAdmin == 'False') {
                 //    $scope.disableHubs = true;
                 //    $scope.User.OrganizationId = $scope.hubId;
                 //}
             }
             else {
                 $scope.modalTitle = "Edit User";
             }
             $scope.showUserModal = !$scope.showUserModal;
             //if (!$scope.showUserModal) {
             //    $scope.init();
             //}
         };

         function validate(user) {
             if (user.FirstName == null || user.FirstName == "") {
                 $scope.User.FirstNameIsEmpty = true;
                 $scope.errorCount++;
             }
             else {
                 $scope.User.FirstNameIsEmpty = false;
             }

             if (user.LastName == null || user.LastName == "") {
                 $scope.User.LastNameIsEmpty = true;
                 $scope.errorCount++;
             }
             else {
                 $scope.User.LastNameIsEmpty = false;
             }

             if (user.Email == null || user.Email == "") {
                 $scope.User.EmailIsEmpty = true;
                 $scope.errorCount++;
             }
             else {
                 $scope.User.EmailIsEmpty = false;
             }

             if (!user.EmailIsEmpty) {
                 var atpos = user.Email.indexOf("@");
                 var dotpos = user.Email.lastIndexOf(".");
                 if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= user.Email.length) {
                     $scope.User.EmailIsInvalid = true;
                     $scope.errorCount++;
                 }
                 else {
                     $scope.User.EmailIsInvalid = false;
                 }
             }
         };

         function showError() {
             $scope.showError = true;
             $timeout(function () {
                 $scope.showError = false;
             }, 5000);
         }

         function showSuccess() {
             $scope.showSuccess = true;
             $timeout(function () {
                 $scope.showSuccess = false;
                 if ($scope.showModal) {
                     $scope.toggleModal();
                 }
             }, 5000);
         }

         $scope.createUser = function () {
             $scope.errorCount = 0;
             $scope.submitted = true;
             $scope.disableSave = true;

             // Practice User Role
             PracticeService.getRoles()
                .success(function (result) {
                    $scope.Roles = result;

                    // proceed to save user
                    $scope.User.Roles = [];
                    for (let role in $scope.Roles) {
                        //  && ($scope.Roles[role].Name == 'Practice User' || $scope.Roles[role].Name == 'ASAPs User')
                        if ($scope.Roles[role].Name == 'Practice User') {
                            $scope.User.Roles.push($scope.Roles[role].Id);
                            $scope.User.RoleId = $scope.Roles[role].Id;
                        }
                    }

                    validate($scope.User);

                    if ($scope.errorCount == 0) {
                        PracticeService.postUser($scope.User)
                        .success(function (result) {
                            $scope.User.UsernameExists = false;
                            $scope.User.EmailExists = false;
                            $scope.practiceUserDropdown.dataSource.read();
                            $scope.toggleModal(false);
                            toaster.success({
                                title: "",
                                body: "User created successfully"
                            });
                        })
                        .error(function (message) {
                            if (message.Message == "Entered user id exists within the system") {
                                $scope.User.UsernameExists = true;
                                $scope.userErrorText = message.Message;
                            }
                            else if (message.Message == "Entered email exists within the system") {
                                $scope.User.UsernameExists = false;
                                $scope.User.EmailExists = true;
                                $scope.emailErrorText = message.Message;
                            }
                            else if (message.ModelState['user.Email'] != undefined) {
                                $scope.User.UsernameExists = false;
                                $scope.User.EmailIsInvalid = true;
                            }
                            else if (message.ModelState[""][0] = "User name bl a is invalid, can only contain letters or digits.") {
                                $scope.User.UsernameExists = true;
                                $scope.userErrorText = "Please enter a valid user ID";
                            }
                            else {
                                $scope.User.EmailExists = false;
                            }
                            $scope.disableSave = false;
                            showError();
                        });
                    }
                    else {
                        $scope.disableSave = false;
                        showError();
                    }
                });
         };

         $scope.DeactivateUser = function (userId) {
             $scope.User = { UserId: userId };
             PracticeService.getUser($scope.User)
             .success(function (result) {
                 $scope.User = result;
             });
             PracticeService.deactivateUser($scope.User)
             .success(function (result) {
                 toaster.success({ title: "", body: "User deactivate successful" });
             }).error(function (error, status) {
                 toaster.error({ title: "", body: "Cannot perform the function. Error occured." });
             });
         }

         $scope.ActivateUser = function (userId) {
             $scope.User = { UserId: userId };
             PracticeService.getUser($scope.User)
             .success(function (result) {
                 $scope.User = result;
             });
             PracticeService.activateUser($scope.User)
             .success(function (result) {
                 toaster.success({ title: "", body: "User activate successful" });
             }).error(function (error, status) {
                 toaster.error({ title: "", body: "Cannot perform the function. Error occured." });
             });
         }

         $scope.getJobRunningStatus = function () {
             // get job running status
             $scope.cubeProcessQueue = [];
             PracticeService.getJobRunningStatus().success(function (res) {
                 _.each(res, function (obj) {
                     $scope.cubeProcessQueue.push(obj.emrId);
                 });
                 $scope.isCubeProcessing = res.length > 0;

                 $scope.showOkModal = $scope.isCubeProcessing;
                 // showOkModal
             });
         }
     })
     .controller('Admin.PracticeManagement.EmrMappingCtrl', function ($scope, $rootScope, $q, $uibModal, $timeout, $location, toaster, $stateParams, PracticeService, ProcedureLevelService) {
         var vm = $scope.vm = {};

         //start confirmation when form dirty
         $scope.cubeProcessQueue = [];
         $scope.isCubeProcessing = false;
         $scope.showOkModal = false;
         $scope.showModal = false;
         $scope.confirmResponded = false;
         $scope.leavePage = false;
         $scope.nextLocation = "#";
         $scope.isSave = false;
         $scope.showProcModal = false;

         $scope.search = { option: "1", searchText: "" };
         $scope.sortOption = "6";

         $scope.$watch('confirmResponded', function () {
             if (!$scope.pageChanging) {
                 if ($scope.confirmResponded) {
                     if ($scope.leavePage) {
                         $timeout(function () {
                             $location.url($scope.nextLocation.split('#')[1]);
                         }, 500);
                     } else {
                         $scope.confirmResponded = false;
                     }
                 }
             } else {
                 if ($scope.confirmResponded) {
                     if ($scope.leavePage) {
                         var grid = $("#emrMapping").data("kendoGrid");
                         grid.dataSource.page($scope.nextPage);
                         $scope.pageChanging = false;
                     } else {
                         $scope.confirmResponded = false;
                         $scope.pageChanging = false;
                     }
                 }
             }
         });

         $scope.$on('$locationChangeStart', function (event, next, current) {
             if (!$scope.confirmResponded) {
                 if ($scope.form) {
                     if ($scope.form.FormEmrMapping.$dirty) {
                         $scope.showModal = true;
                         $scope.statusText = "Are you sure you want to leave this page?";
                         $scope.confirmationType = "2";
                         $scope.nextLocation = next;
                         event.preventDefault();
                     }
                 }
             } else {
                 $scope.showModal = false;
             }
         });

         $scope.confirmNo = function () {
             $scope.leavePage = false;
             $scope.showModal = false;
             $scope.showOkModal = false;
             $scope.confirmResponded = true;
             $(".k-loading-mask").hide();
         }

         $scope.confirmYes = function () {
             //end confirmation when form dirty
             switch ($scope.confirmationType) {
                 case "1":
                     // Search refresh
                     $scope.showModal = false;
                     $scope.confirmResponded = true;

                     // search
                     getSearchData();
                     break;
                 case "2":
                     // Leave page
                     $scope.leavePage = true;
                     $scope.showModal = false;
                     $scope.confirmResponded = true;
                     break;
                 case "3":
                     // Navigate Page
                     //var grid = $("#emrMapping").data("kendoGrid");
                     //grid.dataSource.page($scope.nextPage);
                     $scope.confirmResponded = true;
                     $scope.emrMapping.dataSource.page($scope.nextPage);
                     $scope.showModal = false;
                     break;
                 default:
                     break;
             }
         }

         $rootScope.tempEmrId = $stateParams.emrId;

         //Procedures Gid

         function gridInitializeProcedures() {

             if (!$("#filterSearch").val()) {
                 $scope.procFilter = "";
             }
             $scope.procFilter = !$scope.procFilter ? "NA" : $scope.procFilter;
             $scope.mainGridOptions = {
                 dataSource: {
                     type: "json",
                     transport: {
                         read: {
                             url: function () {
                                 return "/Portal/api/Metadata/GetProcedureLevels/6/1/" + encodeURIComponent($scope.procFilter.trim()) + "/";
                             },
                         }
                     },

                     pageSize: 5,
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
                 selectable: true,
                 sortable: false,
                 pageable: true,
                 scrollable: true,
                 change: function () {
                     var grid = $("#procGrid").data("kendoGrid");
                     var selectedItem = grid.dataItem(grid.select());

                     var uuid = $scope.dataitem.uid
                     $scope.dataitem = selectedItem;
                     //Recall the method to set the return object
                     $("div[kendo-grid='emrMapping'] table tr[data-uid='" + uuid + "'] #procedure").trigger("click");

                     //$('#procModal').modal('hide');
                     $scope.modalInstance.close();
                 },
                 //selectable: "row",
                 columns: [{
                     field: "name",
                     title: "Procedure Name",
                     width: "20%"
                 }, {
                     field: "procedureLevel1",
                     title: "Level 1",
                     width: "20%",
                     template: "{{dataItem.procedureLevel1.name}}"
                 }, {
                     field: "procedureLevel1",
                     title: "Level 2",
                     width: "20%",
                     template: "{{dataItem.procedureLevel2.name}}"
                 }, {
                     field: "procedureLevel1",
                     title: "Level 3",
                     width: "20%",
                     template: "{{dataItem.procedureLevel3.name}}"
                 }, {
                     field: "procedureLevel1",
                     title: "Level 4",
                     width: "20%",
                     template: "{{dataItem.procedureLevel4.name}}"
                 }
                 ]
             };
         }

         //Procedures Grid end

         vm.emrMappingList = vm.emrMappingList || [];
         $scope.isValidMapping = true;

         $scope.init = function () {
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

             $scope.getJobRunningStatus()
         }

         //$scope.searchProc = function () {
         //    $scope.procFilter = $("#filterSearch").val();
         //    $('#procGrid').data('kendoGrid').dataSource.page(1);
         //}

         $scope.searchProc = function () {
             $scope.procFilter = $("#filterSearch").val();

             let typedText = $("#filterSearch").val();

             if (typedText == "") {
                 typedText = "NA";
             }
             typedText = typedText.replace(/\//gi, "_!").replace(/:/gi, "_~");
             // $scope.procFilter.trim()

             $('#procGrid').data('kendoGrid').dataSource.options.transport.read.url = '/Portal/api/Metadata/GetProcedureLevels/6/1/' + encodeURIComponent(typedText) + '/' + '?procedureId=' + $stateParams.userId;
             // $('#procGrid').data('kendoGrid').dataSource.read();
             $('#procGrid').data('kendoGrid').dataSource.page(1);
         }

         $scope.enterPress = function ($event) {
             if ($event.which === 13) {
                 $scope.searchProc();
             }
         }

         $scope.onSearchInputKeyPress = function (event) {
             if (event.charCode == 13) {
                 $scope.searchData();
             }
         }

         $scope.selectProcedure = function (dataitem, triggered) {
             //if (dataitem.id != 0) {
             //    return dataitem.procedure;
             //}
             if ($scope.dataitem && ($scope.dataitem.productTypeId != null || $scope.dataitem.procedureLevel1 != null)) {
                 var item = $scope.dataitem;
                 $scope.dataitem = null;
                 return item;
             } else {
                 $scope.form.FormEmrMapping.$dirty = true;
                 $scope.enableSaveBtn(dataitem);
                 $scope.dataitem = dataitem;
                 openProcModal();
                 return dataitem.procedure;
             }
         }

         function openProcModal() {
             gridInitializeProcedures();

             /*$('#procModal').modal('show');
             $(".modal-dialog").css({
                 'width': "1000px",
                 'height': "auto"
             });*/

             $scope.modalInstance = $uibModal.open({
                 templateUrl: 'procModal.html',
                 appendTo: $('body'),
                 scope: $scope,
                 windowClass: 'newClass',
                 size: 'lg'
             });

             $scope.procModalOpened = true;
         }

         $scope.procModalClosing =
          $('#procModal').on('hidden.bs.modal', function () {
              return $scope.selectedproc;
          });

         function showSpinner() {
             var gridElement = $("#emrMapping");
             var dataArea = gridElement.find(".k-grid-content");
             dataArea.append("<div class=\"k-loading-mask\" style=\"width:100%;height:100%\"><span class=\"k-loading-text\">Loading...</span><div class=\"k-loading-image\"><div class=\"k-loading-color\"></div></div></div>");
         }

         $scope.searchData = function () {
             // $scope.confirmResponded = true;

             if ($scope.form.FormEmrMapping.$dirty) {
                 $scope.showModal = true;
                 $scope.statusText = "Are you sure you want refresh the search?";
                 $scope.confirmationType = "1";
             }
             else {
                 getSearchData();
             }
         };

         var getSearchData = function () {
             let filterText = "NA";
             if (!$scope.search.option || !$scope.search.searchText) {
                 $scope.search.option = "1";
             }

             filterText = ($scope.search.searchText != "") ? $scope.search.searchText : "NA";
             filterText = $scope.emrMapping.dataSource.options.transport.read.url = "/Portal/api/Metadata/GetServicesFromStagingByEMRIdToGrid/" + $stateParams.emrId + "/" + $scope.sortOption + "/" + $scope.search.option + "/" + encodeURIComponent(filterText.trim()) + "/";
             // $scope.emrMapping.dataSource.read();
             $scope.emrMapping.dataSource.page(1);
         }

         $scope.resetSearch = function () {
             $scope.search.option = "1";
             $scope.search.searchText = "";
             $scope.sortOption = "6";
             $scope.emrMapping.dataSource.options.transport.read.url = "/Portal/api/Metadata/GetServicesFromStagingByEMRIdToGrid/" + $stateParams.emrId + "/" + $scope.sortOption + "/";
             // $scope.emrMapping.dataSource.read();
             $scope.emrMapping.dataSource.page(1);
         }

         //EMR Mapping Grid Datasource
         $scope.populateEMRMappingGrid = function (emrId) {
             let filterText = ($scope.search.searchText != "") ? $scope.search.searchText : "NA";
             $scope.emrMappingGridOptions = {
                 dataSource: {
                     type: "json",
                     transport: {
                         read: {
                             url: "/Portal/api/Metadata/GetServicesFromStagingByEMRIdToGrid/" + emrId + "/" + $scope.sortOption + "/" + $scope.search.option + "/" + encodeURIComponent(filterText.trim()) + "/",
                         }
                     },
                     requestStart: function (e) {
                         kendo.ui.progress($("#emrMapping"), true);
                         if (!$scope.confirmResponded) {
                             if ($scope.form) {
                                 if ($scope.form.FormEmrMapping.$dirty) {
                                     $scope.statusText = "Are you sure you want to leave this page?";
                                     $scope.showModal = true;
                                     $scope.$apply();
                                     $scope.nextPage = e.sender._page;
                                     $scope.pageChanging = true;
                                     $scope.confirmationType = "3";
                                     e.preventDefault();
                                 }
                             }
                         } else {
                             $scope.showModal = false;
                         }
                     },
                     requestEnd: function (e) {
                         kendo.ui.progress($("#emrMapping"), false);
                     },
                     pageSize: 50,
                     serverPaging: true,
                     serverGrouping: true,
                     schema: {
                         data: "data",
                         total: "total"
                     },
                 },
                 pageable: true,
                 scrollable: true,
                 dataBinding: function () {
                     $scope.changeServiceIdList = [];
                     var grid = $("#emrMapping").data("kendoGrid");
                     var currentPage = grid.dataSource.page();
                     RECORD = (currentPage - 1) * 50;
                     $scope.confirmResponded = false;
                     $scope.leavePage = false;
                     $scope.form.FormEmrMapping.$dirty = false;
                 },
                 columns: [{
                     title: "#",
                     template: "#= ++RECORD #",
                     width: "7%"
                 }, {
                     field: "serviceName",
                     title: "EMR Service Name",
                     width: "40%",
                 }, {
                     field: "",
                     title: "Status",
                     width: "7%",
                     template: "<input type=\"checkbox\" ng-change=\"isProdcutSale(dataItem)\" ng-disabled=\"false\" ng-model=\"dataItem.isDiscarded\"  value=\"dataItem.isDiscarded\"> Discard"
                 }, {
                     field: "",
                     title: "ANN Filter",
                     width: "46%",
                     template: "<div class=\"row\" ><div class=\"col-xs-12\"><div class=\"col-xs-2\" style=\"margin-top:6px; min-height: 30px;\"><input type=\"checkbox\" ng-change=\"isProdcutSale(dataItem)\" ng-disabled=\"false\" ng-model=\"dataItem.isProductSale\"  value=\"dataItem.isProductSale\"> Is Product</div><div ng-show=\"dataItem.isProductSale != true\" class=\"col-xs-8\" style=\"margin-top:5px;\"><label autocomplete=\"false\" id=\"procedure\" ng-click=\"dataItem.procedure = selectProcedure(dataItem)\" ng-disabled=\"false\" ng-model=\"dataItem.procedure.name\" name=\"procedure\" id=\"procedure\" bs-tooltip placement=\"left\" data-original-title=\"{{getTooltip(dataItem.procedure)}}\">{{!dataItem.procedure.name?\'Select a Procedure...\':dataItem.procedure.name}}</label></div> <div ng-show=\"dataItem.isProductSale == true\" class=\"col-xs-8\"><div class=\"row customRowWrapper\"  style=\"margin-right:-31px;\"><div class=\"col-xs-5\" style=\"margin-right:-20px;\"><input kendo-drop-down-list ng-disabled=\"false\" k-on-change=\"isProdcutSale(dataItem)\"  id=\"company\"  k-option-label=\"'Select a Company..'\" k-ng-model=\"dataItem.company\" k-auto-bind=\"false\" k-data-text-field=\"'name'\" k-data-value-field=\"'id'\"k-filter=\"'contains'\" k-data-source=\"companyDataSoruce\" name=\"procedure\" /></div><div  class=\"col-xs-5\"><input kendo-drop-down-list ng-disabled=\"false\" id=\"product\" k-on-change=\"isProdcutSale(dataItem)\"  k-option-label=\"'Select a Product..'\" k-ng-model=\"dataItem.productType\" k-auto-bind=\"false\"  k-data-text-field=\"'name'\" k-data-value-field=\"'id'\"k-filter=\"'contains'\"k-data-source=\"productsDataSoruce\" name=\"procedure\" /></div></div></div></div></div>"
                 }],
                 dataBound: function (e) {
                     if (!e.sender.dataSource.view().length) {
                         var colspan = e.sender.thead.find("th:visible").length;
                         var emptyRow = "<tr><td colspan=\"7\"><div class=\"no-data-alert\"><div>No data available for the given search criteria</div></div></td></tr>";
                         var gridWrapper = e.sender.wrapper;
                         var gridDataTable = e.sender.table;
                         var gridDataArea = gridDataTable.closest(".k-grid-content");
                         e.sender.tbody.end().html(emptyRow);
                     }
                 }
             };
         }

         $scope.populateEMRMappingGrid($stateParams.emrId);

         $scope.getTooltip = function (procedure) {
             var level1 = "-";
             var level2 = "-";
             var level3 = "-";
             var level4 = "-";

             var tooltip = "";

             if (procedure) {
                 // console.log(procedure);

                 if (procedure.procedureLevel1) {
                     level1 = procedure.procedureLevel1.name;
                 }
                 if (procedure.procedureLevel2) {
                     level2 = procedure.procedureLevel2.name;
                 }
                 if (procedure.procedureLevel3) {
                     level3 = procedure.procedureLevel3.name;
                 }
                 if (procedure.procedureLevel4) {
                     level4 = procedure.procedureLevel4.name;
                 }

                 tooltip = "<div class=\"row\"><div class=\"col-xs-1\"></div><div class=\"col-xs-2\">Name</div><div class=\"col-xs-2\">Level 1</div><div class=\"col-xs-2\">Level 2</div><div class=\"col-xs-2\">Level 3</div><div class=\"col-xs-2\">Level 4</div></div>";
                 tooltip += "<div class=\"row\"><div class=\"col-xs-1\"></div><div class=\"col-xs-2\">-------</div><div class=\"col-xs-2\">-------</div><div class=\"col-xs-2\">-------</div><div class=\"col-xs-2\">-------</div><div class=\"col-xs-2\">-------</div></div>";
                 tooltip += "<div class=\"row\"><div class=\"col-xs-1\"></div><div class=\"col-xs-2\">" + procedure.name + "</div><div class=\"col-xs-2\">" + level1 + "</div><div class=\"col-xs-2\">" + level2 + "</div><div class=\"col-xs-2\">" + level3 + "</div><div class=\"col-xs-2\">" + level4 + "</div></div>";
             }

             return tooltip;
         }

         $scope.isProdcutSale = function (dataItem) {
             $scope.changeServiceIdList = $scope.changeServiceIdList || [];
             var match = $scope.changeServiceIdList.filter(function (item) {
                 return item.emrServiceId == dataItem.emrServiceId;
             });
             if (match.length == 0) {
                 dataItem.previousCompanyId = dataItem.companyId;
                 dataItem.previousProductTypeId = dataItem.productTypeId;
                 dataItem.previousProcedureId = dataItem.procedureId;
                 $scope.changeServiceIdList.push(dataItem);
             }
             if (dataItem.isProductSale) {
                 dataItem.procedure = null;
                 dataItem.procedureId = null;

             } else {

                 dataItem.company = null;
                 dataItem.companyId = null;
                 dataItem.productType = null;
                 dataItem.productTypeId = null;

             }
         }

         $scope.enableSaveBtn = function (dataItem) {
             $scope.changeServiceIdList = $scope.changeServiceIdList || [];
             var match = $scope.changeServiceIdList.filter(function (item) {
                 return item.emrServiceId == dataItem.emrServiceId;
             });
             if (match.length == 0) {
                 $scope.changeServiceIdList.push(dataItem);
             }

             if (dataItem.procedure == null) {
                 $scope.isValidMapping = false;
             } else if (dataItem.company == null && dataItem.productType == null) {
                 $scope.isValidMapping = false;
             } else {
                 $scope.isValidMapping = true;
             }
             // console.log($scope.isValidMapping);
         }

         $scope.createPracticeProcedure = function (procedure) {

             // console.log($scope.form.FormEmrMapping);
             if (procedure.procedure != null && procedure.procedure.displayName == "Select a Procedure..") {
                 procedure.procedure = null;
             }

             if ((!procedure.isProductSale && procedure.procedure == null) || procedure.isProductSale && (procedure.company == null || procedure.productType == null || procedure.company.id == "" || procedure.productType.id == "")) {
                 toaster.error({
                     title: "",
                     body: "Please select an item from drop down to save."
                 });
             } else {
                 procedure.procedureId = procedure.procedure != null ? procedure.procedure.id : null;
                 procedure.companyId = procedure.company != null ? procedure.company.id : null;
                 procedure.productTypeId = procedure.productType != null ? procedure.productType.id : null;


                 PracticeService.createPracticeProcedure(procedure).success(function (data) {
                     var pid = data.emrProcedure * 1;
                     angular.forEach($scope.changeServiceIdList, function (value, key) {
                         if (pid == value) {
                             $scope.changeServiceIdList.splice(key, 1);
                             if ($scope.changeServiceIdList.length == 0) {
                                 $scope.form.FormEmrMapping.$dirty = false;
                             }
                         }
                     });

                     procedure.id = data.id;
                     console.log(":::SAVE PRACTICE PROCEDURE :::");
                     toaster.success({
                         title: "", body: "ANN Filter Mapping saved successfully."
                     });
                 }).error(function (error, status) {
                     if (status == -1) {
                         toaster.error({
                             title: "", body: "Cannot perform the function due to network failure."
                         });
                     } else {
                         toaster.error({
                             title: "", body: "Cannot perform the function Error occured."
                         });
                     }
                 });
             }
             // console.log($scope.emrMappingGridOptions);
             return procedure;
         }

         $scope.addMapping = function () {
             vm.relatedProcedure = {
                 emrprocedureId: 1,
                 emrprocedureName: "test",
                 procedureId: 1
             };
             vm.emrMappingList.unshift(vm.relatedProcedure);
         };

         $scope.createPracticeProcedureList = function () {

             angular.forEach($scope.changeServiceIdList, function (value, key) {
                 // value.previousProcedureId = value.procedureId;
                 //value.previousCompanyId = value.companyId;
                 //value.previousProductTypeId = value.productTypeId;
                 value.emrId = $stateParams.emrId;
                 value.procedureId = value.procedure != null ? value.procedure.id : null;
                 value.companyId = value.company != null ? value.company.id : null;
                 value.productTypeId = value.productType != null ? value.productType.id : null;
                 //if (value.isDiscarded == false) {
                 //    value.isDiscarded = null;
                 //}

             });

             var itemsToSave = $scope.changeServiceIdList.filter(function (value) {
                 // console.log(value);
                 if ((value.isProductSale == true && (value.companyId == "" || value.companyId == null) && (value.isProductSale == true && (value.productTypeId == "" || value.productTypeId == null)))) {
                     return;
                 }
                 if (value.isDiscarded != null && value.isProductSale == true && value.companyId == null && value.productTypeId == null) {
                     value.isProductSale = false;
                 }

                 var isValidItem = false;
                 // This condition is added to fix ANN-172
                 if (!value.isProdcutSale && ((value.companyId == null && value.productTypeId == null & value.previousCompanyId != null && value.previousProductTypeId != null) || (value.productTypeId == null && value.previousProcedureId != null))) {
                     isValidItem = true;
                 }

                 if (value.isProductSale == false && value.procedureId == null && value.previousProcedureId == null) {
                     if ((value.previousCompanyId == null || value.previousCompanyId == "") && (value.previousProductTypeId == null || value.previousProductTypeId == "")) {
                         isValidItem = false;
                     }
                     else
                         isValidItem = true;
                 }

                 return (value.procedureId != null || value.companyId != null || value.productTypeId != null || value.isDiscarded != null || isValidItem);
             });

             if (itemsToSave.length > 0) {
                 PracticeService.createPracticeProcedureList(itemsToSave).success(function (data) {
                     $scope.form.FormEmrMapping.$dirty = false;
                     var gridReload = false;
                     // $('#emrMapping').data('kendoGrid').dataSource.read();
                     $scope.resetSearch();
                     var pid = data.emrProcedure * 1;
                     angular.forEach($scope.changeServiceIdList, function (value, key) {

                         if (value.isDiscarded) {
                             gridReload = true;
                         }

                         if (pid == value) {
                             $scope.changeServiceIdList.splice(key, 1);
                             if ($scope.changeServiceIdList.length == 0) {
                                 $scope.form.FormEmrMapping.$dirty = false;
                             }
                         }
                     });

                     if (gridReload) {
                         //$('#emrMapping').data('kendoGrid').dataSource.read();
                         $scope.resetSearch();
                     }

                     $scope.changeServiceIdList = [];
                     procedure.id = data.id;
                     console.log(":::SAVE PRACTICE PROCEDURE :::");
                     toaster.success({
                         title: "", body: "ANN Filter Mappings saved successfully."
                     });
                 }).error(function (error, status) {
                     if (status == -1) {
                         toaster.error({
                             title: "", body: "Cannot perform the function due to network failure."
                         });
                     } else {
                         toaster.error({
                             title: "", body: "Cannot perform the function Error occured."
                         });
                     }
                 });
             } else {
                 toaster.error({
                     title: "", body: "There are no valid modifications to save."
                 });
             }
         };

         $scope.cancel = function () {
             $location.path('/admin/practice-management');
         }

         $scope.getJobRunningStatus = function () {
             // get job running status
             $scope.cubeProcessQueue = [];
             PracticeService.getJobRunningStatus().success(function (res) {
                 _.each(res, function (obj) {
                     $scope.cubeProcessQueue.push(obj.emrId);
                 });
                 $scope.isCubeProcessing = res.length > 0;

                 $scope.showOkModal = $scope.isCubeProcessing;
                 // showOkModal
             });
         }
     });
})();

//function practiceDeactivate(str, status) {
//    angular.element(document.getElementById('practiceList')).scope().deactiveate(str, status);
//}

//function addToRefreshProcess(str) {
//    angular.element(document.getElementById('practiceList')).scope().addToRefreshProcess(str);
//}

function viewPractice(id, emrId) {
    angular.element(document.getElementById('practiceList')).scope().viewPractice(id, emrId);
}