$(document).ready(function () {
    $('#colorSelector').ColorPicker({
        color: '#5959ab',
        onShow: function (colpkr) {
            $(colpkr).fadeIn(500);
            return false;
        },
        onHide: function (colpkr) {
            $(colpkr).fadeOut(500);
            return false;
        },
        onChange: function (hsb, hex, rgb) {
            $('#colorSelector div').css('backgroundColor', '#' + hex);
            var rgba = $(".event-box-content").css("background-color");
            var rgbaArr = rgba.split(")")[0].split(",");
            var alpha = rgbaArr[rgbaArr.length - 1];
            $(".event-box-content").css("background-color", "rgba(" + rgb.r + "," + rgb.g + "," + rgb.b + "," +alpha+ ")");
        }
    });
});
		
		
	
	
	