

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

function directToUserProfileForCurrentUser(user_id) {
    $('#sidebar-extended-trigger-close').click();

    window.location.href = "/UserProfile/Index?user_id=" + user_id;

    $('#loading-panel').attr('hidden', 'hidden');
}
