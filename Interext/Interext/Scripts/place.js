$(document).ready(
    function () {
        pageInit()
    });
function pageInit() {
    initplacechangecevent(placeSelecter);
}

function placeSelecter(place, geoLng, geoLat) {
    $('#PlaceLongitude').val(geoLng);
    $('#PlaceLatitude').val(geoLat);
};