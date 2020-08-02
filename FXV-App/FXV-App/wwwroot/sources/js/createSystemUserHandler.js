window.onload = function () {
    $('html').css('overflow', 'auto');
}
document.getElementById('submit').onclick = function () {
    $('#loading-panel').removeAttr('hidden');
    $('html').css('overflow', 'hidden');
}

function submit() {
    var form = document.getElementById('CreateSystemUsers');
    var formData = new FormData(form);

    $.ajax({
        type: 'POST',
        url: '../home/CreateSystemUsers',
        data: formData,
        cache: false,
        async: false,
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
                $('#body-content').html(data);
            }
        },
        error: function (data) {
            $("#body-content").html('<div class="content p-0 row"><div class="container col-10 mt-5"><h1 class="text-FXV">Opps</h1><br /><h2 class="text-white">An error occurred while processing your request.</h2></div></div>');
        }
    });
}