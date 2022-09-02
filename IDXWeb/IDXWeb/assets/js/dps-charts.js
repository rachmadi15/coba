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

    function showChartError(chart) {
        if (isErrorHandled) return;
        var en = document.URL.indexOf("/en-us/") >= 0 || document.URL.indexOf("/en/") >= 0;
        var message = 'Failed to load statistic data. Please try again later.';
        if (!en) {
            message = 'Gagal mengambil beberapa data statistik. Cobalah beberapa saat lagi.';
        }
        alert(message);
        $(chart).html('\n        <div class="chart-error">\n            Data Not Found\n        </div>\n    ');
        isErrorHandled = true;
    }

    function consoleLog(e, param1) {
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

    var charts = {
        stockChart: function stockChart() {
            exist('.chart-stock-js').then(function (res) {
                isErrorHandled = false;
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');
                    $.ajax({
                        url: dataChart,
                        method: 'GET',
                        success: function (res) {
                            Highcharts.setOptions({
                                lang: {
                                    rangeSelectorZoom: ''
                                }
                            });

                            Highcharts.stockChart(chart, {
                                rangeSelector: {
                                    enabled: false,
                                    inputEnabled: false
                                },

                                chart: {
                                    height: 130,
                                    spacing: [0, 0, 0, 0],
                                    marginTop: 20
                                },

                                credits: {
                                    enabled: false
                                },

                                exporting: {
                                    enabled: false
                                },

                                title: {
                                    text: null
                                },

                                navigator: {
                                    enabled: false
                                },

                                scrollbar: {
                                    enabled: false
                                },

                                series: [{
                                    name: 'AAPL Stock Price',
                                    data: res,
                                    tooltip: {
                                        valueDecimals: 2
                                    }
                                }]
                            });
                        },
                        error: () => { showChartError(chart); },
                        complete: () => { return; }
                    });
                });
            }).catch(consoleLog);
        },
        compositeStockPriceIndexChart: function compositeStockPriceIndexChart() {
            exist('.composite-stock-price-index-js').then(function (res) {
                isErrorHandled = false;
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');
                    $.ajax({
                        url: dataChart,
                        method: 'GET',
                        success: function (res) {
                            var seriesData = [],
                                optColors = [];

                            res.series.map(function (data, i) {

                                var entry = {
                                    name: data.seriesName,
                                    type: data.type,
                                    yAxis: data.yAxis,
                                    data: data.seriesData.map(function (dt) {
                                        return {
                                            x: new Date(dt.x).getTime(),
                                            y: dt.y
                                        };
                                    }),
                                    tooltip: {
                                        valueSuffix: data.valueSuffix
                                    }
                                };

                                seriesData.push(entry);
                                optColors.push(data.seriesColor);
                            });

                            Highcharts.setOptions({
                                global: {
                                    timezoneOffset: -7 * 60 //selisih 7 jam
                                },
                                lang: {
                                    rangeSelectorZoom: '',
                                    thousandsSep: ','
                                },
                                colors: optColors
                            });

                            Highcharts.stockChart(chart, {
                                chart: {
                                    zoomType: 'xy',
                                },
                                rangeSelector: {
                                    enabled: false,
                                    inputEnabled: false
                                },
                                title: {
                                    text: res.title,
                                    align: 'center'
                                },
                                subtitle: {
                                    text: res.subtitle
                                },
                                credits: {
                                    enabled: false
                                },
                                exporting: {
                                    enabled: false
                                },
                                navigator: {
                                    enabled: false
                                },
                                scrollbar: {
                                    enabled: false
                                },
                                legend: {
                                    enabled: true
                                },
                                xAxis: {
                                    crosshair: true,
                                    dateTimeLabelFormats: { day: '%e-%b' }
                                },
                                yAxis: [{
                                    title: null,
                                    labels: {
                                        format: '{value:,.0f}',
                                        style: {
                                            color: Highcharts.getOptions().colors[0]
                                        }
                                    },
                                    opposite: false
                                }, {
                                    gridLineWidth: 0,
                                    title: null,
                                    labels: {
                                        format: '{value:,.0f}',
                                        style: {
                                            color: Highcharts.getOptions().colors[1]
                                        }
                                    },
                                    opposite: true
                                }],
                                tooltip: {
                                    shared: true,
                                    split: false
                                },
                                series: seriesData,
                                plotOptions: {
                                    line: {
                                        marker: {
                                            enabled: false
                                        }
                                    },
                                    column: {
                                        borderWidth: 0
                                    },
                                    series: {
                                        pointPadding: 0,
                                        groupPadding: 0,
                                        animation: false,
                                        borderWidth: 0,
                                        shadow: false,
                                        dataGrouping: {
                                            forced: true,
                                            dateTimeLabelFormats: {
                                                day: ['%e-%b-%y', '%e-%b-%y', ' %e-%b-%y']
                                            },
                                            units: [['day', [1]]]
                                        }
                                    }
                                }
                                // responsive: {
                                //     rules: [{
                                //         condition: {
                                //             maxWidth: 768
                                //         },
                                //         chartOptions: {
                                //             xAxis: [{
                                //                 tickInterval: 2 * 30 * 24 * 3600 * 1000 // 2 months
                                //             }]
                                //         }
                                //     }]
                                // }
                            });
                        },
                        error: () => { showChartError(chart); },
                        complete: () => { return; }
                    });
                });
            }).catch(consoleLog);
        },
        dailyCompositeStockPriceIndexChart: function dailyCompositeStockPriceIndexChart() {
            exist('.chart-composite-js').then(function (res) {
                isErrorHandled = false;
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');
                    $.ajax({
                        url: dataChart,
                        method: 'GET',
                        success: function (res) {
                            var seriesData = [],
                                optColors = [];

                            res.series.map(function (data, i) {
                                var entry = {
                                    name: data.seriesName,
                                    type: data.type,
                                    yAxis: data.yAxis,
                                    data: data.seriesData
                                };

                                seriesData.push(entry);
                                optColors.push(data.seriesColor);
                            });

                            Highcharts.setOptions({
                                lang: {
                                    rangeSelectorZoom: '',
                                    thousandsSep: ','
                                },
                                colors: optColors
                            });

                            Highcharts.chart(chart, {
                                chart: {
                                    zoomType: 'xy'
                                },
                                title: {
                                    text: res.title,
                                    align: 'center'
                                },
                                subtitle: {
                                    text: res.subtitle
                                },
                                credits: {
                                    enabled: false
                                },
                                exporting: {
                                    enabled: false
                                },
                                xAxis: {
                                    categories: res.days,
                                    crosshair: true
                                },
                                yAxis: [{
                                    title: null,
                                    labels: {
                                        format: '{value:,.0f}',
                                        style: {
                                            color: Highcharts.getOptions().colors[1]
                                        }
                                    },
                                    tickAmount: 6
                                }, {
                                    gridLineWidth: 0,
                                    title: null,
                                    labels: {
                                        format: '{value:,.0f}',
                                        style: {
                                            color: Highcharts.getOptions().colors[2]
                                        }
                                    },
                                    opposite: true,
                                    tickAmount: 7
                                }],
                                series: seriesData,
                                plotOptions: {
                                    series: {
                                        animation: false,
                                        pointPadding: 0,
                                        borderWidth: 0
                                    }
                                }
                            });
                        },
                        error: () => { showChartError(chart); },
                        complete: () => { return; }
                    });
                });
            }).catch(consoleLog);
        },
        jakartaCompositeIndexChart: function jakartaCompositeIndexChart() {
            exist('.jakarta-composite-index-js').then(function (res) {
                isErrorHandled = false;
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');
                    $.ajax({
                        url: dataChart,
                        method: 'GET',
                        success: function (res) {
                            var seriesData = [],
                                optColors = [];

                            res.series.map(function (data, i) {
                                var entry = {
                                    name: data.seriesName,
                                    type: 'line',
                                    data: data.seriesData.map(function (dt) {
                                        return {
                                            x: new Date(dt.x).getTime(),
                                            y: dt.y
                                        };
                                    }),
                                    tooltip: {
                                        valueSuffix: '%',
                                        valueDecimals: 2
                                    }
                                    // pointInterval: 24 * 3600 * 1000
                                };

                                seriesData.push(entry);
                                optColors.push(data.seriesColor);
                            });

                            Highcharts.setOptions({
                                global: {
                                    timezoneOffset: -7 * 60 //selisih 7 jam
                                },
                                lang: {
                                    rangeSelectorZoom: ''
                                },
                                colors: optColors
                            });

                            Highcharts.stockChart(chart, {
                                chart: {
                                    zoomType: 'xy',
                                    height: 700
                                },
                                drilldown: {
                                    activeDataLabelStyle: {
                                        color: 'contrast'
                                    }
                                },
                                rangeSelector: {
                                    enabled: false,
                                    inputEnabled: false,
                                },
                                title: {
                                    text: res.title,
                                    align: 'center'
                                },
                                subtitle: {
                                    text: res.subtitle
                                },
                                credits: {
                                    enabled: false
                                },
                                exporting: {
                                    enabled: false
                                },
                                navigator: {
                                    enabled: false
                                },
                                scrollbar: {
                                    enabled: false
                                },
                                legend: {
                                    enabled: true
                                },
                                xAxis: [{
                                    title: {
                                        text: 'Month',
                                        align: 'high',
                                        style: {
                                            fontWeight: 'bold'
                                        }
                                    },
                                    crosshair: true,
                                    dateTimeLabelFormats: { day: '%e-%b' }
                                    // type: 'datetime',
                                    // tickInterval: 30 * 24 * 3600 * 1000 //30 days (1 month)
                                }],
                                yAxis: {
                                    // title: {
                                    //     text: 'Percentage',
                                    //     align: 'high',
                                    //     offset: 0,
                                    //     rotation: 0,
                                    //     y: -10,
                                    //     style: {
                                    //         fontWeight: 'bold'
                                    //     },
                                    //     textAlign: 'center'
                                    // },
                                    labels: {
                                        format: '{value}%',
                                        x: 15
                                    },
                                    offset: 30
                                    // opposite: false
                                },
                                tooltip: {
                                    shared: true,
                                    split: false
                                },
                                series: seriesData,
                                plotOptions: {
                                    line: {
                                        marker: {
                                            enabled: false
                                        }
                                        // pointStart: Date.UTC(2016,11,30)
                                    },
                                    series: {
                                        pointPadding: 0,
                                        animation: false,
                                        groupPadding: 0,
                                        borderWidth: 0,
                                        shadow: false,
                                        dataGrouping: {
                                            forced: true,
                                            dateTimeLabelFormats: {
                                                day: ['%e-%b-%y', '%e-%b-%y', ' %e-%b-%y']
                                            },
                                            units: [['day', [1]]]
                                        }
                                    }
                                },
                                responsive: {
                                    rules: [{
                                        condition: {
                                            maxWidth: 768
                                        },
                                        chartOptions: {
                                            // chart: {
                                            //     marginTop: 100,
                                            // },
                                            // rangeSelector: {
                                            //     marginTop: 100
                                            // },
                                            legend: {
                                                floating: false,
                                                layout: 'horizontal',
                                                align: 'center',
                                                verticalAlign: 'bottom',
                                                x: 0,
                                                y: 0
                                            },
                                            yAxis: {
                                                title: {
                                                    align: 'middle',
                                                    rotation: 270,
                                                    offset: 10
                                                }
                                            }
                                        }
                                    }]
                                }
                            });
                        },
                        error: () => { showChartError(chart); },
                        complete: () => { return; }
                    });
                });
            }).catch(consoleLog);
        },
        idxIndices12MonthChart: function idxIndices12MonthChart() {
            exist('.idx-indices-12-month-js').then(function (res) {
                isErrorHandled = false;
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');
                    $.ajax({
                        url: dataChart,
                        method: 'GET',
                        success: function (res) {
                            var seriesData = [],
                                optColors = [];

                            res.series.map(function (data, i) {
                                var entry = {
                                    name: data.seriesName,
                                    type: data.type,
                                    yAxis: data.yAxis,
                                    data: data.seriesData.map(function (dt) {
                                        return {
                                            x: new Date(dt.x).getTime(),
                                            y: dt.y
                                        };
                                    })
                                    // pointInterval: 24 * 3600 * 1000
                                };

                                seriesData.push(entry);
                                optColors.push(data.seriesColor);
                            });

                            Highcharts.setOptions({
                                global: {
                                    timezoneOffset: -7 * 60 //selisih 7 jam
                                },
                                lang: {
                                    rangeSelectorZoom: '',
                                    thousandsSep: ','
                                },
                                colors: optColors
                            });

                            Highcharts.stockChart(chart, {
                                chart: {
                                    zoomType: 'xy'
                                },
                                rangeSelector: {
                                    enabled: false,
                                    inputEnabled: false
                                },
                                title: {
                                    text: res.chartName,
                                    align: 'center',
                                    margin: 45
                                },
                                credits: {
                                    enabled: false
                                },
                                exporting: {
                                    enabled: false
                                },
                                legend: {
                                    enabled: true
                                },
                                navigator: {
                                    enabled: false
                                },
                                scrollbar: {
                                    enabled: false
                                },
                                xAxis: [{
                                    crosshair: true,
                                    type: 'datetime'
                                }],
                                yAxis: [{
                                    title: {
                                        text: 'Million<br>Shares',
                                        align: 'high',
                                        offset: 0,
                                        rotation: 0,
                                        y: -30,
                                        style: {
                                            fontWeight: 'bold'
                                        }
                                    },
                                    labels: {
                                        format: '{value:,.0f}',
                                        style: {
                                            color: Highcharts.getOptions().colors[0]
                                        }
                                    },
                                    opposite: false
                                }, {
                                    gridLineWidth: 0,
                                    title: {
                                        text: 'Index',
                                        align: 'high',
                                        offset: 10,
                                        rotation: 0,
                                        y: -15,
                                        style: {
                                            fontWeight: 'bold'
                                        }
                                    },
                                    labels: {
                                        format: '{value:,.0f}',
                                        style: {
                                            color: Highcharts.getOptions().colors[1]
                                        }
                                    },
                                    opposite: true
                                }],
                                tooltip: {
                                    xDateFormat: '%e-%b-%y',
                                    shared: true,
                                    split: false
                                },
                                series: seriesData,
                                plotOptions: {
                                    line: {
                                        marker: {
                                            enabled: false
                                        }
                                    },
                                    column: {
                                        borderWidth: 0
                                    },
                                    series: {
                                        pointPadding: 0,
                                        groupPadding: 0,
                                        borderWidth: 0,
                                        shadow: false,
                                        animation: false,
                                        dataGrouping: {
                                            forced: true,
                                            dateTimeLabelFormats: {
                                                day: ['%e-%b-%y', '%e-%b-%y', ' %e-%b-%y']
                                            },
                                            units: [['day', [1]]]
                                        }
                                    }
                                },
                                responsive: {
                                    rules: [{
                                        condition: {
                                            maxWidth: 768
                                        }
                                    }]
                                }
                            });
                        },
                        error: () => { showChartError(chart); },
                        complete: () => { return; }
                    });
                });
            }).catch(consoleLog);
        },
        dailyTradingChart: function dailyTradingChart() {
            exist('.daily-trading-js').then(function (res) {
                isErrorHandled = false;
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');
                    $.ajax({
                        url: dataChart,
                        method: 'GET',
                        success: function (res) {
                            var _xAxis;

                            var seriesData = [],
                                optColors = [];

                            res.series.map(function (data, i) {
                                var entry = {
                                    gapSize: 1,
                                    name: data.seriesName,
                                    data: data.seriesData.map(function (dt) {
                                        return {
                                            x: new Date(dt.x).getTime(),
                                            y: dt.y
                                        };
                                    }),
                                    pointInterval: 24 * 3600 * 1000
                                };

                                seriesData.push(entry);
                                optColors.push(data.seriesColor);
                            });

                            Highcharts.setOptions({
                                global: {
                                    timezoneOffset: -7 * 60 //selisih 7 jam
                                },
                                lang: {
                                    rangeSelectorZoom: '',
                                    thousandsSep: ',',
                                    numericSymbols: null
                                },
                                colors: optColors
                            });

                            Highcharts.stockChart(chart, {
                                chart: {
                                    zoomType: 'xy',
                                    type: 'column',
                                    marginTop: 30,
                                    marginRight: 10,
                                    backgroundColor: 'transparent'
                                },
                                rangeSelector: {
                                    enabled: false,
                                    inputEnabled: false
                                },
                                title: {
                                    text: ' ',
                                    align: 'left',
                                    margin: 30,
                                    useHTML: true
                                },
                                credits: {
                                    enabled: false
                                },
                                exporting: {
                                    enabled: false
                                },
                                navigator: {
                                    enabled: false
                                },
                                scrollbar: {
                                    enabled: false
                                },
                                legend: {
                                    enabled: true
                                },
                                xAxis: (_xAxis = {
                                    title: {
                                        text: 'Date',
                                        align: 'high',
                                        style: {
                                            fontWeight: 'bold'
                                        }
                                    },
                                    type: 'datetime',
                                    labels: {
                                        tickInterval: 24 * 3600 * 1000,
                                        rotation: -45
                                    },
                                    crosshair: true,
                                    dateTimeLabelFormats: {
                                        day: '%d-%b-%y'
                                    }
                                }, _defineProperty(_xAxis, 'labels', {
                                    formatter: function formatter() {
                                        var date = new Date(this.value);
                                        var day = date.toLocaleDateString('en', { day: '2-digit' });
                                        var month = date.toLocaleDateString('en', { month: 'short' });
                                        var year = date.toLocaleDateString('en', { year: '2-digit' });

                                        return day;
                                    }
                                }), _defineProperty(_xAxis, 'tickPositioner', function tickPositioner() {
                                    return this.series[0].processedXData.filter(function (x, i) {
                                        return i % 2 === 0;
                                    }).slice();
                                }), _xAxis),
                                yAxis: {
                                    title: {
                                        text: res.unit,
                                        align: 'high',
                                        offset: 0,
                                        rotation: 0,
                                        y: -15,
                                        style: {
                                            fontWeight: 'bold'
                                        }
                                    },
                                    showLastLabel: true,
                                    reversedStacks: false,
                                    opposite: false
                                },
                                tooltip: {
                                    xDateFormat: '%e-%b-%y',
                                    shared: false,
                                    split: false
                                },
                                series: seriesData,
                                plotOptions: {
                                    column: {
                                        stacking: 'normal',
                                        dataLabels: {
                                            enabled: false
                                        }
                                        // pointStart: Date.UTC(2019,2,1)
                                    },
                                    series: {
                                        pointRange: 24 * 3600 * 1000,
                                        animation: false,
                                        dataGrouping: {
                                            forced: true,
                                            dateTimeLabelFormats: {
                                                day: ['%e-%b-%y', '%e-%b-%y', ' %e-%b-%y']
                                            },
                                            units: [['day', [1]]]
                                        }
                                    }
                                },
                                responsive: {
                                    rules: [{
                                        condition: {
                                            minWidth: 768
                                        },
                                        chartOptions: {
                                            xAxis: {
                                                title: {
                                                    offset: 20,
                                                    x: 10
                                                },
                                                tickPositioner: function tickPositioner() {
                                                    return this.series[0].processedXData.slice();
                                                }
                                            },
                                            yAxis: {
                                                labels: {
                                                    format: '{value:,.0f}'
                                                }
                                            }
                                        }
                                    }]
                                }
                            });
                        },
                        error: () => { showChartError(chart); },
                        complete: () => { return; }
                    });
                });
            }).catch(consoleLog);
        },
        totalTradingInvestorTypeChart: function totalTradingInvestorTypeChart() {
            exist('.total-trading-investor-js').then(function (res) {
                isErrorHandled = false;
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');
                    $.ajax({
                        url: dataChart,
                        method: 'GET',
                        success: function (res) {
                            var seriesData = [],
                                optColors = [];

                            res.series.map(function (data, i) {
                                var entry = {
                                    gapSize: 1,
                                    name: data.seriesName,
                                    data: data.seriesData.map(function (dt) {
                                        return {
                                            x: new Date(dt.x).getTime(),
                                            y: dt.y
                                        };
                                    }),
                                    pointInterval: 24 * 3600 * 1000
                                };

                                seriesData.push(entry);
                                optColors.push(data.seriesColor);
                            });

                            Highcharts.setOptions({
                                global: {
                                    timezoneOffset: -7 * 60 //selisih 7 jam
                                },
                                lang: {
                                    rangeSelectorZoom: '',
                                    thousandsSep: ','
                                },
                                colors: optColors
                            });

                            Highcharts.stockChart(chart, {
                                chart: {
                                    zoomType: 'xy',
                                    type: 'column',
                                    marginTop: 30,
                                    marginRight: 10,
                                    backgroundColor: 'transparent'
                                },
                                title: null,
                                credits: {
                                    enabled: false
                                },
                                exporting: {
                                    enabled: false
                                },
                                rangeSelector: {
                                    enabled: false,
                                    inputEnabled: false
                                },
                                navigator: {
                                    enabled: false
                                },
                                scrollbar: {
                                    enabled: false
                                },
                                legend: {
                                    enabled: true
                                },
                                xAxis: {
                                    title: {
                                        text: 'Date',
                                        align: 'high',
                                        style: {
                                            fontWeight: 'bold'
                                        }
                                    },
                                    crosshair: true,
                                    labels: {
                                        formatter: function formatter() {
                                            return new Date(this.value).getDate();
                                        }
                                    },
                                    tickPositioner: function tickPositioner() {
                                        return this.series[0].processedXData.filter(function (x, i) {
                                            return i % 2 === 0;
                                        }).slice();
                                    }
                                },
                                yAxis: {
                                    title: {
                                        text: 'Percentage',
                                        align: 'high',
                                        offset: 0,
                                        rotation: 0,
                                        y: -15,
                                        style: {
                                            fontWeight: 'bold'
                                        }
                                    },
                                    opposite: false,
                                    reversedStacks: false,
                                    showLastLabel: true,
                                    labels: {
                                        format: '{value}%'
                                    }
                                },
                                tooltip: {
                                    xDateFormat: '%e-%b-%y',
                                    shared: false,
                                    split: false,
                                    pointFormat: '<span style="color:{point.color}">\u25CF</span> {series.name}: <b>{point.percentage:.0f}%</b><br/>'
                                },
                                series: seriesData,
                                plotOptions: {
                                    column: {
                                        stacking: 'percent',
                                        dataLabels: {
                                            enabled: false
                                        }
                                        // pointStart: Date.UTC(2019,2,1)
                                    },
                                    series: {
                                        animation: false,
                                        dataGrouping: {
                                            forced: true,
                                            dateTimeLabelFormats: {
                                                day: ['%e-%b-%y', '%e-%b-%y', ' %e-%b-%y']
                                            },
                                            units: [['day', [1]]]
                                        }
                                    }
                                },
                                responsive: {
                                    rules: [{
                                        condition: {
                                            minWidth: 768
                                        },
                                        chartOptions: {
                                            xAxis: {
                                                title: {
                                                    offset: 20,
                                                    x: 10
                                                },
                                                tickPositioner: function tickPositioner() {
                                                    return this.series[0].processedXData.slice();
                                                }
                                            }
                                        }
                                    }]
                                }
                            });
                        },
                        error: () => { showChartError(chart); },
                        complete: () => { return; }
                    });
                });
            }).catch(consoleLog);
        },
        netPurchaseForeignersChart: function netPurchaseForeignersChart() {
            exist('.net-purchase-foreigners-js').then(function (res) {
                isErrorHandled = false;
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');
                    $.ajax({
                        url: dataChart,
                        method: 'GET',
                        success: function (res) {
                            Highcharts.setOptions({
                                lang: {
                                    rangeSelectorZoom: '',
                                    thousandsSep: ','
                                },
                                colors: ['#54A661', '#9F0E0F']
                            });

                            Highcharts.stockChart(chart, {
                                chart: {
                                    zoomType: 'xy',
                                    type: 'column',
                                    marginTop: 30,
                                    marginRight: 10,
                                    backgroundColor: 'transparent'
                                },
                                title: null,
                                credits: {
                                    enabled: false
                                },
                                exporting: {
                                    enabled: false
                                },
                                rangeSelector: {
                                    enabled: false,
                                    inputEnabled: false
                                },
                                navigator: {
                                    enabled: false
                                },
                                scrollbar: {
                                    enabled: false
                                },
                                xAxis: {
                                    title: {
                                        text: 'Date',
                                        align: 'high',
                                        style: {
                                            fontWeight: 'bold'
                                        }
                                    },
                                    crosshair: true,
                                    labels: {
                                        formatter: function formatter() {
                                            return new Date(this.value).getDate();
                                        }
                                    },
                                    tickPositioner: function tickPositioner() {
                                        return this.series[0].processedXData.filter(function (x, i) {
                                            return i % 2 === 0;
                                        }).slice();
                                    }
                                    // breaks: [{
                                    //     // break 3 Maret
                                    //     from: Date.UTC(2019,2,2), //from 2 Maret
                                    //     to:  Date.UTC(2019,2,4), // to 2 Maret
                                    //     breakSize: 24 * 3600 * 1000 // 1 day
                                    // }]
                                },
                                yAxis: {
                                    title: {
                                        text: 'Billion IDR',
                                        align: 'high',
                                        offset: 0,
                                        rotation: 0,
                                        y: -15,
                                        style: {
                                            fontWeight: 'bold'
                                        }
                                    },
                                    labels: {
                                        format: '{value:,.0f}'
                                    },
                                    opposite: false,
                                    showLastLabel: true
                                },
                                tooltip: {
                                    xDateFormat: '%e-%b-%y',
                                    split: false,
                                    pointFormat: '<span style="color:{point.color}">\u25CF</span> <b>{point.y:,.0f}</b><br/>'
                                },
                                series: [{
                                    gapSize: 1,
                                    name: null,
                                    data: res.seriesData.map(function (dt) {
                                        return {
                                            x: new Date(dt.x).getTime(),
                                            y: dt.y
                                        };
                                    }),
                                    negativeColor: Highcharts.getOptions().colors[1],
                                    showInLegend: false
                                }],
                                plotOptions: {
                                    series: {
                                        animation: false,
                                    },
                                    column: {
                                        dataLabels: {
                                            enabled: false
                                        }
                                        // pointStart: Date.UTC(2019,2,1)
                                    }
                                },
                                responsive: {
                                    rules: [{
                                        condition: {
                                            minWidth: 768
                                        },
                                        chartOptions: {
                                            xAxis: {
                                                title: {
                                                    offset: 20,
                                                    x: 10
                                                },
                                                tickPositioner: function tickPositioner() {
                                                    return this.series[0].processedXData.slice();
                                                }
                                            }
                                        }
                                    }]
                                }
                            });
                        },
                        error: () => { showChartError(chart); },
                        complete: () => { return; }
                    });
                });
            }).catch(consoleLog);
        },
        tradingMarketTypeChart: function tradingMarketTypeChart() {
            exist('.trading-market-js').then(function (res) {
                isErrorHandled = false;
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');
                    $.ajax({
                        url: dataChart,
                        method: 'GET',
                        success: function (res) {
                            var seriesData = [],
                                optColors = [];
                            var series = res.series;
                            var pointFormat = '<span style="color:{point.color}">\u25CF</span> {series.name}: <b>{point.y:.2f}%</b><br/>';
                            var showValue = false;
                            series.map(function (data, i) {
                                var entry = {
                                    gapSize: 1,
                                    name: data.seriesName,
                                    data: data.seriesData.map(function (dt) {
                                        var result = {
                                            x: new Date(dt.x).getTime(),
                                            y: dt.y
                                        };

                                        if (!showValue && dt.value != null) {
                                            pointFormat = '<span style="color:{point.color}">\u25CF</span> {series.name}: <b>{point.y:.2f}% ({point.value})</b><br/>';
                                            showValue = true;
                                        }
                                        if (showValue) result.value = dt.value;
                                        return result;
                                    }),
                                    pointInterval: 31 * 24 * 3600 * 1000
                                };

                                seriesData.push(entry);
                                optColors.push(data.seriesColor);
                            });

                            Highcharts.setOptions({
                                lang: {
                                    rangeSelectorZoom: '',
                                    thousandsSep: ','
                                },
                                colors: optColors
                            });

                            Highcharts.stockChart(chart, {
                                chart: {
                                    zoomType: 'xy',
                                    type: 'column',
                                    marginTop: 30,
                                    marginRight: 10,
                                    backgroundColor: 'transparent'
                                },
                                title: null,
                                credits: {
                                    enabled: false
                                },
                                exporting: {
                                    enabled: false
                                },
                                rangeSelector: {
                                    enabled: false,
                                    inputEnabled: false
                                },
                                navigator: {
                                    enabled: false
                                },
                                scrollbar: {
                                    enabled: false
                                },
                                legend: {
                                    enabled: true
                                },
                                xAxis: {
                                    title: {
                                        text: 'Month-Year',
                                        align: 'high',
                                        style: {
                                            fontWeight: 'bold'
                                        }
                                    },
                                    crosshair: true,
                                    type: 'datetime',
                                    tickInterval: 30 * 24 * 3600 * 1000,
                                    labels: {
                                        formatter: function formatter() {
                                            var time = new Date(this.value);
                                            return time.toLocaleDateString('en', { month: 'short', year: '2-digit' });
                                        }
                                    },
                                    tickPositioner: function tickPositioner() {
                                        return this.series[0].processedXData.slice();
                                    }
                                },
                                yAxis: {
                                    title: {
                                        text: 'Percentage',
                                        align: 'high',
                                        offset: 0,
                                        rotation: 0,
                                        y: -15,
                                        style: {
                                            fontWeight: 'bold'
                                        }
                                    },
                                    opposite: false,
                                    labels: {
                                        format: '{value:.2f}%'
                                    },
                                    showLastLabel: true
                                },
                                tooltip: {
                                    xDateFormat: '%b-%y',
                                    split: false,
                                    shared: false,
                                    pointFormat: pointFormat
                                },
                                series: seriesData,
                                plotOptions: {
                                    series: {
                                        animation: false,
                                    },
                                    column: {
                                        dataLabels: {
                                            enabled: false
                                        },
                                        pointStart: Date.UTC(2017, 9, 1)
                                    }
                                }
                            });
                        },
                        error: () => { showChartError(chart); },
                        complete: () => { return; }
                    });
                });
            }).catch(consoleLog);
        },
        barsIndustryChart: function barsIndustryChart() {
            exist('.bars-industry-js').then(function (res) {
                isErrorHandled = false;
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');
                    $.ajax({
                        url: dataChart,
                        method: 'GET',
                        success: function (res) {
                            var seriesData = [],
                                optColors = [];

                            res.series.map(function (data, i) {
                                var entry = {
                                    name: data.seriesName,
                                    data: data.seriesData
                                };

                                seriesData.push(entry);
                                optColors.push(data.seriesColor);
                            });

                            Highcharts.setOptions({
                                lang: {
                                    rangeSelectorZoom: '',
                                    thousandsSep: ','
                                },
                                colors: optColors
                            });
                            Highcharts.chart(chart, {
                                chart: {
                                    zoomType: 'xy',
                                    type: 'bar',
                                    marginTop: 30,
                                    marginRight: 10,
                                    backgroundColor: 'transparent'
                                },
                                title: null,
                                credits: {
                                    enabled: false
                                },
                                exporting: {
                                    enabled: false
                                },
                                xAxis: {
                                    title: {
                                        text: 'Sector',
                                        style: {
                                            fontWeight: 'bold'
                                        }
                                    },
                                    crosshair: true,
                                    categories: res.sectors
                                },
                                yAxis: {
                                    title: {
                                        text: 'Percentage',
                                        style: {
                                            fontWeight: 'bold'
                                        }
                                    },
                                    labels: {
                                        format: '{value}%'
                                    },
                                    tickInterval: 5
                                },
                                tooltip: {
                                    xDateFormat: '%b-%y',
                                    pointFormat: '<span style="color:{point.color}">\u25CF</span> {series.name}: <b>{point.y:.2f}%</b><br/>'
                                },
                                series: seriesData,
                                plotOptions: {
                                    series: {
                                        animation: false,
                                    },
                                    column: {
                                        dataLabels: {
                                            enabled: false
                                        },
                                        pointStart: Date.UTC(2017, 9, 1)
                                    }
                                }
                            });
                        },
                        error: () => { showChartError(chart); },
                        complete: () => { return; }
                    });
                });
            }).catch(consoleLog);
        },
        candlestickChart: function candlestickChart() {
            exist('.chart-candlestick-js').then(function (res) {
                isErrorHandled = false;
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');
                    $.ajax({
                        url: dataChart,
                        method: 'GET',
                        success: function (res) {
                            Highcharts.setOptions({
                                lang: {
                                    rangeSelectorZoom: '',
                                    thousandsSep: ','
                                }
                            });

                            Highcharts.stockChart(chart, {
                                chart: {
                                    // zoomType: 'x'
                                },
                                rangeSelector: {
                                    enabled: false,
                                    inputEnabled: false
                                },
                                title: {
                                    text: res.title
                                },
                                subtitle: {
                                    text: res.subtitle
                                },
                                navigator: {
                                    enabled: false
                                },
                                credits: {
                                    enabled: false
                                },
                                scrollbar: {
                                    enabled: false
                                },
                                exporting: {
                                    enabled: false
                                },
                                yAxis: {
                                    labels: {
                                        format: '{value:,.0f}'
                                    },
                                    opposite: false
                                },
                                xAxis: {
                                    tickInterval: 365 * 24 * 3600 * 1000
                                },
                                plotOptions: {
                                    series: {
                                        animation: false,
                                    }
                                },
                                series: [{
                                    type: 'candlestick',
                                    data: res.data,
                                    color: '#9F0E0F',
                                    upColor: '#54A661',
                                    dataGrouping: {
                                        units: [['month', [1, 3, 6]]],
                                        forced: true
                                    },
                                    tooltip: {
                                        pointFormat: 'Prev: {point.open}<br/>Hi: {point.high}<br/>Low: {point.low}<br/>Close: {point.close}<br/>'
                                    }
                                }]
                            });
                        },
                        error: () => { showChartError(chart); },
                        complete: () => { return; }
                    });
                });
            }).catch(consoleLog);
        },
        monthlyTradingVolumeValueChart: function monthlyTradingVolumeValueChart() {
            exist('.monthly-trading-volume-value-js').then(function (res) {
                isErrorHandled = false;
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');
                    $.ajax({
                        url: dataChart,
                        method: 'GET',
                        success: function (res) {
                            var seriesData = [],
                                optColors = [],
                                categories = res.categories;
                            var series = res.series;
                            series.map(function (data, i) {
                                var entry = {
                                    name: data.seriesName,
                                    data: data.seriesData
                                    // pointInterval: 30 * 24 * 3600 * 1000
                                };

                                seriesData.push(entry);
                                optColors.push(data.seriesColor);
                            });

                            Highcharts.setOptions({
                                lang: {
                                    rangeSelectorZoom: '',
                                    thousandsSep: ','
                                },
                                colors: optColors
                            });

                            Highcharts.chart(chart, {
                                chart: {
                                    zoomType: 'xy',
                                    type: 'column',
                                    marginTop: 50,
                                    marginRight: 10,
                                    height: 400,
                                    backgroundColor: 'transparent'
                                },
                                title: null,
                                credits: {
                                    enabled: false
                                },
                                exporting: {
                                    enabled: false
                                },
                                legend: {
                                    align: 'center',
                                    verticalAlign: 'bottom',
                                    x: 0,
                                    y: 0
                                },
                                xAxis: {
                                    title: null,
                                    crosshair: true,
                                    categories: categories
                                    // type: 'datetime',
                                    // tickInterval: 30 * 24 * 3600 * 1000,
                                    // dateTimeLabelFormats: {
                                    //     month: '%b<br>%Y'
                                    // },
                                },
                                yAxis: {
                                    title: null,
                                    labels: {
                                        format: '{value:,.0f}'
                                    },
                                    tickAmount: 6,
                                    tickInterval: 100
                                },
                                tooltip: {
                                    shared: true
                                },
                                series: seriesData,
                                plotOptions: {
                                    column: {
                                        dataLabels: {
                                            enabled: false
                                        }
                                    },
                                    series: {
                                        animation: false,
                                        pointPadding: 0,
                                        borderWidth: 0
                                    }
                                }
                            });
                        },
                        error: () => { showChartError(chart); },
                        complete: () => { return; }
                    });
                });
            }).catch(consoleLog);
        },
        quarterlyTradingVolumeValueChart: function quarterlyTradingVolumeValueChart() {
            exist('.quarterly-trading-volume-value-js').then(function (res) {
                isErrorHandled = false;
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');
                    $.ajax({
                        url: dataChart,
                        method: 'GET',
                        success: function (res) {
                            var seriesData = [],
                                optColors = [];
                            var series = res.series;

                            series.map(function (data, i) {
                                var entry = {
                                    name: data.seriesName,
                                    data: data.seriesData
                                };

                                seriesData.push(entry);
                                optColors.push(data.seriesColor);
                            });

                            Highcharts.setOptions({
                                lang: {
                                    rangeSelectorZoom: '',
                                    thousandsSep: ','
                                },
                                colors: optColors
                            });

                            Highcharts.chart(chart, {
                                chart: {
                                    zoomType: 'xy',
                                    type: 'column',
                                    marginTop: 50,
                                    marginRight: 10,
                                    height: 400,
                                    backgroundColor: 'transparent'
                                },
                                title: null,
                                credits: {
                                    enabled: false
                                },
                                exporting: {
                                    enabled: false
                                },
                                legend: {
                                    align: 'center',
                                    verticalAlign: 'bottom',
                                    x: 0,
                                    y: 0
                                },
                                xAxis: {
                                    title: null,
                                    crosshair: true,
                                    categories: res.categories,
                                    labels: {
                                        rotation: 90
                                    }
                                },
                                yAxis: {
                                    title: null,
                                    labels: {
                                        format: '{value:,.0f}'
                                    },
                                    tickAmount: 7
                                },
                                tooltip: {
                                    shared: true
                                },
                                series: seriesData,
                                plotOptions: {
                                    column: {
                                        dataLabels: {
                                            enabled: false
                                        }
                                    },
                                    series: {
                                        animation: false,
                                        pointPadding: 0,
                                        borderWidth: 0
                                    }
                                },
                                responsive: {
                                    rules: [{
                                        condition: {
                                            minWidth: 768
                                        },
                                        chartOptions: {
                                            xAxis: {
                                                labels: {
                                                    rotation: 0
                                                }
                                            }
                                        }
                                    }]
                                }
                            });
                        },
                        error: () => { showChartError(chart); },
                        complete: () => { return; }
                    });
                });
            }).catch(consoleLog);
        },
        totalTradingInvestorTypeQuarterlyChart: function totalTradingInvestorTypeQuarterlyChart() {
            exist('.total-trading-investor-quarterly-js').then(function (res) {
                isErrorHandled = false;
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');
                    $.ajax({
                        url: dataChart,
                        method: 'GET',
                        success: function (res) {
                            var seriesData = [],
                                optColors = [];

                            res.series.map(function (data, i) {
                                var entry = {
                                    gapSize: 1,
                                    name: data.seriesName,
                                    data: data.seriesData
                                };

                                seriesData.push(entry);
                                optColors.push(data.seriesColor);
                            });

                            Highcharts.setOptions({
                                lang: {
                                    rangeSelectorZoom: '',
                                    thousandsSep: ','
                                },
                                colors: optColors
                            });

                            Highcharts.chart(chart, {
                                chart: {
                                    zoomType: 'xy',
                                    type: 'column',
                                    // marginTop: 30,
                                    // marginRight: 10,
                                    backgroundColor: 'transparent'
                                },
                                title: null,
                                credits: {
                                    enabled: false
                                },
                                exporting: {
                                    enabled: false
                                },
                                xAxis: {
                                    title: null,
                                    crosshair: true,
                                    labels: {
                                        rotation: 90
                                    },
                                    categories: res.categories
                                },
                                yAxis: {
                                    title: null,
                                    reversedStacks: false,
                                    labels: {
                                        format: '{value}%'
                                    }
                                },
                                tooltip: {
                                    xDateFormat: '%e-%b-%y',
                                    pointFormat: '<span style="color:{point.color}">\u25CF</span> {series.name}: <b>{point.percentage:.0f}%</b><br/>'
                                },
                                series: seriesData,
                                plotOptions: {
                                    series: {
                                        animation: false,
                                    },
                                    column: {
                                        stacking: 'percent',
                                        dataLabels: {
                                            enabled: false
                                        }
                                    }
                                }
                            });
                        },
                        error: () => { showChartError(chart); },
                        complete: () => { return; }
                    });
                });
            }).catch(consoleLog);
        },
        netPurchaseForeignersQuarterlyChart: function netPurchaseForeignersQuarterlyChart() {
            exist('.net-purchase-foreigners-quarterly-js').then(function (res) {
                isErrorHandled = false;
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');
                    $.ajax({
                        url: dataChart,
                        method: 'GET',
                        success: function (res) {
                            Highcharts.setOptions({
                                lang: {
                                    rangeSelectorZoom: '',
                                    thousandsSep: ','
                                },
                                colors: ['#54A661', '#9F0E0F']
                            });

                            Highcharts.chart(chart, {
                                chart: {
                                    zoomType: 'xy',
                                    type: 'column',
                                    marginTop: 60,
                                    backgroundColor: 'transparent'
                                },
                                title: {
                                    text: ""
                                },
                                credits: {
                                    enabled: false
                                },
                                exporting: {
                                    enabled: false
                                },
                                xAxis: {
                                    title: null,
                                    labels: {
                                        rotation: 90
                                    },
                                    categories: res.categories
                                },
                                yAxis: {
                                    title: null,
                                    labels: {
                                        enabled: false
                                    }
                                },
                                tooltip: {
                                    pointFormat: '<span style="color:{point.color}">\u25CF</span> <b>{point.y:,.1f}</b><br/>'
                                },
                                series: [{
                                    gapSize: 1,
                                    name: null,
                                    data: res.data,
                                    negativeColor: Highcharts.getOptions().colors[1],
                                    showInLegend: false
                                }],
                                plotOptions: {
                                    series: {
                                        animation: false,
                                    },
                                    column: {
                                        dataLabels: {
                                            enabled: true,
                                            formatter: function formatter() {
                                                if (this.y < 0) return '<span style="color: ' + Highcharts.getOptions().colors[1] + '">' + this.y + '</span>'; else return this.y;
                                            }
                                        }
                                    }
                                }
                            });
                        },
                        error: () => { showChartError(chart); },
                        complete: () => { return; }
                    });
                });
            }).catch(consoleLog);
        },
        totalTradingValueByInvestorsChart: function totalTradingValueByInvestorsChart() {
            exist('.total-trading-value-investor-quarterly-js').then(function (res) {
                isErrorHandled = false;
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');
                    $.ajax({
                        url: dataChart,
                        method: 'GET',
                        success: function (res) {
                            var seriesData = [],
                                optColors = [];
                            var series = res.series;

                            series.map(function (data, i) {
                                var entry = {
                                    gapSize: 1,
                                    name: data.seriesName,
                                    data: data.seriesData,
                                    showInLegend: false
                                };

                                seriesData.push(entry);
                                optColors.push(data.seriesColor);
                            });

                            Highcharts.setOptions({
                                lang: {
                                    rangeSelectorZoom: '',
                                    thousandsSep: ','
                                },
                                colors: optColors
                            });
                            Highcharts.chart(chart, {
                                chart: {
                                    zoomType: 'xy',
                                    type: 'column',
                                    marginRight: 10,
                                    backgroundColor: 'transparent'
                                },
                                title: {
                                    text: "Total Trading Value by Investors",
                                    align: 'center'
                                },
                                subtitle: {
                                    text: res.chartName
                                },
                                credits: {
                                    enabled: false
                                },
                                exporting: {
                                    enabled: false
                                },
                                xAxis: {
                                    title: {
                                        text: 'Type of Investor',
                                        align: 'high',
                                        style: {
                                            fontWeight: 'bold'
                                        }
                                    },
                                    crosshair: true,
                                    categories: res.categories
                                },
                                yAxis: {
                                    title: {
                                        text: 'Percentage',
                                        align: 'high',
                                        offset: 0,
                                        rotation: 0,
                                        y: -15,
                                        style: {
                                            fontWeight: 'bold'
                                        }
                                    },
                                    labels: {
                                        format: '{value:.2f}%'
                                    }
                                },
                                tooltip: {
                                    pointFormat: '<span style="color:{point.color}">\u25CF</span> {series.name}: <b>{point.y:.2f}%</b><br/>'
                                },
                                series: seriesData,
                                plotOptions: {
                                    series: {
                                        animation: false,
                                    },
                                    column: {
                                        dataLabels: {
                                            enabled: false
                                        }
                                    }
                                }
                            });
                        },
                        error: () => { showChartError(chart); },
                        complete: () => { return; }
                    });
                });
            }).catch(consoleLog);
        }
    };

    _extends(charts);
});