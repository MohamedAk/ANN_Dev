(function () {
    'use strict';
    angular.module('annPortalApp')
        .factory('PracticeService', function ($http) {

            var practiceMangementApiUrl = "";
            var rolesApiUrl = IDENTITY_DOMAIN + "api/Role/";
            var usersApiUrl = IDENTITY_DOMAIN + "api/Account/";

            return {
                createPractice: function (practice) {
                    return $http.post("/Portal/api/Practice/CreatePractice", practice);
                },
                updatePractice: function (practice) {
                    return $http.post("/Portal/api/Practice/UpdatePractice", practice);
                },
                deactivatePractice: function (practice) {
                    return $http.post("/Portal/api/Practice/DeActivatePractice", practice);
                },
                getRegions: function () {
                    return $http.get("/Portal/api/Practice/Regions");
                },
                getStates: function () {
                    return $http.get("/Portal/api/Practice/States");
                },
                getZipCodes: function () {
                    return $http.get("/Portal/api/Practice/ZipCodes");
                },
                getPractices: function () {
                    return $http.get("/Portal/api/Practice/Practices");
                },
                getPractice: function (id) {
                    return $http.get("/Portal/api/Practice/GetPracticeById/" + id);
                },
                isPracticeNameUnique: function (practiceName) {
                    return $http.get("/Portal/api/Practice/IsPracticeNameUnique/" + encodeURIComponent(practiceName.trim()));
                },
                isUserAlreadyAdded: function (practiceUser) {
                    return $http.post("/Portal/api/Practice/IsUserAlreadyAdded", practiceUser);
                },
                createASAPUser: function (asapUser) {
                    return $http.post("/Portal/api/Practice/CreateASAPUser", asapUser);
                },
                deactivateASAPUser: function (id) {
                    return $http.post("/Portal/api/Practice/DeactivateASAPUser", id);
                },
                getNextEmrId: function () {
                    return $http.get("/Portal/api/Practice/GetLatestEMRId/");
                },
                getUserByUserName: function (id) {
                    return $http.get(usersApiUrl + "userById/" + id);
                },
                createPracticeProcedure: function (procedure) {
                    return $http.post("/Portal/api/Practice/AddPracticeProcedure", procedure);
                },
                createPracticeProcedureList: function (procedures) {
                    return $http.post("/Portal/api/Practice/AddPracticeProcedures", procedures);
                },
                updateDashboradIdForPreview: function (id) {
                    return $http.get("/Portal/api/Practice/UpdateDashboradIdForPreview/" + id);
                },
                getRoles: function () {
                    return $http.get(rolesApiUrl + "activeRoles");
                },
                postUser: function (data) {
                    return $http.post(usersApiUrl + "create", data);
                },
                deactivateUser: function (data) {
                    return $http.post(usersApiUrl + "deactivate", data);
                },
                activateUser: function (data) {
                    return $http.post(usersApiUrl + "activate", data);
                },
                getUser: function (data) {
                    return $http.post(usersApiUrl + "user", data);
                },
                updateIdentityUsers: function (data) {
                    return $http.post(usersApiUrl + "UpdateUserPractice", data);
                },
                isUserAlreadyAssigned: function (practiceUser) {
                    return $http.post("/Portal/api/Practice/IsUserAlreadyAssigned", practiceUser);
                },
                editPracticeBreastImplants: function (practiceId, emrId) {
                    return $http.post("/Portal/api/Practice/EditPracticeBreastImplants/" + practiceId + "/" + emrId.trim());
                },
                validateCubeRefreshByPractice: function (emrId) {
                    return $http.get("/Portal/api/Practice/ValidateCubeRefreshByPractice/" + emrId);
                },
                practiceReActivatedCheck: function (emrId) {
                    return $http.get("/Portal/api/Practice/PracticeReActivatedCheck/" + emrId);
                },
                practiceHasDataCheck: function (emrId) {
                    return $http.get("/Portal/api/Practice/PracticeHasDataCheck/" + emrId);
                },
                processCube: function (emrList, jobType) {
                    var data = JSON.stringify({ emrList: emrList, jobType: jobType });
                    return $http.post("/Portal/api/Practice/ProcessCube", data);
                },
                getJobRunningStatus: function () {
                    return $http.get("/Portal/api/Practice/GetJobRunningStatus");
                },
                cancelJob: function () {
                    return $http.get("/Portal/api/Practice/CancelPendingJob");
                },
                getLastUpdatedDate: function (emrId) {
                    return $http.get("/Portal/api/Practice/GetLastUpdatedDate/" + emrId);
                },
                isPracticeContainImplants: function (id) {
                    return $http.get("/Portal/api/Practice/IsPracticeContainImplants/" + id);
                },
                validatePracticeList: function (emrList) {
                    return $http.post("/Portal/api/Practice/ValidatePracticeList", JSON.stringify(emrList));
                }
            };
        });
})();