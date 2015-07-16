using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace VSOWeb
{

    [HubName("myVariableObserverMini")]
    public class VariableObserverHub : Hub
    {
        private readonly VariableObserver _varObserver;

        public VariableObserverHub()
            : this(VariableObserver.Instance) 
        { 
        }

        public VariableObserverHub(VariableObserver varObs)
        {
            _varObserver = varObs;
        }

        public IEnumerable<DataObserver> GetAllDataObserver()
        {
            return _varObserver.GetAllDataObserver();
        }
    }

}