function removeZero(input) {
    if (input[0] == '0')
        input = input[1];
    return input;
}
//function validatenumbers()
//{
//    if (
//    $("#txtNumOfParticipantsFrom").val > $("#txtNumOfParticipantsTo").val)
//}

function isPositiveInteger(s) {
    var i = +s; // convert to a number
    if (i < 0) return false; // make sure it's positive
    if (i != ~~i) return false; // make sure there's no decimal part
    return true;
}
function pageInit() {
    $("#DateTimeOfTheEvent").datepicker({ dateFormat: "dd/mm/yy", minDate: 0 });
    $("#slider").slider({
        range: "min",
        value: 80,
        min: 1,
        max: 100,
        slide: function (event, ui) {
            var rgba = $(".event-box-content").css("background-color");
            var rgbaArr = rgba.split("rgba(")[1].split(")")[0].split(",");
            var r = rgbaArr[0];
            var g = rgbaArr[1];
            var b = rgbaArr[2];
            var alpha = ui.value / 100;
            console.log(r + "," + g + "," + b + "," + alpha);
            $(".event-box-content").css("background-color", "rgba(" + r + "," + g + "," + b + "," + alpha + ")");
        }
    });
}

function RangeInputEvents(fromElement, toElement, rangeToShowElement, rangeToShowContainer) {
    $(fromElement).on('input', function () {
        var fromNumber = $(this).val();
        var toNumber = $(toElement).val();
        if (validatenumbers(fromElement, toElement)) {
            $(rangeToShowContainer).css("display", "block");
            if (fromNumber == "") {
                if (toNumber == "") {
                    $(rangeToShowElement).text("");
                    $(rangeToShowContainer).css("display", "none");
                }
                else {
                    $(rangeToShowElement).text("1-" + toNumber);
                }
            }
            else {
                if (isPositiveInteger(fromNumber)) {
                    if (toNumber == "") {
                        $(rangeToShowElement).text(fromNumber + "+");
                    }
                    else {
                        $(rangeToShowElement).text(fromNumber + "-" + toNumber);
                    }
                }
            }
        }
        else
        { $(this).val("") }
    });
    $(toElement).on('input', function () {
        var fromNumber = $(fromElement).val();
        var toNumber = $(this).val();
        if (validatenumbers(fromElement, toElement)) {
            $(rangeToShowContainer).css("display", "block");
            if (toNumber == "") {
                if (fromNumber == "") {
                    $(rangeToShowElement).text("");
                    $(rangeToShowContainer).css("display", "none");
                }
                else {
                    $(rangeToShowElement).text(fromNumber + "+")
                }
            }
            else {
                if (isPositiveInteger(toNumber)) {
                    if (fromNumber == "") {
                        $(rangeToShowElement).text("1-" + toNumber);
                    }
                    else {
                        $(rangeToShowElement).text(fromNumber + "-" + toNumber);
                    }
                }
            }
        }
        else{ $(this).val("") }
    });
}

function validatenumbers(minNumElementID, maxNumElementID) {
    var validNum = true;
    var minNumber = $(minNumElementID).val();
    var maxNumber = $(maxNumElementID).val();
    if (minNumber != "" && isPositiveInteger(minNumber) == false)
    { validNum = false; }
    else if (maxNumber != "" && isPositiveInteger(maxNumber) == false)
    { validNum = false; }
    return validNum;
}

function initEvents() {
    $('#Title').on('input', function () {
        $("#draftTitle").text($(this).val());
    });
    $('#DateTimeOfTheEvent').on('change', function () {
        var date = $(this).val();
        var arrdate = date.split('/');
        $("#draftDate").text(removeZero(arrdate[0]) + "." + removeZero(arrdate[1]));
    });

    $('.ui-autocomplete').on('click', '.ui-menu-item', function () {
        $('.college').trigger('click');
    });

    $('#searchTextField').on('click', function () {
        console.info("test")
        var locationIcon = $("#draftLocation").find("span");
        $("#draftLocation").text("");
        $("#draftLocation").append(locationIcon);
        $("#draftLocation").append($(this).val());
    });
    $('#searchTextField').on('change', function () {
        console.info("test")
        var locationIcon = $("#draftLocation").find("span");
        $("#draftLocation").text("");
        $("#draftLocation").append(locationIcon);
        $("#draftLocation").append($(this).val());
    });
    $('#ImageUrl').on('change', function () {
        var input = this;
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $(".event-box-innerwrap").css("background-image", "url('" + e.target.result + "')");

            }
            reader.readAsDataURL(input.files[0]);
        }
    });

    $('#dpdTextSide').on('change', function () {
        var side = $(this).val();
        $(".event-box-content").removeClass().addClass("event-box-content").addClass(side);
    });
    RangeInputEvents("#txtNumOfParticipantsFrom", "#txtNumOfParticipantsTo", "#draftNumOfParticipants", ".event-num-of-participants-container");
    RangeInputEvents("#txtAgeOfParticipantsFrom", "#txtAgeOfParticipantsTo", "#draftAgeOfParticipants", ".event-age-of-participants-container");
    $('#dpdGender').on('change', function () {
        var gender = $(this).val();
        if (gender == "Both") {
            $(".event-gender-container").css("display", "none");
        }
        else {
            $("#draftGender").text(gender);
        }
    });
}
$(document).ready(function () {
    pageInit();
    initEvents();

});