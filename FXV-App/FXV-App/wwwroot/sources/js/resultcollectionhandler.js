
var SpeedOnFocus = false;

function searchSpeedTests(obj) {

    SpeedOnFocus = true;

    var category = $(obj).attr('id');

    $('#select_categories').find('a').removeClass('btn-warning-customized').addClass('btn-light');
    $(obj).addClass('btn-warning-customized').removeClass('btn-light');
    $('#ResultCollectionSave').html('');

    var e_id = $('#Result_Collection_Block').attr('e_id');

    $.ajax({
        type: 'POST',
        url: '../ResultCollection/GetSpeedTests',
        data: { e_id },
        cache: false,
        dataType: 'html',
        success: function (data) {
            var str = data.toString();
            if (str.indexOf("<!--This is the login layout-->") == 0) {
                alert("The system detects that you have not operated for a long time, please login again");
                document.clear();
                location.reload();
            }
            else {

                var json = JSON.parse(data);
                var html = '<div class="col-4 pt-4" ><h3 class="control-label text-white">Attendee</h3><input autocomplete="off" id="AttendeeFullName" name="AttendeeFullName" class="bg-light text-dark" placeholder="Select Attendee" onkeyup="input_onkeyup(this,\'SearchAttendees\',\'' + category + '\',\'' + e_id + '\')" onfocus="input_onfocus(this)" onfocusout="input_onfocusout(this)"/><span class="text-danger field-validation-valid" data-valmsg-for="AttendeeFullName" data-valmsg-replace="true"></span><ul class="bg-light border border-dark col-12 list-group p-0" hidden=""></ul><div hidden=""><input id="AttendeeId" name="AttendeeId" hidden=""></div></div><div class="col-12 pt-5 row">';

                $.each(json, function (index, item) {
                    html += '<div class="mt-2 pl-0 ml-2 pr-0 mr-2 bg-light col-2" style="height: 100px;"><input autocomplete="off" type="number" id="Test_Results__' + index + '__Result" name="Test_Results[' + index + '].Result" style="height: 100px; width: 70%; font-size: 1.5rem; " class="bg-light border-0" placeholder="' + item.Name + '"><label class="text-dark text-center" style="font-size: 1.2rem; height: 100px; line-height:100px; width:30%;">' + item.Unit + '</label><div hidden=""><input id="Test_Results__' + index + '__Test_ID" name="Test_Results[' + index + '].Test_ID" hidden="" value="' + item.Test_ID + '"></div></div>'
                });
                html += '</div>';
                $('#ResultCollectionSave').append(html);

                $('#submit').prop('disabled', false);
                $('#submit').prop('hidden', false);
            }
        },
        error: function (xhr) {
            $("#body-content").html(xhr.responseText);
        }
    });
}
function searchTests(obj) {

    SpeedOnFocus = false;

    var e_id = $('#Result_Collection_Block').attr('e_id');

    var category = $(obj).attr('id');

    $('#select_categories').find('a').removeClass('btn-warning-customized').addClass('btn-light');
    $(obj).addClass('btn-warning-customized').removeClass('btn-light');
    $('#ResultCollectionSave').html('');

    var isForEventCollection = true;

    $('#ResultCollectionSave').append('<div class="col-12 pt-4"><div class="col-4"><h3 class="control-label text-white">Test</h3><input autocomplete="off" id="Test_Result" name="Test_Result" class="bg-light text-dark" placeholder="Select Test" onkeyup="input_onkeyup(this,\'SearchTests\',\'' + category + '\',\'' + e_id + '\',' + isForEventCollection + ')" onfocus="input_onfocus(this)" onfocusout="input_onfocusout(this)"><ul class="bg-light border border-dark col-12 list-group p-0" hidden="hidden"></ul><div hidden=""><input id="Test_Result__Test_Id" name="Test_Result.Test_Id" hidden=""></div></div></div><div class="col-12 pt-4"><div class="col-4"><h3 class="control-label text-white">Attendee</h3><input autocomplete="off" id="AttendeeFullName" name="AttendeeFullName" class="bg-light text-dark" placeholder="Select Attendee" onkeyup="input_onkeyup(this,\'SearchAttendees\',\'' + category + '\',\'' + e_id + '\')" onfocus="input_onfocus(this)" onfocusout="input_onfocusout(this)"><ul class="bg-light border border-dark col-12 list-group p-0" hidden="hidden"></ul><div hidden=""><input id="AttendeeId" name="AttendeeId" hidden=""></div></div></div><div class="col-12 pt-5 row ml-2"><div id="single_test_result_collection" class="mt-2 pl-0 ml-2 pr-0 mr-2 bg-light col-2" style="height: 100px;display:none"></div></div>');

    $('#submit').prop('disabled', false);
    $('#submit').prop('hidden', false);
}


$('#submit').on('click', function () {
    $('#loading-panel').removeAttr('hidden');

    var e_id = $('#Result_Collection_Block').attr('e_id');

    var form = document.getElementById('ResultCollectionSave');

    var formData = new FormData(form);

    formData.append('e_id', e_id);

    $.ajax({
        type: 'POST',
        url: '../ResultCollection/ResultCollectionSave',
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
                if (data.toString() != "Done") {
                    SuccessTurnToParentPage("Failed", false);
                }
                else {
                    SuccessTurnToParentPage("Success", true);
                }

                $('#ResultCollectionSave input').val('');

                if (SpeedOnFocus) {
                    $('#Speed').click();
                }
            }
        },
        error: function (xhr) {
            $("#body-content").html(xhr.responseText);
        }
    });

    
})