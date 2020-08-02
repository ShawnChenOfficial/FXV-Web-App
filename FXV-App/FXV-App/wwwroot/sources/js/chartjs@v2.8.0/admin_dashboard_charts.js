//Test bar charts

var year = new Date().getFullYear();
var barChart1_labels = [], barChart2_labels = [];
var dataMale = [], dataFemale = [];
var dataTwoYearsBef = [], dataOneYearBef = [], dataCurrentYear = [];


$.ajax({
    url: '../home/GetDashboardTestBarChartsData',
    data: {year},
    type: 'POST',
    cache: false,
    dataType: 'json',
    success: function (data) {

        $.each(data.TestByGenderList, function (index, item) {
            barChart1_labels.push(item.Label);
        });

        $.each(data.TestByGenderList, function (index, item) {
            dataMale.push(item.MaleData);
        });

        $.each(data.TestByGenderList, function (index, item) {
            dataFemale.push(item.FemaleData);
        });

        $.each(data.TestByYearsList, function (index, item) {
            barChart2_labels.push(item.Label);
        })

        $.each(data.TestByYearsList, function (index, item) {
            dataTwoYearsBef.push(item.DataTwoYearsBefore);
        })

        $.each(data.TestByYearsList, function (index, item) {
            dataOneYearBef.push(item.DataOneYearBefore);
        })

        $.each(data.TestByYearsList, function (index, item) {
            dataCurrentYear.push(item.DataCurrentYear);
        })


        initializeTestCharts()
    }

    //error?

});

function initializeTestCharts() {
    var color = Chart.helpers.color;

    var barChartData1 = {
        labels: barChart1_labels,
        datasets: [{
            label: 'Male',
            backgroundColor: color(window.chartColors.yellow).alpha(0.5).rgbString(),
            borderColor: window.chartColors.yellow,
            borderWidth: 1,
            data: dataMale
        }, {
            label: 'Female',
            backgroundColor: color(window.chartColors.orange).alpha(0.5).rgbString(),
            borderColor: window.chartColors.orange,
            borderWidth: 1,
            data: dataFemale
        }]

    };

    var barChartData2 = {
        labels: barChart2_labels,
        datasets: [{
            label: year - 2,
            backgroundColor: color(window.chartColors.yellow).alpha(0.5).rgbString(),
            borderColor: window.chartColors.yellow,
            borderWidth: 1,
            data: dataTwoYearsBef
        }, {
            label: year - 1,
            backgroundColor: color(window.chartColors.orange).alpha(0.5).rgbString(),
            borderColor: window.chartColors.orange,
            borderWidth: 1,
            data: dataOneYearBef
        }, {
            label: year,
            backgroundColor: color(window.chartColors.red).alpha(0.5).rgbString(),
            borderColor: window.chartColors.red,
            borderWidth: 1,
            data: dataCurrentYear
        }
        ]

    };

    var ctx1 = document.getElementById('canvas1').getContext('2d');
    var ctx2 = document.getElementById('canvas2').getContext('2d');
    window.myBar = new Chart(ctx1, {
        type: 'bar',
        data: barChartData1,
        options: {
            responsive: true,
            legend: {
                position: 'top',
            },
            title: {
                fontColor: '#FFF',
                display: true,
                text: 'Participants Per Test By Gender',
                fontSize: 16
            }
        }
    });
    window.myBar = new Chart(ctx2, {
        type: 'bar',
        data: barChartData2,
        options: {
            responsive: true,
            legend: {
                position: 'top',
            },
            title: {
                fontColor: '#FFF',
                display: true,
                text: 'Participants Per Test By Year',
                fontSize: 16
            }
        }
    });
}




// Number of new members

var month = new Date().getMonth() + 1;
var labels = [], numbers = [];

$.ajax({
    url: '../home/GetDashboardMembersData',
    data: {
        year,
        month
    },
    type: 'POST',
    cache: false,
    dataType: 'json',
    success: function (data) {

        $.each(data, function (index, item) {
            labels.push(item.Label);
            numbers.push(item.Number);
        });


        initializeMembers();
    }
});

function initializeMembers() {
    var presets = window.chartColors;
    var utils = Samples.utils;

    var options = {
        maintainAspectRatio: false,
        spanGaps: false,
        elements: {
            line: {
                tension: 0.000001
            }
        },
        plugins: {
            filler: {
                propagate: false
            }
        },
        scales: {
            xAxes: [{
                ticks: {
                    autoSkip: false,
                    maxRotation: 0
                }
            }]
        }
    };

    [false, 'origin', 'start', 'end'].forEach(function (boundary, index) {

        // reset the random seed to generate the same data for all charts
        utils.srand(8);

        new Chart('chart-' + index, {
            type: 'line',
            data: {
                labels: labels,
                datasets: [{
                    backgroundColor: utils.transparentize(presets.red),
                    borderColor: presets.red,
                    data: numbers,
                    label: 'Number Of New Members',
                    fill: boundary
                }]
            },
            options: Chart.helpers.merge(options, {
                title: {
                    fontColor: '#FFF',
                    display: true,
                    text: 'Number Of New Members ' + year,
                    fontSize: 16
                }
            })
        });
    });

}

