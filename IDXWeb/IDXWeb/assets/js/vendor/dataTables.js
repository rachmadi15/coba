import * as _ from './utils.js'

export default {
    datatable() {
        _.exist('.js-datatable').then(res => {
            let $dataTable = $(res)

            const formatData = (data, colInfo, row) => {
                if (colInfo.format === 'timeFrom' && row.listingDate !== row.tradingDate) return 'T:'
                if (colInfo.format === 'timeTo' && row.listingDate !== row.tradingDate) return '-'

                // empty cell
                if (colInfo.columnName === null) return ''

                // Yes/No cell
                if (colInfo.format === 'yesNo') return (!data) ? 'No' : 'Yes'

                // Check(v) / Cross (X) cell
                if (colInfo.format === 'checkCross') return (data) ? '<span class="fa fa-check"></span>' : (data === false) ? '<span class="fa fa-times"></span>' : ''

                // null value
                if (data === null) return '-'

                // no format (string)
                if (!colInfo.format) return data

                // full date format (1-Jan-19)
                if (colInfo.format === 'fullDate') {
                    let date = new Date(data)
                    return `${date.getDate()}-${date.toLocaleDateString('en',{month:'short'})}-${date.toLocaleDateString('en',{year:'2-digit'})}`
                }
                // full date format (1-Jan-2019)
                if (colInfo.format === 'fullDateYear') {
                    let date = new Date(data)
                    return `${date.getDate()}-${date.toLocaleDateString('en',{month:'short'})}-${date.toLocaleDateString('en',{year:'numeric'})}`
                }
                // digit date format (1/1/2019)
                if (colInfo.format === 'fullDateDigit') {
                    let date = new Date(data)
                    return `${date.getDate()}/${date.toLocaleDateString('en',{month:'numeric'})}/${date.toLocaleDateString('en',{year:'numeric'})}`
                }
                // day-month format (1-Jan)
                if (colInfo.format === 'dayMonth') {
                    let date = new Date(data)
                    return `${date.getDate()}-${date.toLocaleDateString('en',{month:'short'})}`
                }
                // month-year format (Jan-19)
                if (colInfo.format === 'monthYear') {
                    let date = new Date(data)
                    return `${date.toLocaleDateString('en',{month:'short'})}-${date.toLocaleDateString('en',{year:'2-digit'})}`
                }
                // month day format (Jan 02)
                if (colInfo.format === 'monthDay') {
                    let date = new Date(data)
                    return `${date.toLocaleDateString('en',{month:'short'})} ${date.toLocaleDateString('en',{day:'2-digit'})}`
                }
                // month only format (Jan)
                if (colInfo.format === 'month') {
                    let date = new Date(1970,data)
                    return date.toLocaleDateString('en',{month:'short'})
                }

                if (colInfo.format === 'nesting') {
                    return colInfo.nesting.map(nested => formatData(row[nested.columnName], nested, row)).join('<br>')
                }

                // numeric format (123,456.78) / percent format (0.23%)
                return ((colInfo.format === 'plusMinus' && data > 0) ? '+' : '') + data.toLocaleString('en', {
                            minimumFractionDigits: colInfo.decimal,
                            maximumFractionDigits: colInfo.decimal
                        }) + ((colInfo.format === 'percent') ? '%' : '')
            }

            const rowTemplate = (rowData, colInfo) => {
                let children = rowData.map(data => {
                    let cells = colInfo.map((col, index) => {
                        if (data[col.columnName] === undefined && col.format !== 'timeFrom' && col.format !== 'timeTo') return `<td></td>`

                        let resData = formatData(data[col.columnName], col, data)
                        return `<td class="${col.className}">${resData}</td>`
                    }).join('')

                    let row = `<tr>${cells}</tr>`

                    return $(row).toArray()
                })

                return children
            }

            $.each($dataTable, function(index, table) {
                let $table = $(table)
                let src = $table.data('json')
                let unorderable = $table.data('unorderable')
                let targets = (!unorderable) ? [] : JSON.parse(`[${unorderable}]`)

                $.getJSON(src, function(res, textStatus) {

                    let columns = res.columns.map(col => ({
                        data: col.columnName,
                        defaultContent: "",
                        visible: !(col.format === 'grouping' || col.format === 'subGrouping'),
                        render: function(data, type, row, meta) {
                            if (type === 'sort' || type === "type") {
                                const dateFormats = ['fullDate','fullDateYear','fullDateDigit','dayMonth','monthYear','monthDay']

                                if (dateFormats.indexOf(col.format) > -1) return new Date(data).getTime()
                                if (col.format === "month" || col.format === 'plusMinus' || col.format === true) return data
                                if (col.format === "closeFormat") return data.value
                                if (col.format === "count") return meta.row+1
                            }
                            if (col.format === "count") return meta.row+1+'.'
                            if (col.format === "closeFormat") return (!!data.value) ? data.value.toLocaleString() : '-'
                            if (row.hasDecimal > 0 && col.format) return data.toLocaleString('en', {
                                minimumFractionDigits: row.hasDecimal,
                                maximumFractionDigits: row.hasDecimal
                            })
                            return formatData(data, col, row)
                        },
                        createdCell: function (td, cellData, rowData) {
                            $(td).addClass(col.className)

                            if (col.format === "closeFormat" && cellData.status) {
                                if (cellData.status === 'increase') {
                                    $(td).addClass('green')
                                } else if (cellData.status === 'decrease') {
                                    $(td).addClass('red')
                                }
                            }

                            if (rowData.mostCloseValue && rowData.mostCloseValue.indexOf(col.columnName) > -1) {
                                if (col.columnName.indexOf('gain') === 0)
                                    $(td).removeClass('bg-increased').addClass('background-green white')
                                if (col.columnName.indexOf('loss') === 0)
                                    $(td).removeClass('bg-decreased').addClass('background-red white')
                            }

                            if ($(td).hasClass('tooltip')) {
                                const type = col.columnName.substr(0, col.columnName.search('Percent'))
                                const template = `
                                    <span class="tooltip-data">
                                        ${cellData}%
                                        <br>
                                        ${rowData[type].toLocaleString()}
                                    </span>
                                `
                                $(td).append(template)
                            }
                        }
                    }))

                    let datatable = $table.DataTable({
                        "language": {
                            "paginate": {
                                "previous": '<span class="fa fa-angle-double-left"></span>',
                                "next": '<span class="fa fa-angle-double-right"></span>'
                            }
                        },
                        data: res.data,
                        searching: false,
                        info: !$table.hasClass('no-paging'),
                        ordering: !$table.hasClass('no-ordering'),
                        paging: !$table.hasClass('no-paging'),
                        lengthChange: !$table.hasClass('no-paging'),
                        scrollX: false,
                        scrollY: $table.hasClass('vertical-scroll') ? '350px' : '',
                        order: [],
                        columns: columns,
                        columnDefs: [
                            {
                                targets: targets, orderable: false
                            },
                            {
                                targets: '_all', orderable: true
                            },
                        ],
                        createdRow: function(row, data, dataIndex, cells) {
                            if (data.childRows && data.childRows.length) {
                                this.api().row(row).child(rowTemplate(data.childRows, res.columns)).show()
                            }
                        },
                        initComplete: function(){
                            if (!$table.hasClass('no-paging')) {
                                let paginate = '#' + $(this).attr('id') + '_paginate'
                                $(paginate).insertAfter($(this).closest('.dataTables_wrapper'))
                            }
                        },
                        drawCallback: function (settings) {
                            let api = this.api()
                            let rows = api.rows().nodes()
                            let lastGroup = null
                            let lastSubGroup = null
                            let counter = 1
                            const groupColumn = res.columns.findIndex(col => col.format === 'grouping')
                            const subGroupColumn = res.columns.findIndex(col => col.format === 'subGrouping')
                            const countColumn = res.columns.findIndex(col => col.format === 'count')
                            const colCount = api.columns(':visible').count()

                            if (groupColumn > -1) {
                                if (subGroupColumn === -1) {
                                    api.column(groupColumn)
                                        .data()
                                        .each(function ( group, i ) {
                                            if (lastGroup !== group) {
                                                $(rows).eq(i).before(
                                                    `<tr class="sub-head text-left text-bold"><td colspan="${colCount}">${group}</td></tr>`
                                                )

                                                lastGroup = group
                                                counter = 1
                                            }

                                            api.cells(i,countColumn).nodes().to$().html(counter+'.')
                                            counter++
                                        })
                                } else {
                                    api.rows().every(function (rowIdx, tableLoop, rowLoop) {
                                        let group = this.data()['group']
                                        let subGroup = this.data()['subGroup']

                                        if (lastGroup !== group) {
                                            $(rows).eq(rowIdx).before(
                                                `${lastGroup === null ? '' : `<tr><td class="no-border"></td></tr>`}
                                                <tr class="sub-head text-left"><td colspan="${colCount}">${group}</td></tr>`
                                            )

                                            lastGroup = group
                                            lastSubGroup = null
                                        }

                                        if (lastSubGroup !== subGroup) {
                                            $(rows).eq(rowIdx).before(
                                                `${lastSubGroup === null ? '' : `<tr><td class="no-border"></td></tr>`}
                                                <tr class="text-left text-bold"><td class="short no-border" colspan="${colCount}">${subGroup}</td></tr>`
                                            )

                                            lastSubGroup = subGroup
                                        }
                                    })
                                }
                            }
                        }
                    })
                })
            });
        }).catch(_.noop)
    },

    collapsibleDatatable() {
        _.exist('.js-datatable-collapse').then(res => {
            let $dataTable = $(res)

            const rowTemplate = (rowData, colInfo) => {
                const months = [
                    'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'
                ]

                let children = rowData.map(data => {
                    let cells = colInfo.map((col, index) => {
                        if (col.columnName === null) return `<td></td>`

                        let resData = (index === 0 && typeof(data.month) === "number")
                                    ? months[data.month]
                                    : (index === 0)
                                    ? (data.month || data.quarter || data.semester)
                                    : (data[col.columnName] === null)
                                    ? '-'
                                    : (!col.format)
                                    ? data[col.columnName]
                                    : data[col.columnName].toLocaleString('en', {
                                        minimumFractionDigits: col.decimal,
                                        maximumFractionDigits: col.decimal
                                    })
                        return `<td class="${col.className}">${resData}</td>`
                    }).join('')

                    let row = `<tr class="text-right">${cells}</tr>`

                    return $(row).toArray()
                })

                return children
            }

            $.each($dataTable, function(index, table) {
                let $table = $(table)
                let src = $table.data('json')

                $.getJSON(src, function(res, textStatus) {

                    let columns = res.columns.map(col => ({
                        data: col.columnName,
                        className: col.className,
                        defaultContent: "",
                        render: function(data, type, row, meta) {
                            if (col.columnName === null) return
                            let asterisk = row.asterisk ? '*' : ''
                            let resData = (data === null)
                                        ? '-'
                                        : (!col.format)
                                        ? (data + asterisk)
                                        : data.toLocaleString('en', {
                                            minimumFractionDigits: col.decimal,
                                            maximumFractionDigits: col.decimal
                                        })
                            return resData
                        }
                    }))

                    let datatable = $table.DataTable({
                        "language": {
                            "paginate": {
                                "previous": '<span class="fa fa-angle-double-left"></span>',
                                "next": '<span class="fa fa-angle-double-right"></span>'
                            }
                        },
                        data: res.data,
                        searching: false,
                        info: true,
                        ordering: false,
                        lengthChange: true,
                        dom: 'l<"mb-8"t>i',
                        scrollX: false,
                        order: [],
                        columns: columns,
                        createdRow: function(row, data, dataIndex) {
                            $(row).addClass('text-right text-bold parent-row')
                        }
                    })

                    $('.js-datatable-collapse tbody').on('click', 'tr.parent-row', function () {
                        let tr = $(this).closest('tr');
                        let row = datatable.row(tr);

                        if (row.child.isShown()) {
                            row.child.hide();
                        } else {
                            datatable.rows('.parent-row').nodes().to$().not(this).map((id, tr) => {
                                if (datatable.row(tr).child.isShown()) datatable.row(tr).child.hide()
                            })
                            row.child(rowTemplate(row.data().months, res.columns)).show();
                        }
                    });
                })
            });

        }).catch(_.noop)
    },

    idxIndicesHighlightsDatatable() {
        _.exist('.js-datatable-indices').then(res => {
            let $dataTable = $(res)

            const formatNum = (data) => data.toLocaleString()
            const formatPercent = (data) => {
                return data.toLocaleString({minimumFractionDigits: 2,maximumFractionDigits: 2}) + '%'
            }
            const formatDate = (data) => {
                let date = new Date(data)
                return date.toLocaleDateString('en', {month:'short'}) + ' ' + date.toLocaleDateString('en', {day:'2-digit'})
            }
            const renderData = (data) => {
                return formatNum(data.val) + '<br>' + formatDate(data.date)
            }
            const renderChangeValue = (data) => {
                let val = formatNum(data.val)
                let percent = formatPercent(data.percent)

                if (data.status.indexOf('decrease') === 0) return `(${val})<br>(${percent})`
                return `${val}<br>${percent}`
            }
            const renderChangeRank = (data) => {
                let icon = (data.status.indexOf('decrease') === 0)
                            ? 'caret-down fa-lg'
                            : (data.status.indexOf('increase') === 0)
                            ? 'caret-up fa-lg'
                            : 'minus'
                return `<i class="fa fa-${icon}"></i><br>${data.rank}`
            }
            const renderClass = (td, cellData, rowData) => {
                let classes = (cellData.status === "decrease")
                            ? 'red'
                            : (cellData.status === "increase")
                            ? 'green'
                            : (cellData.status === "decreaseMost")
                            ? 'red bg-decreased'
                            : (cellData.status === "increaseMost")
                            ? 'green bg-increased'
                            : ''

                $(td).addClass(classes)
            }

            $.each($dataTable, function(index, table) {
                let $table = $(table)
                let src = $table.data('json')

                $.getJSON(src, function(res, textStatus) {

                    let datatable = $table.DataTable({
                        "language": {
                            "paginate": {
                                "previous": '<span class="fa fa-angle-double-left"></span>',
                                "next": '<span class="fa fa-angle-double-right"></span>'
                            }
                        },
                        data: res.data,
                        searching: false,
                        info: false,
                        ordering: false,
                        lengthChange: false,
                        scrollX: true,
                        order: [],
                        columns: [
                            {
                                className: 'text-center',
                                defaultContent: '',
                                render: function(data, type, row, meta) {
                                    return meta.row+1
                                }
                            },
                            {
                                data: 'indexName',
                                createdCell: function(td) {
                                    $(td).addClass('text-left wide')
                                }
                            },
                            {
                                data: 'high',
                                render: renderData
                            },
                            {
                                data: 'low',
                                render: renderData
                            },
                            {
                                data: 'close',
                                render: renderData
                            },
                            {
                                data: 'month1',
                                defaultContent: '',
                                render: renderChangeValue,
                                createdCell: renderClass
                            },
                            {
                                data: 'month1',
                                className: 'text-center',
                                defaultContent: '',
                                render: renderChangeRank,
                                createdCell: renderClass
                            },
                            {
                                data: 'month3',
                                defaultContent: '',
                                render: renderChangeValue,
                                createdCell: renderClass
                            },
                            {
                                data: 'month3',
                                className: 'text-center',
                                defaultContent: '',
                                render: renderChangeRank,
                                createdCell: renderClass
                            },
                            {
                                data: 'month6',
                                defaultContent: '',
                                render: renderChangeValue,
                                createdCell: renderClass
                            },
                            {
                                data: 'month6',
                                className: 'text-center',
                                defaultContent: '',
                                render: renderChangeRank,
                                createdCell: renderClass
                            },
                            {
                                data: 'year1',
                                defaultContent: '',
                                render: renderChangeValue,
                                createdCell: renderClass
                            },
                            {
                                data: 'year1',
                                className: 'text-center',
                                defaultContent: '',
                                render: renderChangeRank,
                                createdCell: renderClass
                            },
                            {
                                data: 'ytd',
                                defaultContent: '',
                                render: renderChangeValue,
                                createdCell: renderClass
                            },
                            {
                                data: 'ytd',
                                className: 'text-center',
                                defaultContent: '',
                                render: renderChangeRank,
                                createdCell: renderClass
                            },
                        ]
                    })
                })
            });

        }).catch(_.noop)
    },
}
