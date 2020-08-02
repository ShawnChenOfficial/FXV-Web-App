
//var finished = false;

//var loadded_count = 0;

//function ApplyHistoryRouting() {

//    history.pushState(null, null, 'http://localhost:5000/Home');  //local test
//    //history.pushState(null, null, 'http://163.47.10.173/Home'); //cloud
//    //history.pushState(null, null, 'https://www.fxv.co.nz/Home'); //release

//    setTimeout(function () {

//        window.addEventListener('popstate', function (event) {


//            if (loadded_count < 1 && !finished) {

//                var page_from_controller = $('#page-info').attr('data-page-from-Controller');
//                var page_from_action = $('#page-info').attr('data-page-from-Action');
//                var page_required_id = $('#page-info').attr('data-page-required-id');

//                setTimeout(function () {
//                    Previous(page_from_controller, page_from_action, page_required_id);
//                }, 50);

//            }
//            else if (finished) {
//                loadded_count = 0;
//                finished = false;
//            }
//        }, false)
//    }, 50);
//}

//function Previous(page_from_controller, page_from_action, id) {

//    if (page_from_controller == "Home") {
//        document.clear();
//        location.reload();
//    }
//    else {
//        $.ajax({
//            type: 'GET',
//            url: '../' + page_from_controller + "/" + page_from_action,
//            data: { id },
//            dataType: 'html',
//            aysnc: false,
//            success: function (data) {
//                var str = data.toString();
//                if (str.indexOf("<!--This is the login layout-->") == 0) {
//                    alert("The system detects that you have not operated for a long time, please login again");
//                    document.clear();
//                    location.reload();
//                }
//                else {
//                    $('#body-content').html(data);
//                }
//            },
//            error: function (xhr) {
//                $("#body-content").html(xhr.responseText);
//            }
//        });
//    }
//    loadded_count++;
//    finished = true;
//}
