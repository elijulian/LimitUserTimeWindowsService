using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SetTimeLimitOnUser
{
    public partial class SetTimeLimitOnUser : ServiceBase
    {
        public SetTimeLimitOnUser()
        {
            InitializeComponent();
            if (!EventLog.SourceExists("UserTime"))
            {
                EventLog.CreateEventSource("UserTime", "UserTimeLog");
            }

            eventLog1.Source = "UserTime";
            eventLog1.Log = "UserTimeLog";
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("in on start");
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("in on stop");
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            var sessionId = changeDescription.SessionId;
            var reason = changeDescription.Reason;
            switch (reason)
            {
                case SessionChangeReason.SessionLogon:
                case SessionChangeReason.SessionUnlock:
                    // report logon event + time
                    eventLog1.WriteEntry("log on or unlock at " + DateTime.Now + " reason: " + reason);
                    break;
                case SessionChangeReason.SessionLogoff:
                case SessionChangeReason.SessionLock:
                    //
                    eventLog1.WriteEntry("", "log on or lock at " + DateTime.Now.ToLongTimeString() +  " reason: " + reason.ToString);
                    break;
            }             
        }
    }
}
