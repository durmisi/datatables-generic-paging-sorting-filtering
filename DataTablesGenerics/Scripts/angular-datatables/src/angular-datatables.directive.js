(function(angular) {
    'use strict';

    angular.module('datatables.directive', ['datatables.renderer', 'datatables.options']).
    directive('datatable', function(DT_DEFAULT_OPTIONS, DTBootstrap, DTRendererFactory) {
        return {
            restrict: 'A',
            scope: {
                dtOptions: '=',
                dtColumns: '=',
                dtColumnDefs: '=',
                datatable: '@'
            },
            compile: function (tElm) {
                var _staticHTML = tElm[0].innerHTML;
                return function postLink($scope, $elem, iAttrs, ctrl) {
                    ctrl.showLoading($elem);
                    $scope.$watch('[dtOptions, dtColumns, dtColumnDefs]', function() {
                        ctrl.render($elem, ctrl.buildOptions(), _staticHTML);
                    }, true);
                };
            },
            controller: function ($scope) {
                this.showLoading = function ($elem) {
                    DTRendererFactory.showLoading($elem);
                };
                this.buildOptions = function () {
                    // Build options
                    var options;
                    if (angular.isDefined($scope.dtOptions)) {
                        options = {};
                        angular.extend(options, $scope.dtOptions);
                        // Set the columns
                        if (angular.isArray($scope.dtColumns)) {
                            options.aoColumns = $scope.dtColumns;
                        }
                        // Set the column defs
                        if (angular.isArray($scope.dtColumnDefs)) {
                            options.aoColumnDefs = $scope.dtColumnDefs;
                        }
                        // Integrate bootstrap (or not)
                        if (options.integrateBootstrap) {
                            DTBootstrap.integrate(options);
                        } else {
                            DTBootstrap.deIntegrate();
                        }
                    }
                    return options;
                };
                this.render = function ($elem, options, staticHTML) {
                    var isNgDisplay = $scope.datatable && $scope.datatable === 'ng';
                    // Render dataTable
                    DTRendererFactory.fromOptions(options, isNgDisplay).render($scope, $elem, staticHTML);
                };
            }
        };
    });
})(angular);
