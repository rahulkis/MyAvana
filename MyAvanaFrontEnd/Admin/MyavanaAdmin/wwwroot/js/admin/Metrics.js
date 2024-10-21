google.charts.load('current', { packages: ['corechart'] });

function drawChart() {
    // Define the chart to be drawn.
    var data = new google.visualization.DataTable();
    data.addColumn('string', 'Browser');
    data.addColumn('number', 'Percentage');
    data.addRows([
        ['Firefox', 20],
        ['IE', 40],
        ['Chrome', 60],
        ['Safari', 20],
        ['Opera', 10],
        ['Others', 30]
    ]);

    // Set chart options
    options = {
        pieSliceText: 'value', 'width': 550,
        'height': 400

    };
    //options.pieSliceTextStyle = { fontSize: 20 };
    // Instantiate and draw the chart.
    var chart = new google.visualization.PieChart(document.getElementById('rating'));
    chart.draw(data, options);
}
google.charts.setOnLoadCallback(drawChart);