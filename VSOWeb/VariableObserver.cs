using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using VS;
using U_TEST;

namespace VSOWeb
{
    public class VariableObserver
    {
        private const string IP_UTEST = "10.23.154.180";

        //Instance du Singleton
        private readonly static Lazy<VariableObserver> _instance = 
            new Lazy<VariableObserver>(() => new VariableObserver(GlobalHost.ConnectionManager.GetHubContext<VariableObserverHub>().Clients));

        private readonly List<DataObserver> _listDataObs;

        private readonly object _updateDObsLOCK = new object();

        private volatile bool _updateDObs = false;

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(200);
        private readonly Timer _timer;

        private VariableController vc;

        private VariableObserver(IHubConnectionContext<dynamic> clients)
        {
            this.Clients = clients;
            
            try
            {
                IControl control = IControl.create();
                control.connect(IP_UTEST, 9090);
                Console.WriteLine("Connexion OK !!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connexion NON OK !!!");
            }

            vc = Vs.getVariableController();

            _listDataObs = new List<DataObserver>{
                new DataObserver { PathName = "CT1_LV/FPGA/LiveBit", ValueObs="" },
                new DataObserver { PathName = "CODES/CT1_M1_PNT/LiveBit", ValueObs="" }
            };

            _timer = new Timer(UpdateDataObservers, null, _updateInterval, _updateInterval);
        }

        public static VariableObserver Instance
        {
            get { return _instance.Value; }
        }

        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }

        public IEnumerable<DataObserver> GetAllDataObserver()
        {
            return _listDataObs;
        }

        private void BroadcastDataObserver(DataObserver dObs)
        {
            Clients.All.updateDataObserver(dObs);
        }

        private void UpdateDataObservers(object state)
        {
            lock (_updateDObsLOCK)
            {
                if (!_updateDObs)
                {
                    _updateDObs = true;

                    foreach (DataObserver rowObserver in _listDataObs)
                    {
                        /*DataObserver dObs = readValue3(rowObserver);

                        InjectionVariableStatus status = new InjectionVariableStatus();
                        vc.getInjectionStatus(rowObserver.PathName, status);

                        if (status.state == InjectionStates.InjectionStates_IsSet)
                        {
                            rowObserver.IsForced = true;
                        }
                        else
                        {
                            rowObserver.IsForced = false;
                        }*/
                        Random r = new Random();
                        rowObserver.DValueObs = r.Next(0, 255);

                        BroadcastDataObserver(rowObserver);
                    }

                    _updateDObs = false;
                }
            }
        }

        /// <summary>
        /// Read value 3 retourne un DataObserver
        /// </summary>
        /// <param name="completeVariable"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private DataObserver readValue3(DataObserver oldDataObs)
        {
            DataObserver dObs = oldDataObs;
            string completeVariable = oldDataObs.PathName;
            int importOk = vc.importVariable(completeVariable);
            int typeVS = -1;
            long oldTimeStamp = oldDataObs.Timestamp;
            long timeStamp = 0;
            string value = "";
            //vc = Vs.getVariableController();
            vc.getType(completeVariable, out typeVS);
            Console.WriteLine("readValue : " + completeVariable + " TYPE " + typeVS + " VC " + importOk);


            if (importOk != 0 && !oldDataObs.IsChanging)
            {
                switch (typeVS)
                {
                    ///=================================================================================================
                    /// Si le type est égal à 1 alors c'est un entier
                    ///=================================================================================================
                    case 1:
                        dObs.Type = VS_Type.INTEGER;
                        IntegerReader intr = vc.createIntegerReader(completeVariable);
                        int valVarInt;

                        if (intr != null)
                        {
                            intr.setBlocking(1 * 200);
                            VariableState t = intr.waitForConnection();

                            if (t == VariableState.Ok)
                            {
                                intr.get(out valVarInt, out timeStamp);
                                value = valVarInt.ToString();
                            }
                        }

                        break;
                    ///=================================================================================================
                    ///=================================================================================================
                    /// Si le type est égal à 2 alors c'est un double
                    ///=================================================================================================
                    case 2:
                        dObs.Type = VS_Type.DOUBLE;
                        DoubleReader dblr = vc.createDoubleReader(completeVariable);
                        double valVarDbl;

                        if (dblr != null)
                        {
                            dblr.setBlocking(1 * 200);
                            VariableState t = dblr.waitForConnection();

                            if (t == VariableState.Ok)
                            {
                                dblr.get(out valVarDbl, out timeStamp);
                                value = valVarDbl.ToString();
                            }
                        }
                        break;
                    ///=================================================================================================
                    default:
                        dObs.Type = VS_Type.INVALID;
                        value = "Undefined";
                        break;
                }

                if (!oldDataObs.ValueObs.Equals(value))
                {
                    dObs.ValueObs = value;
                    dObs.ValueHasChanged = true;
                }
                else
                {
                    dObs.ValueHasChanged = false;
                }

                dObs.Timestamp = timeStamp;
            }

            return dObs;
        }

    }
}