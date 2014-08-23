$(document).ready(function () {
    init();

    function updateSelectedInterests() {
        var allIds = "";
        $(".interests-list").find("input[type='checkbox']").each(function () {
            var ifIsChecked = $(this).prop("checked");
            if (ifIsChecked == true) {
                var id = $(this).attr("data-id");
                allIds += id + ",";
            }
        });
        $("#selectedInterests").val(allIds);

    }
    function init() {
        updateSelectedInterests();
        $(".interests-list li").each(function () {

            var numberOfCheckedInCategory = $(this).find("ul li input[type='checkbox']:checked").length;
            if (numberOfCheckedInCategory > 0) {
                $(this).find("ul").slideDown();
            }
        });
    }

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
            updateSelectedInterests();
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
        updateSelectedInterests();
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
            updateSelectedInterests();
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
            updateSelectedInterests();
        });
    });

});