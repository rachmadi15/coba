function getStatsByCategories(items, start, end) {
    return {
        chart: {
            type: 'pie',
            height: 200,
            width: 200
        },
        plotOptions: {
            pie: {
                dataLabels: [{
                    enabled: false
                }],
                innerSize: "60%",
                size: "100%"
            }
        },
        title: {
            text: "Stats by Category",
            margin: 5,
            style: {
                fontSize: "12px"
            }
        },
        series: [{
            data: [
                { name: "test1", y: 1 },
                { name: "test2", y: 2 },
                { name: "test3", y: 3 },
                { name: "test4", y: 4 },
                { name: "test5", y: 5 },
                { name: "test6", y: 3 }
            ]
        }]
    };
}