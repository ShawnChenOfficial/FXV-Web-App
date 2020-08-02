function submit() {
    $('#loading-panel').removeAttr('hidden');

    var form = document.getElementById('CreateOrganizations');
    var formData = new FormData(form);

    $.ajax({
        type: 'POST',
        url: '../Organizations/Create',
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
                var EXECUTEDSUCCESS = str.indexOf("<!--Organizations-->") == 0;

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

    var form = document.getElementById('EditOrganizations');
    var formData = new FormData(form);

    $.ajax({
        type: 'POST',
        url: '../Organizations/Edit',
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
                var EXECUTEDSUCCESS = str.indexOf("<!--Organizations-->") == 0;

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

var emptyImgUrl = $('#orgImg').attr('src');


$(function () {
    $('#orgImg').css('border-radius', '8px');
    $('#orgImg').css('min-height', ($('#orgImg').width() * 0.55));
    $('#orgImg').css('max-height', ($('#orgImg').width() * 0.55));
});
$(window).resize(function () {
    $('#orgImg').css('min-height', ($('#orgImg').width() * 0.55));
    $('#orgImg').css('max-height', ($('#orgImg').width() * 0.55));
});
function imgCheck(img) {

    if (img.value == null || img.value == "") {
        img.value = "";
        $('#orgImg').attr('src', emptyImgUrl);
    }
    else {
        var fileSize = 0;
        fileSize = img.files[0].size;
        var size = fileSize / 1024;
        if (size > 10000) {
            alert("The image exceeds the limit of 10MB");
            img.value = "";
            $('#orgImg').attr('src', emptyImgUrl);
            return false;
        }
        else {
            var name = img.value;
            var fileName = name.substring(name.lastIndexOf(".") + 1).toLowerCase();
            if (fileName != "jpg" && fileName != "jpeg" && fileName != "png" && fileName != "jpg") {
                alert("Only accpet the format of JPG, JPEG or PNG");
                img.value = "";
                $('#orgImg').attr('src', emptyImgUrl);
                return false;
            }
            else {
                var url = getImgLocalUrl(img.files[0]);
                $('#orgImg').attr('src', url);
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



