(function () {
    'use strict';
    angular.module('annPortalApp')
        .factory('ProcedureLevelService', function ($httpMock, $q, $http) {
            return {
                getProcedureLevel: function (levelId) {
                    return $http.get("/Portal/api/Metadata/GetProcedureLevel/" + levelId);
                },
                saveProcedureLevels: function (level) {
                    return $http.post("/Portal/api/Metadata/SaveProcedureLevels/", level);
                },
                getAllProcedureLevels: function () {
                    return $http.get("/Portal/api/Metadata/GetAllProcedureLevels/");
                },
                isDuplicateProcedureLevelSequence: function (name, procedureLevel1, procedureLevel2, procedureLevel3, procedureLevel4) {
                    return $http.get("/Portal/api/Metadata/IsDuplicateProcedureLevelSequence/" + name + '/' + procedureLevel1 + '/' + procedureLevel2 + '/' + procedureLevel3 + '/' + procedureLevel4);
                },
                saveProcedureLevelSequences: function (procedureLevelSequences) {
                    return $http.post("/Portal/api/Metadata/ProcedureLevelSequences/", procedureLevelSequences);
                },
                isProcedureLevelUnique: function (levelNumber, name) {
                    return $http.get("/Portal/api/Metadata/IsProcedureLevelUnique/" + levelNumber + "/" + name);
                },
                isBreastImplantUnique: function (name) {
                    return $http.get("/Portal/api/Metadata/IsBreastImplantUnique/" + name);
                },
                isProductTypeUnique: function (name) {
                    return $http.get("/Portal/api/Metadata/IsProductTypeUnique/" + name);
                },
                isCompanyUnique: function (name) {
                    return $http.get("/Portal/api/Metadata/IsCompanyUnique/" + name);
                },
                getProcedureById: function (id) {
                    return $http.get("/Portal/api/Metadata/GetProcedureById/" + id);
                },
                procedureRelatedProcedure: function (procedure) {
                    return $http.post("/Portal/api/Metadata/ProcedureRelatedProcedure/", procedure);
                },
                getRelatedProcedureByProcedureId: function (id) {
                    return $http.get("/Portal/api/Metadata/GetRelatedProcedureByProcedureId/" + id);
                },
                deleteProcedure: function (procedureId) {
                    return $http.post("/Portal/api/Metadata/DeleteProcedure/" + procedureId);
                },
                getJobRunningStatus: function () {
                    return $http.get("/Portal/api/Practice/GetJobRunningStatus");
                }
            };
        });
})();
