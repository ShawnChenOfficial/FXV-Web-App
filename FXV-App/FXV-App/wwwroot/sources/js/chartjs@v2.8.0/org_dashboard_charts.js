//Test bar charts

var year = new Date().getFullYear();
var barChart1_labels = [], barChart2_labels = [];
var dataMale = [], dataFemale = [];
var dataTwoYearsBef = [], dataOneYearBef = [], dataCurrentYear = [];

$('#canvas1').html('<div class="content p-0 mt-3"><div class="container-fluid"><div class="row"><div class="col-12"><div class="card"><div class="card-body col-12 text-center" style="padding-top:calc(100vh/2 - 4rem)"><h4>Loading...</h4><img style="width:2rem;height:2rem" src="/sources/img/loading.gif" alt=""/></div></div></div></div></div>');
$('#canvas2').html('<div class="content p-0 mt-3"><div class="container-fluid"><div class="row"><div class="col-12"><div class="card"><div class="card-body col-12 text-center" style="padding-top:calc(100vh/2 - 4rem)"><h4>Loading...</h4><img style="width:2rem;height:2rem" src="/sources/img/loading.gif" alt=""/></div></div></div></div></div>');


$.ajax({
    url: '../home/GetDashboardTestBarChartsData',
    data: { year },
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
