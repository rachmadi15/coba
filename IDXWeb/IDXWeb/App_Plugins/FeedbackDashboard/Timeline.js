function getTickPositions(start, end) {
    var ticks = [];
    var date = start.clone();
    while (date <= end) {
        ticks.push(date.valueOf());
        date = date.clone().add(1, 'M');
    }
    return ticks;
}

function getFeedbackTimelineData(items, visitorTypes, start, end) {
    var series = [];
    start = start.startOf('day');
    end = end.endOf('day');

    for (var i = 0; i < visitorTypes.length; i++) {
        var date = start.clone();
        var data = { name: visitorTypes[i].Name, data: [] };
        var itemsByVisitorType = items.find(o => o.VisitorType === data.name);

        while (date <= end) {
            var count = 0;
            if (typeof itemsByVisitorType !== 'undefined') {
                var itemByDate = itemsByVisitorType.Items.find(o => o.Date === date.format('DD/MM/YYYY'));
                count = typeof itemByDate !== 'undefined' ? itemByDate.Count : 0;
            };
            data.data.push({ x: date.valueOf(), y: count });
            date = date.clone().add(1, 'd');
        }
        series.push(data);
    }

    return {
        chart: {
            type: "area",
            height: 300
            //width: 800
        },
        time: {
            timezone: 'Asia/Jakarta'
        },
        title: {
            text: "Feedback History"
        },
        plotOptions: {
            area: {
                fillOpacity: 0,
                lineWidth: 1,
                marker: {
                    enabled: false,
                    states: {
                        hover: {
                            radiusPlus: 0.2
                        }
                    }
                }
            }
        },
        series: series,
        tooltip: {
            headerFormat: '<span style="font-size: 10px">{point.key:%A, %d %b %Y}</span><br/>'
        },
        xAxis: {
            labels: {
                formatter(tick) {
                    return moment(tick.value).format("D MMM YY");
                }
            },
            min: start.valueOf(),
            max: end.valueOf(),
            tickPositions: getTickPositions(start, end),
            title: {
                text: 'Timeline'
            },
            type: 'datetime'
        },
        yAxis: {
            min: 0,
            tickInterval: 1,
            title: {
                text: 'Feedback'
            }
        }
    };
}