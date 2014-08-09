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


function pageInit() {
    $("#DateOfTheEvent").datepicker({ dateFormat: "dd/mm/yy", minDate: 0 });
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
            $("#BackroundColorOpacity").val(alpha);
        }
    });
    initplacechangecevent(placeSelecter);
}

function RangeInputEvents(fromElement, toElement, rangeToShowElement, rangeToShowContainer) {
    $(fromElement).on('input', function () {
        validateFromToInput($(this), fromElement, toElement);
    });
    $(toElement).on('input', function () {
        $(this).val($(this).val().replace(" ", ""));
        validateFromToInput($(this), fromElement, toElement);
    });

    function validateFromToInput(thisElement, fromElement, toElement) {
        $(thisElement).val($(thisElement).val().replace(" ", ""));
        var fromNumber = $(fromElement).val();
        var toNumber = $(toElement).val();
        if (validateNumbers(fromElement, toElement)) {
            setDraft(fromNumber, toNumber);
        }
        else { $(thisElement).val("") }
    }

    function setDraft(fromNumber, toNumber) {
        $(rangeToShowContainer).css("display", "block");
        if (toNumber == "") {
            if (fromNumber == "") {
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
}
function placeSelecter(place, geoLng, geoLat) {
    var locationIcon = $("#draftLocation").find("span");
    $("#draftLocation").text("");
    $("#draftLocation").append(locationIcon);
    $("#draftLocation").append(place);
    $('#PlaceLongitude').val(geoLng);
    $('#PlaceLatitude').val(geoLat);
};



function validateNumbers(minNumElementID, maxNumElementID) {
    var validNum = true;
    var minNumber = $(minNumElementID).val();
    var maxNumber = $(maxNumElementID).val();
    if (minNumber != "" && isPositiveInteger(minNumber) == false)
    { validNum = false; }
    else if (maxNumber != "" && isPositiveInteger(maxNumber) == false)
    { validNum = false; }
    return validNum;
}
function updateDraftTitle() {
    var titledom = $("#Title");
    var val = $(titledom).val();
    if ($(titledom).val() != "") {
        $("#draftTitle").text(val);
    }
}

function updateEventDateDraft() {
    var itemtoupdate = $("#DateOfTheEvent");
    // var hour = $("#HourTimeOfTheEvent").val();
    // var minute = $("#MinuteTimeOfTheEvent").val();

    var date = $(itemtoupdate).val();
    if (date != "" && date != undefined) {
        var arrdate = date.split('/');
        //   var datetext = ("#draftTitle").text;
        $("#draftDate").text(removeZero(arrdate[0]) + "." + removeZero(arrdate[1]));
    }
}

function updateImageDraft(input) {
    // var input = $('#ImageUrl');
    if (input != null) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $(".event-box-innerwrap").css("background-image", "url('" + e.target.result + "')");
            }
            reader.readAsDataURL(input.files[0]);
        }
    }
}

function updateSideOfTheTextDraft() {
    var textsidedom = $('#dpdTextSide');
    var side = $(textsidedom).val();
    if (side != "") {
        $(".event-box-content").removeClass().addClass("event-box-content").addClass(side);
    }
}

function updateDraftGender() {
    var dpdGender = $('#dpdGender');
    var gender = $(dpdGender).val();
    if (gender != null) {
        if (gender == "Both") {
            $(".event-gender-container").css("display", "none");
        }
        else {
            $("#draftGender").text(gender);
        }
    }
}

function initEvents() {
    $('#Title').on('input', function () {
        updateDraftTitle();
    });
    $('#DateOfTheEvent').on('change', function () {
        updateDateTime();
        updateEventDateDraft();
    });

    $('#ImageUrl').on('change', function () {
        updateImageDraft(this);
    });

    $('#dpdTextSide').on('change', function () {
        updateSideOfTheTextDraft();
    });

    $('#HourTimeOfTheEvent').on('input', function () {
        updateDateTime();
        $(this).val($(this).val().replace(" ", ""));
        var hour = $(this).val();
        if (isPositiveInteger(hour) === false || hour > 23) {
            $(this).val("");
        }
        else {
            updateEventDateDraft();
        }
    });

    $('#MinuteTimeOfTheEvent').on('input', function () {
        updateDateTime();
        $(this).val($(this).val().replace(" ", ""));
        var minute = $(this).val();
        if (isPositiveInteger(minute) === false || minute > 59) {
            $(this).val("");
        }
        else {
            updateEventDateDraft();
        }
    });

    function updateDateTime() {
        var minute = $('#MinuteTimeOfTheEvent').val();
        var hour = $('#HourTimeOfTheEvent').val();
        var date = $('#DateOfTheEvent').val();
        if (date != '') {
            var h = "00";
            var m = "00";
            if (minute != '') {
                m = minute;
            }
            if (hour != '') {
                h = hour;
            }
            $('#DateTimeOfTheEvent').val(date + " " + h + ":" + m + ":" + "00")
        }
        else {
            $('#DateTimeOfTheEvent').val("");
        }
    }

    RangeInputEvents("#txtNumOfParticipantsFrom", "#txtNumOfParticipantsTo", "#draftNumOfParticipants",
        ".event-num-of-participants-container");
    RangeInputEvents("#txtAgeOfParticipantsFrom", "#txtAgeOfParticipantsTo", "#draftAgeOfParticipants",
        ".event-age-of-participants-container");
    $('#dpdGender').on('change', function () {
        updateDraftGender();
    });
}
$(document).ready(function () {
    pageInit();
    initEvents();
    updateEventDateDraft();
    updateDraftTitle();
    updateSideOfTheTextDraft();
    updateDraftGender();
});