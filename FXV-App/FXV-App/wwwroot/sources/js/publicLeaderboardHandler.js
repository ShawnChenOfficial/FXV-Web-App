
        var num = 1;
        var lengthOfResultRows = 2;
    var last_user_id = 0;
    $(window).on('load', function () {
        var event_id = $('body').attr('tag-event-id');
        var HTML = "";
        $.ajax({
            type: 'POST',
            url: '../PublicLeaderboard/GetFundersLogo',
            data: { event_id },
            cache: false,
            async: false,
            dataType: 'json',
            success: function (data) {
                var num = 1;
                $.each(data, function (index, item) {
                    if (num == 1) {
                        HTML += '<div class="carousel-item active" style="background-image:url(' + item + ');"></div>';
                        num++;
                    }
                    else {
                        HTML += '<div class="carousel-item" style="background-image:url(' + item + ');"></div>';
                    }
                });

                $('#slider').append(HTML);
            },
            error: function () {

            }
        });
    });
    function startEvent() {
        var event_id = $('body').attr('tag-event-id');
        $.ajax({
            type: 'POST',
            url: '../PublicLeaderboard/StartEvent',
            data: { event_id, last_user_id },
            cache: false,
            async: false,
            dataType: 'json',
            success: function (data) {
                var str = data.toString();
                if (str.indexOf("Ready to display leaderboard") == 0) {
                    StartRefreshingResult();
                }
                else {
                    last_user_id = (data.Attendee_ID > last_user_id) ? data.Attendee_ID : 0;
                    var HTML = '<div id="profile" last-user-id="' + last_user_id + '" class="container-fluid p-0">'
                        + '<div class="row pt-3 ">'
                        + '<div class="card col-10 offset-1 p-0 rounded">'
                        + '<div class="card-header card-header-secondary rounded">'
                        + '<h1 class="card-title text-center">' + data.Event_Name + '</h1>'
                        + '</div>'
                        + '</div>'
                        + '</div>'
                        + '<div class="row pt-3">'
                        + '<div class="card col-10 offset-1 p-0 rounded">'
                        + '<div class="card-header card-header-secondary rounded">'
                        + '<h3 class="card-title text-center">Profile</h3>'
                        + '</div>'
                        + '</div>'
                        + '</div>'
                        + '<div class="row pt-3">'
                        + '<div class="card col-10 offset-1 p-0 rounded">'
                        + '<div class="card-header card-header-secondary rounded">'
                        + '<div class="p-2">'
                        + '<h2 class="card-title">' + data.Attendee_FirstName + ' ' + data.Attendee_LastName + '</h2>';
                    $.each(data.Achievement, function (index, item) {
                        HTML += '<h3 class="card-title">' + item + '</h3>';
                    });
                    HTML += '</div>'
                        + '</div>'
                        + '</div>'
                        + '</div>'
                        + '</div>';
                    $('body').html(HTML);

                    var interval = self.setInterval(function () {
                        $.ajax({
                            type: 'POST',
                            url: '../PublicLeaderboard/StartEvent',
                            data: { event_id, last_user_id },
                            cache: false,
                            async: false,
                            dataType: 'json',
                            success: function (data) {
                                var str = data.toString();
                                if (str.indexOf("Ready to display leaderboard") == 0) {
                                    clearInterval(interval);
                                    StartRefreshingResult();
                                }
                                else {
                                    last_user_id = (data.Attendee_ID > last_user_id) ? data.Attendee_ID : 0;
                                    var HTML = '<div id="profile" last-user-id="' + last_user_id + '" class="container-fluid p-0">'
                                        + '<div class="row pt-3 ">'
                                        + '<div class="card col-10 offset-1 p-0 rounded">'
                                        + '<h1 class="card-title text-center mb-0" style="background: url(../img/Logo-trans.png);background-size: auto 5rem;background-repeat: no-repeat;background-position: 5% center;">@event_name</h1>'
                                        + '</div>'
                                        + '</div>'
                                        + '</div>'
                                        + '<div class="row pt-3">'
                                        + '<div class="card col-10 offset-1 p-0 rounded">'
                                        + '<div class="card-header card-header-secondary rounded">'
                                        + '<h3 class="card-title text-center">Profile</h3>'
                                        + '</div>'
                                        + '</div>'
                                        + '</div>'
                                        + '<div class="row pt-3">'
                                        + '<div class="card col-10 offset-1 p-0 rounded">'
                                        + '<div class="card-header card-header-secondary rounded">'
                                        + '<div class="p-2">'
                                        + '<h2 class="card-title">' + data.Attendee_FirstName + ' ' + data.Attendee_LastName + '</h2>';
                                    $.each(data.Achievement, function (index, item) {
                                        HTML += '<h3 class="card-title">' + item + '</h3>';
                                    });
                                    HTML += '</div>'
                                        + '</div>'
                                        + '</div>'
                                        + '</div>'
                                        + '</div>';
                                    $('body').html(HTML);
                                }
                            },
                            error: function () {

                            }
                        })
                    }, 6500);
                }
            },
            error: function () {

            }
        })
    };

        function StartRefreshingResult() {

        var event_id = $('body').attr('tag-event-id');

            self.setInterval(function () {

                $.ajax({
                    type: 'POST',
                    url: '../PublicLeaderboard/GetOverallResultLength',
                    data: { event_id },
                    cache: false,
                    async: false,
                    dataType: 'json',
                    success: function (data) {
                        lengthOfResultRows = Math.ceil(data / 4) + 1;
                    }, error: function () {

                    }
                });
            if (num < lengthOfResultRows) {
                $.ajax({
                    type: 'POST',
                    url: '../PublicLeaderboard/GetResult',
                    data: { event_id, num },
                    cache: false,
                    async: false,
                    dataType: 'json',
                    success: function (data) {
                        var HTML = '<div id="profile" class="container-fluid p-0">'
                            + '<div class="row pt-3 ">'
                            + '<div class="card col-10 offset-1 p-0 rounded">'
                            + '<div class="card-header card-header-secondary rounded">'
                            + '<h1 class="card-title text-center mb-0" style="background: url(../img/Logo-trans.png);background-size: auto 5rem;background-repeat: no-repeat;background-position: 5% center;">@event_name</h1>'
                            + '</div>'
                            + '</div>'
                            + '</div>'
                            + '<div class="row">'
                            + '<div class="card col-10 offset-1 rounded mt-2"><div class="text-center card-header card-header-secondary row"><h3 class="border-0 col-2">#</h3><h3 class="col-7">Name</h3><h3 class="border-0 col-3">Score</h3></div></div>'
                            + '<table class=" card col-10 offset-1 rounded  mt-2" >'
                            + '</thead>'
                            + '<tbody class="card-header card-header-secondary pt-1">';

                        if (Object.keys(data).length == 0) {
                            num = lengthOfResultRows;
                        }
                        else {
                            $.each(data, function (index, item) {
                                var rank = ((item.Point == 0 || item.Point == "" || item.Point == null) ? '#' : (num - 1) * 4 + index + 1);
                                HTML += '<tr class="text-center row mt-3">'
                                    + '<td class="col-2"><h3>' + rank + '</h3></td>'
                                    + '<td class="col-7"><h3>' + item.FullName + '</h3></td>'
                                    + '<td class="col-3"><h3>' + item.Point + '</h3></td>'
                                    + '</tr>';

                            });

                            HTML += '</tbody>'
                                + '</table >'
                                + '</div>';
                            $('body').html(HTML);
                        }
                    }, error: function () {

                    }
                })
                num += 1;
            }
            else {
                $.ajax({
                    type: 'POST',
                    url: '../PublicLeaderboard/GetLastOrNext',
                    data: { event_id},
                    cache: false,
                    async: false,
                    dataType: 'json',
                    success: function (data) {
                        var HTML = '<div id="profile" class="container-fluid p-0">'
                            + '<div class="row pt-3 ">'
                            + '<div class="card col-10 offset-1 p-0 rounded">'
                            + '<div class="card-header card-header-secondary rounded">'
                            + '<h1 class="card-title text-center mb-0" style="background: url(../img/Logo-trans.png);background-size: auto 5rem;background-repeat: no-repeat;background-position: 5% center;">@event_name</h1>'
                            + '</div>'
                            + '</div>'
                            + '</div>'
                            + '<div class="row pt-3">'
                            + '<div class="card col-10 offset-1 p-0 rounded">'
                            + '<div class="card-header card-header-secondary rounded">'
                            + '<h3 class="card-title text-center">' + ((data.IsFinish) ? "Last Run" : "Running Next") + '</h3>'
                            + '</div>'
                            + '</div>'
                            + '</div>'
                            + '<div class="row pt-3">'
                            + '<div class="card col-10 offset-1 p-0 rounded">'
                            + '<div class="card-header card-header-secondary rounded">'
                            + '<div class="p-2">'
                            + '<h2 class="card-title">' + data.Attendee_FirstName + ' ' + data.Attendee_LastName + '</h2>';
                        $.each(data.Achievement, function (index, item) {
                            HTML += '<h3 class="card-title">' + item + '</h3>';
                        });
                        HTML += '</div>'
                            + '</div>'
                            + '</div>'
                            + '</div>'
                            + '</div>';
                        $('body').html(HTML);
                    }, error: function () {

                    }
                })
                num = 1;
            }
        }, 6500);
    }