//$(document).ready(function () {
//    $("#BirthDate").datepicker({ dateFormat: "dd/mm/yy", maxDate: 0 });
//});
function setBirthdate(thisElement) {
    var currentTime = new Date();
    $(thisElement).val($(thisElement).val().replace(" ", ""));
    var day = $('#BirthDateDay').val();
    var month = $('#BirthDateMonth').val();
    var year = $('#BirthDateYear').val();
    if (day != '' &&
        month != '' &&
        year != '') {
        $('#BirthDate').val(day + '/' + month + '/' + year);
    }
}
$(document).ready(function () {
    $('#BirthDateDay').on('change', function () { setBirthdate($(this)) });
    $('#BirthDateMonth').on('change', function () { setBirthdate($(this)) });
    $('#BirthDateYear').on('change', function () { setBirthdate($(this)) });

    $(".next-button").click(function () {
        $(".all-parts").animate({
            "margin-left": "-=315px"
        }, 800);

    });
    $(".back-button").click(function () {
        $(".all-parts").animate({
            "margin-left": "+=315px"
        }, 800);

    });

    $(".Gender span").click(function () {
        $(this).prev().prop("checked", true);;

    });


    $('.form-signin .interests-container').slimscroll({
        color: '#333',
        size: '10px',
        width: '300px',
        height: '270px',
        opacity: '1'
    });
});

function onRegisterFailed() {
    $(".form-signin .third-part .success").css({ "display": "none" });
    $(".form-signin .third-part .failure").css({ "display": "block" });
    $(".all-parts").animate({
        "margin-left": "-=315px"
    }, 800);
}
function onRegisterSuccess(response) {
    $(".form-signin .third-part .failure").css({ "display": "none" });
    $(".form-signin .third-part .success").css({ "display": "block" });
    $(".all-parts").animate({
        "margin-left": "-=315px"
    }, 800);
}

jQuery.validator.addMethod('birthdatee', function (value, element, params) {
    if (!/Invalid|NaN/.test(new Date(value))) {
        return new Date(value) > new Date();
    }
    return isNaN(value) && isNaN($(params).val()) || (parseFloat(value) > parseFloat($(params).val()));
}, '');

jQuery.validator.unobtrusive.adapters.add('birthdatee', {}, function (options) {
    options.rules['birthdatee'] = true;
    options.messages['birthdatee'] = options.message;
});