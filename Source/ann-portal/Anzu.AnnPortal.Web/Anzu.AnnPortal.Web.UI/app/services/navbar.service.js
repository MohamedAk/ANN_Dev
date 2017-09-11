(function () {
    'use strict';
    angular.module('annPortalApp')
        .factory('NavBarService', function ($httpMock, $q, $http) {
            return {
                getAll: function () {
                    return $httpMock.get()
                        .respond([{
                            Id: 1,
                            Name: 'Abc'
                        }, {
                            Id: 2,
                            Name: 'Def'
                        }, {
                            Id: 3,
                            Name: 'Ghi'
                        }, {
                            Id: 4,
                            Name: 'Jkl'
                        }, {
                            Id: 5,
                            Name: 'Mno'
                        }, ]);
                }, 
                getNavigationBarInformation: function () {
                    return $http.get("/Portal/api/NavigationBar/GetNavigationBarInformation/");
                }

            };
        });
})();