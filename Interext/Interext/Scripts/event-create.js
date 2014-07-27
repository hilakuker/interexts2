﻿function removeZero(input) {
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
            $("#BackroundColorOpacity").val(alpha);
        }
    });
    initplacechangecevent(placeSelecter);
}

function RangeInputEvents(fromElement, toElement, rangeToShowElement, rangeToShowContainer) {
    $(fromElement).on('input', function () {
        var fromNumber = $(this).val();
        var toNumber = $(toElement).val();
        if (validateNumbers(fromElement, toElement)) {
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
        if (validateNumbers(fromElement, toElement)) {
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
function placeSelecter(place) {
            var locationIcon = $("#draftLocation").find("span");
            $("#draftLocation").text("");
            $("#draftLocation").append(locationIcon);
            $("#draftLocation").append(place);
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
function updateDraftTitle()
{
    var titledom = $("#Title");
    var val = $(titledom).val();
    if ($(titledom).val() != "") {
        $("#draftTitle").text(val);
    }
}

function updateEventDateDraft() {
    var itemtoupdate = $("#DateTimeOfTheEvent");
    var date = $(itemtoupdate).val();
    if (date != "" && date != undefined) {
        var arrdate = date.split('/');
        var datetext = ("#draftTitle").text;
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
    $('#DateTimeOfTheEvent').on('change', function () {
        updateEventDateDraft();
    });

    $('#ImageUrl').on('change', function () {
        updateImageDraft(this);
    });

    $('#dpdTextSide').on('change', function () {
        updateSideOfTheTextDraft();
    });
    RangeInputEvents("#txtNumOfParticipantsFrom", "#txtNumOfParticipantsTo", "#draftNumOfParticipants", ".event-num-of-participants-container");
    RangeInputEvents("#txtAgeOfParticipantsFrom", "#txtAgeOfParticipantsTo", "#draftAgeOfParticipants", ".event-age-of-participants-container");
    $('#dpdGender').on('change', function () {
        updateDraftGender();
    });
}
$(document).ready(function () {
    pageInit();
    initEvents();
    updateEventDateDraft();
    updateDraftTitle();
   // updateImageDraft();
    updateSideOfTheTextDraft();
    updateDraftGender();
});