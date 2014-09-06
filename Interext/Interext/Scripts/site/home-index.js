function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

$(document).ready(function () {
    initplacechangecevent(setLatLng);
    var firstTimeLocationSelected = true;
    function setLatLng(place, geoLng, geoLat) {
        $('#PlaceLongitude').val(geoLng);
        $('#PlaceLatitude').val(geoLat);
        if (firstTimeLocationSelected)
        {
            $('#radiusOfTheLocation').val(2);
            firstTimeLocationSelected = false;
        }
    }

    $("#searchEventsForm .close-btn").on("click", function () {
        $("#searchEventsForm").parent().stop(true, true).slideUp('slow');
    });

    $("#DateOfTheEventFrom").datepicker({ dateFormat: "dd/mm/yy" });
    $("#DateOfTheEventTo").datepicker({ dateFormat: "dd/mm/yy" });


    $(".search-txt-container input[type='text']").on('input', function () {
        $("#FreeText").val($(this).val());
    });

    $('.popup-content').slimscroll({
        color: '#333',
        size: '10px',
        width: '350px',
        height: '270px',
        opacity: '1',
        alwaysVisible: true
    });


    if (getParameterByName("advanced") == "1") {
        $(".search-txt-container .fa-reorder").click();
    }

   // http://www.aspsnippets.com/Articles/Show-users-current-location-on-Google-Map-using-GeoLocation-API-in-website.aspx

    $("#searchword").val(getParameterByName("searchword"));
    $("#advanced").val(getParameterByName("advanced"));


    //if (navigator.geolocation) {
    //    navigator.geolocation.getCurrentPosition(successCallback, errorCallback, { timeout: 10000 });

    //}
    //else {
    //    $("#loadUserEvents").submit();
    //}

    $("#loadUserEvents").submit();
});

//function successCallback(position) {
//        $("#myLocationLatitude").val(position.coords.latitude);
//        $("#myLocationLongitude").val(position.coords.longitude);
//        $("#loadUserEvents").submit();
//    };
//    function errorCallback() {
//        $("#loadUserEvents").submit();
//    };