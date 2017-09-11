(function() {
    'use strict';
    angular.module('annPortalApp')
        .factory('BreastImplantService', function($httpMock, $q,  $http) {
            return {
                getAll: function() {
                    return $httpMock.get()
                        .respond([{
                            Id: 1,
                            Name: 'Abc' 
                        },{
                            Id: 2,
                            Name: 'Def' 
                        },{
                            Id: 3,
                            Name: 'Ghi' 
                        },{
                            Id: 4,
                            Name: 'Jkl' 
                        },{
                            Id: 5,
                            Name: 'Mno' 
                        },]);
                },
                save: function() {
                    return $httpMock.get()
                        .respond({
                            Id: id,
                            Name: 'Wayne Inc.',
                            AddressLine1: '4, Anderson road,',
                            AddressLine2: 'Seastreet, Negombo',
                            City: 'Gotham',
                            ContactPerson: 'John Doe',
                            ContactNo: '94777301150'
                        });
                },
                getBreastImplants: function () {
                    return $http.get("/Portal/api/Metadata/GetBreastImplants/");
                },
                saveBreastImplants: function (breastImplants) {
                    return $http.post("/Portal/api/Metadata/SaveBreastImplants/", breastImplants);
                },
                getProducts: function () {
                    return $http.get("/Portal/api/Metadata/GetProducts/");
                },
                saveProducts: function (productsSolds) {
                    return $http.post("/Portal/api/Metadata/SaveProducts/", productsSolds);
                },
                getCompanies: function () {
                    return $http.get("/Portal/api/Metadata/GetCompanies/");
                },
                saveCompanies: function (productCompanies) {
                    return $http.post("/Portal/api/Metadata/SaveCompanies/", productCompanies);
                }

            };
        });
})();