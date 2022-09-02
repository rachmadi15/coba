$(function () {
    var _extends = function (target) {
        for (var i = 0; i < arguments.length; i++) {
            var source = arguments[i];
            for (var key in source) {
                if (typeof source[key] === 'function') {
                    source[key]();
                }
            }
        }
    };
    var isErrorHandled = false;
    
    var $loader = $('.table-loading');

    var fetchFailedAlert = function (err) {
        $loader.fadeOut();
        if (isErrorHandled) return;
        var en = document.URL.indexOf("/en-us/") >= 0 || document.URL.indexOf("/en/") >= 0;
        var message = 'Failed to load statistic data. Please try again later.';
        if (!en) {
            message = 'Gagal mengambil beberapa data statistik. Cobalah beberapa saat lagi.';
        }
        alert(message);
        isErrorHandled = true;
    }

    function consoleLog(e) {
        if (typeof e === 'undefined') return;
        console.log(e);
    }

    function exist(selector) {
        return new Promise(function (resolve, reject) {
            if ($(selector).length > 0) {
                resolve(selector);
            }
            reject();
            // reject('No element found for ' + selector);
        });
    }

    var datatables = {
        datatable: function () {
            exist('.js-datatable').then(function (res) {
                isErrorHandled = false;
                var $dataTable = $(res);

                var formatData = function formatData(data, colInfo, row) {
                    if (colInfo.format === 'timeFrom' && row.listingDate !== row.tradingDate) return 'T:';
                    if (colInfo.format === 'timeTo' && row.listingDate !== row.tradingDate) return '-';

                    var date = new Date(data);

                    
                    if (colInfo.columnName === null) { // empty cell
                        return '';
                    } else if (colInfo.format === 'yesNo') { // Yes/No cell
                        return !data ? 'No' : 'Yes';
                    } else if (colInfo.format === 'checkCross') {
                        return data ? '<span class="fa fa-check"></span>' : data === false ? '<span class="fa fa-times"></span>' : '';
                    } else if (typeof data === 'undefined' || data === null) { // null value
                        return '-';
                    } else if (!colInfo.format) { // no format (string)
                        return data;
                    } else if (colInfo.format === 'fullDate') { // full date format (1-Jan-19)
                        return date.getDate() + '-' + date.toLocaleDateString('en', { month: 'short' }) + '-' + date.toLocaleDateString('en', { year: '2-digit' });
                    } else if (colInfo.format === 'fullDateYear') { // full date format (1-Jan-2019)
                        return date.getDate() + '-' + date.toLocaleDateString('en', { month: 'short' }) + '-' + date.toLocaleDateString('en', { year: 'numeric' });
                    } else if (colInfo.format === 'fullDateDigit') { // digit date format (1/1/2019)
                        return date.getDate() + '/' + date.toLocaleDateString('en', { month: 'numeric' }) + '/' + date.toLocaleDateString('en', { year: 'numeric' });
                    } else if (colInfo.format === 'dayMonth') { // day-month format (1-Jan)
                        return date.getDate() + '-' + date.toLocaleDateString('en', { month: 'short' });
                    } else if (colInfo.format === 'monthYear') { // month-year format (Jan-19)
                        return date.toLocaleDateString('en', { month: 'short' }) + '-' + date.toLocaleDateString('en', { year: '2-digit' });
                    } else if (colInfo.format === 'monthDay') { // month day format (Jan 02)
                        return date.toLocaleDateString('en', { month: 'short' }) + ' ' + date.toLocaleDateString('en', { day: '2-digit' });
                    } else if (colInfo.format === 'month') { // month only format (Jan)
                        date = new Date(1970, data);
                        return date.toLocaleDateString('en', { month: 'short' });
                    } else if (colInfo.format === 'nesting') {
                        return colInfo.nesting.map(function (nested) {
                            return formatData(row[nested.columnName], nested, row);
                        }).join('<br>');
                    } else if (colInfo.format !== 'percent' && data == 0) {
                        return '-';
                    }

                    // numeric format (123,456.78) / percent format (0.23%)
                    return (colInfo.format === 'plusMinus' && data > 0 ? '+' : '') + data.toLocaleString('en', {
                        minimumFractionDigits: colInfo.decimal,
                        maximumFractionDigits: colInfo.decimal
                    }) + (colInfo.format === 'percent' ? '%' : '');
                };

                var rowTemplate = function (rowData, colInfo) {
                    var children = rowData.map(function (data) {
                        var cells = colInfo.map(function (col, index) {
                            if (data[col.columnName] === undefined && col.format !== 'timeFrom' && col.format !== 'timeTo') return '<td></td>';

                            var className = col.className;
                            if (data.months != null && data.months.length > 0) {
                                className += " text-bold parent-row is-closed";
                            }
                            var resData = formatData(data[col.columnName], col, data);
                            return '<td class="' + className + '">' + resData + '</td>';
                        }).join('');

                        var row = '<tr>' + cells + '</tr>';

                        return $(row).toArray();
                    });

                    return children;
                };

                $.each($dataTable, function (index, table) {
                    var $table = $(table);
                    var src = $table.data('json');
                    var unorderable = $table.children('thead').data('unorderable');
                    var targets = !unorderable ? [] : JSON.parse('[' + unorderable + ']');
                    $.ajax({
                        url: src,
                        method: 'GET',
                        success: function (res) {
                            var columns = res.columns.map(function (col) {
                                return {
                                    data: col.columnName,
                                    defaultContent: "",
                                    visible: !(col.format === 'grouping' || col.format === 'subGrouping'),
                                    render: function render(data, type, row, meta) {
                                        if (type === 'sort' || type === "type") {
                                            var dateFormats = ['fullDate', 'fullDateYear', 'fullDateDigit', 'dayMonth', 'monthYear', 'monthDay'];

                                            if (dateFormats.indexOf(col.format) > -1) return new Date(data).getTime();
                                            if (col.format === "month" || col.format === 'plusMinus' || col.format === true) return data;
                                            if (col.format === "closeFormat") return data.value;
                                            if (col.format === "count") return meta.row + 1;
                                        }
                                        if (col.format === "count") return meta.row + 1 + '.';
                                        if (col.format === "closeFormat") return !!data.value ? data.value.toLocaleString() : '-';
                                        if (row.hasDecimal > 0 && col.format) return data.toLocaleString('en', {
                                            minimumFractionDigits: row.hasDecimal,
                                            maximumFractionDigits: row.hasDecimal
                                        });

                                        return formatData(data, col, row);
                                    },
                                    createdCell: function createdCell(td, cellData, rowData) {
                                        $(td).addClass(col.className);

                                        if (col.format === "closeFormat" && cellData.status) {
                                            if (cellData.status === 'increase') {
                                                $(td).addClass('green');
                                            } else if (cellData.status === 'decrease') {
                                                $(td).addClass('red');
                                            }
                                        }

                                        if (rowData.mostCloseValue && rowData.mostCloseValue.indexOf(col.columnName) > -1) {
                                            if (col.columnName.indexOf('gain') === 0) $(td).removeClass('bg-increased').addClass('background-green white');
                                            if (col.columnName.indexOf('loss') === 0) $(td).removeClass('bg-decreased').addClass('background-red white');
                                        }

                                        if ($(td).hasClass('tooltip')) {
                                            var type = col.columnName.substr(0, col.columnName.search('Percent'));
                                            var template = '\n                                    <span class="tooltip-data">\n                                        ' + cellData + '%\n                                        <br>\n                                        ' + rowData[type].toLocaleString() + '\n                                    </span>\n                                ';
                                            $(td).append(template);
                                        }
                                    }
                                };
                            });

                            var datatable = $table.DataTable({
                                "language": {
                                    "paginate": {
                                        "previous": '<span class="fa fa-angle-double-left"></span>',
                                        "next": '<span class="fa fa-angle-double-right"></span>'
                                    }
                                },
                                searching: true,
                                info: !$table.hasClass('no-paging'),
                                ordering: !$table.hasClass('no-ordering'),
                                paging: !$table.hasClass('no-paging'),
                                pagingType: 'first_last_numbers',
                                lengthChange: !$table.hasClass('no-paging'),
                                scrollX: false,
                                scrollY: $table.hasClass('vertical-scroll') ? '350px' : '',
                                dom: 'l<"table-wrapper"t>ip',
                                scrollCollapse: true,
                                order: [],
                                columns: columns,
                                data: res.data,
                                columnDefs: [
                                    { targets: targets, orderable: false },
                                    { targets: '_all', orderable: true }
                                ],
                                createdRow: function createdRow(row, data, dataIndex, cells) {
                                    if (data.childRows && data.childRows.length) {
                                        this.api().row(row).child(rowTemplate(data.childRows, res.columns)).show();
                                    }
                                },
                                initComplete: function initComplete(settings, json) {
                                    if (!$table.hasClass('no-paging')) {
                                        var paginate = '#' + $(this).attr('id') + '_paginate';
                                        $(paginate).insertAfter($(this).closest('.dataTables_wrapper'));
                                    }

                                    $loader.fadeOut();
                                },
                                footerCallback: function footerCallback(row, data, start, end, display) {
                                    var api = this.api();
                                    if (res.footer != undefined && res.footer.length > 1) {
                                        if ($(this).children("tfoot").length <= 0) {
                                            $(this).append("<tfoot></tfoot>")
                                            var footerTable = $(this).children("tfoot")
                                            var footerHtml = ""
                                            res.footer.forEach((item, index) => {
                                                footerHtml += "<tr>"
                                                res.columns.forEach((col, i) => {
                                                    if (col.columnName != "group") {
                                                        var check = item[col.columnName] == undefined || item[col.columnName] == null || item[col.columnName] == 0
                                                        var val = check || item[col.columnName] == "0001-01-01T00:00:00" ? "" : item[col.columnName]
                                                        footerHtml += "<td class='no-border " + col.className + "'>" + val.toLocaleString() + "</td>"
                                                    }
                                                })
                                                footerHtml += "</tr>"
                                            })
                                            footerTable.html(footerHtml)
                                        }
                                    }
                                    else {
                                        api.columns('.sum').every(function (col) {
                                            var sum = this.data().reduce(function (a, b) {
                                                var x = parseFloat(a) || 0;
                                                var y = parseFloat(b) || 0;
                                                return x + y;
                                            }, 0);


                                            $('tr:eq(0) td:eq(' + col + ')', api.table().footer()).html(sum.toLocaleString());
                                            // $(`tr:eq(1) td:eq(${col})`, api.table().footer()).html(sum.toLocaleString())
                                        });
                                    }
                                },
                                drawCallback: function drawCallback(settings) {
                                    var api = this.api();
                                    var rows = api.rows().nodes();
                                    var lastGroup = null;
                                    var lastSubGroup = null;
                                    var counter = 1;
                                    var groupColumn = res.columns.findIndex(function (col) {
                                        return col.format === 'grouping';
                                    });
                                    var subGroupColumn = res.columns.findIndex(function (col) {
                                        return col.format === 'subGrouping';
                                    });
                                    var countColumn = res.columns.findIndex(function (col) {
                                        return col.format === 'count';
                                    });
                                    var colCount = api.columns(':visible').count();

                                    if (groupColumn > -1) {
                                        if (subGroupColumn === -1) {
                                            api.column(groupColumn).data().each(function (group, i) {
                                                if (lastGroup !== group) {
                                                    $(rows).eq(i).before('<tr class="sub-head text-left text-bold"><td colspan="' + colCount + '">' + group + '</td></tr>');

                                                    lastGroup = group;
                                                    counter = 1;
                                                }

                                                api.cells(i, countColumn).nodes().to$().html(counter + '.');
                                                counter++;
                                            });
                                        } else {
                                            api.rows().every(function (rowIdx, tableLoop, rowLoop) {
                                                var group = this.data()['group'];
                                                var subGroup = this.data()['subGroup'];

                                                if (lastGroup !== group) {
                                                    $(rows).eq(rowIdx).before((lastGroup === null ? '' : '<tr><td class="no-border"></td></tr>') + '\n                                                <tr class="sub-head text-left text-bold"><td colspan="' + colCount + '">' + group + '</td></tr>');

                                                    lastGroup = group;
                                                    lastSubGroup = null;
                                                }

                                                if (lastSubGroup !== subGroup) {
                                                    $(rows).eq(rowIdx).before((lastSubGroup === null ? '' : '<tr><td class="no-border"></td></tr>') + '\n                                                <tr class="text-left text-bold"><td class="short no-border" colspan="' + colCount + '">' + subGroup + '</td></tr>');

                                                    lastSubGroup = subGroup;
                                                }
                                            });
                                        }
                                    }
                                }
                            });
                        },
                        error: fetchFailedAlert
                    });
                });
            }).catch(consoleLog);
        },
        collapsibleDatatable: function () {
            exist('.js-datatable-collapse').then(function (res) {
                isErrorHandled = false;
                var $dataTable = $(res);

                $.each($dataTable, function (index, table) {
                    var $table = $(table);
                    var src = $table.data('json');
                    //var arrowPosition = $table.children('thead').data('arrow-position');
                    var arrowPosition = 0;
                    $.ajax({
                        url: src,
                        method: 'GET',
                        success: function (res) {
                            collapsibleResults = res;
                            var columns = collapsibleResults.columns.map(function (col) {
                                return {
                                    data: col.columnName,
                                    className: col.className,
                                    defaultContent: "",
                                    render: function render(data, type, row, meta) {
                                        if (col.columnName === null) return;
                                        var asterisk = row.asterisk ? '*' : '';
                                        var resData = (typeof data === 'undefined' || data === null) ? '-' : !col.format ? data + asterisk : data;
                                        if (!col.format) return resData;
                                        if (col.format !== 'percent' && resData == 0 && resData !== " ") {
                                            resData = '-';
                                        }
                                        else if (col.format !== 'year' && resData != " ") {
                                            resData = resData.toLocaleString('en', {
                                                minimumFractionDigits: col.decimal,
                                                maximumFractionDigits: col.decimal
                                            });
                                        }

                                        return resData;
                                    }
                                };
                            });

                            var datatable = $table.DataTable({
                                "language": {
                                    "paginate": {
                                        "previous": '<span class="fa fa-angle-double-left"></span>',
                                        "next": '<span class="fa fa-angle-double-right"></span>'
                                    }
                                },
                                data: collapsibleResults.data,
                                searching: true,
                                info: false,
                                ordering: false,
                                paging: false,
                                lengthChange: false,
                                scrollX: false,
                                dom: 'l<"table-wrapper"t>i',
                                initComplete: function initComplete(settings, json) {
                                    $loader.fadeOut();
                                },
                                order: [],
                                columns: columns,
                                createdRow: function createdRow(row, data, dataIndex) {
                                    if (typeof data.months !== 'undefined' && data.months !== null && data.months.length > 0) {
                                        $(row).off("click", "**").on('click', function (e) {
                                            var clickedName = $(e.currentTarget).data("name");
                                            var segments = clickedName.toString().split('.');
                                            if (segments.length > 1 && collapsibleResults.NameRow !== 'MasterIndex') {
                                                var clickedCode = segments[0];
                                                addParentRowClickListener(datatable, collapsibleResults, clickedCode);
                                            } else {
                                                var dtRow = datatable.row(row);
                                                if (dtRow.child.isShown()) {
                                                    dtRow.child.hide();
                                                } else {
                                                    datatable.rows('.parent-row').nodes().to$().not(this).map(function (id, tr) {
                                                        if (datatable.row(tr).child.isShown()) datatable.row(tr).child.hide();
                                                    });
                                                    var newRows = collapsibleRowTemplate(data.months, res.columns);
                                                    dtRow.child(newRows).show();
                                                }
                                            }
                                        });
                                    }

                                    $(row).addClass('text-bold parent-row is-closed').attr('data-name', data.Name);
                                    if (arrowPosition !== undefined) {
                                        $(row).children().eq(arrowPosition).addClass('arrow-indicator');
                                    }
                                },
                                footerCallback: function footerCallback(row, data, start, end, display) {
                                    var api = this.api();

                                    if (collapsibleResults.footer != undefined && collapsibleResults.footer.length > 1) {
                                        if ($(this).children("tfoot").length <= 0) {
                                            $(this).append("<tfoot></tfoot>")
                                            var footerTable = $(this).children("tfoot")
                                            var footerHtml = ""
                                            collapsibleResults.footer.forEach((item, index) => {
                                                footerHtml += "<tr>"
                                                collapsibleResults.columns.forEach((col, i) => {
                                                    if (col.columnName != "group") {
                                                        var check = item[col.columnName] == undefined || item[col.columnName] == null || item[col.columnName] == 0
                                                        var val = check || item[col.columnName] == "0001-01-01T00:00:00" ? "" : item[col.columnName]
                                                        footerHtml += "<td class='no-border " + col.className + "'>" + val.toLocaleString() + "</td>"
                                                    }
                                                })
                                                footerHtml += "</tr>"
                                            })
                                            footerTable.html(footerHtml)
                                        }


                                    }
                                    else {
                                        api.columns('.sum').every(function (col) {
                                            var sum = this.data().reduce(function (a, b) {
                                                var x = parseFloat(a) || 0;
                                                var y = parseFloat(b) || 0;
                                                return x + y;
                                            }, 0);


                                            $('tr:eq(0) td:eq(' + col + ')', api.table().footer()).html(sum.toLocaleString());
                                            // $(`tr:eq(1) td:eq(${col})`, api.table().footer()).html(sum.toLocaleString())
                                        });
                                    }
                                },
                            });
                        },
                        error: fetchFailedAlert,
                        complete: function () { $loader.fadeOut(); }
                    });
                });
            }).catch(consoleLog);
        },
        idxIndicesHighlightsDatatable: function () {
            exist('.js-datatable-indices').then(function (res) {
                isErrorHandled = false;
                var $dataTable = $(res);

                var formatNum = function (data) {
                    return data.toLocaleString();
                };
                var formatPercent = function (data) {
                    return data.toLocaleString({ minimumFractionDigits: 2, maximumFractionDigits: 2 }) + '%';
                };
                var formatDate = function (data) {
                    var date = new Date(data);
                    return date.toLocaleDateString('en', { month: 'short' }) + ' ' + date.toLocaleDateString('en', { day: '2-digit' });
                };
                var renderData = function (data) {
                    return formatNum(data.val) + '<br>' + formatDate(data.date);
                };
                var renderChangeValue = function (data) {
                    var val = formatNum(data.val);
                    var percent = formatPercent(data.percent);

                    if (data.status.indexOf('decrease') === 0) return '(' + val + ')<br>(' + percent + ')';
                    return val + '<br>' + percent;
                };
                var renderChangeRank = function (data) {
                    var icon = data.status.indexOf('decrease') === 0 ? 'caret-down fa-lg' : data.status.indexOf('increase') === 0 ? 'caret-up fa-lg' : 'minus';
                    return '<i class="fa fa-' + icon + '"></i><br>' + data.rank;
                };
                var renderClass = function (td, cellData, rowData) {
                    var classes = '';
                    if (cellData.status.indexOf('decrease') >= 0) {
                        classes = 'red';
                    } else if (cellData.status.indexOf('increase') >= 0) {
                        classes = 'green';
                    }

                    if (cellData.status.indexOf('increase-decreaseMost') >= 0) {
                        classes = 'green bg-decreased';
                    } else if (cellData.status.indexOf('decrease-increaseMost') >= 0) {
                        classes = 'red bg-increased';
                    }else if (cellData.status.indexOf('decreaseMost') >= 0) {
                        classes += (classes === '' ? 'red ' : ' ') + 'bg-decreased';
                    } else if (cellData.status.indexOf('increaseMost') >= 0) {
                        classes += (classes === '' ? 'green ' : ' ') + 'bg-increased';
                    }
                    $(td).addClass(classes);
                };

                $.each($dataTable, function (index, table) {
                    var $table = $(table);
                    var src = $table.data('json');
                    src = setQueryString(src, 'filteredType', $('#filteredType').val());
                    $.ajax({
                        url: src,
                        method: 'GET',
                        success: function (res) {
                            var datatable = $table.DataTable({
                                "language": {
                                    "paginate": {
                                        "first": "First page",
                                        "previous": '<span class="fa fa-angle-double-left"></span>',
                                        "next": '<span class="fa fa-angle-double-right"></span>',
                                        "last": "Last page"
                                    }
                                },
                                data: res.data,
                                searching: true,
                                info: !$table.hasClass('no-paging'),
                                ordering: !$table.hasClass('no-ordering'),
                                paging: !$table.hasClass('no-paging'),
                                pagingType: 'first_last_numbers',
                                lengthChange: !$table.hasClass('no-paging'),
                                scrollX: false,
                                dom: 'l<"table-wrapper"t>ip',
                                //dom: "fltip",
                                order: [],
                                initComplete: function initComplete(settings, json) {
                                    $loader.fadeOut();
                                },
                                columns: [{
                                    className: 'text-center',
                                    defaultContent: '',
                                    render: function render(data, type, row, meta) {
                                        return meta.row + 1;
                                    }
                                }, {
                                    data: 'indexName',
                                    createdCell: function createdCell(td) {
                                        $(td).addClass('text-left wide');
                                    }
                                }, {
                                    data: 'high',
                                    render: renderData
                                }, {
                                    data: 'low',
                                    render: renderData
                                }, {
                                    data: 'close',
                                    render: renderData
                                }, {
                                    data: 'month1',
                                    defaultContent: '',
                                    render: renderChangeValue,
                                    createdCell: renderClass
                                }, {
                                    data: 'month1',
                                    className: 'text-center',
                                    defaultContent: '',
                                    render: renderChangeRank,
                                    createdCell: renderClass
                                }, {
                                    data: 'month3',
                                    defaultContent: '',
                                    render: renderChangeValue,
                                    createdCell: renderClass
                                }, {
                                    data: 'month3',
                                    className: 'text-center',
                                    defaultContent: '',
                                    render: renderChangeRank,
                                    createdCell: renderClass
                                }, {
                                    data: 'month6',
                                    defaultContent: '',
                                    render: renderChangeValue,
                                    createdCell: renderClass
                                }, {
                                    data: 'month6',
                                    className: 'text-center',
                                    defaultContent: '',
                                    render: renderChangeRank,
                                    createdCell: renderClass
                                }, {
                                    data: 'year1',
                                    defaultContent: '',
                                    render: renderChangeValue,
                                    createdCell: renderClass
                                }, {
                                    data: 'year1',
                                    className: 'text-center',
                                    defaultContent: '',
                                    render: renderChangeRank,
                                    createdCell: renderClass
                                }, {
                                    data: 'ytd',
                                    defaultContent: '',
                                    render: renderChangeValue,
                                    createdCell: renderClass
                                }, {
                                    data: 'ytd',
                                    className: 'text-center',
                                    defaultContent: '',
                                    render: renderChangeRank,
                                    createdCell: renderClass
                                }]
                            });
                        },
                        error: fetchFailedAlert,
                        complete: function () { $loader.fadeOut(); }
                    });
                });
            }).catch(consoleLog);
        },
        etpDatatable: function () {
            exist('.js-datatable-etp').then(function (res) {
                isErrorHandled = false;
                var $dataTable = $(res);

                $.each($dataTable, function (index, table) {
                    var $table = $(table);

                    var datatable = $table.DataTable({
                        "language": {
                            "paginate": {
                                "previous": '<span class="fa fa-angle-double-left"></span>',
                                "next": '<span class="fa fa-angle-double-right"></span>'
                            }
                        },
                        searching: $table.hasClass('searchable'),
                        info: !$table.hasClass('no-paging'),
                        ordering: !$table.hasClass('no-ordering'),
                        paging: !$table.hasClass('no-paging'),
                        lengthChange: !$table.hasClass('no-paging'),
                        scrollX: false,
                        scrollY: $table.hasClass('vertical-scroll') ? '350px' : '',
                        scrollCollapse: true,
                        order: [],
                        initComplete: function initComplete(settings, json) {
                            if (!$table.hasClass('no-paging')) {
                                var paginate = '#' + $(this).attr('id') + '_paginate';
                                $(paginate).insertAfter($(this).closest('.dataTables_wrapper'));
                            }
                        }
                    });
                });
            }).catch(consoleLog);
        }
    };

    _extends(datatables);
});