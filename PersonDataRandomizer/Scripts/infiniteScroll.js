var country = 1;
var seed = 1;
var page = 1;
$(function () {
    var _inCallback = false;
    function loadItems() {
        if (!_inCallback) {
            _inCallback = true;
            page++;
            $.ajax({
                type: 'GET',
                url: '/Home/Index?country=' + country + '&page=' + page,
                success: function (data, textstatus) {
                    $("#scrolList").append(data);
                    _inCallback = false;
                }
            });
        }
    }
    $(window).scroll(function () {
        if ($(window).scrollTop() + 1 >= $(document).height() - $(window).height()) {
            loadItems();
        }
    });
})()

//var country = 1;
//var seed = 1;
//var page = 1;

//function loadItems() {
//    page++;
//    $.ajax({
//        type: 'GET',
//        url: '/Home/Index?country=' + country + '&page=' + page,
//        success: function (data, textstatus) {
//            $("#scrolList").append(data);
//        }
//    });
//}

//$(function () {
//    $(window).scroll(function () {
//        if ($(window).scrollTop() + 1 >= $(document).height() - $(window).height()) {
//            loadItems();
//        }
//    });
//})()


function seedChange(x) {
    if (x == 2) seed = Math.floor(Math.random() * 10000000);
    else {
        var s = document.getElementById('seed').value;
        if (s > 0) seed = Math.floor(s);
        else seed = 0;
    }
    document.getElementById('seed').value = seed;
    refresh();
}

function refresh() {
    var countries = document.getElementsByName('country');
    for (i = 0; i < countries.length; i++) {
        if (countries[i].checked) {
            country = countries[i].value;
            break;
        }
    }
    seed = document.getElementById('seed').value;
    $.ajax({
        type: 'GET',
        url: '/Home/Index?country=' + country + '&seed=' + seed,
        success: function (data, textstatus) {
            $("#scrolList").html(data);
            _inCallback = false;
        }
    });
    loadItems();
    loadItems();
}