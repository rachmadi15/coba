//var isChecking = false;
function CustomDashboardController($timeout, authResource, $http, umbRequestHelper) {
    var url = '/umbraco/backoffice/Api/SessionChecker/GetAuthStatus';
    
    var checkUser = function () {
        return umbRequestHelper.resourcePromise(
            $http.get(url), "Failed to get user session status."
        );
    }
    var checker = null;

    checker = function () {
        checkUser().then(function (res) {
            //if (!isChecking) {
                $timeout(checker, 5000);
            //    isChecking = true;
            //} else {
            //    isChecking = false;
            //}
            if (res) return;
            //20220730
            //authResource.performLogout();
        });
    }

    checker();
}

angular.module("umbraco").controller("CustomDashboardController", CustomDashboardController);