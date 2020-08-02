$(function () {

    var tb = $("#achievement-area").first();

    var count = tb.find("tbody").find("tr").length;

    for (var i = 0; i < count; i++) {

        var newTR = tb.find("tr").eq(i).html().replace(/\[\d+\]/g, '[' + i + ']').replace(/_\d+/g, '_' + i);

        tb.find("tr").eq(i).html(newTR);
    }
})
function addAchievement() {
    var index = $("#achievement-area").find("tbody").find("tr").length;
    var HTML = ('<tr><td class="col-12 pl-0"><textarea rows="4" autocomplete="off" id="AthleteAchievements_{}" class="form-control w-100" name="AthleteAchievements[{}]" placeholder="Type your achievement"></textarea></td><td><a href="javascript:void(0)" onclick="removeAchievement(this)">Remove</a></td></tr>').replace(/{}/g, index);
    $("#achievement-area").find("tbody").append(HTML);
}
function removeAchievement(e) {

    $(e).parents("tr").remove();

    var tb = $("#achievement-area").first();

    var count = tb.find("tbody").find("tr").length;

    for (var i = 0; i < count; i++) {

        var newTR = tb.find("tr").eq(i).html().replace(/\[\d+\]/g, '[' + i + ']').replace(/_\d+/g, '_' + i);

        tb.find("tr").eq(i).html(newTR);
    }
}

function editSubmit() {
    $('#loading-panel').removeAttr('hidden');

    var form = document.getElementById('EditProfile');
    var formData = new FormData(form);

    $.ajax({
        type: 'POST',
        url: '../UserProfile/Edit',
        data: formData,
        cache: false,
        processData: false,
        contentType: false,
        dataType: 'html',
        success: function (data) {
            var str = data.toString();
            if (str.indexOf("<!--This is the login layout-->") == 0) {
                alert("The system detects that you have not operated for a long time, please login again");
                document.clear();
                location.reload();
            }
            else {
                var EXECUTEDSUCCESS = str.indexOf("<!--UserProfileIndex-->") == 0;

                switch (EXECUTEDSUCCESS) {
                    case true:
                        $('#body-content').html(data);
                        SuccessTurnToParentPage("Success", true);
                        return;
                    case false:
                        $('#body-content').html(data);
                        SuccessTurnToParentPage("Failed", false);
                        return;
                }
            }
        },
        error: function (xhr) {
            $("#body-content").html(xhr.responseText);
        }
    });
}