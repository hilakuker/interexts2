var colors = [
{
    color: "#f7464a",
    colorOnHover: "#f77275"
},
{
    color: "#02bd9b",
    colorOnHover: "#2dcfb1"
},
{
    color: "#bdd84d",
    colorOnHover: "#d5ea80"
},
{
    color: "#fea2a2",
    colorOnHover: "#fdc5c5"
},
{
    color: "#78647d",
    colorOnHover: "#a493a8"
},
{
    color: "#ffb347",
    colorOnHover: "#ffce88"
},
{
    color: "#cbcbcb",
    colorOnHover: "#e2e0e0"
},
{
    color: "#779ab2",
    colorOnHover: "#b1c6d4"
},
{
    color: "#949fb1",
    colorOnHover: "#bfc8d7"
},
{
    color: "#4d5360",
    colorOnHover: "#7d818c"
}


];

//var colors = [
//{
//    color: "#f7464a",
//    colorOnHover: "#f77275"
//},
//{
//    color: "#02bd9b",
//    colorOnHover: "#2dcfb1"
//},
//{
//    color: "#bdd84d",
//    colorOnHover: "#d5ea80"
//},
//{
//    color: "#ffb347",
//    colorOnHover: "#ffce88"
//},
//{
//    color: "#cbcbcb",
//    colorOnHover: "#e2e0e0"
//},
//{
//    color: "#78647d",
//    colorOnHover: "#a493a8"
//},
//{
//    color: "#fea2a2",
//    colorOnHover: "#fdc5c5"
//},
//{
//    color: "#779ab2",
//    colorOnHover: "#b1c6d4"
//},
//{
//    color: "#949fb1",
//    colorOnHover: "#bfc8d7"
//},
//{
//    color: "#4d5360",
//    colorOnHover: "#7d818c"
//}


//];


var doughnutDataInterests = [];
var doughnutDataAge = [];
var doughnutDataGender = [];

function onJoinEventFailure(response) {
    alert("failure");
}
function onJoinEventSuccess(response) {
    if (response == false) {
        onJoinEventFailure(response)
    }
    else {
        location.reload();
    }
}

function onDeleteEventFailure(response) {
    alert("failure");

}
function onDeleteEventSuccess(response) {
    if (response == false) {
        onDeleteEventFailure(response)
    }
    else {
        location.href = "/";
    }
}

function onReportEventFailure(response) {
    alert("failure");

}
function onReportEventSuccess(response) {
    if (response == false) {
        onDeleteEventFailure(response)
    }
    else {
        $(".report-spam").css("display", "none");
        $(".report-spam-sent").css("display", "block");
    }
}

function LoadStatisticCanvases() {
    var ctx = document.getElementById("chart-area-interests").getContext("2d");
    window.myDoughnut = new Chart(ctx).Doughnut(doughnutDataInterests, { responsive: true });
    var ctx2 = document.getElementById("chart-area-gender").getContext("2d");
    window.myDoughnut2 = new Chart(ctx2).Doughnut(doughnutDataGender, { responsive: true });
    var ctx3 = document.getElementById("chart-area-age").getContext("2d");
    window.myDoughnut3 = new Chart(ctx3).Doughnut(doughnutDataAge, { responsive: true });
};

function fillStatistics(statistics, container) {
    var index = 0;
    $.each(statistics, function () {
        var number = this.number;
        var title = this.title;
        var percentage = this.percentage;
        var statisticItem = {
            value: number,
            color: colors[index].color,
            highlight: colors[index].colorOnHover,
            label: title
            //label: title + ":" + number
        };
        container.push(statisticItem);
        index++;
    });
};
var fontClasses = [
{
    fontClass: "big-font"
},
{
    fontClass: "medium-font"
},
{
    fontClass: "small-font"
}
];


