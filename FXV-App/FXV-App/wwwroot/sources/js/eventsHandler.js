var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
    "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
];

var last_event_id = 0;

$(document).ready(function () {
    last_event_id = 0;
});

window.onload = getMoreEvents();

function getMoreEvents() {

    $('#more').removeAttr('onclick');
    $('#more').html('Loading...<img style="width: 2rem; height: 2rem; margin-left:1rem" src="/sources/img/loading.gif" alt=""/>');

    $.ajax({
        url: '../Events/GetMoreEvents',
        data: { last_event_id },
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

                var isAdmin = json.IsAdmin;

                $.each(json.Events, function (index, item) {

                    var date = new Date(item.Date);
                    var time = new Date(item.Time);

                    var event_time = time.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true });

                    var html = '<div class="card col-xl-4 col-md-6 col-12 mt-0" style="width: 18rem;">'
                        + '<div class="position-absolute"><h4 class="text-center text-FXV pt-1 pb-1 bg-dark date-month-day" >' + date.getDate() + ' ' + monthNames[date.getMonth()] + '</h4>'
                        + '<h4 class="text-center text-FXV date-triangle"></h4>'
                        + '<h4 class="text-center text-FXV pt-1 pb-1 bg-dark date-year">' + date.getFullYear() + '</h4></div>'
                        + '<img src =' + item.Img_Path + ' alt="" onclick="getEventDetail(' + item.E_ID + ')" class="card-img-top rounded-top" style="min-height:275px;max-height:275px;object-fit: contain;" alt = "..." >'
                        + '<div class="card-body row text-white m-0 pl-0 pr-0 rounded-bottom">'
                        + '<h4 class="card-title col-12 text-FXV">' + item.Name + '</h4>'
                        + '<p class="card-text col-12 mb-0 content-font-size">' + event_time + '</p>'
                        + '<p class="card-text col-12 content-font-size">' + item.Description + '</p>'
                        + '<p class="col-10 pl-2 content-font-size mb-0"><span class="material-icons">room</span>' + item.Location + '</p>';

                    if (isAdmin) {
                        html += '<a class="material-icons col-1 p-0" id="edit" href="javascript:void(0)" onclick="editEvent(' + item.E_ID + ')">edit</a>'
                            + '<a class="material-icons col-1 p-0" href="javascript:void(0)" onclick="deleteEvent(' + item.E_ID + ',\'event_' + item.E_ID + '\')">delete</a>'
                            + '</div>'
                            + '</div >';
                    }
                    else {
                        html += '</div></div >';
                    }

                    $('#all_event_list').append(html);
                    last_event_id = (item.E_ID > last_event_id) ? item.E_ID : last_event_id;
                });

                if (json.Events.length < 5) {
                    $('#more').attr('disabled', 'true').html("No more");
                }
                else {
                    $('#more').html("More...");
                    $('#more').attr('onclick', 'loadMoreEvents()');
                }
            }
        }, error: function (xhr) {
            $("#body-content").html(xhr.responseText);
        }
    });
}

function deleteEventConfirmed(e_id, tag) {
    $.ajax({
        url: '../Events/RemoveEvent',
        type: 'POST',
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
                $('#deleteConfirmPopUp').css('display', 'none');
                $('#deleteConfirm').removeAttr('onclick');
                try {
                    var json = JSON.parse(data);
                    var success = json.Success;
                    if (success) {
                        alert("Success");
                        document.getElementById(tag).remove();
                    }
                    else {
                        alert(json.Reason);
                    }
                }
                catch (e) {
                    alert("Something problem just happened, please contact our administrator.");
                }
            }
        }, error: function (xhr) {
            $("#body-content").html(xhr.responseText);
        }
    });
}


function loadMoreEvents() {
    $('#more').removeAttr('onclick');
    $('#more').html('Loading...<img style="width: 2rem; height: 2rem; margin-left:1rem" src="/sources/img/loading.gif" alt=""/>');
    setTimeout(function () {
        getMoreEvents();
    }, 1500);
}

//get the popup panel

var panel = $('#popUp');

// Get the <span> element that closes the modal

var span = document.getElementsByClassName("close")[0];

function getEventDetail(id) {

    $.ajax({
        url: '../Events/GetEventDetails',
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

                    var date = new Date(item.Date);

                    var time = new Date(item.Time);

                    var event_time = time.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true });

                    panel.find('#date').html(date.getDate() + ' ' + monthNames[date.getMonth()]);
                    panel.find('#year').html(date.getFullYear());
                    panel.find('#details-content').find('img').attr('src', item.Img_Path);
                    panel.find('#title').html(item.Name);
                    panel.find('#time').html(event_time);
                    panel.find('#description').html(item.Description);
                    panel.find('#location').html('<span class="material-icons p-0"></span><span class="material-icons p-0">room</span>' + item.Location);
                    panel.find('#getEventInfo').attr('href', 'javascript:getEventInfo(' + item.E_ID + ')')
                    panel.find('#runEvent').attr('href', 'javascript:runEvent(' + item.E_ID + ')')

                    panel.find('#details-content').removeAttr('hidden');
                    panel.css('display', 'block');
                })
            }
        }, error: function (xhr) {
            $("#body-content").html(xhr.responseText);
        }

    })
}

function runEvent(id) {
    $.ajax({
        url: '../EventMenu/EventMenu',
        type: 'Get',
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
                $('#body-content').html(data);
            }
        }, error: function (xhr) {
            $("#body-content").html(xhr.responseText);
        }
    })

    
}

function createEvents() {
    $.ajax({
        url: '../Events/CreateEvents',
        type: 'GET',
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
                $('#body-content').html(data);
            }
        }, error: function (xhr) {
            $("#body-content").html(xhr.responseText);
        }

    });

    
}

// When the user clicks on <span> (x), close the modal

span.onclick = function () {
    panel.css('display', 'none');
    popUpClear();
}

// When the user clicks anywhere outside of the modal, close it

window.onclick = function (event) {
    if (event.target == document.getElementById("popUp")) {
        panel.css('display', 'none');
        popUpClear();
    }
}

function popUpClear() {

    panel.find('#details-content').attr('hidden', '');
    panel.find('#date').html('');
    panel.find('#year').html('');
    panel.find('#details-content').find('img').attr('src', '');
    panel.find('#title').html('');
    panel.find('#description').html('');
    panel.find('#location').html('');
}
function getEventInfo(id) {
    $.ajax({
        url: '../EventInfo/EventInfo',
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
                $('#body-content').html(data);
            }
        }, error: function (xhr) {

            $("#body-content").html(xhr.responseText);
        }
    });

    
}


function deleteEvent(e_id, tag) {
    $('#deleteConfirmPopUp').css('display', 'block');
    $('#deleteConfirm').attr('onclick', 'deleteEventConfirmed(' + e_id + ',' + tag + ')');
    $('#deleteCancel').on('click', function () {
        $('#deleteConfirmPopUp').css('display', 'none');
        $('#deleteConfirm').removeAttr('onclick');
    })
}

function editEvent(e_id) {

    $.ajax({
        url: '../Events/Edit',
        data: { e_id },
        type: 'Get',
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
                    $("#body-content").html(data);
                }
            }
        }, error: function (xhr) {
            $("#body-content").html(xhr.responseText);
        }
    })
}