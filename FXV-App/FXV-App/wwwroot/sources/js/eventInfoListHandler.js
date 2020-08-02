var event_id = $('#event-id-tag').attr('event_id');


var last_combine_id = 0;
var last_attendee_id = 0;


$(document).ready(function () {
    last_combine_id = 0;
    last_attendee_id = 0;
});

window.onload = getMoreEventAttendees();


//get the popup panel
var combine_panel = $('#combine-popUp');
var attendee_panel = $('#attendee-popUp');

// Get the <span> element that closes the modal

var combine_span = $('#close');
var attendee_span = $('#attendee-close');


// When the user clicks on <span> (x), close the modal

combine_span.on('click', function () {
    combine_panel.css('display', 'none');
    combine_popUpClear();
})

attendee_span.on('click', function () {
    attendee_panel.css('display', 'none');
    attendee_popUpClear();
})
// When the user clicks anywhere outside of the modal, close it

function combine_popUpClear() {
    combine_panel.find('#details-content').attr('hidden', '');
    combine_panel.find('#details-content').find('img').attr('src', '');
    combine_panel.find('#title').html('');
    combine_panel.find('#description').html('');
    combine_panel.find('#combine-tests').html('');
}

function attendee_popUpClear() {
    attendee_panel.find('#details-content').attr('hidden', '');
    attendee_panel.find('#details-content').find('img').attr('src', '');
    attendee_panel.find('#name').html('');
    attendee_panel.find('#gender').html('');
    attendee_panel.find('#age').html('');
}
// When the user clicks anywhere outside of the modal, close it

window.onclick = function (event) {
    if (event.target == document.getElementById("combine-popUp")) {
        combine_panel.css('display', 'none');
        combine_popUpClear();
    }
    else if (event.target == document.getElementById("attendee-popUp")) {
        attendee_panel.css('display', 'none');
        attendee_popUpClear();
    }
}

//combine

function getCombineDetail(id) {

    $.ajax({
        url: '../Combines/GetCombineDetails',
        type: 'POST',
        data: { id },
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
                $.each(json, function (index, item) {
                    combine_panel.find('#details-content').find('img').attr('src', item.Img_Path);
                    combine_panel.find('#title').html(item.Name);
                    combine_panel.find('#description').html(item.Description);

                    $.each(item.Tests, function (index, x) {
                        combine_panel.find('#combine-tests').append('<h5>' + x.Name + '</h5 >');
                    })

                    combine_panel.find('#details-content').removeAttr('hidden');
                    combine_panel.css('display', 'block');
                });
            }
        }, error: function (xhr) {
            $("#body-content").html(xhr.responseText);

        }

    })
}


function getMoreEventAttendees() {

    $('#attendee_more').removeAttr('onclick');
    $('#attendee_more').html('Loading...<img style="width: 2rem; height: 2rem; margin-left:1rem" src="/sources/img/loading.gif" alt=""/>');

    $.ajax({
        url: '../EventInfo/GetMoreEventAttendees',
        data: { event_id, last_attendee_id },
        type: 'POST',
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

                var AdminAccess = json.AdminAccess;
                var NotPastEvent = json.NotPastEvent;

                $.each(json.Attendees, function (index, item) {

                    var age = getAge(item.DOB);

                    var html = '<div id="eve-attendee-'+ item.Id +'" class="card col-xl-4 col-md-6 col-12 mt-0" style="width: 18rem;">'
                        + '<img src =' + item.Profile_Img_Path + ' alt="" onclick="getEventAttendeeDetail(' + item.Id + ')" class="card-img-top rounded-top" style="min-height:275px;max-height:275px;object-fit: contain;" alt = "..." >'
                        + '<div class="card-body row text-white m-0 pl-0 pr-0 rounded-bottom">'
                        + '<h4 class="card-title col-12 text-FXV">' + item.FirstName + ' ' + item.LastName + '</h4>'
                        + '<p class="col-10 content-font-size">' + item.Gender + '</p>'
                        + '<p class="card-text col-12 content-font-size">' + age + ' Age</p>';

                    if (AdminAccess && NotPastEvent) {
                        html += '<a class="material-icons col-1 offset-11 p-0" id="delete" href="javascript:void(0)" onclick="deleteEventAttendee(' + item.Id + ',\'eve-attendee-' + item.Id + '\')">delete</a>'
                            + '</div>'
                            + '</div >';
                    }
                    else {
                        html += '</div></div>';
                    }

                    $('#all_attendee_list').append(html);
                    last_attendee_id = (item.Id > last_attendee_id) ? item.Id : last_attendee_id;
                });

                if (json.Attendees.length < 5) {
                    $('#attendee_more').attr('disabled', 'true').html("No more");
                }
                else {
                    $('#attendee_more').html("More...");
                    $('#attendee_more').attr('onclick', 'loadMoreEventAttendees()');
                }
            }
        }, error: function (xhr) {
            $("#body-content").html(xhr.responseText);
        }

    });
}