function fillSubInterestsStatistics(statistics) {
    var index = 0;
    var text = "<table>";
    $.each(statistics, function () {
        text += "<tr class=\"main-category-statistics\">";
        var title = this.categoryTitle;
        text += "<td><div class=\"sub-category-icon\" style=\"background-color:" + colors[index].color + "\"></div>";
        text += "<div class=\"title\" style=\"color:" + colors[index].color + "\">" + title + ":</div></td>";
        var subStatistics = this.subCategories;
        var textForSubCatTitles = "<td><ul class=\"sub-categories\">";
        var textForSubCatPrecentage = textForSubCatTitles;

        var index2 = 0;
        $.each(subStatistics, function () {
            textForSubCatTitles += "<li class=\"" + fontClasses[index2].fontClass + "\">" + this.title + "</li>";
            textForSubCatPrecentage += "<li class=\"" + fontClasses[index2].fontClass + "\">" + this.number + "%</li>";
            index2++;
        });
        textForSubCatTitles += "</ul></td>";
        textForSubCatPrecentage += "</ul></td>";
        text += textForSubCatTitles + textForSubCatPrecentage + "</tr>";
        index++;
    });
    text += "</table>"
    $("#sub-categories-statistics").html(text);
}


function onDeleteCommentFailure() {
    return "Error in deleting the comment";
}
function onDeleteCommentSuccess() {

}



function onCommentsLoadFailure() {
    return "Error in loading the events";
}
function onCommentsLoadSuccess() {
    $("#comment").val("");
}

$(document).ready(function () {
    var statisticValues = jQuery.parseJSON($("#hidStatisticValues").val());
    fillStatistics(statisticValues.Gender, doughnutDataGender);
    fillStatistics(statisticValues.Age, doughnutDataAge);
    fillStatistics(statisticValues.Interests, doughnutDataInterests);
    fillSubInterestsStatistics(statisticValues.SubCategoriesInterests);
    LoadStatisticCanvases();


    var confirmDeleteDialog = $("#dialog-confirm").dialog({
        autoOpen: false,
        resizable: false,
        height: 200,
        modal: true,
        buttons: {
            "Continue": function () {
                finalSubmit = true;
                var form = $("#deleteEventForm");
                $(form).submit();
                $(this).dialog("close");
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });

    var finalSubmit = false;

    $("#deleteEventForm").submit(function () {
        if (!finalSubmit) {
            confirmDeleteDialog.dialog("open");
            return false;
        }
    });


    var confirmReportSpamDialog = $("#dialog-report-spam-confirm").dialog({
        autoOpen: false,
        resizable: false,
        height: 200,
        modal: true,
        buttons: {
            "Continue": function () {
                finalReportSpamSubmit = true;
                var form = $("#report-spam-form");
                $(form).submit();
                $(this).dialog("close");
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });

    var finalReportSpamSubmit = false;

    $("#report-spam-form").submit(function () {
        if (!finalReportSpamSubmit) {
            confirmReportSpamDialog.dialog("open");
            return false;
        }
    });

   
    

    var confirmCommentDeleteDialog = $("#dialog-delete-comment-confirm").dialog({
        autoOpen: false,
        resizable: false,
        height: 200,
        modal: true,
        buttons: {
            "Continue": function () {
                finalCommentDeleteSubmit = true;
                var form = $(formCommentDeleteForSubmit);
                if (form != null) {
                    $(form).submit();
                }
                $(this).dialog("close");
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });

    var startDeleteCommentDialog = function () {
            formCommentDeleteForSubmit = $(this).parent().parent();
            confirmCommentDeleteDialog.dialog("open");
    };
    var formCommentDeleteForSubmit = null;

    $(document).off("click", ".submit-button", startDeleteCommentDialog).on("click", ".submit-button", startDeleteCommentDialog);

    var confirmRemoveUserFromWaitingDialog = $("#dialog-remove-from-waiting-confirm").dialog({
        autoOpen: false,
        resizable: false,
        height: 200,
        modal: true,
        buttons: {
            "Continue": function () {
                var form = $(formRemoveUserFromWaitingForSubmit);
                if (form != null) {
                    $(form).submit();
                }
                $(this).dialog("close");
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });

    var startRemoveUserFromWaitingDialog = function () {
        formRemoveUserFromWaitingForSubmit = $(this).parent();
        confirmRemoveUserFromWaitingDialog.dialog("open");
    };
    var formRemoveUserFromWaitingForSubmit = null;

    $(document).off("click", ".remove-from-waiting-user-btn", startRemoveUserFromWaitingDialog).on("click", ".remove-from-waiting-user-btn", startRemoveUserFromWaitingDialog);


    var startApproveUserSubmit = function () {
        $(this).parent().submit();
    };
    $(document).off("click", ".accept-user-btn", startApproveUserSubmit).on("click", ".accept-user-btn", startApproveUserSubmit);
   
});




