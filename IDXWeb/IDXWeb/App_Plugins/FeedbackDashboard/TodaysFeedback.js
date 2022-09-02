function getTodaysFeedbackData(items, start) {
    var values = [];
    var date = moment(start, 'DD/MM/YYYY').add(1, 'day');
    while (date <= moment()) {
        var itemByDate = items.find(o => o.Date === date.format('DD/MM/YYYY'));
        var count = typeof itemByDate !== 'undefined' ? itemByDate.Count : 0;
        values.push({ timestamp: date.valueOf(), value: count });
        date = date.clone().add(1, 'd');
    }

    return {
        time: {
            timezone: 'Asia/Jakarta'
        },
        chart: {
            type: 'areaspline',
            height: 200
            //width: 400
        },
        title: {
            text: "Today's Feedback",
            style: {
                fontSize: "14px"
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size: 10px">{point.key:%A, %d %b %Y}</span><br/>',
            pointFormat: '<span>Feedback</span>: <b>{point.y}</b><br/>'
        },
        legend: {
            enabled: false
        },
        xAxis: {
            gridLineWidth: 0,
            labels: {
                enabled: false
            },
            tickLength: 0
        },
        yAxis: {
            gridLineWidth: 0,
            labels: {
                enabled: false
            },
            title: {
                enabled: false
            }
        },
        series: [{
            data: values.map(o => ({
                x: o.timestamp,
                y: o.value
            }))
        }]
    };
}