function loadMoreEventAttendees() {
    $('#attendee_more').removeAttr('onclick');
    $('#attendee_more').html('Loading...<img style="width: 2rem; height: 2rem; margin-left:1rem" src="/sources/img/loading.gif" alt=""/>');
    setTimeout(function () {
        getMoreEventAttendees();
    }, 1500);
}


function AssignAttendee(id) {
    $.ajax({
        url: '../EventInfo/AssignEventAttendee',
        type: 'GET',
        data: { id },
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
                try {
                    var json = JSON.parse(data);
                    if (!json.Result) {
                        alert(json.Reason);
                    }
                }
                catch (e) {
                    $('#body-content').html(data);
                }
            }
        }, error: function (xhr) {
            $("#body-content").html(xhr.responseText);
        }

    })
}


function getEventAttendeeDetail(id) {
    $.ajax({
        url: '../EventInfo/GetEventAttendeeDetail',
        type: 'POST',
        data: { id },
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

                attendee_panel.find('#details-content').find('img').attr('src', json.User.Profile_Img_Path);
                attendee_panel.find('#name').html(json.User.FirstName + ' ' + json.User.LastName);
                attendee_panel.find('#gender').html(json.User.Gender);
                attendee_panel.find('#age').html(getAge(json.User.DOB) + " Age");

                if (json.AdminAccess) {
                    attendee_panel.find('#getAttendeeInfo').attr('onclick', 'directToUserProfile(' + json.User.Id + ')')
                }

                attendee_panel.find('#details-content').removeAttr('hidden');
                attendee_panel.css('display', 'block');
            }
        }, error: function (xhr) {
            $("#body-content").html(xhr.responseText);
        }

    })
}

function directToUserProfile(user_id) {

    $.ajax({
        url: '../UserProfile/Index',
        type: 'GET',
        data: { user_id },
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
                $("#body-content").html(data);

                $('#page-info').attr('data-page-from-Controller', 'EventInfo');
                $('#page-info').attr('data-page-from-Action', 'EventInfo');
                $('#page-info').attr('data-page-required-id', event_id);
            }
        }, error: function (xhr) {
            $("#body-content").html(xhr.responseText);
        }
    });

    
}

function getAge(DOB) {
    var returnAge;
    var strBirthdayArr = DOB.split("-");
    var birthYear = strBirthdayArr[0];
    var birthMonth = strBirthdayArr[1];
    var birthDay = strBirthdayArr[2];

    d = new Date();
    var nowYear = d.getFullYear();
    var nowMonth = d.getMonth() + 1;
    var nowDay = d.getDate();

    if (nowYear == birthYear) {
        returnAge = 0;
    }
    else {
        var ageDiff = nowYear - birthYear;
        if (ageDiff > 0) {
            if (nowMonth == birthMonth) {
                var dayDiff = nowDay - birthDay;
                if (dayDiff < 0) {
                    returnAge = ageDiff - 1;
                }
                else {
                    returnAge = ageDiff;
                }
            }
            else {
                var monthDiff = nowMonth - birthMonth;
                if (monthDiff < 0) {
                    returnAge = ageDiff - 1;
                }
                else {
                    returnAge = ageDiff;
                }
            }
        }
        else {
            returnAge = -1;
        }
    }

    return returnAge;
};


function deleteConfirmed(id, tag) {
    $.ajax({
        url: '../EventInfo/RemoveEventAttendee',
        type: 'POST',
        data: { id, event_id },
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
                try {
                    var json = JSON.parse(data);
                    var success = json.Success;

                    $('#deleteConfirmPopUp').css('display', 'none');
                    $('#deleteConfirm').removeAttr('onclick');

                    if (success) {
                        alert("Success!");
                        document.getElementById(tag).remove();
                    }
                    else {
                        alert(json.Reason);
                    }
                }
                catch (e) {
                    $("#body-content").html(data);
                }
            }
        }, error: function (xhr) {
            $("#body-content").html(xhr.responseText);
        }
    });
}

function deleteEventAttendee(id, tag) {
    $('#deleteConfirmPopUp').css('display', 'block');
    $('#deleteConfirm').attr('onclick', 'deleteConfirmed(' + id + ',\'' + tag + '\')');
    $('#deleteCancel').on('click', function () {
        $('#deleteConfirmPopUp').css('display', 'none');
        $('#deleteConfirm').removeAttr('onclick');
    })
}