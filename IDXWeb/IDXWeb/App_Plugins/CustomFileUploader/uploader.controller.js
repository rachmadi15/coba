angular.module("umbraco")
    .controller('CustomFileUploadController', ['$scope', 'assetsService', 'Upload', '$http', '$timeout', function ($scope, assetsService, Upload, $http, $timeout) {
        assetsService
            .load([
                "~/Umbraco/lib/ng-file-upload/ng-file-upload.min.js"
            ])
            .then(function () {
                $scope.model.acceptExt = "*";
                if ($scope.model.config.acceptExt !== null || $scope.model.config.acceptExt !== "") {
                    $scope.model.acceptExt = $scope.model.config.acceptExt;
                }

                $scope.uploadPic = function(file) {
                    file.upload = Upload.upload({
                        url: $scope.model.config.uploadUrl+"/api/upload",
                        file: file
                    });

                    file.upload.then(function (response) {
                        $timeout(function () {
                            $scope.model.value = $scope.model.config.uploadUrl+"/"+response.data;
                        });
                    });
                    file.upload.progress(function(evt) {
                        file.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
                    })
                    return false;
                }
            });
    }]);