function getOverallRatingData(stats) {
    var value = stats.OverallRating;

    return {
        chart: {
            type: 'pie',
            height: 200,
            width: 200
        },
        title: {
            text: "Overall Rating",
            style: {
                fontSize: "14px"
            }
        },
        plotOptions: {
            pie: {
                dataLabels: [{
                    enabled: false
                }],
                endAngle: value <= 0 ? 0 : value / 5 * 360,
                innerSize: "80%",
                size: "100%"
            }
        },
        yAxis: {
            labels: {
                enabled: false
            }
        },
        tooltip: { enabled: false },
        series: [{
            label: {
                enabled: false
            },
            data: [
                { y: value }
            ]
        }],
        value: value.toFixed(1)
    }
}