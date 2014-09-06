
$(".open-popup").each(function () {
    //init
    var dataForPopupElement = $(this).parent().find(".data-for-popup");
    var dataForPopup = dataForPopupElement.html();
    var popupContentElement = $(".popup-content");
    $(popupContentElement).html(dataForPopup);
    $(dataForPopupElement).html("");
    init();
    //end init

    function init()
    {
        updateSelectedInterests();
        $(".interests-list li").each(function () {

            var numberOfCheckedInCategory = $(this).find("ul li input[type='checkbox']:checked").length;
            if (numberOfCheckedInCategory > 0)
            {
                $(this).find("ul").slideDown();
            }
        });
    }

    function updateSelectedInterests() {
        var allIds = "";
        $(popupContentElement).find("input[type='checkbox']").each(function () {
            var ifIsChecked = $(this).prop("checked");
            if (ifIsChecked == true) {
                var id = $(this).attr("data-id");
                allIds += id + ",";
            }
        });
        $("#selectedInterests").val(allIds);

    }

    $(this).on("click", function () {
        
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


        $('span.close').off("click").on("click", function () {
            $(hiddenSection).fadeOut();
            updateSelectedInterests();
        });
        $(".hidden-popup-container").off("click").on("click", function () {
            $(hiddenSection).fadeOut();
            updateSelectedInterests();
        });
        $(".main-category").each(function () {
            $(this).off("click").on("click", function (event) {
                event.stopPropagation();
                var mainCategory = $(this);
                var allCatInterests = $(mainCategory).find("input.allCategoryInterests");
                var ifIsChecked = $(allCatInterests).prop("checked");
                if (ifIsChecked == false) {
                    $(allCatInterests).prop("checked", true);
                    checkAll(allCatInterests);
                    $(mainCategory).parent().find("ul").slideDown();
                }
                else if (ifIsChecked == true) {
                    $(allCatInterests).prop("checked", false);
                    checkAll(allCatInterests);
                }
            });
        });

        function checkAll(allCatInterests) {
            var ifIsChecked = $(allCatInterests).prop("checked");
            if (ifIsChecked == true) {
                $(allCatInterests).parent().parent().find("input.sub-category").each(function () {
                    $(this).prop("checked", true);
                });
            }
            else if (ifIsChecked == false) {
                $(allCatInterests).parent().parent().find("input.sub-category").each(function () {
                    $(this).prop("checked", false);
                });
            }
        };

        $("input.allCategoryInterests").each(function () {
            $(this).off("click").on("click", function (event) {
                event.stopPropagation();
                var allCatInterests = $(this);
                checkAll(allCatInterests);
                $(allCatInterests).parent().parent().find("ul").slideDown();
            });
        });

        $(".interests-list li ul li").each(function () {
            $(this).off("click").on("click", function (event) {
                event.stopPropagation();
                var subCategory = $(this).find(".sub-category");
                var ifIsChecked = $(subCategory).prop("checked");
                if (ifIsChecked == true) {
                    $(subCategory).prop("checked", false);
                    $(this).parent().parent().find("input.allCategoryInterests").each(function () {
                        $(this).prop("checked", false);
                    });
                }
                else {
                    $(subCategory).prop("checked", true);
                }
            });
        });

        $(".interests-list li ul li input.sub-category").each(function () {
            $(this).off("click").on("click", function (event) {
                event.stopPropagation();
                var ifIsChecked = $(this).prop("checked");
                if (ifIsChecked == false) {
                    $(this).parent().parent().parent().find("input.allCategoryInterests").each(function () {
                        $(this).prop("checked", false);
                    });
                }
            });
        });

    });
});