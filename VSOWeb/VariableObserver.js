$(function () {
    //$.connection.myVariableObserverMini correspond à la ligne :
    //[HubName("myVariableObserverMini")] dans VariableObserverHub
    var ticker = $.connection.myVariableObserverMini, // the generated client-side hub proxy
    $varTable = $('#variableTable'),
    //On cherche la balise tbody du tableau avec l'id variableTable
        $varTableBody = $varTable.find('tbody');


    function init()
    {
        ticker.server.getAllDataObserver().done(function (listDObs) {
            $varTableBody.empty();

            $.each(listDObs, function () {
                
            });

        });
    }

    // Add a client-side hub method that the server will call
    ticker.client.updateDataObserver = function (dobs) {


        
    }

    // Start the connection
    $.connection.hub.start().done(init);

});
