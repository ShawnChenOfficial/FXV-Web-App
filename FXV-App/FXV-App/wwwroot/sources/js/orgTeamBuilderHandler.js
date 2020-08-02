
function addMember() {
    var index = $("#members").find("tbody").find("tr").length;
    var HTML = ('<tr class="row pb-2"><td class="col-7"><input autocomplete="off" id="Members_Names_{}" onkeyup="input_onkeyup(this,\'SearchUsers\')" onfocus="input_onfocus(this)" onfocusout="input_onfocusout(this)" class="w-100 form-control" name="Members_Names[{}]" type="text" value=""><ul class="border border-dark col-6 list-group p-0" id="Members_List" hidden=""></ul><div hidden=""><input id="Members_{}__Member__Id" name="Members[{}].Member.Id" hidden=""></div></td><td class="col-5"><a href="javascript:void(0)" onclick="removeMember(this)">Remove</a></td></tr >').replace(/{}/g, index);
    $("#members").find("tbody").append(HTML);
}

function removeMember(e) {
    $(e).parents("tr").remove();
    var tb = $("#members").first();
    var count = tb.find("tbody").find("tr").length;
    for (var i = 0; i < count; i++) {
        var newTR = tb.find("tr").eq(i).html().replace(/\[\d+\]/g, '[' + i + ']').replace(/_\d+/g, '_' + i);
        tb.find("tr").eq(i).html(newTR);
    }
}
function submit(org_id) {
    $('#loading-panel').removeAttr('hidden');

    var form = document.getElementById('CreateOrganizationTeams');
    var formData = new FormData(form);
    formData.append('_org_id', org_id);

    $.ajax({
        type: 'POST',
        url: '../OrganizationInfo/Create',
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
                var EXECUTEDSUCCESS = str.indexOf("<!--OrganizationInfo-->") == 0;

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

function editSubmit(org_id) {
    $('#loading-panel').removeAttr('hidden');

    var form = document.getElementById('EditOrganizationTeams');
    var formData = new FormData(form);
    formData.append('org_id', org_id);

    $.ajax({
        type: 'POST',
        url: '../OrganizationInfo/Edit',
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
                var EXECUTEDSUCCESS = str.indexOf("<!--OrganizationInfo-->") == 0;

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

var emptyImgUrl = $('#teamImg').attr('src');


$(function () {
    $('#teamImg').css('border-radius', '8px');
    $('#teamImg').css('min-height', ($('#teamImg').width() * 0.55));
    $('#teamImg').css('max-height', ($('#teamImg').width() * 0.55));
});
$(window).resize(function () {
    $('#teamImg').css('min-height', ($('#teamImg').width() * 0.55));
    $('#teamImg').css('max-height', ($('#teamImg').width() * 0.55));
});
function imgCheck(img) {
    if (img.value == null || img.value == "") {
        img.value = "";
        $('#teamImg').attr('src', emptyImgUrl);
    }
    else {
        var fileSize = 0;
        fileSize = img.files[0].size;
        var size = fileSize / 1024;
        if (size > 10000) {
            alert("The image exceeds the limit of 10MB");
            img.value = "";
            $('#teamImg').attr('src', emptyImgUrl);
            return false;
        }
        else {
            var name = img.value;
            var fileName = name.substring(name.lastIndexOf(".") + 1).toLowerCase();
            if (fileName != "jpg" && fileName != "jpeg" && fileName != "png" && fileName != "jpg") {
                alert("Only accpet the format of JPG, JPEG or PNG");
                img.value = "";
                $('#teamImg').attr('src', emptyImgUrl);
                return false;
            }
            else {
                var url = getImgLocalUrl(img.files[0]);
                $('#teamImg').attr('src', url);
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
