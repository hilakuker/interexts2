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
function getTodayDate()
{
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    today = dd + '/' + mm + '/' + yyyy;
    return today;
}

function pageInit() {
    $("#DateOfTheEvent").datepicker({ dateFormat: "dd/mm/yy", minDate: 0 });
    $("#DateOfTheEvent").val(getTodayDate());
    
    var minutes = $("#MinuteTimeOfTheEvent");
    var hour = $("#HourTimeOfTheEvent");
    if (!$(".time-set").prop("checked"))
    {
        $(minutes).attr({ "disabled": "" });
        $(hour).attr({ "disabled": "" });
    }
    if ($(minutes).val() == "") {
        $(minutes).val("00");
    }
    if ($(hour).val() == "") {
        $(hour).val("00");
    }
    var initOpacity = ($("#BackroundColorOpacity").val()*100);
    $("#slider").slider({
        range: "min",
        value: initOpacity,
        min: 1,
        max: 100,
        slide: function (event, ui) {
            var rgba = $(".event-box-content").css("background-color");
            var rgbaSplit = rgba.split("rgba(");
            if (rgbaSplit[1] === undefined) {
                var rgbaSplit = rgba.split("rgb(");
            }
            var rgbaArr = rgbaSplit[1].split(")")[0].split(",");
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

$(document).ready(function () {
    $("#EventForm").submit(function () {
        var isValid = true;
        if ($("#ImageUrl:not(.from-edit)").val() == "") {

            if ($(".input-validation-error").length == 0) {

                $(".validation-summary-valid").css({ "display": "block" });
                if ($(".validation-summary-valid ul li.image-upload").length == 0) {
                    $(".validation-summary-valid ul").append("<li class=\"image-upload\">Please upload event image</li>");
                }
            }
            else {

                if ($(".validation-summary-errors ul li.image-upload").length == 0) {
                    $(".validation-summary-errors ul").append("<li class=\"image-upload\">Please upload event image</li>");
                }
            }
            isValid = false;
        }
        else {
            $("li.image-upload").each(function () {
                $(this).remove();
            })
        }
        if ($("#selectedInterests").val() == "") {
            if ($(".input-validation-error").length == 0) {

                $(".validation-summary-valid").css({ "display": "block" });
                if ($(".validation-summary-valid ul li.interests").length == 0) {
                    $(".validation-summary-valid ul").append("<li class=\"interests\">Please select Interests</li>");
                }
            }
            else {
                if ($(".validation-summary-errors ul li.interests").length == 0) {
                    $(".validation-summary-errors ul").append("<li class=\"interests\">Please select Interests</li>");
                }
            }
            isValid = false;
        }
        else {
            $("li.interests").each(function () {
                $(this).remove();
            })
        }
        if (!isValid)
            return isValid;
    });

    $(".time-set").click(function () {
        var ifIsChecked = $(this).prop("checked");
        if (ifIsChecked == true) {
            var minutes = $("#MinuteTimeOfTheEvent");
            var hours = $("#HourTimeOfTheEvent");
            $(minutes).removeAttr("disabled");
            $(hours).removeAttr("disabled");

            if ($(minutes).val() == "")
            {
                $(minutes).val("00");
            }

            if ($(hours).val() == "") {
                $(hours).val("00");
            }
        }
        else {
            $("#MinuteTimeOfTheEvent").attr({"disabled":""});
            $("#HourTimeOfTheEvent").attr({ "disabled": "" });

        }
    });
    $("#MinuteTimeOfTheEvent").focus(function () {
        $(this).val("");
    });
    $("#HourTimeOfTheEvent").focus(function () {
        $(this).val("");
    });
    updateDraftGender();
});

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
            $(".event-gender-container").css("display", "block");
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
    RangeInputEvents("#txtNumOfParticipantsFrom", "#txtNumOfParticipantsTo", "#draftNumOfParticipants",
        ".event-num-of-participants-container");
    RangeInputEvents("#txtAgeOfParticipantsFrom", "#txtAgeOfParticipantsTo", "#draftAgeOfParticipants",
        ".event-age-of-participants-container");
    $('#dpdGender').on('change', function () {
        updateDraftGender();
    });
}

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

$(document).ready(function () {
    pageInit();
    initEvents();
    updateEventDateDraft();
    updateDraftTitle();
    updateSideOfTheTextDraft();
    updateDraftGender();
    updateDateTime();
    $("#DateOfTheEvent").val(getTodayDate());
});