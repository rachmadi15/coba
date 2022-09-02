function FeedbackHelper($q, $http, umbRequestHelper) {
    return {
        getAll(start, end) {
            return umbRequestHelper.resourcePromise(
                $http.get("backoffice/Api/Feedback/GetAll?start=" + start + "&end=" + end), "Failed to retrieve all Feedback data."
            );
        },
        getStats(nid, start, end, mergeVT = false) {
            return umbRequestHelper.resourcePromise(
                $http.get("backoffice/Api/Feedback/GetStats?nid=" + nid + "&start=" + start + "&end=" + end + (mergeVT ? '&merge_vt=true' : '')), "Failed to retrieve all Feedback data."
            );
        },
        getVisitorTypes(currentNodeId) {
            return umbRequestHelper.resourcePromise(
                $http.get("backoffice/Api/Feedback/GetVisitorTypes?currentNodeId=" + currentNodeId), "Failed to retrieve all Feedback data."
            );
        }
    }
}
angular.module("umbraco.resources").factory("feedbackHelper", FeedbackHelper);

function chart() {
    return {
        restrict: 'E',
        template: '<div></div>',
        scope: {
            options: '='
        },
        link(scope, element) {
            Highcharts.chart(element[0], scope.options);
        }
    }
}

function updateChart(vm, nid, helper) {
    vm.statsLoading = true;
    helper.getStats(nid, vm.statsStart, vm.statsEnd).then(stats => {
        vm.timelineData = getFeedbackTimelineData(stats.List, vm.visitorTypes, moment(vm.statsStart, 'DD/MM/YYYY'), moment(vm.statsEnd, 'DD/MM/YYYY'));
        vm.overallRatingData = getOverallRatingData(stats);
        vm.statsLoading = false;
    });
}

angular.module("umbraco").directive("byCategory", chart);
angular.module("umbraco").directive("todaysFeedback", chart);
angular.module("umbraco").directive("timelineChart", chart);
angular.module("umbraco").directive("overallRating", chart);

function FeedbackDashboardController($scope, $routeParams, assetsService, editorState, feedbackHelper) {
    assetsService.loadCss('~/App_Plugins/FeedbackListView/datepicker.min.css');
    var vm = this;
    vm.title = "Feedback Stats";
    vm.todaysStatsLoading = true;
    vm.statsLoading = true;
    vm.timelineData = null;
    vm.todaysFeedbackData = null;
    vm.getVisitorTypes = [];
    vm.statsStart = moment().subtract(30, 'days').startOf('day').format('DD/MM/YYYY');
    vm.statsEnd = moment().add(31, 'days').endOf('day').format('DD/MM/YYYY');

    assetsService.load([
        '~/App_Plugins/FeedbackDashboard/js/moment-timezone.js',
        '~/App_Plugins/FeedbackListView/datepicker.min.js'
    ]).then(function () {
        // Get Today's feedback
        // Range: (today - 7 days) until tomorrow(12.00 AM)
        var todaysFeedbackStart = moment().subtract(7, 'd').format('DD/MM/YYYY');
        var todaysFeedbackEnd = moment().format('DD/MM/YYYY');
        feedbackHelper.getStats($routeParams.id, todaysFeedbackStart, todaysFeedbackEnd, true).then(stats => {
            vm.todaysFeedbackData = getTodaysFeedbackData(stats, todaysFeedbackStart);
            vm.todaysStatsLoading = false;
        });

        // Get feedback for a specified date range
        // Default: (today - 30 days) until (today + 30 days)

        feedbackHelper.getVisitorTypes(editorState.current.id).then(visitorTypes => {
            vm.visitorTypes = visitorTypes;

            updateChart(vm, $routeParams.id, feedbackHelper);
    
            $('#stats-start').datepicker({
                autoHide: true,
                autoPick: true,
                date: moment(vm.statsStart, 'DD/MM/YYYY'),
                endDate: moment(vm.statsEnd, 'DD/MM/YYYY').add(1, 'd'),
                format: 'dd/mm/yyyy'
            });
            $('#stats-end').datepicker({
                autoHide: true,
                autoPick: true,
                date: moment(vm.statsEnd, 'DD/MM/YYYY'),
                startDate: moment(vm.statsStart, 'DD/MM/YYYY').subtract(1, 'd'),
                format: 'dd/mm/yyyy'
            });
            $('#stats-start').on('pick.datepicker', function (e) {
                $('#stats-end').datepicker('setStartDate', e.date);
                var newVal = moment(e.date);
                vm.statsStart = newVal.format('DD/MM/YYYY');
                updateChart(vm, $routeParams.id, feedbackHelper);
            });

            $('#stats-end').on('pick.datepicker', function (e) {
                $('#stats-start').datepicker('setEndDate', e.date);
                var newVal = moment(e.date);
                vm.statsEnd = newVal.format('DD/MM/YYYY');
                updateChart(vm, $routeParams.id, feedbackHelper);
            });
        });
    });
}

angular.module("umbraco").controller("FeedbackDashboardController", FeedbackDashboardController);