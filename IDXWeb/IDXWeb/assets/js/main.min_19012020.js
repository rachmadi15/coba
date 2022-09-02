var Site = (function () {
    'use strict';

    function _toConsumableArray(arr) { if (Array.isArray(arr)) { for (var i = 0, arr2 = Array(arr.length); i < arr.length; i++) { arr2[i] = arr[i]; } return arr2; } else { return Array.from(arr); } }

    function noop() { }







































    function queryAll(context) {
        return function (selector) {
            return [].concat(_toConsumableArray(context.querySelectorAll(selector)));
        };
    }




    //var table = $('#example').DataTable();

    //// #myInput is a <input type="text"> element
    //$('#myInput').on('keyup', function () {
    //    table.search(this.value).draw();
    //});






    function exist(selector) {
        return new Promise(function (resolve, reject) {
            var elems = queryAll(document)(selector);

            if (elems.length) resolve(elems);
            reject('no element found for ' + selector);
        });
    }

    var activeStateMobile = function () {
        document.addEventListener('touchstart', noop, true);
    };

    var WPViewportFix = function () {
        var isIEMobile = '-ms-user-select' in document.documentElement.style && navigator.userAgent.match(/IEMobile/);

        if (!isIEMobile) return;

        var style = document.createElement('style');
        var fix = document.createTextNode('@-ms-viewport{width:auto!important}');
        style.appendChild(fix);
        document.head.appendChild(style);
    };

    var objectFitPolyfill = function () {
        objectFitImages();
    };

    var slider = {
        mainSlider: function mainSlider() {
            exist('.main-slider').then(function (res) {
                var $slider = $(res);
                $slider.slick({
                    autoplay: true,
                    autoplaySpeed: 2000,
                    arrows: false
                });
            }).catch(noop);
        },
        holidaySlider: function holidaySlider() {
            exist('.holiday-slider').then(function (res) {
                var $slider = $(res);
                $slider.slick({
                    autoplay: false,
                    autoplaySpeed: 2000,
                    nextArrow: $('.btn-next-icon'),
                    prevArrow: $('.btn-prev-icon')
                });
            }).catch(noop);
        }
    };

    var chart = {
        stockChart: function stockChart() {
            exist('.chart-stock-js').then(function (res) {
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');

                    $.getJSON(dataChart, function (res, textStatus) {
                        /*optional stuff to do after success */

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
                    });
                });
            }).catch(noop);
        },
        compositeStockPriceIndexChart: function compositeStockPriceIndexChart() {
            exist('.composite-stock-price-index-js').then(function (res) {
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');

                    $.getJSON(dataChart, function (res, textStatus) {
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
                                crosshair: true
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
                                    format: '{value:,.0f}'
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
                                    dataGrouping: {
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
                    });
                });
            }).catch(noop);
        },
        dailyCompositeStockPriceIndexChart: function dailyCompositeStockPriceIndexChart() {
            exist('.chart-composite-js').then(function (res) {
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');

                    $.getJSON(dataChart, function (res, textStatus) {
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
                                min: 5000,
                                tickAmount: 7
                            }],
                            tooltip: {
                                shared: true,
                                headerFormat: 'Day {point.key}<br>'
                            },
                            series: seriesData,
                            plotOptions: {
                                series: {
                                    pointPadding: 0,
                                    borderWidth: 0
                                }
                            }
                        });
                    });
                });
            }).catch(noop);
        },
        jakartaCompositeIndexChart: function jakartaCompositeIndexChart() {
        exist('.jakarta-composite-index-js').then(function (res) {
            var $chartContainer = $(res);
            $.each($chartContainer, function (index, chart) {
                var dataChart = $(this).attr('data-chart');

                $.getJSON(dataChart, function (res, textStatus) {
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
                            zoomType: 'xy'
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
                            title: {
                                text: 'Percentage',
                                align: 'high',
                                offset: 0,
                                rotation: 0,
                                y: -10,
                                style: {
                                    fontWeight: 'bold'
                                }
                            },
                            labels: {
                                format: '{value}%'
                            },
                            opposite: false
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
                                    chart: {
                                        marginTop: 100
                                    },
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
                                            offset: -30
                                        }
                                    }
                                }
                            }]
                        }
                    });
                });
            });
        }).catch(noop);
    },
        idxIndices12MonthChart: function idxIndices12MonthChart() {
            exist('.idx-indices-12-month-js').then(function (res) {
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');

                    $.getJSON(dataChart, function (res, textStatus) {
                        Highcharts.setOptions({
                            lang: {
                                rangeSelectorZoom: '',
                                thousandsSep: ','
                            },
                            colors: ['#4a8ccc', '#C3C3C3']
                        });

                        Highcharts.chart(chart, {
                            chart: {
                                zoomType: 'xy'
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
                            xAxis: [{
                                crosshair: true,
                                type: 'datetime',
                                tickInterval: 30 * 24 * 3600 * 1000 //30 days (1 month)
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
                                        color: Highcharts.getOptions().colors[1]
                                    }
                                }
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
                                        color: Highcharts.getOptions().colors[0]
                                    }
                                },
                                opposite: true
                            }],
                            tooltip: {
                                xDateFormat: '%e-%b-%y',
                                shared: true
                            },
                            series: [{
                                name: 'Trading Volume (LHS)',
                                type: 'column',
                                data: res.tradingVolumeData,
                                tooltip: {
                                    // valueSuffix: ' mm'
                                },
                                color: Highcharts.getOptions().colors[1],
                                pointInterval: 24 * 3600 * 1000,
                                showInLegend: false
                            }, {
                                name: 'Index (RHS)',
                                type: 'line',
                                yAxis: 1,
                                data: res.indexData,
                                tooltip: {
                                    // valueSuffix: ' °C'
                                },
                                color: Highcharts.getOptions().colors[0],
                                pointInterval: 24 * 3600 * 1000, // 24 hours (1 day)
                                showInLegend: false
                            }],
                            plotOptions: {
                                line: {
                                    marker: {
                                        enabled: false
                                    },
                                    pointStart: Date.UTC(2018, 2, 1)
                                },
                                column: {
                                    pointStart: Date.UTC(2018, 2, 1),
                                    borderWidth: 0
                                },
                                series: {
                                    pointPadding: 0,
                                    groupPadding: 0,
                                    borderWidth: 0,
                                    shadow: false
                                }
                            },
                            responsive: {
                                rules: [{
                                    condition: {
                                        maxWidth: 768
                                    },
                                    chartOptions: {
                                        xAxis: [{
                                            tickInterval: 2 * 30 * 24 * 3600 * 1000 // 2 months
                                        }]
                                    }
                                }]
                            }
                        });
                    });
                });
            }).catch(noop);
        },
        dailyTradingChart: function dailyTradingChart() {
            exist('.daily-trading-js').then(function (res) {
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');

                    $.getJSON(dataChart, function (res, textStatus) {
                        var seriesData = [],
                            optColors = [];

                        res.series.map(function (data, i) {
                            var entry = {
                                gapSize: 1,
                                name: data.seriesName,
                                data: data.seriesData,
                                pointInterval: 24 * 3600 * 1000
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
                                marginTop: 30,
                                marginRight: 10,
                                backgroundColor: 'transparent'
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
                            xAxis: {
                                title: {
                                    text: 'Date',
                                    align: 'high',
                                    style: {
                                        fontWeight: 'bold'
                                    }
                                },
                                crosshair: true,
                                type: 'datetime',
                                dateTimeLabelFormats: {
                                    day: '%d-%b-%y'
                                },
                                tickInterval: 24 * 3600 * 1000
                                // breaks: [{
                                //     // break 3 Maret
                                //     from: Date.UTC(2019,2,2), //from 2 Maret
                                //     to:  Date.UTC(2019,2,4), // to 2 Maret
                                //     breakSize: 24 * 3600 * 1000 // 1 day
                                // }]
                            },
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
                                reversedStacks: false
                            },
                            tooltip: {
                                xDateFormat: '%e-%b-%y'
                            },
                            series: seriesData,
                            plotOptions: {
                                column: {
                                    stacking: 'normal',
                                    dataLabels: {
                                        enabled: false
                                    },
                                    pointStart: Date.UTC(2019, 2, 1)
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
                    });
                });
            }).catch(noop);
        },
        totalTradingInvestorTypeChart: function totalTradingInvestorTypeChart() {
            exist('.total-trading-investor-js').then(function (res) {
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');

                    $.getJSON(dataChart, function (res, textStatus) {
                        var seriesData = [],
                            optColors = [];

                        res.series.map(function (data, i) {
                            var entry = {
                                gapSize: 1,
                                name: data.seriesName,
                                data: data.seriesData,
                                pointInterval: 24 * 3600 * 1000
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
                                    text: 'Date',
                                    align: 'high',
                                    style: {
                                        fontWeight: 'bold'
                                    }
                                },
                                crosshair: true,
                                type: 'datetime',
                                dateTimeLabelFormats: {
                                    day: '%e'
                                },
                                tickInterval: 24 * 3600 * 1000
                                // breaks: [{
                                //     // break 3 Maret
                                //     from: Date.UTC(2019,2,2), //from 2 Maret
                                //     to:  Date.UTC(2019,2,4), // to 2 Maret
                                //     breakSize: 24 * 3600 * 1000 // 1 day
                                // }]
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
                                column: {
                                    stacking: 'percent',
                                    dataLabels: {
                                        enabled: false
                                    },
                                    pointStart: Date.UTC(2019, 2, 1)
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
                                            }
                                        }
                                    }
                                }]
                            }
                        });
                    });
                });
            }).catch(noop);
        },
        netPurchaseForeignersChart: function netPurchaseForeignersChart() {
            exist('.net-purchase-foreigners-js').then(function (res) {
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');

                    $.getJSON(dataChart, function (res, textStatus) {
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
                                    text: 'Date',
                                    align: 'high',
                                    style: {
                                        fontWeight: 'bold'
                                    }
                                },
                                crosshair: true,
                                type: 'datetime',
                                dateTimeLabelFormats: {
                                    day: '%e'
                                },
                                tickInterval: 24 * 3600 * 1000
                                // breaks: [{
                                //     // break 3 Maret
                                //     from: Date.UTC(2019,2,2), //from 2 Maret
                                //     to:  Date.UTC(2019,2,4), // to 2 Maret
                                //     breakSize: 24 * 3600 * 1000 // 1 day
                                // }]
                            },
                            yAxis: {
                                title: {
                                    text: 'b. IDR',
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
                                max: 1000,
                                min: -1000
                            },
                            tooltip: {
                                xDateFormat: '%e-%b-%y',
                                pointFormat: '<span style="color:{point.color}">\u25CF</span> <b>{point.y:,.0f}</b><br/>'
                            },
                            series: [{
                                gapSize: 1,
                                name: null,
                                data: res.data,
                                negativeColor: Highcharts.getOptions().colors[1],
                                pointInterval: 24 * 3600 * 1000,
                                showInLegend: false
                            }],
                            plotOptions: {
                                column: {
                                    dataLabels: {
                                        enabled: false
                                    },
                                    pointStart: Date.UTC(2019, 2, 1)
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
                                            }
                                        }
                                    }
                                }]
                            }
                        });
                    });
                });
            }).catch(noop);
        },
        tradingMarketTypeChart: function tradingMarketTypeChart() {
            exist('.trading-market-js').then(function (res) {
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');

                    $.getJSON(dataChart, function (res, textStatus) {
                        var seriesData = [],
                            optColors = [];
                        var series = res.series;
                        // vertical sum
                        var sums = series.map(function (v) {
                            return v.seriesData;
                        }).reduce(function (r, a) {
                            return r.map(function (b, i) {
                                return a[i] + b;
                            });
                        });

                        series.map(function (data, i) {
                            var entry = {
                                gapSize: 1,
                                name: data.seriesName,
                                data: data.seriesData.map(function (v, i) {
                                    return v / sums[i] * 100;
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

                        Highcharts.chart(chart, {
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
                                tickInterval: 30 * 24 * 3600 * 1000
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
                                xDateFormat: '%b-%y',
                                pointFormat: '<span style="color:{point.color}">\u25CF</span> {series.name}: <b>{point.y:.2f}%</b><br/>'
                            },
                            series: seriesData,
                            plotOptions: {
                                column: {
                                    dataLabels: {
                                        enabled: false
                                    },
                                    pointStart: Date.UTC(2017, 9, 1)
                                }
                            }
                        });
                    });
                });
            }).catch(noop);
        },
        barsIndustryChart: function barsIndustryChart() {
            exist('.bars-industry-js').then(function (res) {
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');

                    $.getJSON(dataChart, function (res, textStatus) {
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
                                column: {
                                    dataLabels: {
                                        enabled: false
                                    },
                                    pointStart: Date.UTC(2017, 9, 1)
                                }
                            }
                        });
                    });
                });
            }).catch(noop);
        },
        candlestickChart: function candlestickChart() {
            exist('.chart-candlestick-js').then(function (res) {
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');

                    $.getJSON(dataChart, function (res, textStatus) {
                        // let data = res.filter(el => {
                        //     return new Date(el[0]).getDate() <= 5
                        // })

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
                                text: 'Jakarta Composite Stock Price Index'
                            },
                            subtitle: {
                                text: '2005 - Q2 2019'
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
                            series: [{
                                type: 'candlestick',
                                name: 'Jakarta Composite Stock Price Index',
                                data: res,
                                color: '#9F0E0F',
                                dataGrouping: {
                                    units: [['month', [1, 2, 3]]],
                                    forced: true
                                },
                                tooltip: {
                                    pointFormat: 'Open: {point.open}<br/>Hi: {point.high}<br/>Low: {point.low}<br/>Close: {point.close}<br/>'
                                }
                            }]
                        });
                    });
                });
            }).catch(noop);
        },
        monthlyTradingVolumeValueChart: function monthlyTradingVolumeValueChart() {
            exist('.monthly-trading-volume-value-js').then(function (res) {
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');

                    $.getJSON(dataChart, function (res, textStatus) {
                        var seriesData = [],
                            optColors = [],
                            categories = [];
                        var series = res.series;
                        var startMonth = new Date(res.startMonth),
                            endMonth = new Date(res.endMonth);
                        var yearCounter = null,
                            startYear = startMonth.getFullYear();
                        var monthCount = (endMonth.getFullYear() - startMonth.getFullYear() + 1) * 12 - (startMonth.getMonth() + endMonth.getMonth() + 1);

                        series.map(function (data, i) {
                            var entry = {
                                name: data.seriesName,
                                data: data.seriesData
                                // pointInterval: 30 * 24 * 3600 * 1000
                            };

                            seriesData.push(entry);
                            optColors.push(data.seriesColor);
                        });

                        for (var i = 0; i < monthCount; i++) {
                            var year = new Date(startYear, i + startMonth.getMonth()).getFullYear();
                            var month = new Date(startYear, i + startMonth.getMonth()).toLocaleString('en', { month: 'short' });
                            if (yearCounter !== year) {
                                yearCounter = year;
                                categories.push(month + '<br>' + yearCounter);
                            } else categories.push(month);
                        }

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
                                height: 200,
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
                                verticalAlign: 'top',
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
                                    pointPadding: 0,
                                    borderWidth: 0
                                }
                            }
                        });
                    });
                });
            }).catch(noop);
        },
        quarterlyTradingVolumeValueChart: function quarterlyTradingVolumeValueChart() {
            exist('.quarterly-trading-volume-value-js').then(function (res) {
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');

                    $.getJSON(dataChart, function (res, textStatus) {
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
                                height: 200,
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
                                verticalAlign: 'top',
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
                    });
                });
            }).catch(noop);
        },
        totalTradingInvestorTypeQuarterlyChart: function totalTradingInvestorTypeQuarterlyChart() {
            exist('.total-trading-investor-quarterly-js').then(function (res) {
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');

                    $.getJSON(dataChart, function (res, textStatus) {
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
                                column: {
                                    stacking: 'percent',
                                    dataLabels: {
                                        enabled: false
                                    }
                                }
                            }
                        });
                    });
                });
            }).catch(noop);
        },
        netPurchaseForeignersQuarterlyChart: function netPurchaseForeignersQuarterlyChart() {
            exist('.net-purchase-foreigners-quarterly-js').then(function (res) {
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');

                    $.getJSON(dataChart, function (res, textStatus) {
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
                                text: "Net Purchase by Foreginers 2013 - Q2 2019"
                            },
                            subtitle: {
                                text: "trillion IDR"
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
                    });
                });
            }).catch(noop);
        },
        totalTradingValueByInvestorsChart: function totalTradingValueByInvestorsChart() {
            exist('.total-trading-value-investor-quarterly-js').then(function (res) {
                var $chartContainer = $(res);
                $.each($chartContainer, function (index, chart) {
                    var dataChart = $(this).attr('data-chart');

                    $.getJSON(dataChart, function (res, textStatus) {
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
                                column: {
                                    dataLabels: {
                                        enabled: false
                                    }
                                }
                            }
                        });
                    });
                });
            }).catch(noop);
        }
    };

    var tabMenu = {
        tabStockInfo: function tabStockInfo() {
            exist('.tab-anchor-js').then(function (res) {
                var $tabAnchor = $(res);
                var $tableContent = $('.table-information-content');

                $tabAnchor.on('click', function (event) {
                    event.preventDefault();
                    event.stopPropagation();
                    /* Act on the event */
                    $tabAnchor.removeClass('is-active');
                    $tableContent.removeClass('is-active');

                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');
                });
            }).catch(noop);
        },
        tabMarketOverview: function tabMarketOverview() {
            exist('.tab-market-js').then(function (res) {
                var $tabAnchor = $(res);
                var $chartContent = $('.market-overview-chart-stock');

                $tabAnchor.on('click', function (event) {
                    event.preventDefault();
                    event.stopPropagation();
                    /* Act on the event */
                    $chartContent.removeClass('is-active');
                    $tabAnchor.removeClass('is-active');
                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');
                });
            }).catch(noop);
        },
        tabTop5: function tabTop5() {
            exist('.top-5-title-js').then(function (res) {
                var $tabAnchor = $(res);
                var $tableContent = $('.top-5-content-js');

                $tabAnchor.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $tableContent.removeClass('is-active');
                    $tabAnchor.removeClass('is-active');
                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');
                });
            }).catch(noop);
        },
        tabPeople: function tabPeople() {
            exist('.people-tab-js').then(function (res) {
                var $tabPeople = $(res);
                var $peopleContent = $('.people-container');
                var moreText = $('[data-more]').attr('data-more');
                var closeText = $('[data-more]').attr('data-close');

                $tabPeople.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $peopleContent.removeClass('is-active');
                    $tabPeople.removeClass('is-active');
                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');

                    $('.hof-figcaption-description').each(function (index, el) {
                        if ($(this).height() > 108) {
                            $(this).addClass('hof-figcaption-description--limit');
                            $(this).parent().append('<a href="" class="hof-more is-active">' + moreText + '</a><a href="" class="hof-close">' + closeText + '</a>');
                        }
                    });
                });
            }).catch(noop);
        },
        tabHistory: function tabHistory() {
            exist('.history-tab-js').then(function (res) {
                var $tabHistory = $(res);
                var $historyContent = $('.history-container');

                $tabHistory.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $historyContent.removeClass('is-active');
                    $tabHistory.removeClass('is-active');
                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');
                });
            }).catch(noop);
        },
        tabStatistic: function tabStatistic() {
            exist('.statistic-tab-js').then(function (res) {
                var $tabStatistic = $(res);
                var $statisticContent = $('.statistic-container');

                $tabStatistic.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $statisticContent.removeClass('is-active');
                    $tabStatistic.removeClass('is-active');
                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');
                });
            }).catch(noop);
        },
        tabEtpTrading: function tabEtpTrading() {
            exist('.etp-trading-tab-js').then(function (res) {
                var $tabEtp = $(res);
                var $etpContent = $('.etp-trading-container');

                $tabEtp.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $etpContent.removeClass('is-active');
                    $tabEtp.removeClass('is-active');
                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');
                });
            }).catch(noop);
        },
        exchangeTrade: function exchangeTrade() {
            exist('.exchange-trade-tab-js').then(function (res) {
                var $tabExchange = $(res);
                var $exchangeContent = $('.exchange-trade-container');

                $tabExchange.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $exchangeContent.removeClass('is-active');
                    $tabExchange.removeClass('is-active');
                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');
                });
            }).catch(noop);
        },
        tabTradingSummary: function tabTradingSummary() {
            exist('.trading-summary-title-js').then(function (res) {
                var $tabAnchor = $(res);
                var $tableContent = $('.trading-summary-content-js');

                $tabAnchor.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $tableContent.removeClass('is-active');
                    $tabAnchor.removeClass('is-active');
                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');
                });
            }).catch(noop);
        },
        companyProfileTab: function companyProfileTab() {
            exist('.company-profile-anchor-js').then(function (res) {
                var $tabAnchor = $(res);
                var $tableContent = $('.company-profile-content');

                $tabAnchor.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $tableContent.removeClass('is-active');
                    $tabAnchor.removeClass('is-active');
                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');
                });
            }).catch(noop);
        },
        companyProfileDropdown: function companyProfileDropdown() {
            exist('.financial-report-js').then(function (res) {
                var $dropdown = $(res);
                var $contentContainer = $('.financial-report-content');

                $dropdown.on('change', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $contentContainer.removeClass('is-active');
                    var otherElem = $dropdown.val();
                    $(otherElem).addClass('is-active');
                });
            }).catch(noop);
        }
    };

    var navMenu = {
        openMobileNav: function openMobileNav() {
            exist('.open-nav-js').then(function (res) {
                var $openNav = $(res);
                var $mobileNav = $('.header-main-mobile-nav');

                $openNav.on('click', function (event) {
                    event.preventDefault();
                    event.stopPropagation();
                    /* Act on the event */
                    $mobileNav.toggleClass('is-active');
                    $(this).toggleClass('fa-times');
                    $(this).toggleClass('fa-bars');
                });
            }).catch(noop);
        },
        toggleMainNav: function toggleMainNav() {
            exist('.main-nav-js').then(function (res) {
                var $tabMenu = $(res);
                var $navItem = $('.header-main-mobile-nav-item');

                $tabMenu.on('click', function (event) {
                    event.stopPropagation();
                    /* Act on the event */
                    var $this = $(this);

                    if ($this.hasClass('is-active')) {
                        $this.removeClass('is-active');
                    } else {
                        $navItem.parents('.header-main-mobile-nav-list').find('.header-main-mobile-nav-item').removeClass('is-active');
                        $this.find('.header-main-mobile-subnav-item').removeClass('is-active');
                        $this.addClass('is-active');
                        $this.slideDown();
                    }
                });
            }).catch(noop);
        },
        toggleSubNav: function toggleSubNav() {
            exist('.main-subnav-js').then(function (res) {
                var $tabSubnav = $(res);
                var $subNavItem = $('.header-main-mobile-subnav-item');

                $tabSubnav.on('click', function (event) {
                    event.preventDefault();
                    event.stopPropagation();
                    /* Act on the event */
                    var $this = $(this);
                    console.log('halo');
                    if ($this.hasClass('is-active')) {
                        $this.removeClass('is-active');
                    } else {
                        $subNavItem.parents('.header-main-mobile-subnav-list').find('.header-main-mobile-subnav-item').removeClass('is-active');
                        $this.addClass('is-active');
                        $this.slideDown();
                    }
                });
            }).catch(noop);
        },
        toggleChildNav: function toggleChildNav() {
            exist('.header-main-mobile-childnav-item').then(function (res) {
                var $tabSubnav = $(res);

                $tabSubnav.on('click', function (event) {
                    event.stopPropagation();
                    /* Act on the event */
                });
            }).catch(noop);
        }
    };

    var datePicker = {
        datePicker: function datePicker() {
            exist('.date-picker-js').then(function (res) {
                var $datePicker = $(res);
                var datepicker = $datePicker.flatpickr({
                    dateFormat: "d-m-Y",
                    onReady: function onReady(dateObj, dateStr, instance) {
                        $('.calendar-box').each(function (i, o) {
                            var $this = $(this);
                            if ($this.find('.calendar-clear').length < 1) {
                                $this.append('<a href="" class="calendar-clear">&times;</a>');
                            }
                        });
                    }
                });

                $('.calendar-box').on('click', '.calendar-clear', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $(this).parent().find('.date-picker-js').val('');
                });
            }).catch(noop);
        },
        reportSearchDatePicker: function reportSearchDatePicker() {
            exist('.report-search-datepicker-js').then(function (res) {
                var $datePicker = $(res);
                var datepicker = $datePicker.flatpickr({
                    dateFormat: "j-M-y",
                    onReady: function onReady(dateObj, dateStr, instance) {
                        $('.calendar-box').each(function (i, o) {
                            var $this = $(this);
                            if ($this.find('.calendar-clear').length < 1) {
                                $this.append('<a href="" class="calendar-clear">&times;</a>');
                            }
                        });
                    }
                });

                $('.calendar-box').on('click', '.calendar-clear', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $(this).parent().find('.report-search-datepicker-js').val('');
                });
            }).catch(noop);
        }
    };

    var accordion = {
        bodAccordion: function bodAccordion() {
            exist('.hof-accordion-js').then(function (res) {
                var $hofContainer = $(res);
                var $showMore = $('.hof-more');
                var $close = $('.hof-close');

                var moreText = $('[data-more]').attr('data-more');
                var closeText = $('[data-more]').attr('data-close');

                $('.hof-figcaption-description').each(function (index, el) {
                    if ($(this).height() > 108) {
                        $(this).addClass('hof-figcaption-description--limit');
                        $(this).parent().append('<a href="" class="hof-more is-active">' + moreText + '</a><a href="" class="hof-close">' + closeText + '</a>');
                    }
                });

                $('body').on('click', '.hof-more', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $(this).removeClass('is-active');
                    $(this).parent().find('.hof-figcaption-description').addClass('is-active');
                    $(this).parent().find('.hof-close').addClass('is-active');
                    $(this).parents('.hof-figure').addClass('is-active');
                    $(this).parents('.hof-accordion-js').addClass('main-content');
                });

                $('body').on('click', '.hof-close', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $(this).removeClass('is-active');
                    $(this).parent().find('.hof-figcaption-description').removeClass('is-active');
                    $(this).parents('.hof-figure').removeClass('is-active');
                    $(this).parent().find('.hof-more').addClass('is-active');
                    $(this).parents('.hof-accordion-js').removeClass('main-content');
                });
            }).catch(noop);
        },
        faqAccordion: function faqAccordion() {
            exist('.accordion-text').then(function (res) {
                var $faqContainer = $(res);

                $faqContainer.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $(this).toggleClass('is-active');
                    $(this).next().toggleClass('is-active');
                });
            }).catch(noop);
        },
        statisticAccordion: function statisticAccordion() {
            exist('.js-statistic-accordion').then(function (res) {
                var $accordion = $(res);

                $accordion.on('click', '.accordion-head', function (e) {
                    var $this = $(e.currentTarget);
                    var $thisTarget = $($this.data('collapse'));

                    $thisTarget.siblings().removeClass('is-active');

                    $thisTarget.toggleClass('is-active');
                });
            }).catch(noop);
        }
    };

    var filterDisplay = {
        otcTrade: function otcTrade() {
            exist('.otc-radio-js').then(function (res) {
                var $radio = $(res);
                var $otcContainer = $('.otc-filter-container');

                $radio.on('change', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $otcContainer.removeClass('is-active');
                    var filterElem = $(this).val();
                    $('#' + filterElem).addClass('is-active');
                });
            }).catch(noop);
        }
    };

    var scrollToTop = {
        scrollToTop: function scrollToTop() {
            exist('.scroll-top-container').then(function (res) {
                var $scrollContainer = $(res);
                var $scrollTopBtn = $('.scroll-top-btn');

                $(window).on('scroll', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    // console.log($(window).scrollTop())
                    if ($(window).scrollTop() > 1000) {
                        $scrollContainer.addClass('is-active');
                    } else if ($(window).scrollTop() < 1000) {
                        $scrollContainer.removeClass('is-active');
                    }
                });

                $scrollTopBtn.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $('html, body').animate({
                        scrollTop: 0
                    }, 500);
                    // $(window).scrollTop(0);
                });
            }).catch(noop);
        }
    };

    var selectInput = function () {
        $('.select2-js').select2();
    };

    var clamp = function () {
        exist('.clamp-js').then(function (res) {
            var $clamp = $(res);

            $('.clamp-js').clamp({ clamp: 3 });
        }).catch(noop);
    };

    var calendarClose = {
        calendarClose: function calendarClose() {
            exist('.calendar-pop-up-js').then(function (res) {

                $(res).on('click', function (event) {
                    event.preventDefault();
                    event.stopPropagation();
                    /* Act on the event */
                    $('.calendar-pop-up').removeClass('is-active');
                    $('.pop-up-background').removeClass('is-active');
                    $('body').removeClass('overflow-hidden');
                });
            }).catch(noop);
        }
    };

    var livetime = function () {
        exist('.livetime-js').then(function (res) {
            var $time = $(res);

            function updateTime() {
                var date = moment(new Date());
                var time = moment(date).format('dddd, DD MMM YYYY | HH:mm');
                if ($(window).width() > 400) {
                    $time.html(time + ' WIB');
                } else {
                    $time.html(time);
                }
            }

            updateTime();
            setInterval(function () {
                updateTime();
            }, 1000);
        }).catch(noop);
    };

    var countdown = function () {
        // _.exist('.countdown-js').then(res => {
        //   	const $countdown = $('.countdown-js')
        //       const data = $countdown.attr('data-countdown');
        //       const targetDate = new Date(data)
        //       const currentDate = new Date()
        //       const convertedDate = (targetDate.getTime()/1000) - (currentDate.getTime()/1000);

        //       var clock = $('.countdown-js').FlipClock(targetDate, {
        // 	clockFace: 'DailyCounter',
        //           countdown: true
        // });

        // }).catch(_.noop)
    };

    var dropdownToggle = {
        searchOptionsToggle: function searchOptionsToggle() {
            exist('.report-search').then(function (res) {
                var $searchField = $(res);

                $searchField.on('click', '.toggle-options', function (e) {
                    e.preventDefault();
                    var $this = $(e.currentTarget);
                    var $searchOptions = $('.report-search-options');
                    var $relativeToggle = $('.report-search .relative-toggle');

                    $searchOptions.toggleClass('is-active');
                    setTimeout(function () {
                        return $relativeToggle.toggleClass('is-active');
                    }, 50);
                });
            }).catch(noop);
        },
        downloadDropdown: function downloadDropdown() {
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
            }).catch(noop);
        },
        multiSelectDropdown: function multiSelectDropdown() {
            exist('.form-dropdown').then(function (res) {
                var $toggle = $(res);

                $toggle.on('click', function (e) {
                    e.stopPropagation();
                    var $this = $(e.currentTarget);
                    var targetId = $this.data('toggle');
                    var $thisTarget = $(targetId);

                    $('.form-dropdown__list').not(targetId).removeClass('is-active');
                    $thisTarget.toggleClass('is-active');
                });

                $('.form-dropdown__list').on('click', '.select-all', function (e) {

                    var $this = $(e.currentTarget);
                    var $parent = $this.parents('.form-dropdown__list');
                    var value = $this.is(':checked') ? $this.val() : 'None';

                    $parent.find('input[type="checkbox"]').prop('checked', $this.is(':checked'));
                    $parent.siblings('.form-dropdown').val(value);
                }).on('click', 'input[type="checkbox"]:not(.select-all)', function (e) {

                    var $this = $(e.currentTarget);
                    var $parent = $this.parents('.form-dropdown__list');
                    var $siblings = $parent.find('input[type="checkbox"]').not('.select-all');
                    var $checkedSiblings = $parent.find('input[type="checkbox"]:checked').not('.select-all');
                    var value = $siblings.length === $checkedSiblings.length ? $($parent.find('.select-all')[0]).val() : $checkedSiblings.length > 1 ? $checkedSiblings.length + ' selected' : $checkedSiblings.length === 1 ? $($checkedSiblings[0]).val() : 'None';

                    $parent.find('.select-all').prop('checked', $siblings.length === $checkedSiblings.length);
                    $parent.siblings('.form-dropdown').val(value);
                }).on('click', function (e) {
                    e.stopPropagation();
                });

                $(document).on('click', function (e) {
                    $('.form-dropdown__list').removeClass('is-active');
                });
            }).catch(noop);
        },
        gotoPageDropdown: function gotoPageDropdown() {
            exist('.js-select-url').then(function (res) {
                var $select = $(res);

                $select.on('change', function (e) {
                    var $this = $(e.currentTarget),
                        targetUrl = $('option:selected', $this).data('url');

                    window.location.href = './' + targetUrl;
                });
            }).catch(noop);
        }
    };

    var dataTables = {
        datatable: function datatable() {
            exist('.js-datatable').then(function (res) {
                var $dataTable = $(res);

                var formatData = function formatData(data, colInfo, row) {
                    if (colInfo.format === 'timeFrom' && row.listingDate !== row.tradingDate) return 'T:';
                    if (colInfo.format === 'timeTo' && row.listingDate !== row.tradingDate) return '-';

                    // empty cell
                    if (colInfo.columnName === null) return '';

                    // Yes/No cell
                    if (colInfo.format === 'yesNo') return !data ? 'No' : 'Yes';

                    // Check(v) / Cross (X) cell
                    if (colInfo.format === 'checkCross') return data ? '<span class="fa fa-check"></span>' : data === false ? '<span class="fa fa-times"></span>' : '';

                    // null value
                    if (data === null) return '-';

                    // no format (string)
                    if (!colInfo.format) return data;

                    // full date format (1-Jan-19)
                    if (colInfo.format === 'fullDate') {
                        var date = new Date(data);
                        return date.getDate() + '-' + date.toLocaleDateString('en', { month: 'short' }) + '-' + date.toLocaleDateString('en', { year: '2-digit' });
                    }
                    // full date format (1-Jan-2019)
                    if (colInfo.format === 'fullDateYear') {
                        var _date = new Date(data);
                        return _date.getDate() + '-' + _date.toLocaleDateString('en', { month: 'short' }) + '-' + _date.toLocaleDateString('en', { year: 'numeric' });
                    }
                    // digit date format (1/1/2019)
                    if (colInfo.format === 'fullDateDigit') {
                        var _date2 = new Date(data);
                        return _date2.getDate() + '/' + _date2.toLocaleDateString('en', { month: 'numeric' }) + '/' + _date2.toLocaleDateString('en', { year: 'numeric' });
                    }
                    // day-month format (1-Jan)
                    if (colInfo.format === 'dayMonth') {
                        var _date3 = new Date(data);
                        return _date3.getDate() + '-' + _date3.toLocaleDateString('en', { month: 'short' });
                    }
                    // month-year format (Jan-19)
                    if (colInfo.format === 'monthYear') {
                        var _date4 = new Date(data);
                        return _date4.toLocaleDateString('en', { month: 'short' }) + '-' + _date4.toLocaleDateString('en', { year: '2-digit' });
                    }
                    // month day format (Jan 02)
                    if (colInfo.format === 'monthDay') {
                        var _date5 = new Date(data);
                        return _date5.toLocaleDateString('en', { month: 'short' }) + ' ' + _date5.toLocaleDateString('en', { day: '2-digit' });
                    }
                    // month only format (Jan)
                    if (colInfo.format === 'month') {
                        var _date6 = new Date(1970, data);
                        return _date6.toLocaleDateString('en', { month: 'short' });
                    }

                    if (colInfo.format === 'nesting') {
                        return colInfo.nesting.map(function (nested) {
                            return formatData(row[nested.columnName], nested, row);
                        }).join('<br>');
                    }

                    // numeric format (123,456.78) / percent format (0.23%)
                    return (colInfo.format === 'plusMinus' && data > 0 ? '+' : '') + data.toLocaleString('en', {
                        minimumFractionDigits: colInfo.decimal,
                        maximumFractionDigits: colInfo.decimal
                    }) + (colInfo.format === 'percent' ? '%' : '');
                };

                var rowTemplate = function rowTemplate(rowData, colInfo) {
                    var children = rowData.map(function (data) {
                        var cells = colInfo.map(function (col, index) {
                            if (data[col.columnName] === undefined && col.format !== 'timeFrom' && col.format !== 'timeTo') return '<td></td>';

                            var resData = formatData(data[col.columnName], col, data);
                            return '<td class="' + col.className + '">' + resData + '</td>';
                        }).join('');

                        var row = '<tr>' + cells + '</tr>';

                        return $(row).toArray();
                    });

                    return children;
                };

                $.each($dataTable, function (index, table) {
                    var $table = $(table);
                    var src = $table.data('json');
                    var unorderable = $table.data('unorderable');
                    var targets = !unorderable ? [] : JSON.parse('[' + unorderable + ']');

                    $.getJSON(src, function (res, textStatus) {

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
                            data: res.data,
                            searching: true,
                            info: false,
                            ordering: !$table.hasClass('no-ordering'),
                            paging: !$table.hasClass('no-paging'),
                            lengthChange: false,
                            retrieve: true,
                            scrollX: false,
                            order: [],
                            columns: columns,
                            columnDefs: [{
                                targets: targets, orderable: false
                            }, {
                                targets: '_all', orderable: true
                            }],
                            createdRow: function createdRow(row, data, dataIndex, cells) {
                                if (data.childRows && data.childRows.length) {
                                    this.api().row(row).child(rowTemplate(data.childRows, res.columns)).show();
                                }
                            },
                            initComplete: function initComplete() {
                                if (!$table.hasClass('no-paging')) {
                                    var paginate = '#' + $(this).attr('id') + '_paginate';
                                    $(paginate).insertAfter($(this).closest('.dataTables_wrapper'));
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
                                                $(rows).eq(rowIdx).before((lastGroup === null ? '' : '<tr><td class="no-border"></td></tr>') + '\n                                                <tr class="sub-head text-left"><td colspan="' + colCount + '">' + group + '</td></tr>');

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
                    });
                });
            }).catch(noop);
        },
        collapsibleDatatable: function collapsibleDatatable() {
            exist('.js-datatable-collapse').then(function (res) {
                var $dataTable = $(res);

                var rowTemplate = function rowTemplate(rowData, colInfo) {
                    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

                    var children = rowData.map(function (data) {
                        var cells = colInfo.map(function (col, index) {
                            if (col.columnName === null) return '<td></td>';

                            var resData = index === 0 && typeof data.month === "number" ? months[data.month] : index === 0 ? data.month || data.quarter || data.semester : data[col.columnName] === null ? '-' : !col.format ? data[col.columnName] : data[col.columnName].toLocaleString('en', {
                                minimumFractionDigits: col.decimal,
                                maximumFractionDigits: col.decimal
                            });
                            return '<td class="' + col.className + '">' + resData + '</td>';
                        }).join('');

                        var row = '<tr class="text-right">' + cells + '</tr>';

                        return $(row).toArray();
                    });

                    return children;
                };

                $.each($dataTable, function (index, table) {
                    var $table = $(table);
                    var src = $table.data('json');

                    $.getJSON(src, function (res, textStatus) {

                        var columns = res.columns.map(function (col) {
                            return {
                                data: col.columnName,
                                className: col.className,
                                defaultContent: "",
                                render: function render(data, type, row, meta) {
                                    if (col.columnName === null) return;
                                    var asterisk = row.asterisk ? '*' : '';
                                    var resData = data === null ? '-' : !col.format ? data + asterisk : data.toLocaleString('en', {
                                        minimumFractionDigits: col.decimal,
                                        maximumFractionDigits: col.decimal
                                    });
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
                            data: res.data,
                            searching: false,
                            info: false,
                            ordering: false,
                            paging: false,
                            lengthChange: false,
                            scrollX: false,
                            order: [],
                            columns: columns,
                            createdRow: function createdRow(row, data, dataIndex) {
                                $(row).addClass('text-right text-bold parent-row');
                            }
                        });

                        $('.js-datatable-collapse tbody').on('click', 'tr.parent-row', function () {
                            var tr = $(this).closest('tr');
                            var row = datatable.row(tr);

                            if (row.child.isShown()) {
                                row.child.hide();
                            } else {
                                datatable.rows('.parent-row').nodes().to$().not(this).map(function (id, tr) {
                                    if (datatable.row(tr).child.isShown()) datatable.row(tr).child.hide();
                                });
                                row.child(rowTemplate(row.data().months, res.columns)).show();
                            }
                        });
                    });
                });
            }).catch(noop);
        },
        idxIndicesHighlightsDatatable: function idxIndicesHighlightsDatatable() {
            exist('.js-datatable-indices').then(function (res) {
                var $dataTable = $(res);

                var formatNum = function formatNum(data) {
                    return data.toLocaleString();
                };
                var formatPercent = function formatPercent(data) {
                    return data.toLocaleString({ minimumFractionDigits: 2, maximumFractionDigits: 2 }) + '%';
                };
                var formatDate = function formatDate(data) {
                    var date = new Date(data);
                    return date.toLocaleDateString('en', { month: 'short' }) + ' ' + date.toLocaleDateString('en', { day: '2-digit' });
                };
                var renderData = function renderData(data) {
                    return formatNum(data.val) + '<br>' + formatDate(data.date);
                };
                var renderChangeValue = function renderChangeValue(data) {
                    var val = formatNum(data.val);
                    var percent = formatPercent(data.percent);

                    if (data.status.indexOf('decrease') === 0) return '(' + val + ')<br>(' + percent + ')';
                    return val + '<br>' + percent;
                };
                var renderChangeRank = function renderChangeRank(data) {
                    var icon = data.status.indexOf('decrease') === 0 ? 'caret-down fa-lg' : data.status.indexOf('increase') === 0 ? 'caret-up fa-lg' : 'minus';
                    return '<i class="fa fa-' + icon + '"></i><br>' + data.rank;
                };
                var renderClass = function renderClass(td, cellData, rowData) {
                    var classes = cellData.status === "decrease" ? 'red' : cellData.status === "increase" ? 'green' : cellData.status === "decreaseMost" ? 'red bg-decreased' : cellData.status === "increaseMost" ? 'green bg-increased' : '';

                    $(td).addClass(classes);
                };

                $.each($dataTable, function (index, table) {
                    var $table = $(table);
                    var src = $table.data('json');

                    $.getJSON(src, function (res, textStatus) {

                        var datatable = $table.DataTable({
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
                    });
                });
            }).catch(noop);
        }
    };

    var tooltip = {
        cellTooltip: function cellTooltip() {
            exist('.js-table-tooltip').then(function (res) {
                var $table = $(res);
                var $tooltip = $('#tooltiptext');

                $table.on('mousemove', '.tooltip', function (e) {
                    var text = $(e.currentTarget).find('.tooltip-data').html();
                    $tooltip.html(text).animate({ left: e.pageX + 10, top: e.pageY }, 0);
                    if (!$tooltip.is(':visible')) $tooltip.show();
                });

                $table.on('mouseleave', '.tooltip', function (e) {
                    $tooltip.hide();
                });
            }).catch(noop);
        }
    };

    var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

    var App = _extends({
        activeStateMobile: activeStateMobile,
        WPViewportFix: WPViewportFix,
        objectFitPolyfill: objectFitPolyfill
    }, slider, chart, tabMenu, navMenu, datePicker, accordion, filterDisplay, scrollToTop, {
            selectInput: selectInput,
            clamp: clamp
        }, calendarClose, {
            livetime: livetime,
            countdown: countdown
        }, dropdownToggle, dataTables, tooltip);

    for (var fn in App) {
        App[fn]();
    }

    return App;

}());
