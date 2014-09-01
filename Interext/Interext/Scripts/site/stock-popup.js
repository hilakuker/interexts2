$(document).ready(function () {

    $(".open-stock-popup").each(function () {

        //init
        var dataForPopupElement = $(this).parent().find(".data-for-popup");
        var dataForPopup = dataForPopupElement.html();
        var popupContentElement = $(".popup-content-for-stock");
        $(popupContentElement).html(dataForPopup);
        $(dataForPopupElement).html("");
        //init();
        //end init

        //function init()
        //{
        //    updateSelectedInterests();
        //    $(".interests-list li").each(function () {

        //        var numberOfCheckedInCategory = $(this).find("ul li input[type='checkbox']:checked").length;
        //        if (numberOfCheckedInCategory > 0)
        //        {
        //            $(this).find("ul").slideDown();
        //        }
        //    });
        //}

        //function updateSelectedInterests() {
        //    var allIds = "";
        //    $(popupContentElement).find("input[type='checkbox']").each(function () {
        //        var ifIsChecked = $(this).prop("checked");
        //        if (ifIsChecked == true) {
        //            var id = $(this).attr("data-id");
        //            allIds += id + ",";
        //        }
        //    });
        //    $("#selectedInterests").val(allIds);

        //}








        $(this).on("click", function () {

            var hiddenSection = $('section.hidden-popup-container-for-stock');
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
                //updateSelectedInterests();
            });
            $(".hidden-popup-container-for-stock").off("click").on("click", function () {
                $(hiddenSection).fadeOut();
                //updateSelectedInterests();
            });

            $(".stock-img-wrapper").each(function () {
                $(this).off("click").on("click", function (event) {
                    event.stopPropagation();
                    var checkedImageWrapper = $(this);
                    var radioButton = $(checkedImageWrapper).find("input[type=radio]");
                    var ifIsChecked = $(radioButton).prop("checked");
                    if (ifIsChecked == false) {
                        $(radioButton).prop("checked", true); 
                    }
                    else if (ifIsChecked == true) {
                        $(radioButton).prop("checked", false);
                    }
                    updateDraftWithImageFromStock(radioButton);
                });
            });
            $("input[type=radio]").each(function () {
                $(this).off("click").on("click", function (event) {
                    event.stopPropagation();
                    var radio = $(this);
                    updateDraftWithImageFromStock(radio);
                });
            });


            function updateDraftWithImageFromStock(radioButton)
            {
                //var image = $(checkedImageWrapper).find(".stock-img").css("background-image");
                //image = image.replace("url(", "");
                //image = image.replace(")", "");
                var image = $(radioButton).val();
                $(".event-box-innerwrap").css("background-image", "url('" + image + "')");
                $("#ImageFromStock").val(image);
                $("#isImageFromStock").val("true");
            }

        });


    });

});