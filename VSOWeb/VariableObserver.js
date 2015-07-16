$(function () {
    //$.connection.myVariableObserverMini correspond à la ligne :
    //[HubName("myVariableObserverMini")] dans VariableObserverHub
    var ticker = $.connection.myVariableObserverMini, // the generated client-side hub proxy
    $varTable = $('#variableTable'),
    //On cherche la balise tbody du tableau avec l'id variableTable
        $varTableBody = $varTable.find('tbody');


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

        
    }

    // Start the connection
    $.connection.hub.start().done(init);

});
