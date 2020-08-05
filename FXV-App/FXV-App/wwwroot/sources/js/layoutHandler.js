$(window).on('load', function () {
    $('#sidebar-extended-trigger-close').click();
    SuccessTurnToParentPage("Welcome",true);
});

//nav tags onclicked
$('#sidebar-extended-trigger-close').on('click', function () {
    $('#sidebar-extended').attr('data-sidebar-extended-on','false');
    $('#sidebar-collapse').attr('data-sidebar-collapse-on', 'true');

    $('#sidebar-extended').attr('style', 'transform: translate3d(-288px,0,0) !important;');
    setTimeout(function () {
        $('#sidebar-collapse').attr('style', 'transform: translate3d(-0px,0,0) !important; ');
    }, 200);
});

$('#sidebar-collapse-trigger-close').on('click', function () {
    $('#sidebar-extended').attr('data-sidebar-extended-on', 'true');
    $('#sidebar-collapse').attr('data-sidebar-collapse-on', 'false');

    $('#sidebar-collapse').attr('style', 'transform: translate3d(-288px,0,0) !important; ');
    setTimeout(function () {
        $('#sidebar-extended').attr('style', 'transform: translate3d(0px,0,0) !important;');
    }, 200);
});

function tags_onclick(obj) {
    $('#sidebar-extended-trigger-close').click();

    $('.sidebar-wrapper ul li').each(function () {
        $(this).removeClass('active');
        $(this).removeAttr('onclick');
    });

    $('#loading-panel').removeAttr('hidden');

    $(obj).addClass('active');
    var id = obj.id;
    $.ajax({
        url: '../' + id + '/' + id,
        type: 'GET',
        cache: false,
        dataType: 'html',
        success: function (data) {
            var str = data.toString();
            if (str.indexOf("<!--This is the login layout-->") == 0) {
                $("#body-content").html("<div class='content p-0 mt-3'><div class='container-fluid'><div class='row'><div class='col-12'><div class='card'><div class='card-header'></div><div class='card-body col-12' style='padding-left:10%;padding-top:10%'><h1>Opps</h1><h4 class='pt-3'>An error occurred while processing your request.</h4></div></div></div></div></div>");
                alert("The system detects that you have not operated for a long time, please login again");
                document.clear();
                location.reload();
            }
            else {
                $('#loading-panel').attr('hidden','hidden');
                $("#body-content").html(data);
            }

            $('.sidebar-wrapper ul li').each(function () {
                if (this.id != 'nav-bottom-item' && this.id != "Index") {
                    $(this).attr('onclick', 'tags_onclick(this)');
                }
            });

        },
        error: function (xhr) {

            $('#loading-panel').attr('hidden', 'hidden');
            $("#body-content").html(xhr.responseText);
            $('.sidebar-wrapper ul li').each(function () {
                if (this.id != 'nav-bottom-item' && this.id != "Index") {
                    $(this).attr('onclick', 'tags_onclick(this)');
                }
            });
        }
    });
};

function directToUserProfileForCurrentUser(user_id) {
    $('#sidebar-extended-trigger-close').click();
    $('#loading-panel').removeAttr('hidden');

    $.ajax({
        url: '/UserProfile/Index',
        type: 'GET',
        data: { user_id },
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
                $('#page-info').attr('data-page-from-Controller', 'Home');
                $('#page-info').attr('data-page-from-Action', 'Index');
            }
        }, error: function (xhr) {
            $("#body-content").html(xhr.responseText);
        }
    });

    $('#loading-panel').attr('hidden', 'hidden');
}

$('.main-panel').on('click', function () {
    if ($('#sidebar-extended').attr('data-sidebar-extended-on') =='true' && $('#sidebar-collapse').attr('data-sidebar-collapse-on') == 'false') {
        $('#sidebar-extended').attr('style', 'transform: translate3d(-288px,0,0) !important;');
        setTimeout(function () {
            $('#sidebar-collapse').attr('style', 'transform: translate3d(-0px,0,0) !important; ');
        }, 200);
    }
})

function SuccessTurnToParentPage(content, success) {
    if (!success) {
        $('#toastnotification').find('div').attr('class','toastnotification-content-failed text-center')
    }
    if (success) {
        $('#toastnotification').find('div').attr('class', 'toastnotification-content-success text-center')
    }

    $('#toastnotification').find('div').find('h6').text(content);
    $('#toastnotification').attr('class', 'toastnotification toastnotification-show');

    var interval = setTimeout(
        function () {
            $('#toastnotification').attr('class', 'toastnotification toastnotification-hidden');
            $('#toastnotification').find('div').find('h6').text("");
        },
        2500
    );
}

