//$(document).ready(function () {
//    $("#BirthDate").datepicker({ dateFormat: "dd/mm/yy", maxDate: 0 });
//});
function setBirthdate(thisElement) {
    var currentTime = new Date();

    $(thisElement).val($(thisElement).val().replace(" ", ""));
    if (isPositiveInteger(thisElement.val()) == true) {
        var day = $('#BirthDateDay').val();
        var month = $('#BirthDateMonth').val();
        var year = $('#BirthDateYear').val();
        if ((day != '' && day > 31) || (month != '' && month > 12) ||
             (year != '' && year > (currentTime.getFullYear() - 10)))
        { thisElement.val(""); }
        if (day != '' && 
            month != '' && 
            year != '') {
            $('#BirthDate').val(day + '/' + month + '/' + year);
        }
    }
    else
    {
        thisElement.val("");
    }
}
$(document).ready(function () {
    $('#BirthDateDay').on('input', function () { setBirthdate($(this)) });
    $('#BirthDateMonth').on('input', function () {setBirthdate($(this))});
    $('#BirthDateYear').on('input', function () { setBirthdate($(this)) });
});