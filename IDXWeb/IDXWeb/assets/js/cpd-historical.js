
        function getNH() {
            var emitenCode = getParams('kodeEmiten');
            var tableName = '#NHTable';
            var selectedYear = $('#hYearFilter option:selected').val();
            var url = '/umbraco/Surface/ListedCompany/GetPerubahanNamaHistory?KodeEmiten=' + emitenCode + '&Year=' + selectedYear;
            if ($.fn.DataTable.isDataTable(tableName)) {
                $(tableName).DataTable().ajax.url(url).load();
            } else {
                $(tableName).dataTable({
                    'ajax': {
                        'url': url,
                        'dataSrc' : function(obj) {
                            if(!obj.results || !obj.results.data) {
                              return [];
                            }
                            return obj.results.data;
                        }
                    },
                    'filter': false,
                    'sort': true,
                    'processing': true,
                    'order': [[0, "desc"]],
                    'paging': true,
                    'deferRender': true,
                    'columns': [
                        { 'data': 'TglEfektif' },
                        { 'data': 'NamaEmiten' },
                        { 'data': 'NamaEmitenBaru' }
                    ],
                    'columnDefs': [
                        {
                            'targets': [0],
                            'createdCell': function (td, cellData, rowData, row, col) {
                                return $(td).addClass('text-center');
                            },
                            'render': function (data, type, row, meta) {
                                if(data != null){
                                    return moment(data).locale(currentLang).format('DD-MMM-YYYY');
                                }
                                return data;
                            }
                        },
                        {
                            'targets': [1, 2],
                            'createdCell': function (td, cellData, rowData, row, col) {
                                return $(td).addClass('text-center');
                            }
                        },
                    ]
                });
            }
        }
        function getDH() {
            var emitenCode = getParams('kodeEmiten');
            var tableName = '#DHTable'
            var selectedYear = $('#dhYearFilter option:selected').val();
            var url = '/umbraco/Surface/ListedCompany/GetDividenTunai?KodeEmiten=' + emitenCode + '&Year=' + selectedYear;
            if ($.fn.DataTable.isDataTable(tableName)) {
                $(tableName).DataTable().ajax.url(url).load();
            } else {
                $(tableName).dataTable({
                    'ajax': {
                        'url': url,
                        'dataSrc' : function(obj) {
                            if(!obj.results || !obj.results.data) {
                              return [];
                            }
                            return obj.results.data;
                        }
                    },
                    'filter': false,
                    'sort': true,
                    'order': [[1, "desc"]],
                    'paging': true,
                    'deferRender': true,
                    'columns': [
                        { 'data': 'Nama' },
                        { 'data': 'Periode' },
                        { 'data': 'Jenis'},
                        { 'data': 'MataUang' },
                        { 'data': 'Total' },
                        { 'data': 'TglCumReg' },
                        { 'data': 'TglCumTunai' },
                        { 'data': 'TglExReg' },
                        { 'data': 'TglExTunai' },
                        { 'data': 'TglDps' },
                        { 'data': 'TglPembayaran' }
                    ],
                    'columnDefs': [
                        {
                            'targets': [5, 6, 7, 8, 9, 10],
                            'createdCell': function (td, cellData, rowData, row, col) {
                                return $(td).addClass('text-center');
                            },
                            'render': function (data, type, row, meta) {
                                if (data != null) {
                                    return moment(data).locale(currentLang).format('DD-MMM-YYYY');
                                }
                                return data;
                            }
                        },
                        {
                            'targets': [1, 2, 3],
                            'createdCell': function (td, cellData, rowData, row, col) {
                                return $(td).addClass('text-center');
                            },
                            'render': function (data, type, row, meta) {
                                return data;
                            }
                        },
                        {
                            'targets': [0],
                            'createdCell': function (td, cellData, rowData, row, col) {
                                return $(td).addClass('text-center');
                            },
                            'render': function (data, type, row, meta) {
                                return $('#emitenName').text();
                            }
                        },
                        {
                            'targets': [4],
                            'createdCell': function (td, cellData, rowData, row, col) {
                                return $(td).addClass('text-center');
                            },
                            'render': function (data, type, row, meta) {
                                if (data != null) {
                                    return data.toLocaleString(currentLang);
                                }
                                return data;
                            }
                        }
                    ]
                });
            }
        }
        function getDH2() {
            var emitenCode = getParams('kodeEmiten');
            var tableName = '#DH2Table'
            var selectedYear = $('#dhYearFilter option:selected').val();
            var url = '/umbraco/Surface/ListedCompany/GetDividenSaham?KodeEmiten=' + emitenCode + '&Year=' + selectedYear;
            if ($.fn.DataTable.isDataTable(tableName)) {
                $(tableName).DataTable().ajax.url(url).load();
            } else {
                $(tableName).dataTable({
                    'ajax': {
                        'url': url,
                        'dataSrc' : function(obj) {
                            if(!obj.results || !obj.results.data) {
                              return [];
                            }
                            return obj.results.data;
                        }
                    },
                    'filter': false,
                    'sort': true,
                    'processing': true,
                    'order': [[1, "desc"]],
                    'paging': true,
                    'deferRender': true,
                    'columns': [
                        { 'data': 'Nama' },
                        { 'data': 'Periode' },
                        { 'data': 'Jenis' },
                        { 'data': 'Rasio'},
                        { 'data': 'Total' },
                        { 'data': 'TglCumReg' },
                        { 'data': 'TglCumTunai' },
                        { 'data': 'TglExReg' },
                        { 'data': 'TglExTunai' },
                        { 'data': 'TglDps' },
                        { 'data': 'TglPembagian' }
                    ],
                    'columnDefs': [
                        {
                            'targets': [5, 6, 7, 8, 9, 10],
                            'createdCell': function (td, cellData, rowData, row, col) {
                                return $(td).addClass('text-center');
                            },
                            'render': function (data, type, row, meta) {
                                if (data != null) {
                                    return moment(data).locale(currentLang).format('DD-MMM-YYYY');
                                }
                                return data;
                            }
                        },
                        {
                            'targets': [1, 2, 3],
                            'createdCell': function (td, cellData, rowData, row, col) {
                                return $(td).addClass('text-center');
                            }
                        },
                        {
                            'targets': [0],
                            'createdCell': function (td, cellData, rowData, row, col) {
                                return $(td).addClass('text-center');
                            },
                            'render': function (data, type, row, meta) {
                                return $('#emitenName').text();
                            }
                        },
                        {
                            'targets': [4],
                            'createdCell': function (td, cellData, rowData, row, col) {
                                return $(td).addClass('text-center');
                            },
                            'render': function (data, type, row, meta) {
                                if (data != null) {
                                    return data.toLocaleString(currentLang);
                                }
                                return data;
                            }
                        },
                    ]
                });
            }
        }
        function getCBH() {
            var emitenCode = getParams('kodeEmiten');
            var tableName = '#CBHTable'
            var selectedYear = $('#hYearFilter option:selected').val();
            var url = '/umbraco/Surface/ListedCompany/GetCoreBusinessHistory?KodeEmiten=' + emitenCode + '&Year=' + selectedYear;
            if ($.fn.DataTable.isDataTable(tableName)) {
                $(tableName).DataTable().ajax.url(url).load();
            } else {
                $(tableName).dataTable({
                    'ajax': {
                        'url': url,
                        'dataSrc' : function(obj) {
                            if(!obj.results || !obj.results.data) {
                              return [];
                            }
                            return obj.results.data;
                        }
                    },
                    'filter': false,
                    'sort': true,
                    'processing': true,
                    'order': [[0, "desc"]],
                    'paging': true,
                    'deferRender': true,
                    'columns': [
                        { 'data': 'TglRups' },
                        { 'data': 'KegiatanSemula' },
                        { 'data': 'KegiatanBaru' }
                    ],
                    'columnDefs': [
                        {
                            'targets': [0],
                            'createdCell': function (td, cellData, rowData, row, col) {
                                return $(td).addClass('text-center');
                            },
                            'render': function (data, type, row, meta) {
                                if (data != null) {
                                    return moment(data).locale(currentLang).format('DD-MMM-YYYY');
                                }
                                return data;
                            }
                        },
                        {
                            'targets': [1, 2],
                            'createdCell': function (td, cellData, rowData, row, col) {
                                return $(td).addClass('text-center');
                            }
                        },
                    ]
                });
            }
        }

        function getSHHP() {
            var emitenCode = getParams('kodeEmiten');
            var tableName = '#SHHPTable';
            var selectedYear = $('#hYearFilter option:selected').val();
            var url = '/umbraco/Surface/ListedCompany/GetPemegangSahamPengendaliHistory?KodeEmiten=' + emitenCode + '&year=' + selectedYear;
            if ($.fn.DataTable.isDataTable(tableName)) {
                $(tableName).DataTable().ajax.url(url).load();
            } else {
                $(tableName).dataTable({
                    'ajax': {
                        'url': url,
                        'dataSrc' : function(obj) {
                            if(!obj.results || !obj.results.data) {
                              return [];
                            }
                            return obj.results.data;
                        }
                    },
                    'filter': false,
                    'sort': true,
                    'processing': true,
                    'order': [[0, "desc"]],
                    'paging': true,
                    'deferRender': true,
                    'columns': [
                        { 'data': 'Periode'},
                        { 'data': 'Nama' },
                        { 'data': 'Status' },
                        { 'data': 'Jumlah' },
                        { 'data': 'Persentase' }
                    ],
                    'columnDefs': [
                        {
                            'targets': [1, 2, 3, 4],
                            'createdCell': function (td, cellData, rowData, row, col) {
                                return $(td).addClass('text-center');
                            },
                            'render': function (data, type, row, meta) {
                                if (data != null) {
                                    return data.toLocaleString(currentLang);
                                }
                                return data;
                            }
                        },
                        {
                            'targets': [0],
                            'createdCell': function (td, cellData, rowData, row, col) {
                                return $(td).addClass('text-center');
                            },
                            'render': function (data, type, row, meta) {
                                if (data != null) {
                                    return moment(data).locale(currentLang).format('DD-MMM-YYYY');
                                }
                                return data;
                            }
                        },
                    ]
                });
            }
        }
        function getSHH() {
            var emitenCode = getParams('kodeEmiten');
            var tableName = '#SHHTable';
            var selectedYear = $('#hYearFilter option:selected').val();
            var url = '/umbraco/Surface/ListedCompany/GetPemegangSahamHistory?KodeEmiten=' + emitenCode + '&year=' + selectedYear;
            if ($.fn.DataTable.isDataTable(tableName)) {
                $(tableName).DataTable().ajax.url(url).load();
            } else {
                $(tableName).dataTable({
                    'ajax': {
                        'url': url,
                        'dataSrc' : function(obj) {
                            if(!obj.results || !obj.results.data) {
                              return [];
                            }
                            return obj.results.data;
                        }
                    },
                    'filter': false,
                    'sort': true,
                    'processing': true,
                    'order': [[0, "desc"]],
                    'paging': true,
                    'deferRender': true,
                    'columns': [
                        { 'data': 'Periode' },
                        { 'data': 'Nama' },
                        { 'data': 'Jumlah' },
                        { 'data': 'Persentase' }
                    ],
                    'columnDefs': [
                        {
                            'targets': [1, 2, 3],
                            'createdCell': function (td, cellData, rowData, row, col) {
                                return $(td).addClass('text-center');
                            },
                            'render': function (data, type, row, meta) {
                                if (data != null) {
                                    return data.toLocaleString(currentLang);
                                }
                                return data;
                            }
                        },
                        {
                            'targets': [0],
                            'createdCell': function (td, cellData, rowData, row, col) {
                                return $(td).addClass('text-center');
                            },
                            'render': function (data, type, row, meta) {
                                if (data != null) {
                                    return moment(data).locale(currentLang).format('DD-MMM-YYYY');
                                }
                                return data;
                            }
                        },
                    ]
                });
            }
         }