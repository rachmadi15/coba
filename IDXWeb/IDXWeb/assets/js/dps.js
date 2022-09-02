var collapsibleResults = null;

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

function generateRowData(jData, clickedCode, results, level = 0) {
    for (var item of jData) {
        if (level > 0) {
            item.level = level;
            results.push(item);
        }
        var itemCode = item.Name.split('.')[0];
        if (clickedCode.startsWith(itemCode) && typeof item.months !== 'undefined' && item.months.length > 0) {
            generateRowData(item.months, clickedCode, results, level + 1);
        }
    }
}

function collapsibleRowTemplate(rowData, colInfo) {
    var months = ['', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    var children = rowData.map(function (data) {
        var cells = colInfo.map(function (col, index) {
            if (col.columnName === null) return '<td></td>';
            if (col.format === "closeFormat") {
                status = data[col.columnName].status === '' ? '' : data[col.columnName].status === 'increase' ? 'green bg-increased' : 'red bg-decreased';
                return '<td class="' + col.className + ' ' + status + '">' + data[col.columnName].value.toLocaleString('en', {
                    minimumFractionDigits: col.decimal,
                    maximumFractionDigits: col.decimal
                }) + '</td>';
            }
            if (col.format !== 'percent' && data[col.columnName] == 0) {
                return '<td class="' + col.className + ' ' + status + '">-</td>';
            }

            var resData = data[col.columnName];
            if (index === 0 && data.quarter != null && data.quarter !== '')
                resData = data.quarter;
            else if (index === 0 && col.columnName.toLowerCase() != 'year' && col.columnName.toLowerCase() != 'name' && typeof data.month === "number")
                resData = months[data.month];
            else if (index === 0)
                resData = data.month || data.quarter || data.Quarter || data.semester || data.Name;
            else if (data[col.columnName] === null)
                resData = '-';
            else if (!col.format)
                resData = data[col.columnName]

            if (index === 0 && col.format === 'fullDate') {
                var date = new Date(resData);
                resData = date.getDate() + '-' + date.toLocaleDateString('en', { month: 'short' }) + '-' + date.toLocaleDateString('en', { year: '2-digit' });
            }
            else {
                resData = resData.toLocaleString('en', {
                    minimumFractionDigits: col.decimal,
                    maximumFractionDigits: col.decimal
                })
            }

            var extraClassName = '';
            if (typeof data.months !== 'undefined' && data.months !== null && data.months.length > 0) {
                extraClassName += " text-bold parent-row is-closed";
            }
            var tdAttr = '';
            if (data.level > 0) {
                tdAttr = 'style="padding-left: ' + (16 + (10 * data.level)) + 'px;"'
            }

            return '<td class="' + col.className + extraClassName + '" ' + tdAttr + '>' + resData + '</td>';
        }).join('');

        var trAttr = '';
        if (data.months !== null && typeof data.months !== 'undefined' && data.months.length > 0) {
            trAttr = ' data-name="' + data.Name + '"';
        }

        var row = '<tr class="text-right"' + trAttr + '>' + cells + '</tr>';
        return $(row).toArray();
    });

    return children;
}

function addParentRowClickListener(datatable, rootData, clickedCode, level = 0) {
    var self = this;
    var childrenData = [];
    generateRowData(rootData.data, clickedCode, childrenData);

    var row = null;
    datatable.rows().every(function (rowIdx, tableLoop, rowLoop) {
        if (row !== null) return;

        var rowCode = this.data().Name.split('.')[0];
        if (clickedCode.startsWith(rowCode)) {
            row = this;
        }
    });
    if (row === null) return;

    if (level <= 0 && row.child.isShown()) {
        row.child.hide();
    } else {
        $('#currentTable').find('tr').each(function () {
            var name = $(this).data('name');
            if (typeof name === 'undefined' || name === null) return;
            var code = name.split('.')[0];
            if (clickedCode.startsWith(code)) return;
            datatable.row(this).child.hide();
        });

        var newRows = collapsibleRowTemplate(childrenData, rootData.columns);
        row.child(newRows).show();

        for (var newRow of newRows) {
            $(newRow).on('click', function (e) {
                var newTr = $(e.currentTarget);
                if (typeof newTr.data('name') === 'undefined' || newTr.data('name') === '') return;
                addParentRowClickListener(datatable, rootData, newTr.data('name').split('.')[0], level + 1);
            })
        }
    }
}

function setQueryString(uri, key, value) {
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator = uri.indexOf('?') !== -1 ? "&" : "?";
    if (uri.match(re)) {
        return uri.replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
        return uri + separator + key + "=" + value;
    }
}

$(function() {
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

    var modules = {
        downloadDropdown: function () {
            exist('.download-toggle').then(function (res) {
                var $toggleButton = $(res);

                $toggleButton.on('click', function (e) {
                    e.stopPropagation();
                    var $this = $(e.currentTarget);
                    var targetId = $this.data('toggle');
                    var $thisTarget = $(targetId);

                    $('.download-list').not(targetId).removeClass('is-active');
                    $thisTarget.toggleClass('is-active');
                }).on('click', '.download-list', function (e) {
                    e.stopPropagation();
                });

                $(document).on('click', function (e) {
                    $('.download-list').removeClass('is-active');
                });
            }).catch(consoleLog);
        }
    };
    _extends(modules);

    $('.header-img-container').addClass('printable');
})

