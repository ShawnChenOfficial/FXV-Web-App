
$.ajax({
    url: '../home/GetDashboardIndividualTestData',
    type: 'POST',
    cache: false,
    dataType: 'json',
    success: function (data) {

        $("#dashboard_charts").html("");

        if (data.length > 6) {
            $("#dashboard_charts").css({ display: "inline-flex", overflow: "auto" }).removeClass("row");
        }

        $.each(data, function (index, item) {
            var id = "chartdiv_" + index;
            $("#dashboard_charts").append('<div class="col-lg-2 col-md-4 col-6 pb-4" style="height:400px"><div id = "' + id + '" ></div></div>');
            create_chart(id, item.Test_Title, item.Result, item.Unit, item.Top_Result);
        });
    },
    error: function () {
        $("#leaderboard_list_body").html('<div class="content p-0 row"><div class="container col-10 mt-5"><h1 class="text-FXV">Opps</h1><br /><h2 class="text-white">An error occurred while processing your request.</h2></div></div>');
    }
});


function create_chart(id,test_title,result,unit,top_Result) {
    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_kelly);
        am4core.useTheme(am4themes_animated);
        // Themes end

        var chart = am4core.create(id, am4charts.XYChart);
        chart.hiddenState.properties.opacity = 0; // this creates initial fade-in

        chart.data = [
            {
                test: test_title,
                result: result,
                unit: unit
            }
        ];
        chart.color = "#ffffff";

        var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
        categoryAxis.renderer.grid.template.location = 0;
        categoryAxis.dataFields.category = "test";
        categoryAxis.renderer.minGridDistance = 40;
        categoryAxis.renderer.opposite = true;
        categoryAxis.renderer.grid.template.disabled = true;

        var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
        valueAxis.min = 0;
        valueAxis.max = top_Result;
        valueAxis.strictMinMax = true;
        valueAxis.renderer.grid.template.disabled = true;
        valueAxis.renderer.labels.template.disabled = true;

        var series = chart.series.push(new am4charts.ColumnSeries());
        series.dataFields.categoryX = "test";
        series.dataFields.valueY = "result";
        series.columns.template.tooltipText = "{valueY.value}" + "{unit}";
        series.columns.template.tooltipY = 0;
        series.columns.template.strokeOpacity = 0;

        var gradient = new am4core.LinearGradient();
        gradient.addColor(am4core.color("#ffdd00"));
        gradient.addColor(am4core.color("#101010"));
        gradient.rotation = 90;

        series.columns.template.fill = gradient;
        series.columns.template.column.cornerRadiusTopLeft = 20;
        series.columns.template.column.cornerRadiusTopRight = 20;
        series.columns.template.column.fillOpacity = 0.8;


        var labelBullet = series.bullets.push(new am4charts.LabelBullet());
        labelBullet.label.text = "[bold font-size:0.7rem]{valueY}" + "{unit}";
        labelBullet.label.fill = am4core.color("#fff");
        labelBullet.locationY = 0;
        labelBullet.label.maxWidth = 150;
        labelBullet.label.wrap = true;
        labelBullet.label.truncate = false;
        labelBullet.label.textAlign = "middle";
        labelBullet.label.verticalCenter = "bottom";
        labelBullet.label.paddingTop = 20;
        labelBullet.label.paddingBottom = 20;

    });
}

