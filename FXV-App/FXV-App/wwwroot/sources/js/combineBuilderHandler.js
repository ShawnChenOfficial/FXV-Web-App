var categories = [];
var weights = [];

function removeAllTestsOnGenderChanged() {
    $('#test_block').find('tr').remove();
}

function addTest(category) {

    var action = "SearchTests";

    var index1 = $('#test_block').find('div').find('#select_tests').find('tbody').find('tr').length;

    var HTML = ('<tr class="row pb-2 text-center"><td class="col-8"><input autocomplete="off" id="Tests_{}_Name" class="w-100 form-control" name="Tests[{}].Name" type="text" value="" onkeyup="input_onkeyup(this,\'' + action + '\',\'' + category + '\')" onfocus="input_onfocus(this)" onfocusout="input_onfocusout(this)" type="text" value="" placeholder="Search tests by name"><ul class="border border-dark col-12 list-group p-0" hidden=""></ul><div hidden=""><input id="Tests_{}__Test_ID" name="Tests[{}].Test_ID" hidden=""><input id="Tests_{}__Test_Name" name="Tests[{}].Test_Name" readonly hidden></div ></td><td class="col-4"><a href="javascript:void(0)" onclick="removeTests(this,\'' + category + '\')">Remove</a></td></tr>').replace(/{}/g, index1);

    $('#tests_block_' + category + '').find('#select_tests').find('tbody').append(HTML);

}

function removeTests(e, category) {

    $(e).parents("tr").remove();

    var tb = $('#test_block');

    var count = tb.find("tbody").find("tr").length;

    for (var i = 0; i < count; i++) {

        var newTR = tb.find("tr").eq(i).html().replace(/\[\d+\]/g, '[' + i + ']').replace(/_\d+/g, '_' + i);

        tb.find("tr").eq(i).html(newTR);
    }
}

function submit() {
    $('#loading-panel').removeAttr('hidden');

    var total_weight = 0;

    for (var x = 0; x < weights.length; x++) {
        total_weight += parseInt(weights[x].toString());
    }

    if (total_weight != 100) {
        SuccessTurnToParentPage("Failed", false);
        $('#formError').text('The overall weight of test categories does not meet 100%, please check before create a Combine.');
    }
    else {

        var cate_weight_pair = [];

        for (var xx = 0; xx < categories.length; xx++) {

            var obj = new Object();
            obj.Category = categories[xx];
            obj.Weight = weights[xx];

            cate_weight_pair.push(JSON.stringify(obj));
        }

        var form = document.getElementById('CreateCombines');

        var formData = new FormData(form);

        formData.append('cate_weight_pair', cate_weight_pair);

        $.ajax({
            type: 'POST',
            url: '../Combines/CreateCombines',
            data: formData,
            cache: false,
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
                    var EXECUTEDSUCCESS = str.indexOf("<!--Combines-->") == 0;

                    switch (EXECUTEDSUCCESS) {
                        case true:
                            $('#body-content').html(data);
                            SuccessTurnToParentPage("Success", true);
                            return;
                        case false:
                            $('#body-content').html(data);
                            SuccessTurnToParentPage("Failed", false);
                            return;
                    }
                }
            },
            error: function (xhr) {
                $("#body-content").html(xhr.responseText);
            }
        });
    }
}

function editSubmit(id) {
    $('#loading-panel').removeAttr('hidden');

    var total_weight = 0;

    for (var x = 0; x < weights.length; x++) {
        total_weight += parseInt(weights[x].toString());
    }

    if (total_weight != 100) {
        $('#loading-panel').attr('hidden', 'true');
        SuccessTurnToParentPage("Failed", false);
        $('#formError').text('The overall weight of test categories does not meet 100%, please check before create a Combine.');
    }
    else {

        var cate_weight_pair = [];


        for (var xx = 0; xx < categories.length; xx++) {

            var obj = new Object();
            obj.Category = categories[xx];
            obj.Weight = weights[xx];

            cate_weight_pair.push(JSON.stringify(obj));
        }

        var form = document.getElementById('EditCombine');

        var formData = new FormData(form);

        formData.append('cate_weight_pair', cate_weight_pair);

        $.ajax({
            type: 'POST',
            url: '../Combines/Edit',
            data: formData,
            cache: false,
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
                    var EXECUTEDSUCCESS = str.indexOf("<!--Combines-->") == 0;

                    switch (EXECUTEDSUCCESS) {
                        case true:
                            $('#body-content').html(data);
                            SuccessTurnToParentPage("Success", true);
                            return;
                        case false:
                            $('#body-content').html(data);
                            SuccessTurnToParentPage("Failed", false);
                            return;
                    }
                }
            },
            error: function (xhr) {
                $("#body-content").html(xhr.responseText);
            }
        });
    }
}

var emptyImgUrl = $('#combineImg').attr('src');


