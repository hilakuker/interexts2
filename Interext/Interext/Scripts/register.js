


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
function updateImageDraft(input) {
    if (input != null) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $(".image-preview").css({"background-image": "url('" + e.target.result + "')"});
            }
            reader.readAsDataURL(input.files[0]);
        }
    }
}
$(document).ready(function () {
    $("#RegisterForm").submit(function () {
        var isValid = true;
        if ($("#ImageUrl").val() == "") {

            if ($(".input-validation-error").length == 0) {

                $(".validation-summary-valid").css({ "display": "block" });
                if ($(".validation-summary-valid ul li.image-upload").length == 0) {
                    $(".validation-summary-valid ul").append("<li class=\"image-upload\">Please upload plofile image</li>");
                }
            }
            else {

                if ($(".validation-summary-errors ul li.image-upload").length == 0) {
                    $(".validation-summary-errors ul").append("<li class=\"image-upload\">Please upload plofile image</li>");
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
                    $(".validation-summary-valid ul").html("<li class=\"interests\">Please select Interests</li>");
                }
            }
            else {
                if ($(".validation-summary-errors ul li.interests").length == 0) {
                    $(".validation-summary-errors ul").html("<li class=\"interests\">Please select Interests</li>");
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
    $('#BirthDateDay').on('change', function () { setBirthdate($(this)) });
    $('#BirthDateMonth').on('change', function () { setBirthdate($(this)) });
    $('#BirthDateYear').on('change', function () { setBirthdate($(this)) });
    
    $('#ImageUrl').on('change', function () {
        updateImageDraft(this);
    });
    
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
        $(this).prev().prop("checked", true);

    });

    $(".image-preview").click(function () {
        $("#ImageUrl").click();

    });

    $('.form-signin .interests-container').slimscroll({
        color: '#333',
        size: '10px',
        width: '300px',
        height: '426px',
        opacity: '1',
        alwaysVisible: true
    });
});


