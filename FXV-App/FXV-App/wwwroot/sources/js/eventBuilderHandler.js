
function removeAllAttendeesOnGenderChanged() {
    $('#attendees-area').find('tr').remove();
}

function addAttendees() {
    var action = "SearchUsers";
    var index = $("#attendees-area").find("tbody").find("tr").length;
    var HTML = ('<tr class="row pb-2"><td class="col-8"><input autocomplete="off" id="Attendees_{}_FullName" class="w-100 form-control" name="Attendees[{}].FullName" type="text" value="" onkeyup="input_onkeyup(this,\'' + action + '\')" onfocus="input_onfocus(this)" onfocusout="input_onfocusout(this)" type="text" value="" placeholder="Search athlete by name"><ul class="border border-dark col-12 list-group p-0" hidden=""></ul><div hidden=""><input id="Attendees_{}__Id" name="Attendees[{}].Id" hidden=""></div></td><td class="col-4"><a href="javascript:void(0)" onclick="removeAttendees(this)">Remove</a></td></tr>').replace(/{}/g, index);
    $("#attendees-area").find("tbody").append(HTML);
}
function removeAttendees(e) {

    $(e).parents("tr").remove();

    var tb = $("#attendees-area").first();

    var count = tb.find("tbody").find("tr").length;

    for (var i = 0; i < count; i++) {

        var newTR = tb.find("tr").eq(i).html().replace(/\[\d+\]/g, '[' + i + ']').replace(/_\d+/g, '_' + i);

        tb.find("tr").eq(i).html(newTR);
    }
}
function submit() {
    $('#loading-panel').removeAttr('hidden');

    var form = document.getElementById('CreateEvents');
    var formData = new FormData(form);

    $.ajax({
        type: 'POST',
        url: '../Events/CreateEvents',
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
                var EXECUTEDSUCCESS = str.indexOf("<!--Events-->") == 0;

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

function editSubmit() {

    $('#loading-panel').removeAttr('hidden');

    var form = document.getElementById('EditEvent');
    var formData = new FormData(form);

    $.ajax({
        type: 'POST',
        url: '../Events/Edit',
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
                var EXECUTEDSUCCESS = str.indexOf("<!--Events-->") == 0;

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

var emptyImgUrl = $('#testImg').attr('src');


$(function () {
    $('#eventImg').css('border-radius', '8px');
    $('#eventImg').css('min-height', ($('#eventImg').width() * 0.55));
    $('#eventImg').css('max-height', ($('#eventImg').width() * 0.55));

    var tb = $("#attendees-area").first();

    var count = tb.find("tbody").find("tr").length;

    for (var i = 0; i < count; i++) {

        var newTR = tb.find("tr").eq(i).html().replace(/\[\d+\]/g, '[' + i + ']').replace(/_\d+/g, '_' + i);

        tb.find("tr").eq(i).html(newTR);
    }
});
$(window).resize(function () {
    $('#eventImg').css('min-height', ($('#eventImg').width() * 0.55));
    $('#eventImg').css('max-height', ($('#eventImg').width() * 0.55));
});
function imgCheck(img) {
    if (img.value == null || img.value == "") {
        img.value = "";
        $('#eventImg').attr('src', emptyImgUrl);
    }
    else {
        var fileSize = 0;
        fileSize = img.files[0].size;
        var size = fileSize / 1024;
        if (size > 10000) {
            alert("The image exceeds the limit of 10MB");
            img.value = "";
            $('#eventImg').attr('src', emptyImgUrl);
            return false;
        }
        else {
            var name = img.value;
            var fileName = name.substring(name.lastIndexOf(".") + 1).toLowerCase();
            if (fileName != "jpg" && fileName != "jpeg" && fileName != "png" && fileName != "jpg") {
                alert("Only accpet the format of JPG, JPEG or PNG");
                img.value = "";
                $('#eventImg').attr('src', emptyImgUrl);
                return false;
            }
            else {
                var url = getImgLocalUrl(img.files[0]);
                $('#eventImg').attr('src', url);
            }
        }
    }
}

function getImgLocalUrl(file) {
    var url = null;
    if (window.createObjectURL != undefined) {
        url = window.createObjectURL(file);
    } else if (window.URL != undefined) {
        //webkit or chrome ie11 ie10 firefox oprea
        url = window.URL.createObjectURL(file);
    } else if (window.webkitURL != undefined) { // webkit or chrome

        url = window.webkitURL.createObjectURL(file);
    }
    return url;
}