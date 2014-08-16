var doughnutDataInterests = [
				{
				    value: 300,
				    color: "#F7464A",
				    highlight: "#FF5A5E",
				    label: "Books"
				},
				{
				    value: 150,
				    color: "#02a789",
				    highlight: "#5AD3D1",
				    label: "Dogs"
				},
				{
				    value: 120,
				    color: "#bdd84d",
				    highlight: "#FFC870",
				    label: "Guitar"
				},
				{
				    value: 100,
				    color: "#FFB347",
				    highlight: "#A8B3C5",
				    label: "Cooking"
				},
				{
				    value: 50,
				    color: "#949FB1",
				    highlight: "#616774",
				    label: "Football"
				},
                {
                    value: 30,
                    color: "#4D5360",
                    highlight: "#616774",
                    label: "Football"
                }

];


//var doughnutDataInterests = [
//				{
//				    value: 300,
//				    color: "#F7464A",
//				    highlight: "#FF5A5E",
//				    label: "Books"
//				},
//                {
//                    value: 150,
//                    color: "#bdd84d",
//                    highlight: "#616774",
//                    label: "Football"
//                },
//                {
//                    value: 120,
//                    color: "#02a789",
//                    highlight: "#616774",
//                    label: "Tracking"
//                },
//                {
//                    value: 100,
//                    color: "#fee05e",
//                    highlight: "#FFC870",
//                    label: "Guitar"
//                },
//				{
//				    value: 50,
//				    color: "#7db6d3",
//				    highlight: "#5AD3D1",
//				    label: "Dogs"
//				},
//				{
//				    value: 40,
//				    color: "#46BFBD",
//				    highlight: "#A8B3C5",
//				    label: "Cooking"
//				}


//];


var doughnutDataAge = [

                {
                    value: 300,
                    color: "#F7464A",
                    highlight: "#FF5A5E",
                    label: "26-30"
                },
				{
				    value: 150,
				    color: "#02a789",
				    highlight: "#5AD3D1",
				    label: "16-20"
				},
				{
				    value: 120,
				    color: "#bdd84d",
				    highlight: "#FFC870",
				    label: "21-25"
				},
				{
				    value: 100,
				    color: "#FFB347",
				    highlight: "#A8B3C5",
				    label: "35-40"
				}
];

var doughnutDataGender = [
				{
				    value: 100,
				    color: "#F7464A",
				    highlight: "#FF5A5E",
				    label: "Women"
				},
				{
				    value: 70,
				    color: "#02a789",
				    highlight: "#5AD3D1",
				    label: "Men"
				}

];

window.onload = function () {
    var ctx = document.getElementById("chart-area-interests").getContext("2d");
    window.myDoughnut = new Chart(ctx).Doughnut(doughnutDataInterests, { responsive: true });
    var ctx2 = document.getElementById("chart-area-gender").getContext("2d");
    window.myDoughnut2 = new Chart(ctx2).Doughnut(doughnutDataGender, { responsive: true });
    var ctx3 = document.getElementById("chart-area-age").getContext("2d");
    window.myDoughnut3 = new Chart(ctx3).Doughnut(doughnutDataAge, { responsive: true });
};