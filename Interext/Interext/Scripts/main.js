$(document).ready(function () {   /*sliding down the submenu*/
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

    $(".search-txt-container .fa-gear").on("click", function () {
        $("#searchEventsForm").parent().stop(true, true).slideDown('slow', function () {
            return true;
        });
    });
    $("#searchEventsForm .close-btn").on("click", function () {
        $("#searchEventsForm").parent().stop(true, true).slideUp('slow');
    });

    $("#DateOfTheEventFrom").datepicker({ dateFormat: "dd/mm/yy"});
    $("#DateOfTheEventTo").datepicker({ dateFormat: "dd/mm/yy"});
   

    $(".search-txt-container input[type='text']").on('input', function () {
        $("#FreeText").val($(this).val());
    });

    $(".open-popup").each(function () { 
        $(this).on("click", function () {
            var dataForPopupElement = $(this).parent().find(".data-for-popup");
            var dataForPopup = dataForPopupElement.html();
            var popupContentElement = $(".popup-content");
            popupContentElement.html(dataForPopup);
            var hiddenSection = $('section.hidden-popup-container');
            hiddenSection.fadeIn()
                // unhide section.hidden
                .css({ 'display': 'block' })
                // set to full screen
                .css({ width: $(window).width() + 'px', height: $(window).height() + 'px' })
                .css({
                    top: ($(window).height() - hiddenSection.height()) / 2 + 'px',
                    left: ($(window).width() - hiddenSection.width()) / 2 + 'px'
                })
                // greyed out background
                .css({ 'background-color': 'rgba(0,0,0,0.5)' })
                .appendTo('body');
            $(".popup").click(function () { return false; });
            $('span.close').click(function () {
                $(hiddenSection).fadeOut();
                dataForPopupElement.html(popupContentElement.html());
            });
            $(".hidden-popup-container").one("click", function () {
                $(hiddenSection).fadeOut();
                dataForPopupElement.html(popupContentElement.html());
            });
        });
    });
        
});