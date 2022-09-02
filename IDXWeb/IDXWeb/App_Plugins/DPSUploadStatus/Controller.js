function DPSUploadStatusController($scope, $timeout, editorState, contentResource) {
    var timeout = null;
    var content = null;

    var self = this;
    self.limiter = 0;
    self.refresh = function () {
        content = editorState.getCurrent();
        if (content.contentTypeAlias !== 'digitalStatisticUploaderItem' || content.id <= 0) {
            $timeout.cancel(timeout);
            return;
        }

        contentResource.getById(content.id)
            .then(resp => {
                content = resp;
                var properties = resp.properties;
                if (properties == null || properties.length <= 0) return;

                var status = properties.find(x => x.alias === 'status');
                if (status == null) return;

                $scope.model.value = status.value;
                self.limiter = 0;
            });
    }

    var checker = null;
    checker = function () {
        timeout = $timeout(function () {
            
            self.refresh();
            if (self.limiter > 0) return;
            checker();
            self.limiter++;
        }, 4000);
    }
    //checker();
    self.autoRefreshEnabled = false;
    self.startAutoRefresh = function () {
        self.autoRefreshEnabled = true;
        self.limiter = 0;
        checker();
    }

    self.stopAutoRefresh = function () {
        self.autoRefreshEnabled = false;
        self.limiter = 1;
    }

    var textarea = document.getElementById('status');
    $scope.$watch('model.value', function (newVal) {
        textarea.scrollTop = textarea.scrollHeight + 8;
    });
}

angular.module("umbraco").controller("DPSUploadStatusController", DPSUploadStatusController);