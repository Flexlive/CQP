using Flexlive.CQP.Framework;
using System;
using System.Collections.Generic;

namespace Flexlive.CQP.CSharpProxy
{
    public class LogManager
    {
        public event EventHandler<CQLogEventArgs> NewLogWrite = null;

        private List<string> logMessages = null;

        private static LogManager _instance = null;

        private LogManager()
        {
            this.logMessages = new List<string>();
        }

        public static LogManager GetInstance()
        {
            if(_instance == null)
            {
                _instance = new LogManager();
            }

            return _instance;
        }

        public List<string> Logs
        {
            get
            {
                return this.logMessages;
            }
        }

        public void AddLog(string message)
        {
            this.logMessages.Add(message);

            if(this.logMessages.Count > 100)
            {
                this.logMessages.RemoveAt(0);
            }

            if(this.NewLogWrite != null)
            {
                this.NewLogWrite(this, new CQLogEventArgs() { LogMessage = message });
            }
        }
    }
}