$(function () {
    $('#combineImg').css('border-radius', '8px');
    $('#combineImg').css('min-height', ($('#combineImg').width() * 0.55));
    $('#combineImg').css('max-height', ($('#combineImg').width() * 0.55));
    var category_blocks = $('#select_categories').find('a');

    $.each(category_blocks, function (index, item) {
        var Category = $(item).attr('id');
        var Weight = $(item).find('span').html().replace(' ', '').replace('%', '');
        categories.push(Category);
        weights.push(Weight);
    })

    var tb = $('#test_block');

    var count = tb.find("tbody").find("tr").length;

    for (var i = 0; i < count; i++) {

        var newTR = tb.find("tr").eq(i).html().replace(/\[\d+\]/g, '[' + i + ']').replace(/_\d+/g, '_' + i);

        tb.find("tr").eq(i).html(newTR);
    }
});

$(window).resize(function () {
    $('#combineImg').css('min-height', ($('#combineImg').width() * 0.55));
    $('#combineImg').css('max-height', ($('#combineImg').width() * 0.55));
});
function imgCheck(img) {
    if (img.value == null || img.value == "") {
        img.value = "";
        $('#combineImg').attr('src', emptyImgUrl);
    }
    else {
        var fileSize = 0;
        fileSize = img.files[0].size;
        var size = fileSize / 1024;
        if (size > 10000) {
            alert("The image exceeds the limit of 10MB");
            img.value = "";
            $('#combineImg').attr('src', emptyImgUrl);
            return false;
        }
        else {
            var name = img.value;
            var fileName = name.substring(name.lastIndexOf(".") + 1).toLowerCase();
            if (fileName != "jpg" && fileName != "jpeg" && fileName != "png" && fileName != "jpg") {
                alert("Only accpet the format of JPG, JPEG or PNG");
                img.value = "";
                $('#combineImg').attr('src', emptyImgUrl);
                return false;
            }
            else {
                var url = getImgLocalUrl(img.files[0]);
                $('#combineImg').attr('src', url);
            }
        }
    }
}

function getImgLocalUrl(file) {
    var url = null;
    if (window.createObjectURL != undefined) {
        url = window.createObjectURL(file);
    } else if (window.URL != undefined) {
        //webkit or chrome ie11 ie10 firefox oprea
        url = window.URL.createObjectURL(file);
    } else if (window.webkitURL != undefined) { // webkit or chrome

        url = window.webkitURL.createObjectURL(file);
    }
    return url;
}


function addCategory() {
    $('#search_test_category_popup').attr('style', 'display:block');
}

$('#close').on('click', function () {
    $('#search_test_category_popup').attr('style', 'display:none');
});

function confirm_category() {

    var cate = $('#category').val();

    if (cate == "") {
        alert('Please enter a category.');
    }
    else if ($.inArray(cate, categories) == 0) {
        alert(cate + ' has been selected already.');
    }
    else {
        categories.push(cate);
        weights.push(0);

        var HTML = ('<a id="' + cate + '" class="col-2 text-center btn btn-light" onclick="display_Test_Block(\'' + cate + '\')">' + cate + '<span id="weightPercent" class="pl-1 text-white"></span></a>');
        $(HTML).insertBefore($("#select_categories").find('div').first());
        $('#test_block').append('<div id="tests_block_' + cate + '" style = "display:none" class="col-12 mt-4" ><label id="category_title" class="control-label text-white pl-3"></label><table id="select_tests" class="container-fluid" ><tbody></tbody></table><div id="addTests" style="margin-top:10px" class="btn btn-light text-capitalize" ><a href="javascript:void(0)">Add Tests<i class="material-icons">add</i></a></div><span asp-validation-for="Tests" class="col-12 text-danger"></span></div></div>');

        $('#tests_block_' + cate + '').find('#category_title').html('<div class="row mb-1"><h5 class="mb-0 mr-4">' + cate + '</h5><input  autocomplete="off" type="number" class="mr-1" placeholder="Weight" onchange="checkWeightOnEach(this)"><span class="row pl-4">%</span></div><a>(Please select Tests from the drop down list)</a></label>');
        $('#tests_block_' + cate + '').find('#addTests').attr('onclick', 'addTest(\'' + cate + '\')');

        $('#search_test_category_popup').attr('style', 'display:none');
        $('#search_test_category_popup').find('#category').val('');
    }

}

function display_Test_Block(category) {
    $('#test_block').children('div').attr('style', 'display:none');
    $('#tests_block_' + category + '').attr('style', 'display:block');
}

function checkWeightOnEach(obj) {

    if ($(obj).val() > 100) {
        alert('The you cannot have the weight of a Test over 100%');
        $(obj).val('');
    }
    else if ($(obj).val() < 1) {
        alert('The you cannot have the weight of a Test less than 1%');
        $(obj).val('');
    }
    else {
        var id = $(obj).parent().parent().parent().attr('id').replace(/tests_block_/g, '');
        $('#' + id).children('#weightPercent').html($(obj).val() + '%');
        var index = categories.indexOf(id);
        weights.splice(index, 1, $(obj).val());
    }
}