function addOrg() {
    var action = "SearchOrgs";
    var index = $("#orgs").find("tbody").find("tr").length;
    var HTML = ('<tr class="row pb-2"><td class="col-8 col-xl-7"><input autocomplete="off" id="Organizations_{}__Name" class="w-100 form-control" name="Organizations[{}].Name" onkeyup="input_onkeyup(this,\'' + action + '\')" onfocus="input_onfocus(this)" onfocusout="input_onfocusout(this)" type="text" value="" placeholder="Search organization by name"><ul class="border border-dark col-12 list-group p-0" hidden=""></ul><div hidden=""><input id="Organizations_{}__Org_ID" name="Organizations[{}].Org_ID" hidden=""></div ></td><td class="col-4 col-xl-5"><a href="javascript:void(0)" onclick="removeOrgs(this)">Remove</a></td></tr>').replace(/{}/g, index);
    $("#orgs").find("tbody").append(HTML);
}
function removeOrgs(e) {
    $(e).parents("tr").remove();
    var tb = $("#orgs").first();
    var count = tb.find("tbody").find("tr").length;
    for (var i = 0; i < count; i++) {
        var newTR = tb.find("tr").eq(i).html().replace(/\[\d+\]/g, '[' + i + ']').replace(/_\d+/g, '_' + i);
        tb.find("tr").eq(i).html(newTR);
    }
} function addTeams() {
    var action = "SearchTeams";
    var index = $("#teams").find("tbody").find("tr").length;
    var HTML = ('<tr class="row pb-2"><td class="col-8 col-xl-7"><input autocomplete="off" id="Teams_{}__Name" class="w-100 form-control" name="Teams[{}].Name" onkeyup="input_onkeyup(this,\'' + action + '\')" onfocus="input_onfocus(this)" onfocusout="input_onfocusout(this)" type="text" value="" placeholder="Search team by name"><ul class="border border-dark col-12 list-group p-0" hidden=""></ul><div hidden=""><input id="Teams_{}__Team_ID" name="Teams[{}].Team_ID" hidden=""></div ></td><td class="col-4 col-xl-5"><a href="javascript:void(0)" onclick="removeTeams(this)">Remove</a></td></tr>').replace(/{}/g, index);
    $("#teams").find("tbody").append(HTML);
}
function removeTeams(e) {
    $(e).parents("tr").remove();
    var tb = $("#teams").first();
    var count = tb.find("tbody").find("tr").length;
    for (var i = 0; i < count; i++) {
        var newTR = tb.find("tr").eq(i).html().replace(/\[\d+\]/g, '[' + i + ']').replace(/_\d+/g, '_' + i);
        tb.find("tr").eq(i).html(newTR);
    }
}
function submit() {

    $('#loading-panel').removeAttr('hidden');

    var form = document.getElementById('CreateTests');
    var formData = new FormData(form);

    $.ajax({
        type: 'POST',
        url: '../Tests/CreateTests',
        data: formData,
        cache: false,
        async: true,
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
                var EXECUTEDSUCCESS = str.indexOf("<!--Tests-->") == 0;

                console.log(str.indexOf("<!--Tests-->"));

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
    $('#loading-panel').attr('hidden', 'hidden');
}

function editSubmit() {

    $('#loading-panel').removeAttr('hidden');

    var form = document.getElementById('EditTest');
    var formData = new FormData(form);

    $.ajax({
        type: 'POST',
        url: '../Tests/Edit',
        data: formData,
        cache: false,
        async: true,
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
                var EXECUTEDSUCCESS = str.indexOf("<!--Tests-->") == 0;

                console.log(str.indexOf("<!--Tests-->"));

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

    $('#loading-panel').attr('hidden', 'hidden');
}

var emptyImgUrl = $('#testImg').attr('src');


$(function () {
    $('#testImg').css('border-radius', '8px');
    $('#testImg').css('min-height', ($('#testImg').width() * 0.55));
    $('#testImg').css('max-height', ($('#testImg').width() * 0.55));
});
$(window).resize(function () {
    $('#testImg').css('min-height', ($('#testImg').width() * 0.55));
    $('#testImg').css('max-height', ($('#testImg').width() * 0.55));
});
function imgCheck(img) {
    if (img.value == null || img.value == "") {
        img.value = "";
        $('#testImg').attr('src', emptyImgUrl);
    }
    else {
        var fileSize = 0;
        fileSize = img.files[0].size;
        var size = fileSize / 1024;
        if (size > 10000) {
            alert("The image exceeds the limit of 10MB");
            img.value = "";
            $('#testImg').attr('src', emptyImgUrl);
            return false;
        }
        else {
            var name = img.value;
            var fileName = name.substring(name.lastIndexOf(".") + 1).toLowerCase();
            if (fileName != "jpg" && fileName != "jpeg" && fileName != "png" && fileName != "jpg") {
                alert("Only accpet the format of JPG, JPEG or PNG");
                img.value = "";
                $('#testImg').attr('src', emptyImgUrl);
                return false;
            }
            else {
                var url = getImgLocalUrl(img.files[0]);
                $('#testImg').attr('src', url);
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
