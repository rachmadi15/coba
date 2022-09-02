var sliders = null;
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
        $(chart).html('\n        <div class="chart-error">\n            Data Not Found\n        </div>\n    ');
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

    var _sliders = {
        mainSlider: function () {
            exist('.main-slider').then(function (res) {
                var $slider = $(res);
                $slider.slick({
                    autoplay: true,
                    autoplaySpeed: 2000,
                    arrows: false
                });
            }).catch(consoleLog);
        },
        holidaySlider: function () {
            exist('.holiday-slider').then(function (res) {
                var $slider = $(res);
                $slider.slick({
                    autoplay: false,
                    autoplaySpeed: 2000,
                    nextArrow: $('.btn-next-icon'),
                    prevArrow: $('.btn-prev-icon')
                });
            }).catch(consoleLog);
        },
        chartSlider: function () {
            exist('.chart-slider').then(function (res) {
                var $slider = $(res);
                var chartInit = function (chart) {
                    var dataChart = $(chart).attr('data-chart');
                    if (typeof dataChart === 'undefined') return;
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
                            navigator: {
                                enabled: false
                            },
                            legend: {
                                enabled: true
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
                                    x: 10,
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

                        $(chart).addClass('loaded');
                    }).catch(showChartError(chart));
                };

                var onChange = function (event, slick) {
                    var currSlideNo = slick.currentSlide;
                    var currChart = $(slick.$slides[currSlideNo]).find('.idx-indices-12-month-js')[0];
                    //var currChart = slick.$slider.find('.idx-indices-12-month-js')[slick.currentSlide];

                    if (!$(currChart).hasClass('loaded')) {
                        chartInit(currChart);
                    }
                };

                $slider.unbind('init afterChange');
                $slider.on('init afterChange', onChange);

                $slider.slick({
                    autoplay: false,
                    infinite: false,
                    appendArrows: $('.chart-slider__nav'),
                    slidesToShow: 1,
                    slidesToScroll: 1
                });
                
            }).catch(consoleLog);
        },
        benchmarkSlider: function () {
            exist('.benchmark-quotation-slider').then(function (res) {
                var $slider = $(res);
                var $search = $('.search-instrument');

                $search.on('submit', function (e) {
                    e.preventDefault();
                    var selectedSlide = $('#selectInstrument')[0].value;
                    if (selectedSlide && selectedSlide != -1) $slider.slick('slickGoTo', selectedSlide);
                });

                $slider.slick({
                    autoplay: false,
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    swipe: false,
                    prevArrow: '<button class="slick-prev slick-arrow" aria-label="Previous"><span class="fa fa-angle-left"></span></button>',
                    nextArrow: '<button class="slick-next slick-arrow" aria-label="Next"><span class="fa fa-angle-right"></span></button>'
                });
            }).catch(consoleLog);
        }
    };
    sliders = _sliders;

    _extends(_sliders);
});