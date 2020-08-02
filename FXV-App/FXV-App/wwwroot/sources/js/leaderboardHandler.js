function btn_test_onclick() {
    $("#collapse_combine").removeClass('show');
    $("#collapse_event").removeClass('show');

    setTestsDropDownList();
    setTestTeamsDropDownList();

}

function setTestsDropDownList() {

    var gender = $('#collapse_test #select_gender').val();

    $('#collapse_test #select_test').html('');
    $('#collapse_test #select_test').html('<option value = "0" selected >Select Test</option>');

    $.ajax({
        url: '../Leaderboards/GetLeaderboardTests',
        type: 'POST',
        data: {
            gender
        },
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
                    $('#collapse_test #select_test').append('<option value=' + item.Test_ID + '>' + item.Name + '</option>');
                })
            }
        },
        error: function (xhr) {
            $("#leaderboard_list_body").html(xhr.responseText);
        }
    });
}

function setTestTeamsDropDownList() {

    var gender = $('#collapse_test #select_gender').val();

    $('#collapse_test #select_team').html('');
    $('#collapse_test #select_team').html('<option value = "0" selected >Select Team</option>');

    $.ajax({
        url: '../Leaderboards/GetLeaderboardTestTeams',
        type: 'POST',
        data: {
            gender
        },
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
                    $('#collapse_test #select_team').append('<option value=' + item.Team_ID + '>' + item.Name + '</option>');
                })
            }
        },
        error: function (xhr) {
            $("#leaderboard_list_body").html(xhr.responseText);
        }
    });
}

function btn_combine_onclick() {

    $("#collapse_test").removeClass('show');
    $("#collapse_event").removeClass('show');

    setCombinesDropDownList();
    setCombineTeamsDropDownList();
}

function setCombinesDropDownList() {
    var gender = $('#collapse_combine #select_gender').val();

    $('#collapse_combine #select_combine').html('');
    $('#collapse_combine #select_combine').html('<option value = "0" selected >Select Combine</option>');

    $.ajax({
        url: '../Leaderboards/GetLeaderboardCombines',
        type: 'POST',
        data: {
            gender
        },
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
                    $('#collapse_combine #select_combine').append('<option value=' + item.C_ID + '>' + item.Name + '</option>');
                })
            }
        },
        error: function (xhr) {
            $("#leaderboard_list_body").html(xhr.responseText);
        }
    });
}

function setCombineTeamsDropDownList() {

    $('#collapse_combine #select_team').html('');
    $('#collapse_combine #select_team').html('<option value = "0" selected >Select Team</option>');

    $.ajax({
        url: '../Leaderboards/GetLeaderboardCombineTeams',
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
                $.each(json, function (index, item) {
                    $('#collapse_combine #select_team').append('<option value=' + item.Team_ID + '>' + item.Name + '</option>');
                })
            }
        },
        error: function (xhr) {
            $("#leaderboard_list_body").html(xhr.responseText);
        }
    });
}

function btn_event_onclick() {
    $("#collapse_test").removeClass('show');
    $("#collapse_combine").removeClass('show');
    setEventsDropDownList();
    setEventTeamsDropDownList();
}

function setEventsDropDownList() {
    var gender = $('#collapse_event #select_gender').val();

    $('#collapse_event #select_event').html('');
    $('#collapse_event #select_event').html('<option value = "0" selected >Select Event</option>');

    $.ajax({
        url: '../Leaderboards/GetLeaderboardEvents',
        type: 'POST',
        data: {
            gender
        },
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
                    $('#collapse_event #select_event').append('<option value=' + item.E_ID + '>' + item.Name + '</option>');
                })
            }
        },
        error: function (xhr) {
            $("#leaderboard_list_body").html(xhr.responseText);
        }
    });
}

function setEventTeamsDropDownList() {

    $('#collapse_event #select_team').html('');
    $('#collapse_event #select_team').html('<option value = "0" selected >Select Team</option>');

    $.ajax({
        url: '../Leaderboards/GetLeaderboardEventTeams',
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
                $.each(json, function (index, item) {
                    $('#collapse_event #select_team').append('<option value=' + item.Team_ID + '>' + item.Name + '</option>');
                })
            }
        },
        error: function (xhr) {
            $("#leaderboard_list_body").html(xhr.responseText);
        }
    });
}

