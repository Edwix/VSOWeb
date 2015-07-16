// A simple templating method for replacing placeholders enclosed in curly braces.
if (!String.prototype.supplant) {
    String.prototype.supplant = function (o) {
        return this.replace(/{([^{}]*)}/g,
            function (a, b) {
                var r = o[b];
                return typeof r === 'string' || typeof r === 'number' ? r : a || typeof r === 'boolean' ? r : a;
            }
        );
    };
}

$(function () {
    //$.connection.myVariableObserverMini correspond à la ligne :
    //[HubName("myVariableObserverMini")] dans VariableObserverHub
    var ticker = $.connection.myVariableObserverMini, // the generated client-side hub proxy
    $varTable = $('#variableTable'),
    //On cherche la balise tbody du tableau avec l'id variableTable
        $varTableBody = $varTable.find('tbody'),
    //On définit le template pour chaque ligne
        rowTemplate = '<tr id="{PathName}"><td>{PathName}</td><td>{ValObs}</td><td>{IsForced}</td></tr>';

    function formatDataObserver(dObs)
    {
        return $.extend(dObs, {
            PathName: dObs.PathName,
            ValObs: dObs.DValueObs,
            IsForced: dObs.IsForced
        });
    }

    function init()
    {
        ticker.server.getAllDataObserver().done(function (listDObs) {
            $varTableBody.empty();

            $.each(listDObs, function () {
                var dObs = formatDataObserver(this);
                $varTableBody.append(rowTemplate.supplant(dObs));
            });

        });
    }

    // Add a client-side hub method that the server will call
    ticker.client.updateDataObserver = function (dobs) {
        /*var displayStock = formatDataObserver(dobs),
            $row = $(rowTemplate.supplant(displayStock));

        $varTableBody.find('tr[data-symbol=' + dobs.PathName + ']')
            .replaceWith($row);*/

        
    }

    // Start the connection
    $.connection.hub.start().done(init);

});
