(function () {
    'use strict';
    angular.module('annPortalApp')
        .config(function (stateHelperProvider) {
            stateHelperProvider
                .state({
                    name: 'admin.user-management',
                    url: '/user-management',
                    templateUrl: 'app/admin/user-management/user-management.html',
                    controller: 'Admin.UserManagement'
                });
        })
        .controller('Admin.UserManagement', function ($scope, $rootScope, $timeout, toaster, $location, $filter, UserService, PracticeService) {

            $scope.disableHubs = false;
            $scope.PracticeId = "NA";

            $scope.User = {
                Id: null,
                UserId: null,
                FirstName: null,
                LastName: null,
                OrganizationId: 0,
                UserDesignation: null,
                Email: null,
                DesignationId: 0,
                Roles: []
            };

            $scope.Init = function () {
                $scope.yesClicked = false;
                $scope.errorCount = 0;
                $scope.disableSave = false;
                $scope.isEditMode = false;
                $scope.showModal = false;
                $scope.submitted = false;
                $scope.Roles = null;
                $scope.Users = null;
                $scope.Hubs = [];
                $scope.Hub = {
                    Id: null,
                    Name: null,
                    selected: false
                };

                $scope.firstName = 'NA';
                $scope.lastName = 'NA';
                $scope.userId = 'NA';

                $scope.UserTypes = [];
                $scope.User = {
                    Id: null,
                    UserId: null,
                    FirstName: null,
                    LastName: null,
                    OrganizationId: 0,
                    UserDesignation: null,
                    Email: null,
                    DesignationId: 0,
                    //SecondaryHubIds: [],
                    Roles: []
                };

                populateUsers($scope.firstName, $scope.lastName, $scope.userId);

                UserService.getRoles()
                .success(function (result) {
                    $scope.Roles = result;
                });

            };

            // Key press search 
            $scope.onSearchInputKeyPress = function (event) {
                if (event.charCode == 13) {
                    $scope.searchUsers();
                    $scope.active = 1;
                }
            }

            function populateUsers() {
                $scope.mainUsersGridOptions =
                      {
                          dataSource: {
                              type: "json",
                              transport: {
                                  read: {
                                      url: function () {
                                          return IDENTITY_DOMAIN + "api/Account/users/" + $scope.firstName + '/' + $scope.lastName + "/" + $scope.userId + "/" + $scope.PracticeId + "/NA";
                                      },
                                  }
                              },
                              pageSize: 10,
                              serverPaging: true,
                              serverFiltering: true,
                              serverGrouping: true,
                              serverAggregates: true,
                              schema: {
                                  data: "Data",
                                  total: "Total"
                              },
                          },
                          pageable: true,
                          columns: [
                          {
                              field: "item.Email",
                              title: "User ID",
                              width: "18%"
                          }, {
                              field: "item.FullName",
                              title: "User Name",
                              width: "14%"
                          }, {
                              field: "item.UserRolesDisplay",
                              title: "Role",
                              width: "12%"
                          }, {
                              field: "item.PracticeName",
                              title: "Practice ID",
                              width: "12%"
                          }, {
                              field: "item.LastModifiedDateDisplay",
                              title: "Last Modified",
                              width: "12%"
                          },
                          {
                              field: "",
                              title: "",
                              width: "auto"
                          }],
                          dataBound: function (e) {
                              if (!e.sender.dataSource.view().length) {
                                  var colspan = e.sender.thead.find("th:visible").length;
                                  var emptyRow = "<tr><td colspan=\"6\"><div class=\"no-data-alert\"><div>No data available for the given search criteria</div></div></td></tr>";
                                  var gridWrapper = e.sender.wrapper;
                                  var gridDataTable = e.sender.table;
                                  var gridDataArea = gridDataTable.closest(".k-grid-content");
                                  var scroller = gridDataArea.find('div.k-scrollbar.k-scrollbar-vertical');
                                  //scroller.outerHTML = '';
                                  //e.sender.tbody.parent().end().html(scroller);
                                  e.sender.tbody.end().html(emptyRow);
                              }
                          },
                      };
            }

            $scope.searchUsers = function () {

                if ($scope.search.FirstName == undefined || $scope.search.FirstName == '') {
                    $scope.firstName = 'NA';
                }
                else {
                    $scope.firstName = $scope.search.FirstName;
                }

                if ($scope.search.LastName == undefined || $scope.search.LastName == '') {
                    $scope.lastName = 'NA';
                }
                else {
                    $scope.lastName = $scope.search.LastName;
                }

                if ($scope.search.UserId == undefined || $scope.search.UserId == '') {
                    $scope.userId = 'NA';
                }
                else {
                    $scope.userId = $scope.search.UserId;
                }

                if ($scope.search.PracticeId == undefined || $scope.search.PracticeId == '') {
                    $scope.PracticeId = 'NA';
                }
                else {
                    $scope.PracticeId = $scope.search.PracticeId;
                }

                $scope.userGrid.dataSource.read({
                    firstName: $scope.firstName,
                    lastName: $scope.lastName,
                    userId: $scope.userId,
                });
                $scope.userGrid.dataSource.page(1);
            };

            $scope.selectOptions = {
                optionLabel: "Select hub...",
                dataTextField: "Name",
                dataValueField: "Id",
                valuePrimitive: true,
                dataSource: {
                    serverFiltering: true,
                    transport: {
                        read: {
                            url: IDENTITY_DOMAIN + "api/Organization/activeOrganizations/0",
                            type: "post",
                            dataType: "json"
                        }
                    }
                }
            };

            $scope.selectHubsOptions = {
                placeholder: "Select hubs...",
                dataTextField: "Name",
                dataValueField: "Id",
                valuePrimitive: true,
                dataSource: {
                    serverFiltering: true,
                    transport: {
                        read: {
                            url: function () {
                                return IDENTITY_DOMAIN + "api/Organization/activeOrganizations/" + $scope.User.OrganizationId;
                            },
                            type: "post"
                        }
                    }
                }
            };

            $scope.designationOptions = {
                optionLabel: "Select user type...",
                dataTextField: "DisplayName",
                dataValueField: "Id",
                valuePrimitive: true,
                dataSource: {
                    serverFiltering: true,
                    transport: {
                        read: {
                            url: IDENTITY_DOMAIN + "api/Designation/",
                            type: "get",
                            dataType: "json"
                        }
                    }
                }
            };

            $scope.$watch('User.OrganizationId', function () {
                if ($scope.isUserAdmin == 'True' && $scope.showModal) {
                    if ($scope.User.OrganizationId == undefined || $scope.User.OrganizationId == "") {
                        $scope.User.OrganizationId = 0;
                    }
                    var data = $("#select_secondaryhub").data("kendoMultiSelect");
                    data.dataSource.read();
                }
            });

            $scope.Edit = function (userId) {
                $scope.isEditMode = true;
                $scope.toggleModal();
                $scope.User.UserId = userId;

                UserService.getRoles()
                .success(function (result) {
                    $scope.Roles = result;

                    UserService.getUser($scope.User)
                    .success(function (result) {
                        // console.log($scope.User);
                        $scope.User = result;
                        // console.log($scope.User);
                        $scope.RoleId = $scope.User.Roles[0];
                        //for (let selectrole in $scope.User.Roles) {
                        //    for (let role in $scope.Roles) {
                        //        if ($scope.Roles[role].Name == $scope.User.Roles[selectrole]) {
                        //            $scope.Roles[role].selected = true;
                        //        }
                        //    }
                        //}
                    });
                });
            }

            $scope.Deactivate = function (userId) {
                $scope.User.UserId = userId;
                UserService.getUser($scope.User)
                .success(function (result) {
                    $scope.User = result;
                });
                UserService.deactivateUser($scope.User)
                .success(function (result) {
                    toaster.success({ title: "", body: "User deactivate successful" });
                    $scope.userGrid.dataSource.read();
                    $scope.userGrid.dataSource.page(1);
                    $scope.yesClicked = false;
                    $("#confirmPopup").removeClass('showPopup');
                }).error(function (error, status) {
                    toaster.error({ title: "", body: "Cannot perform the function. Error occured." });
                    $scope.yesClicked = false;
                });
            }

            $scope.Activate = function (userId) {
                $scope.User.UserId = userId;
                UserService.getUser($scope.User)
                .success(function (result) {
                    $scope.User = result;
                });
                UserService.activateUser($scope.User)
                .success(function (result) {
                    toaster.success({ title: "", body: "User activate successful" });
                    $scope.userGrid.dataSource.read();
                    $scope.userGrid.dataSource.page(1);
                    $("#confirmPopup").removeClass('showPopup');
                    $scope.yesClicked = false;
                }).error(function (error, status) {
                    toaster.error({ title: "", body: "Cannot perform the function. Error occured." });
                    $scope.yesClicked = false;
                });
            }

            $scope.Reset = function (userId) {
                if (userId) {
                    $scope.User.UserId = userId;
                }

                UserService.getUser($scope.User)
                .success(function (result) {
                    $scope.User = result;
                    $("#confirmPopup").removeClass('showPopup');
                });

                UserService.resetUser($scope.User)
                .success(function (result) {
                    toaster.success({ title: "", body: "User credential reset successful" });
                    $("#confirmPopup").removeClass('showPopup');
                    $scope.yesClicked = false;
                }).error(function (error, status) {
                    toaster.error({ title: "", body: "Cannot perform the function. Error occured." });
                    $scope.yesClicked = false;
                });
            }

            $scope.clearSearch = function () {

                $scope.search.FirstName = "";
                $scope.search.LastName = "";
                $scope.search.UserId = "";
                $scope.search.PracticeId = "";

                $scope.firstName = "NA";
                $scope.lastName = "NA";
                $scope.userId = "NA";
                $scope.PracticeId = "NA";

                $scope.userGrid.dataSource.read({
                    firstName: $scope.firstName,
                    lastName: $scope.lastName,
                    userId: $scope.userId,
                });
                $scope.userGrid.dataSource.page(1);
            }

            $scope.Save = function () {
                $scope.errorCount = 0;
                $scope.submitted = true;
                $scope.disableSave = true;
                $scope.User.Roles = [];

                for (let role in $scope.Roles) {
                    if ($scope.Roles[role].Id == $scope.User.RoleId) {
                        $scope.User.Roles.push($scope.Roles[role].Name);
                    }
                }

                validate($scope.User);

                if ($scope.errorCount == 0) {
                    if ($scope.User.Id != null) {
                        UserService.updateUser($scope.User)
                                .success(function (result) {
                                    $scope.User.UsernameExists = false;
                                    $scope.User.EmailExists = false;
                                    $scope.userGrid.dataSource.read();
                                    showSuccess();
                                    $scope.disableSave = false;
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
                                    else if (message.Message == "The request is invalid") {

                                    }
                                    else {
                                        $scope.User.EmailExists = false;
                                        showError();
                                    }
                                    $scope.disableSave = false;
                                });
                    }
                    else {
                        UserService.postUser($scope.User)
                        .success(function (result) {
                            $scope.User.UsernameExists = false;
                            $scope.User.EmailExists = false;
                            $scope.userGrid.dataSource.read();
                            showSuccess();
                            $scope.disableSave = false;
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
                        });
                    }
                }
                else {
                    $scope.disableSave = false;
                }
            };

            $scope.toggleModal = function (isCreate) {
                if (isCreate) {
                    $scope.modalTitle = "Create New User";
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

                    UserService.getRoles()
                    .success(function (result) {
                        $scope.Roles = result;
                    });

                    if ($scope.isUserHubAdmin == 'True' && $scope.isUserAdmin == 'False') {
                        $scope.disableHubs = true;
                        $scope.User.OrganizationId = $scope.hubId;
                    }
                }
                else {
                    $scope.modalTitle = "Edit User";
                }
                $scope.showModal = !$scope.showModal;
                if (!$scope.showModal) {
                    $scope.Init();
                }
            };

            function validate(user) {
                // console.log(user)
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

                if (user.Roles == null || user.Roles.length == 0) {
                    $scope.User.RolesIsEmpty = true;
                    $scope.errorCount++;
                }
                else {
                    $scope.User.RolesIsEmpty = false;
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
                }, 2000);
            }

            function showDeleteError() {
                $scope.showDeleteError = true;
                $timeout(function () {
                    $scope.showDeleteError = false;
                }, 5000);
            }

            function showDeleteSuccess() {
                $scope.showDeleteSuccess = true;
                $timeout(function () {
                    $scope.showDeleteSuccess = false;
                }, 5000);
            }

            $scope.showConfirmPopup = function (userId, status) {
                $scope.popupSwitch = 1;
                $scope.tempUserId = userId;
                $scope.tempUserStatus = status;
                $scope.showModal = false;
                var statusCheck = status ? "activate" : "deactivate";

                $("#popText").html('Are you sure you want to <strong>' + statusCheck + '</strong> this user <strong>' + userId + '</strong>?');
                $("#confirmPopup").addClass('showPopup');
            };

            $scope.showConfirmPopupForReset = function (userId) {
                $scope.popupSwitch = 2;
                $scope.tempUserId = userId;
                $("#popText").html('Are you sure you want to <strong>Reset Credentials</strong> for the user <strong>' + userId + '</strong>?');
                $("#confirmPopup").addClass('showPopup');
            };

            $scope.confirmYes = function () {
                $scope.yesClicked = true;
                switch ($scope.popupSwitch) {
                    case 1:
                        // activate / deactivate user
                        // $scope.tempUserId = userId;
                        if ($scope.tempUserStatus) {
                            $scope.Activate($scope.tempUserId);
                        }
                        else {
                            $scope.Deactivate($scope.tempUserId);
                        }
                        break;
                    case 2:
                        $scope.Reset($scope.tempUserId);
                        break;
                    default:
                        break;
                }
            }

            $scope.confirmNo = function () {
                $("#confirmPopup").removeClass('showPopup');
            }

            $scope.isAdmin = function (user) {
                return (user.UserRolesDisplay.includes("Administrator") || user.UserRolesDisplay.includes("Super Admin"));
            }

            $scope.isSuperAdmin = function (user) {
                return (user.UserRolesDisplay.includes("Super Admin"));
            }
        })
        .service('UserService', ['$http', function ($http) {
            var usersApiUrl = IDENTITY_DOMAIN + "api/Account/";
            var rolesApiUrl = IDENTITY_DOMAIN + "api/Role/";
            var hubsApiUrl = IDENTITY_DOMAIN + "api/Organization/";
            var notificationApiUrl = CORE_SERVICE_DOMAIN;


            this.postUser = function (data) {
                return $http.post(usersApiUrl + "create", data);
            };

            this.getRoles = function () {
                return $http.get(rolesApiUrl + "activeRoles");
            };

            this.getRole = function (data) {
                result = $http.get(rolesApiUrl + "?roleId=" + data);
                return result;
            };

            this.getUsers = function () {
                data = $http.get(usersApiUrl + "users");
                return data;
            }

            this.getUser = function (data) {
                return $http.post(usersApiUrl + "user", data);
            }

            this.updateUser = function (data) {
                return $http.post(usersApiUrl + "update", data);;
            }

            this.deactivateUser = function (data) {
                return $http.post(usersApiUrl + "deactivate", data);;
            }

            this.activateUser = function (data) {
                return $http.post(usersApiUrl + "activate", data);;
            }

            this.resetUser = function (data) {
                return $http.post(usersApiUrl + "reset", data);;
            }

            this.getHubs = function () {
                data = $http.post(hubsApiUrl + "activeOrganizations");
                return data;
            }

            this.AddNotificationToNewUsers = function (data, hubId) {
                return $http.post(notificationApiUrl + "api/notification/AddNotificationToNewUsers/" + hubId, data);
            };

        }]);
})();