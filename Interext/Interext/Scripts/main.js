function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}
$(document).ready(function () {
    initplacechangecevent(setLatLng);
    /*sliding down the submenu*/
    $('.top-sub-menu').each(function () {
        var res;     /*when hover on the menu_item_title */
        $(this).parent().eq(0).hoverIntent(function () {
            res = false;
            res = $('.top-sub-menu:eq(0)', this).stop(true, true).slideDown('fast', function () {
                return true;
            });
        },
        function () {
            if (res)
                $('.top-sub-menu:eq(0)', this).stop(true, true).slideUp('slow');
        });
    });

    $(".search-txt-container .fa-reorder").on("click", function () {
        var pathname = window.location.pathname.toLowerCase();
        if (pathname == "/" || pathname == "/home/index") {
            $("#searchEventsForm").parent().stop(true, true).slideDown('slow', function () {
                return true;
            });
        }
        else {
            var searchWord = $(".search-txt-container input[type='text']").val();
            if (searchWord == "") {
                window.location = "/?advanced=1";
            }
            else {
                window.location = "/?advanced=1&&searchword=" + searchWord;
            }
        }
    });
    $(".fa-search").click(function () {
        
        var pathname = window.location.pathname.toLowerCase();
        if (pathname == "/" || pathname == "/home/index") {
            $(".search-btn").click();
            $(".fa-reorder").click();
        }
        else {
            var searchWord = $(".search-txt-container input[type='text']").val();
            if (searchWord == "") {
                window.location = "/";
            }
            else {
                window.location = "/?searchword=" + searchWord;
            }
        }
    });

    $("#searchEventsForm .close-btn").on("click", function () {
        $("#searchEventsForm").parent().stop(true, true).slideUp('slow');
    });

    $("#DateOfTheEventFrom").datepicker({ dateFormat: "dd/mm/yy" });
    $("#DateOfTheEventTo").datepicker({ dateFormat: "dd/mm/yy" });


    $(".search-txt-container input[type='text']").on('input', function () {
        $("#FreeText").val($(this).val());
    });
   
    function setLatLng(place, geoLng, geoLat) {
        $('#PlaceLongitude').val(geoLng);
        $('#PlaceLatitude').val(geoLat);
    }

    $('.popup-content').slimscroll({
        color: '#333',
        size: '10px',
        width: '350px',
        height: '270px',
        opacity: '1',
        alwaysVisible:true
    });

    if (getParameterByName("advanced") == "1") {
        $(".search-txt-container .fa-reorder").click();
    }
    
});