function FeedbackListHelper($q, $http, $routeParams, umbRequestHelper) {
    return {
        getVisitorTypes() {
            return umbRequestHelper.resourcePromise(
                $http.get("backoffice/Api/Feedback/GetVisitorTypes"), "Failed to retrieve all Feedback data."
            );
        },
        getCategories() {
            return umbRequestHelper.resourcePromise(
                $http.get("backoffice/Api/Feedback/GetCategories"), "Failed to retrieve all Feedback data."
            );
        },
        getItems(pageNumber = null, pageSize = null, orderBy = null, orderDirection = null, orderBySystemField = false, filter = '') {
            orderDirection = orderDirection === 'desc' ? 'Descending' : 'Ascending';

            var query = "?id=" + $routeParams.id;
            query += pageNumber ? "&pageNumber=" + pageNumber : "";
            query += pageSize ? "&pageSize=" + pageSize : "";
            query += orderBy ? "&orderBy=" + orderBy : "";
            query += orderDirection ? "&orderDirection=" + orderDirection : "";
            query += "&orderBySystemField=" + orderBySystemField + "&filter=" + filter;

            return umbRequestHelper.resourcePromise(
                $http.get("backoffice/UmbracoApi/Content/GetChildren" + query), "Failed to retrieve all Feedback data."
            );
        },
        export(start, end) {
            var param = '';
            if (start !== '') {
                param += '?start=' + start;
            }
            if (end !== '') {
                param += (param === '' ? '?' : '&') + 'end=' + end;
            }

            var win = window.open("backoffice/Api/Feedback/Export" + param, '_blank');
            if (win) {
                win.focus();
            } else {
                alert('Please allow popups for this website');
            }

            //return umbRequestHelper.resourcePromise(
            //    $http.get(), "Failed to retrieve all Feedback data."
            //);
        }
    }
}

angular.module("umbraco.resources").factory("feedbackListHelper", FeedbackListHelper);

function FeedbackListViewController($scope, listViewHelper, $location, assetsService, feedbackListHelper) {
    console.log('FeedbackListViewController');
    assetsService.loadCss('~/App_Plugins/FeedbackListView/datepicker.min.css');
    $scope.exportStart = '';
    $scope.exportEnd = '';

    var vm = this;

    vm.exportBtn = {};
    vm.exportBtn.state = 'init';
    vm.exportBtn.action = () => {
        feedbackListHelper.export($scope.exportStart, $scope.exportEnd);
        //feedbackListHelper.export($scope.exportStart, $scope.exportEnd).then(response => {
            //var wb = XLSX.read(response);
            //XLSX.writeFile(wb, 'FeedbackReport.xls', {});
            //vm.exportBtn.state = 'init';
        //});
    };

    function mapItems() {
        if ($scope.items.length > 0) {
            vm.items = $scope.items.map(item => {
                return {
                    id: item.id,
                    key: item.key,
                    icon: item.icon,
                    name: item.name,
                    category: item.category || (item.properties.find(o => o.alias === 'category') ? item.properties.find(o => o.alias === 'category').value : null),
                    otherCategory: item.otherCategory || (item.properties.find(o => o.alias === 'otherCategory') ? item.properties.find(o => o.alias === 'otherCategory').value : null),
                    rating: item.rating || item.properties.find(o => o.alias === 'rating') ? item.properties.find(o => o.alias === 'rating').value : null,
                    description: item.description || (item.properties.find(o => o.alias === 'description') ? item.properties.find(o => o.alias === 'description').value : null),
                    createDate: item.createDate || (item.properties.find(o => o.alias === 'createDate') ? item.properties.find(o => o.alias === 'createDate').value : null),
                    visitorType: item.visitorType || (item.properties.find(o => o.alias === 'visitorType') ? item.properties.find(o => o.alias === 'visitorType').value : null)
                };
            });
        } else {
            vm.items = [];
        }

        vm.visitorTypes = [];
        vm.loadingVisitorTypes = true;
        feedbackListHelper.getVisitorTypes().then(response => {
            vm.loadingVisitorTypes = false;
            vm.items = vm.items.map(item => {
                var visitorType = response.find(o => {
                    return o.Uid === item.visitorType.replace("umb://document/", "");
                });
                item.visitorType = visitorType ? visitorType.Name : null
                return item;
            });
        });

        vm.categories = [];
        vm.loadingCategories = true;
        feedbackListHelper.getCategories().then(response => {
            vm.loadingCategories = false;
            vm.items = vm.items.map(item => {
                var categories = {};
                if (typeof item.category === 'object') {
                    categories = item.category.value.split(',');
                } else if (typeof item.category === 'string') {
                    categories = item.category.split(',');
                } else {
                    return {};
                }
                
                item.category = '';
                for (var i = 0; i < categories.length; i++) {
                    var category = response.find(o => {
                        return o.Uid === categories[i].replace("umb://document/", "");
                    });
                    if (!category) continue;

                    item.category += (item.category === '' ? category.Name : ', ' + category.Name);
                }

                if (item.otherCategory !== null && item.otherCategory !== "") {
                    item.category = (item.category === null || item.category === "") ? item.otherCategory : item.category + ', ' + item.otherCategory;
                }

                return item;
            });
        });
    }
    mapItems();

    assetsService.load(['~/App_Plugins/FeedbackListView/datepicker.min.js']).then(function () {
        $('#export-start').datepicker({
            autoHide: true,
            format: 'dd/mm/yyyy'
        });
        $('#export-end').datepicker({
            autoHide: true,
            format: 'dd/mm/yyyy'
        });
        $('#export-start').on('pick.datepicker', function (e) {
            $('#export-end').datepicker('setStartDate', e.date);
        });

        $('#export-end').on('pick.datepicker', function (e) {
            $('#export-start').datepicker('setEndDate', e.date);
        });
    });

    vm.allowSelectAll = true;

    vm.selectItem = (selectedItem, index, $event) => {
        listViewHelper.selectHandler(selectedItem, index, vm.items, $scope.selection, $event);
        $event.stopPropagation();
    };

    vm.clickItem = (item) => {
        $location.path($scope.entityType + '/' + $scope.entityType + '/edit/' + item.id);
    }

    vm.selectAll = ($event) => {
        listViewHelper.selectAllItems(vm.items, $scope.selection, $event);
    }

    vm.isSelectedAll = () => {
        if ($scope.selection.length <= 0) {
            listViewHelper.clearSelection(vm.items, null, $scope.selection);
        }
        return listViewHelper.isSelectedAll(vm.items, $scope.selection);
    }

    vm.sort = (col, allow, param1) => {
        listViewHelper.setSorting(col, allow, $scope.options);
        $scope.options.orderBySystemField = false;
        var options = $scope.options;
        if (col === 'createDate') {
            $scope.options.orderBySystemField = true;
        }

        var filter = $('.form-control.search-input').val();
        feedbackListHelper.getItems(options.pageNumber, options.pageSize, col, options.orderDirection, options.orderBySystemField, filter).then(response => {
            $scope.items = response.items;
            mapItems();
        });
    }
}
angular.module("umbraco").controller("FeedbackListViewController", FeedbackListViewController);