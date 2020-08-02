
function toPublicLeaderBoard(event_id) {
    window.open("../PublicLeaderboard/Index?event_id=" + event_id);
}

function toResultCollection(id) {
    $.ajax({
        url: '../ResultCollection/ResultCollection',
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
                $("#body-content").html(data);
            }
        }, error: function () {
            $("#body-content").html('<div class="content p-0 row"><div class="container col-10 mt-5"><h1 class="text-FXV">Opps</h1><br /><h2 class="text-white">An error occurred while processing your request.</h2></div></div>');

        }

    });
}