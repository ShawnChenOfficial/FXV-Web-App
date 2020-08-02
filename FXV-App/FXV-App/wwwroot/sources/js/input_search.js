var cpLock = false;

function input_onkeyup(obj, action, category, e_id, isForEventCollection) {

    var gender = $('#Gender').val();

    if (!cpLock && $(obj).val() != '') {

        var fullName = $(obj).val();

        $.ajax({
            type: 'POST',
            url: '../SharedTools/' + action,
            data: {
                fullName, category, e_id, gender
            },
            cache: false,
            async: false,
            dataType: 'html',
            success: function (data) {
                var str = data.toString();
                if (str.indexOf("<!--This is the login layout-->") == 0) {
                    alert("The system detects that you have not operated for a long time, please login again");
                    document.clear();
                    location.reload();
                }
                else {
                    switch (action) {
                        case "SearchUsers":
                            appendList_Users(JSON.parse(data), obj);
                            break;
                        case "SearchAttendees":
                            appendList_Users(JSON.parse(data), obj);
                            break;
                        case "SearchOrgs":
                            appendList_Orgs(JSON.parse(data), obj);
                            break;
                        case "SearchTeams":
                            appendList_Teams(JSON.parse(data), obj);
                            break;
                        case "SearchTests":
                            if (isForEventCollection) {
                                appendList_Tests(JSON.parse(data), obj, isForEventCollection);
                            }
                            else {
                                appendList_Tests(JSON.parse(data), obj);
                            }
                            break;
                        case "SearchCombines":
                            appendList_Combines(JSON.parse(data), obj);
                            break;
                        case "SearchCategory":
                            appendList_TestCategory(JSON.parse(data), obj);
                            break;
                    }
                }
            },
            error: function (xhr) {
                $("#body-content").html(xhr.responseText);
            }
        });

    }
}
function input_onfocus(obj) {
    $(obj).parent().find('ul').removeAttr('hidden');
}

function input_onfocusout(obj) {
    $(obj).parent().find('ul').attr('hidden', 'true');
}



function appendList_Users(list, obj) {
    var managers_List = $(obj).parent().find('ul');
    managers_List.html('');
    var len = list.length;
    var item = null;
    if (list.length == 0) {
        item = managers_List.append('<li class="list-group-item text-muted")">No Result</li>');
    }
    for (var i = 0; i < len; i++) {
        item = managers_List.append('<li class="list-group-item" onmousedown="list_onclick(this,\'' + list[i].FirstName + ' ' + list[i].LastName + '\',\'' + list[i].Id + '\')">' + list[i].FirstName + ' ' + list[i].LastName + '</li>');
    }
}
function appendList_Orgs(list, obj) {
    var managers_List = $(obj).parent().find('ul');
    managers_List.html('');
    var len = list.length;
    var item = null;
    if (list.length == 0) {
        item = managers_List.append('<li class="list-group-item text-muted")">No Result</li>');
    }
    for (var i = 0; i < len; i++) {
        item = managers_List.append('<li id="Manager_List_li" class="list-group-item" onmousedown="list_onclick(this,\'' + list[i].Name + '\',\'' + list[i].Org_ID + '\')">' + list[i].Name + '</li>');
    }
}
function appendList_Teams(list, obj) {
    var managers_List = $(obj).parent().find('ul');
    managers_List.html('');
    var len = list.length;
    var item = null;
    if (list.length == 0) {
        item = managers_List.append('<li class="list-group-item text-muted")">No Result</li>');
    }
    for (var i = 0; i < len; i++) {
        item = managers_List.append('<li class="list-group-item" onmousedown="list_onclick(this,\'' + list[i].Name + '\',\'' + list[i].Team_ID + '\')">' + list[i].Name + '</li>');
    }
}
function appendList_Tests(list, obj, isForEventCollection) {
    var managers_List = $(obj).parent().find('ul');
    managers_List.html('');
    var len = list.length;
    var item = null;
    if (list.length == 0) {
        item = managers_List.append('<li class="list-group-item text-muted" disable)">No Result</li>');
    }
    for (var i = 0; i < len; i++) {
        if (isForEventCollection) {
            item = managers_List.append('<li class="list-group-item" onmousedown="list_onclick_for_event(this,\'' + list[i].Name + '\',\'' + list[i].Test_ID + '\',\'' + list[i].Unit + '\')">' + list[i].Name + '</li>');
        }
        else {
            item = managers_List.append('<li class="list-group-item" onmousedown="list_onclick(this,\'' + list[i].Name + '\',\'' + list[i].Test_ID + '\')">' + list[i].Name + '</li>');
        }
    }
}
function appendList_Combines(list, obj) {
    var managers_List = $(obj).parent().find('ul');
    managers_List.html('');
    var len = list.length;
    var item = null;
    if (list.length == 0) {
        item = managers_List.append('<li class="list-group-item text-muted")">No Result</li>');
    }
    for (var i = 0; i < len; i++) {
        item = managers_List.append('<li class="list-group-item" onmousedown="list_onclick(this,\'' + list[i].Name + '\',\'' + list[i].C_ID + '\',\'' + list[i].Gender + '\')">' + list[i].Name + '</li>');
    }
}
function appendList_TestCategory(list, obj) {
    var managers_List = $(obj).parent().find('ul');
    managers_List.html('');
    var len = list.length;
    var item = null;
    if (list.length == 0) {
        item = managers_List.append('<li class="list-group-item text-muted pl-0 pr-0")">No Result</li>');
    }
    for (var i = 0; i < len; i++) {
        item = managers_List.append('<li class="list-group-item text-body" onmousedown="list_onclick(this,\'' + list[i].Category + '\',\'' + list[i].TC_id + '\')">' + list[i].Category + '</li>');
    }
}
function list_onclick(obj, value, id, gender) {

    $(obj).parent().parent().find('input').val(value);
    $(obj).parent().parent().find('div').find('input').val(id);

    if (typeof gender !== 'undefined' && gender != null && gender != "") {
        $('#Gender').val(gender);
    }
}
function list_onclick_for_event(obj, value, id, unit) {

    $(obj).parent().parent().find('input').val(value);
    $(obj).parent().parent().find('div').find('input').val(id);

    $('#single_test_result_collection').html('');
    $('#single_test_result_collection').attr('style', 'display:block');
    $('#single_test_result_collection').append('<input autocomplete="off" type="number" id="Test_Result__Result" name="Test_Result.Result" style="height: 100px; width: 70%; font-size: 1.5rem; " class="bg-light border-0" placeholder="Result"><label class="text-dark text-center" style="font-size: 1.2rem; height: 100px; line-height:100px; width:30%;">' + unit + '</label><div hidden=""><input id="Test_Result__Test_ID" name="Test_Result.Test_ID" hidden=""></div>');

}
