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
    color: "#ffb347",
    colorOnHover: "#ffce88"
},
{
    color: "#cbcbcb",
    colorOnHover: "#e2e0e0"
},
{
    color: "#78647d",
    colorOnHover: "#a493a8"
},
{
    color: "#fea2a2",
    colorOnHover: "#fdc5c5"
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

function fillStatistics(statistics, container) {
    var index = 0;
    $.each(statistics, function () {
        var number = this.number;
        var title = this.title;
        var statisticItem = {
            value: number,
            color: colors[index].color,
            highlight: colors[index].colorOnHover,
            label: title
        };
        container.push(statisticItem);
        index++;
    });
};

function LoadStatisticCanvases() {
    var ctx = document.getElementById("chart-area-interests").getContext("2d");
    window.myDoughnut = new Chart(ctx).Doughnut(doughnutDataInterests, { responsive: true });
    var ctx2 = document.getElementById("chart-area-gender").getContext("2d");
    window.myDoughnut2 = new Chart(ctx2).Doughnut(doughnutDataGender, { responsive: true });
    var ctx3 = document.getElementById("chart-area-age").getContext("2d");
    window.myDoughnut3 = new Chart(ctx3).Doughnut(doughnutDataAge, { responsive: true });
};

$(document).ready(function () {
    var statisticValues = jQuery.parseJSON($("#hidStatisticValues").val());
    fillStatistics(statisticValues.Gender, doughnutDataGender);
    fillStatistics(statisticValues.Age, doughnutDataAge);
    fillStatistics(statisticValues.Interests, doughnutDataInterests);
    LoadStatisticCanvases();


    var confirmDeleteDialog = $("#dialog-confirm").dialog({
        autoOpen: false,
        resizable: false,
        height: 140,
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

});



//$("#deleteEventForm").onSubmit(function () {
//    var form = $(this);
//    var result = confirm("Want to delete?");
//    if (result == true) {
//        //Logic to delete the item
//        $(form).submit();
//    }
//});
