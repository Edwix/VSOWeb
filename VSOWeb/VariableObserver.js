$(function () {
    //$.connection.myVariableObserverMini correspond à la ligne :
    //[HubName("myVariableObserverMini")] dans VariableObserverHub
    var ticker = $.connection.myVariableObserverMini, // the generated client-side hub proxy
    $varTable = $('#variableTable'),
    //On cherche la balise tbody du tableau avec l'id variableTable
        $varTableBody = $varTable.find('tbody');


    ///On prend le contexte du graphique
    var ctx = document.getElementById("myChart").getContext("2d");

    var data = {
        labels: [],
        datasets: [
            {
                label: "My First dataset",
                fillColor: "rgba(220,220,220,0.2)",
                strokeColor: "rgba(220,220,220,1)",
                pointColor: "rgba(220,220,220,1)",
                pointStrokeColor: "#fff",
                pointHighlightFill: "#fff",
                pointHighlightStroke: "rgba(220,220,220,1)",
                data: []
            },
            {
                label: "My Second dataset",
                fillColor: "rgba(151,187,205,0.2)",
                strokeColor: "rgba(151,187,205,1)",
                pointColor: "rgba(151,187,205,1)",
                pointStrokeColor: "#fff",
                pointHighlightFill: "#fff",
                pointHighlightStroke: "rgba(151,187,205,1)",
                data: []
            }
        ]
    };

    var options = {

        ///Boolean - Whether grid lines are shown across the chart
        scaleShowGridLines: true,

        //String - Colour of the grid lines
        scaleGridLineColor: "rgba(0,0,0,.05)",

        //Animation at false to avoid some lags
        animation : false,

        responsive: false,

        //Number - Width of the grid lines
        scaleGridLineWidth: 1,

        //Boolean - Whether to show horizontal lines (except X axis)
        scaleShowHorizontalLines: true,

        //Boolean - Whether to show vertical lines (except Y axis)
        scaleShowVerticalLines: true,

        //Boolean - Whether the line is curved between points
        bezierCurve: false,

        //Number - Tension of the bezier curve between points
        bezierCurveTension: 0.4,

        //Boolean - Whether to show a dot for each point
        pointDot: false,

        //Number - Radius of each point dot in pixels
        pointDotRadius: 4,

        //Number - Pixel width of point dot stroke
        pointDotStrokeWidth: 1,

        //Number - amount extra to add to the radius to cater for hit detection outside the drawn point
        pointHitDetectionRadius: 20,

        //Boolean - Whether to show a stroke for datasets
        datasetStroke: true,

        //Number - Pixel width of dataset stroke
        datasetStrokeWidth: 2,

        //Boolean - Whether to fill the dataset with a colour
        datasetFill: true,



        //String - A legend template
        legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].strokeColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>"

    };

    var myLineChart = new Chart(ctx).Line(data, options);


    function rowObserver(dObs)
    {
        return "<tr pathName=" + dObs.DataId + "><td>" + dObs.PathName + "</td>" +
                    "<td>" + dObs.ValueObs + "</td>" +
                    "<td>" + dObs.IsForced + "</td></tr>";

    }

    function init()
    {
        ///On récupère toutes les variables VS définis dans la Classe VariableObserver.cs
        ticker.server.getAllDataObserver().done(function (listDObs) {
            $varTableBody.empty();

            $.each(listDObs, function () {
                $varTableBody.append(
                        rowObserver(this)
                    );
            });

        });
    }

    // Add a client-side hub method that the server will call
    //Cette fonction est appelé dès qu'il y a un changement de variable
    ticker.client.updateDataObserver = function (dobs) {


        $varTableBody.find('tr[pathName=' + dobs.DataId + ']').replaceWith(rowObserver(dobs));

        //Lorsqu'on a cent élément ont les efface afin de ne pas surcharger le navigateur
        if (data.labels.length >= 100) {
            myLineChart.removeData();
        }
        myLineChart.addData([dobs.ValueObs], dobs.Timestamp);
        
    }

    // Start the connection
    $.connection.hub.start().done(init);

});
