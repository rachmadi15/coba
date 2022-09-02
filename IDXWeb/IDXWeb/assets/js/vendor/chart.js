import * as _ from './utils.js'

export default {
	stockChart() {
		_.exist('.chart-stock-js').then(res => {
			const $chartContainer = $(res)
			$.each($chartContainer, function(index, chart) {
				const dataChart = $(this).attr('data-chart');

				$.getJSON(dataChart, function(res, textStatus) {
					/*optional stuff to do after success */

					Highcharts.setOptions({
				        lang:{
				            rangeSelectorZoom: ''
				        }
					});

					Highcharts.stockChart(chart, {
				        rangeSelector: {
				            enabled: false,
				            inputEnabled: false,
				        },

				        chart: {
				        	height: 130,
				        	spacing: [0,0,0,0],
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
		}).catch(_.noop)
	},

    compositeStockPriceIndexChart() {
        _.exist('.composite-stock-price-index-js').then(res => {
            const $chartContainer = $(res)
            $.each($chartContainer, function(index, chart) {
                const dataChart = $(this).attr('data-chart');

                $.getJSON(dataChart, function(res, textStatus) {
                    let seriesData = [], optColors = [];

                    res.series.map((data,i) => {

                        let entry = {
                            name: data.seriesName,
                            type: data.type,
                            yAxis: data.yAxis,
                            data: data.seriesData.map(dt => ({
                                x: new Date(dt.x).getTime(),
                                y: dt.y
                            })),
                            tooltip: {
                                valueSuffix: data.valueSuffix
                            }
                        }

                        seriesData.push(entry)
                        optColors.push(data.seriesColor)
                    })

                    Highcharts.setOptions({
                        lang:{
                            rangeSelectorZoom: '',
                            thousandsSep: ',',
                        },
                        colors: optColors
                    });

                    Highcharts.stockChart(chart, {
                        chart: {
                            zoomType: 'xy'
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
                        xAxis: {
                            crosshair: true
                        },
                        yAxis: [
                            {
                                title: null,
                                labels: {
                                    format: '{value:,.0f}',
                                    style: {
                                        color: Highcharts.getOptions().colors[0]
                                    }
                                },
                                opposite: false
                            },
                            {
                                gridLineWidth: 0,
                                title: null,
                                labels: {
                                    format: '{value:,.0f}'
                                },
                                opposite: true
                            }
                        ],
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
                                    units: [['day',[1]]]
                                }
                            }
                        },
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
        }).catch(_.noop)
    },

    dailyCompositeStockPriceIndexChart() {
        _.exist('.chart-composite-js').then(res => {
            const $chartContainer = $(res)
            $.each($chartContainer, function(index, chart) {
                const dataChart = $(this).attr('data-chart');

                $.getJSON(dataChart, function(res, textStatus) {
                    let seriesData = [], optColors = [];

                    res.series.map((data, i) => {
                        let entry = {
                            name: data.seriesName,
                            type: data.type,
                            yAxis: data.yAxis,
                            data: data.seriesData
                        };

                        seriesData.push(entry);
                        optColors.push(data.seriesColor);
                    });

                    Highcharts.setOptions({
                        lang:{
                            rangeSelectorZoom: '',
                            thousandsSep: ',',
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
                        yAxis: [
                            {
                                title: null,
                                labels: {
                                    format: '{value:,.0f}',
                                    style: {
                                        color: Highcharts.getOptions().colors[1],
                                    }
                                },
                                tickAmount: 6
                            },
                            {
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
                            }
                        ],
                        tooltip: {
                            shared: true,
                            headerFormat: 'Day {point.key}<br>'
                        },
                        series: seriesData,
                        plotOptions: {
                            series: {
                                pointPadding: 0,
                                borderWidth: 0,
                            }
                        },
                    });
                });

            })
        }).catch(_.noop)
    },

    jakartaCompositeIndexChart() {
        _.exist('.jakarta-composite-index-js').then(res => {
            const $chartContainer = $(res)
            $.each($chartContainer, function(index, chart) {
                const dataChart = $(this).attr('data-chart');

                $.getJSON(dataChart, function(res, textStatus) {
                    let seriesData = [], optColors = [];

                    res.map((data, i) => {
                        let entry = {
                            name: data.seriesName,
                            type: 'line',
                            data: data.seriesData,
                            tooltip: {
                                valueSuffix: '%',
                                valueDecimals: 2
                            },
                            pointInterval: 24 * 3600 * 1000
                        };

                        seriesData.push(entry);
                        optColors.push(data.seriesColor);
                    });

                    Highcharts.setOptions({
                        lang: {
                            rangeSelectorZoom: '',
                        },
                        colors: optColors
                    });

                    Highcharts.chart(chart, {
                        chart: {
                            zoomType: 'xy',
                        },
                        title: {
                            text: 'Jakarta Composite Index and Sectoral Indices Movement',
                            align: 'center'
                        },
                        subtitle: {
                            text: '30 December 2016 - 29 December 2017'
                        },
                        credits: {
                            enabled: false
                        },
                        exporting: {
                            enabled: false
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
                            type: 'datetime',
                            tickInterval: 30 * 24 * 3600 * 1000 //30 days (1 month)
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
                                format: '{value}%',
                            }
                        },
                        tooltip: {
                            xDateFormat: '%e-%b-%y',
                            shared: true
                        },
                        series: seriesData,
                        plotOptions: {
                            line: {
                                marker: {
                                    enabled: false
                                },
                                pointStart: Date.UTC(2016,11,30)
                            }
                        },
                        responsive: {
                            rules: [{
                                condition: {
                                    maxWidth: 768
                                },
                                chartOptions: {
                                    legend: {
                                        floating: false,
                                        layout: 'horizontal',
                                        align: 'center',
                                        verticalAlign: 'bottom',
                                        x: 0,
                                        y: 0
                                    },
                                    xAxis: [{
                                        tickInterval: 2 * 30 * 24 * 3600 * 1000 // 2 months
                                    }]
                                }
                            }]
                        }
                    });
                });
            });
        }).catch(_.noop)
    },

    idxIndices12MonthChart() {
        _.exist('.idx-indices-12-month-js').then(res => {
            const $chartContainer = $(res)
            $.each($chartContainer, function(index, chart) {
                const dataChart = $(this).attr('data-chart');

                $.getJSON(dataChart, function(res, textStatus) {
                    Highcharts.setOptions({
                        lang:{
                            rangeSelectorZoom: '',
                            thousandsSep: ',',
                        },
                        colors: [
                            '#4a8ccc',
                            '#C3C3C3'
                        ]
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
                        yAxis: [
                            {
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
                                },
                            },
                            {
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
                                opposite: true,
                            }
                        ],
                        tooltip: {
                            xDateFormat: '%e-%b-%y',
                            shared: true
                        },
                        series: [
                            {
                                name: 'Trading Volume (LHS)',
                                type: 'column',
                                data: res.tradingVolumeData,
                                tooltip: {
                                    // valueSuffix: ' mm'
                                },
                                color: Highcharts.getOptions().colors[1],
                                pointInterval: 24 * 3600 * 1000,
                                showInLegend: false
                            },
                            {
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
                            }
                        ],
                        plotOptions: {
                            line: {
                                marker: {
                                    enabled: false
                                },
                                pointStart: Date.UTC(2018,2,1)
                            },
                            column: {
                                pointStart: Date.UTC(2018,2,1),
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
        }).catch(_.noop)
    },

    dailyTradingChart() {
        _.exist('.daily-trading-js').then(res => {
            const $chartContainer = $(res)
            $.each($chartContainer, function(index, chart) {
                const dataChart = $(this).attr('data-chart');

                $.getJSON(dataChart, function(res, textStatus) {
                    let seriesData = [], optColors = [];

                    res.series.map((data, i) => {
                        let entry = {
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
                            tickInterval: 24 * 3600 * 1000,
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
                            reversedStacks: false,
                        },
                        tooltip: {
                            xDateFormat: '%e-%b-%y',
                        },
                        series: seriesData,
                        plotOptions: {
                            column: {
                                stacking: 'normal',
                                dataLabels: {
                                    enabled: false
                                },
                                pointStart: Date.UTC(2019,2,1)
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
                                            x: 10,
                                        },
                                    },
                                    yAxis: {
                                        labels: {
                                            format: '{value:,.0f}',
                                        }
                                    }
                                }
                            }]
                        }
                    });
                });
            });
        }).catch(_.noop)
    },

    totalTradingInvestorTypeChart() {
        _.exist('.total-trading-investor-js').then(res => {
            const $chartContainer = $(res)
            $.each($chartContainer, function(index, chart) {
                const dataChart = $(this).attr('data-chart');

                $.getJSON(dataChart, function(res, textStatus) {
                    let seriesData = [], optColors = [];

                    res.series.map((data, i) => {
                        let entry = {
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
                            tickInterval: 24 * 3600 * 1000,
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
                                format: '{value}%',
                            }
                        },
                        tooltip: {
                            xDateFormat: '%e-%b-%y',
                            pointFormat: '<span style="color:{point.color}">\u25CF</span> {series.name}: <b>{point.percentage:.0f}%</b><br/>',
                        },
                        series: seriesData,
                        plotOptions: {
                            column: {
                                stacking: 'percent',
                                dataLabels: {
                                    enabled: false
                                },
                                pointStart: Date.UTC(2019,2,1)
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
                                            x: 10,
                                        },
                                    }
                                }
                            }]
                        }
                    });
                });
            });
        }).catch(_.noop)
    },

    netPurchaseForeignersChart() {
        _.exist('.net-purchase-foreigners-js').then(res => {
            const $chartContainer = $(res)
            $.each($chartContainer, function(index, chart) {
                const dataChart = $(this).attr('data-chart');

                $.getJSON(dataChart, function(res, textStatus) {
                    let seriesData = [];

                    Highcharts.setOptions({
                        lang: {
                            rangeSelectorZoom: '',
                            thousandsSep: ','
                        },
                        colors: [
                            '#54A661',
                            '#9F0E0F',
                        ]
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
                            tickInterval: 24 * 3600 * 1000,
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
                                format: '{value:,.0f}',
                            },
                            max: 1000,
                            min: -1000
                        },
                        tooltip: {
                            xDateFormat: '%e-%b-%y',
                            pointFormat: '<span style="color:{point.color}">\u25CF</span> <b>{point.y:,.0f}</b><br/>',
                        },
                        series: [
                            {
                                gapSize: 1,
                                name: null,
                                data: res.data,
                                negativeColor: Highcharts.getOptions().colors[1],
                                pointInterval: 24 * 3600 * 1000,
                                showInLegend: false
                            }
                        ],
                        plotOptions: {
                            column: {
                                dataLabels: {
                                    enabled: false
                                },
                                pointStart: Date.UTC(2019,2,1)
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
                                            x: 10,
                                        },
                                    }
                                }
                            }]
                        }
                    });
                });
            });
        }).catch(_.noop)
    },

    tradingMarketTypeChart() {
        _.exist('.trading-market-js').then(res => {
            const $chartContainer = $(res)
            $.each($chartContainer, function(index, chart) {
                const dataChart = $(this).attr('data-chart');

                $.getJSON(dataChart, function(res, textStatus) {
                    let seriesData = [], optColors = [];
                    let series = res.series;
                    // vertical sum
                    let sums = series.map(v => v.seriesData).reduce((r,a) => r.map((b, i) => a[i] + b));

                    series.map((data, i) => {
                        let entry = {
                            gapSize: 1,
                            name: data.seriesName,
                            data: data.seriesData.map((v,i) => v / sums[i] * 100),
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
                                format: '{value:.2f}%',
                            }
                        },
                        tooltip: {
                            xDateFormat: '%b-%y',
                            pointFormat: '<span style="color:{point.color}">\u25CF</span> {series.name}: <b>{point.y:.2f}%</b><br/>',
                        },
                        series: seriesData,
                        plotOptions: {
                            column: {
                                dataLabels: {
                                    enabled: false
                                },
                                pointStart: Date.UTC(2017,9,1)
                            }
                        }
                    });
                });
            });
        }).catch(_.noop)
    },

    barsIndustryChart() {
        _.exist('.bars-industry-js').then(res => {
            const $chartContainer = $(res)
            $.each($chartContainer, function(index, chart) {
                const dataChart = $(this).attr('data-chart');

                $.getJSON(dataChart, function(res, textStatus) {
                    let seriesData = [], optColors = [];

                    res.series.map((data, i) => {
                        let entry = {
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
                                format: '{value}%',
                            },
                            tickInterval: 5
                        },
                        tooltip: {
                            xDateFormat: '%b-%y',
                            pointFormat: '<span style="color:{point.color}">\u25CF</span> {series.name}: <b>{point.y:.2f}%</b><br/>',
                        },
                        series: seriesData,
                        plotOptions: {
                            column: {
                                dataLabels: {
                                    enabled: false
                                },
                                pointStart: Date.UTC(2017,9,1)
                            }
                        }
                    });
                });
            });
        }).catch(_.noop)
    },

    candlestickChart() {
        _.exist('.chart-candlestick-js').then(res => {
            const $chartContainer = $(res)
            $.each($chartContainer, function(index, chart) {
                const dataChart = $(this).attr('data-chart');

                $.getJSON(dataChart, function(res, textStatus) {
                    // let data = res.filter(el => {
                    //     return new Date(el[0]).getDate() <= 5
                    // })

                    Highcharts.setOptions({
                        lang:{
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
                            inputEnabled: false,
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
                                units: [
                                    [
                                        'month',
                                        [1, 2, 3]
                                    ]
                                ],
                                forced: true
                            },
                            tooltip: {
                                pointFormat: `Open: {point.open}<br/>Hi: {point.high}<br/>Low: {point.low}<br/>Close: {point.close}<br/>`
                            }
                        }]
                    });
                });
            });
        }).catch(_.noop)
    },

    monthlyTradingVolumeValueChart() {
        _.exist('.monthly-trading-volume-value-js').then(res => {
            const $chartContainer = $(res)
            $.each($chartContainer, function(index, chart) {
                const dataChart = $(this).attr('data-chart');

                $.getJSON(dataChart, function(res, textStatus) {
                    let seriesData = [], optColors = [], categories = [];
                    let series = res.series;
                    let startMonth = new Date(res.startMonth), endMonth = new Date(res.endMonth)
                    let yearCounter = null, startYear = startMonth.getFullYear();
                    let monthCount = ((endMonth.getFullYear() - startMonth.getFullYear() + 1) * 12) - (startMonth.getMonth() + endMonth.getMonth() + 1)

                    series.map((data, i) => {
                        let entry = {
                            name: data.seriesName,
                            data: data.seriesData,
                            // pointInterval: 30 * 24 * 3600 * 1000
                        };

                        seriesData.push(entry);
                        optColors.push(data.seriesColor);
                    });

                    for (let i=0; i<monthCount; i++) {
                        let year = new Date(startYear,i+startMonth.getMonth()).getFullYear()
                        let month = new Date(startYear,i+startMonth.getMonth()).toLocaleString('en', {month:'short'})
                        if (yearCounter !== year) {
                            yearCounter = year
                            categories.push(`${month}<br>${yearCounter}`)
                        } else categories.push(month)
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
                                format: '{value:,.0f}',
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
                                borderWidth: 0,
                            }
                        }
                    });
                });
            });
        }).catch(_.noop)
    },

    quarterlyTradingVolumeValueChart() {
        _.exist('.quarterly-trading-volume-value-js').then(res => {
            const $chartContainer = $(res)
            $.each($chartContainer, function(index, chart) {
                const dataChart = $(this).attr('data-chart');

                $.getJSON(dataChart, function(res, textStatus) {
                    let seriesData = [], optColors = [], categories = [];
                    let series = res.series;

                    series.map((data, i) => {
                        let entry = {
                            name: data.seriesName,
                            data: data.seriesData,
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
                                format: '{value:,.0f}',
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
                                borderWidth: 0,
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
        }).catch(_.noop)
    },

    totalTradingInvestorTypeQuarterlyChart() {
        _.exist('.total-trading-investor-quarterly-js').then(res => {
            const $chartContainer = $(res)
            $.each($chartContainer, function(index, chart) {
                const dataChart = $(this).attr('data-chart');

                $.getJSON(dataChart, function(res, textStatus) {
                    let seriesData = [], optColors = [];

                    res.series.map((data, i) => {
                        let entry = {
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
                                format: '{value}%',
                            }
                        },
                        tooltip: {
                            xDateFormat: '%e-%b-%y',
                            pointFormat: '<span style="color:{point.color}">\u25CF</span> {series.name}: <b>{point.percentage:.0f}%</b><br/>',
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
        }).catch(_.noop)
    },

    netPurchaseForeignersQuarterlyChart() {
        _.exist('.net-purchase-foreigners-quarterly-js').then(res => {
            const $chartContainer = $(res)
            $.each($chartContainer, function(index, chart) {
                const dataChart = $(this).attr('data-chart');

                $.getJSON(dataChart, function(res, textStatus) {
                    Highcharts.setOptions({
                        lang: {
                            rangeSelectorZoom: '',
                            thousandsSep: ','
                        },
                        colors: [
                            '#54A661',
                            '#9F0E0F',
                        ]
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
                            pointFormat: '<span style="color:{point.color}">\u25CF</span> <b>{point.y:,.1f}</b><br/>',
                        },
                        series: [
                            {
                                gapSize: 1,
                                name: null,
                                data: res.data,
                                negativeColor: Highcharts.getOptions().colors[1],
                                showInLegend: false
                            }
                        ],
                        plotOptions: {
                            column: {
                                dataLabels: {
                                    enabled: true,
                                    formatter: function() {
                                        if (this.y < 0) return `<span style="color: ${Highcharts.getOptions().colors[1]}">${this.y}</span>`
                                        else return this.y
                                    }
                                }
                            }
                        }
                    });
                });
            });
        }).catch(_.noop)
    },

    totalTradingValueByInvestorsChart() {
        _.exist('.total-trading-value-investor-quarterly-js').then(res => {
            const $chartContainer = $(res)
            $.each($chartContainer, function(index, chart) {
                const dataChart = $(this).attr('data-chart');

                $.getJSON(dataChart, function(res, textStatus) {
                    let seriesData = [], optColors = [];
                    let series = res.series;

                    series.map((data, i) => {
                        let entry = {
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
                                format: '{value:.2f}%',
                            }
                        },
                        tooltip: {
                            pointFormat: '<span style="color:{point.color}">\u25CF</span> {series.name}: <b>{point.y:.2f}%</b><br/>',
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
        }).catch(_.noop)
    },
}
