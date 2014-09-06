$(document).ready(function () {
    initplacechangecevent(setLatLng);
    function setLatLng(place, geoLng, geoLat) {
        $('#PlaceLongitude').val(geoLng);
        $('#PlaceLatitude').val(geoLat);
    }
});


