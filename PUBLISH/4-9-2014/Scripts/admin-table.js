$(document).ready(function () {
    $('#checkAll').on('click', function () {
        if ($(this).prop('checked')) {
            $('tbody tr td input[type="checkbox"]').each(function () {
                $(this).prop('checked', true);
            });
        }
        else {
            $('tbody tr td input[type="checkbox"]').each(function () {
                $(this).prop('checked', false);
            });
        }
    })
}
);
