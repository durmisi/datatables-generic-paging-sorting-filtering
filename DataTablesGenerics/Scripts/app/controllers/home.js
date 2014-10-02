

'use strict';
MyApp.controller('homeController', [
    '$scope', '$rootScope', '$filter', 'DTOptionsBuilder', 'DTColumnBuilder',
    function ($scope, $rootScope, $filter, dtOptionsBuilder, dtColumnBuilder) {

        var webserviceUrl = '/Home/GetGridData';

        $scope.dtOptions = dtOptionsBuilder
            .fromSource(webserviceUrl)
            .withDataProp('data')
            .withOption("searching", true)
            .withOption("serverSide", true)
            .withOption("lengthChange", false)
            .withOption('responsive', true)
            .withBootstrap()
        ;

        $scope.dtColumns = [
            dtColumnBuilder.newColumn('Name').withTitle('Name'),
            dtColumnBuilder.newColumn('Email').withTitle('Email'),
            dtColumnBuilder.newColumn('Position').withTitle('Position').withOption("searchable", true),
            dtColumnBuilder.newColumn('Salary')
            .renderWith(function (d) {
                return $filter('currency')(d, '$');
            })
            .withTitle('Salary').withOption("searchable", true)
        ];



    }
]);