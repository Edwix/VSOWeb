using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VS;

namespace VSOWeb
{
    /// <summary>
    /// Classe qui permet de récupérer les données brutes
    /// </summary>
    public class DataObserver
    {
        private string _pathName;
        private string _path;
        private string _var;
        private long _ts;
        private string _val;
        private string _valF;
        private string _map;
        private string _color;
        private string _commentColor;
        private string _wUpdated;
        private bool _hasChanged;
        private bool _loocked;
        private bool _isForced;
        private bool _isChanging;
        private VS_Type _type;

        public DataObserver()
        {
            _loocked = false;
            _isForced = false;
        }

        public int DataId
        {
            get;
            set;
        }

        public string PathName
        {
            get { return _pathName; }
            set { _pathName = value;  }
        }
        
        public string Path
        { 
            get { return _path; } 
            set { _path = value;  } 
        }

        public string Variable
        {
            get { return _var; }
            set { _var = value;  }
        }

        public string ValueObs
        {
            get;
            set;
        }

        public int DValueObs
        {
            get;
            set;
        }

        public string ValueF
        {
            get { return _valF; }
            set { _valF = value;  }
        }

        public string Mapping
        {
            get { return _map; }
            set { _map = value;  }
        }

        public string WhenUpdated
        {
            get { return _wUpdated; }
            set { _wUpdated = value;  }
        }

        public long Timestamp
        {
            get;
            set;
        }

        public bool ValueHasChanged
        {
            get { return _hasChanged; }
            set { _hasChanged = value;  }
        }

        public bool IsChanging
        {
            get { return _isChanging; }
            set { _isChanging = value;  }
        }

        public VS_Type Type
        {
            get { return _type; }
            set { _type = value;  }
        }
        

        public bool IsLocked
        {
            get { return _loocked; }
            set 
            { _loocked = value;  }
        }

        public bool IsForced
        {
            get { return _isForced; }
            set { _isForced = value;  }
        }

        public string Color
        {
            get { return _color; }
            set { _color = value;  }
        }

        public string CommentColor
        {
            get { return _commentColor; }
            set { _commentColor = value;  }
        }
    }
}
