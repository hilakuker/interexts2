$(document).ready(function () {   /*sliding down the submenu*/
    console.log("dsdds");
    $('.top-sub-menu').each(function () {
        var res;     /*when hover on the menu_item_title */
        $(this).parent().eq(0).hoverIntent(function () {
            res = false;
            res = $('.top-sub-menu:eq(0)', this).stop(true, true).slideDown('fast', function () {
                return true;
            });
        },
        function () {
            if (res)
                $('.top-sub-menu:eq(0)', this).stop(true, true).slideUp('slow');
        });
    });
});