function _search_test() {
    var s1 = ($("#collapse_test #select_test option:selected").val() == '0') ? '' : $("#collapse_test #select_test option:selected").text() + ' ';
    var s2 = ($("#collapse_test #select_gender option:selected").val() == '0') ? '' : $("#collapse_test #select_gender option:selected").text() + ' ';
    var s3 = ($("#collapse_test #select_sport option:selected").val() == '0') ? '' : $("#collapse_test #select_sport option:selected").text() + ' ';
    var s5 = ($("#collapse_test #select_team option:selected").val() == '0') ? '' : 'Of ' + $("#collapse_test #select_team option:selected").text() + ' ';


    var Test_Id = $("#collapse_test #select_test").val();
    var Gender = s2
    var Sport_Id = $("#collapse_test #select_sport").val();
    var Team_Id = $("#collapse_test #select_team").val();

    var str = (s1.includes(s2)) ? s1 + s3 + s5 : s1 + s2 + s3 + s5;

    if (getTitle(s1, str, "test")) {
        response_result_test(Test_Id, Gender, Sport_Id, Team_Id);
    }
}
function _search_combine() {
    var s1 = ($("#collapse_combine #select_combine option:selected").val() == '0') ? '' : $("#collapse_combine #select_combine option:selected").text() + ' ';
    var s2 = ($("#collapse_combine #select_gender option:selected").val() == '0') ? '' : $("#collapse_combine #select_gender option:selected").text() + ' ';
    var s3 = ($("#collapse_combine #select_sport option:selected").val() == '0') ? '' : $("#collapse_combine #select_sport option:selected").text() + ' ';
    var s5 = ($("#collapse_combine #select_team option:selected").val() == '0') ? '' : 'Of ' + $("#collapse_combine #select_team option:selected").text() + ' ';


    var Combine_Id = $("#collapse_combine #select_combine").val();
    var Gender = s2
    var Sport_Id = $("#collapse_combine #select_sport").val();
    var Team_Id = $("#collapse_combine #select_team").val();

    var str = (s1.includes(s2)) ? s1 + s3 + s5 : s1 + s2 + s3 + s5;

    if (getTitle(s1, str, "combine")) {
        response_result_combine(Combine_Id, Gender, Sport_Id, Team_Id);
    }
}
function _search_event() {
    var s1 = ($("#collapse_event #select_event option:selected").val() == '0') ? 0 : $("#collapse_event #select_event option:selected").text() + ' ';
    var s2 = ($("#collapse_event #select_gender option:selected").val() == '0') ? '' : $("#collapse_event #select_gender option:selected").text() + ' ';
    var s3 = ($("#collapse_event #select_sport option:selected").val() == '0') ? '' : $("#collapse_event #select_sport option:selected").text() + ' ';
    var s5 = ($("#collapse_event #select_team option:selected").val() == '0') ? '' : 'Of ' + $("#collapse_event #select_team option:selected").text() + ' ';


    var Event_Id = $("#collapse_event #select_event").val();
    var Gender = s2
    var Sport_Id = $("#collapse_event #select_sport").val();
    var Team_Id = $("#collapse_event #select_team").val();

    var str = (s1.includes(s2)) ? s1 + s3 + s5 : s1 + s2 + s3 + s5;

    if (getTitle(s1, str , "event")) {
        response_result_event(Event_Id, Gender, Sport_Id, Team_Id);
    };
}

function getTitle(key,str, type) {
    var success = false;
    if (key == 0) {
        switch (type) {
            case 'test':
                alert("Please select at lease the test.");
                break;
            case 'combine':
                alert("Please select at least the combine.");
                break;
            case 'event':
                alert("Please select at least the event.");
                break;
        }
        success = false;
    }
    else {
        switch (type) {
            case 'test':
                $("#btn_test").click();
                str = "Test: " + str;
                break;
            case 'combine':
                $("#btn_combine").click();
                str = "Combine: " + str;
                break;
            case 'event':
                $("#btn_event").click();
                str = "Event: " + str;
                break;
        }
        $('#leaderboard_list').addClass('card');
        $('#leaderboard_list').html('<div id="leaderboard_list_header" class="card-header p-0 card-header-secondary"><h2 class="medium-bold card-title text-center"></h2></div><div id="leaderboard_list_body" class="card-body col-12"></div>');
        $('#leaderboard_list_header h2').html(str);
        $('#leaderboard_list_body').html('<div class="row"><div class="col-12 text-center"><div style="padding-top:calc(100vh/4 - 4rem)"><h4>Loading...</h4><img style="width:2rem;height:2rem" src="/sources/img/loading.gif" alt=""/></div></div>');
        success = true;
    }

    return success;
}

function response_result_test(Test_Id, Gender, Sport_Id, Team_Id) {
    $.ajax({
        url: '../Leaderboards/GetLeaderboardTestResults',
        data: {
            testID: Test_Id,
            gender: Gender,
            sportID: Sport_Id,
            teamID: Team_Id
        },
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
                $("#leaderboard_list_body").html("");
                $("#leaderboard_list_body").html(data);
            }
        },
        error: function (xhr) {
            $("#leaderboard_list_body").html(xhr.responseText);
        }
    });
} function response_result_combine(Combine_Id, Gender, Sport_Id, Team_Id) {

    $.ajax({
        url: '../Leaderboards/GetLeaderboardCombineResults',
        data: {
            combineid: Combine_Id,
            gender: Gender,
            sportid: Sport_Id,
            teamid: Team_Id
        },
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
                $("#leaderboard_list_body").html("");
                $("#leaderboard_list_body").html(data);
            }
        },
        error: function (xhr) {
            $("#leaderboard_list_body").html(xhr.responseText);
        }
    });
} function response_result_event(Event_Id, Gender, Sport_Id, Team_Id) {

    $.ajax({
        url: '../Leaderboards/GetLeaderboardEventResults',
        data: {
            eventid: Event_Id,
            gender: Gender,
            sportid: Sport_Id,
            teamid: Team_Id
        },
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
                $("#leaderboard_list_body").html("");
                $("#leaderboard_list_body").html(data);
            }
        },
        error: function (xhr) {
            $("#leaderboard_list_body").html(xhr.responseText);
        }
    });
}