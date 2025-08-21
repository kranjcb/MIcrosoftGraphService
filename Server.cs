using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIcrosoftGraphService.Model;

namespace MIcrosoftGraphService
{
    class Server
    {
        private static NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        private System.Threading.Timer timer;
        private DateTime lastKeepAlive = DateTime.Now;

        private ServerFunctions serverFunctions;

        private readonly AutoResetEvent ev = new AutoResetEvent(false);

        private int timeoutMinutes = 5;

        void TimerCallback(object state)
        {
            try
            {
                var start = DateTime.Now;
                if (start.Subtract(lastKeepAlive).TotalMinutes > timeoutMinutes)
                {

                    logger.Info("No request for {Minutes}, shutting down.", new { Minutes = start.Subtract(lastKeepAlive).TotalMinutes });

                    ev.Set();

                    return;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            timer.Change(1000, Timeout.Infinite);
        }
        public bool Start(string[] args)
        {
            try
            {
                string pipeName = "MIcrosoftGraphService";

                if (args.Length > 0)
                {
                    pipeName = args[0];
                }

                logger.Trace("Starting NamedPipes on pipe {0}", pipeName);

                using (var server = new i4.Services.PipeServerAsync())
                {

                    if (args.Length > 1)
                    {
                        Int32.TryParse(args[1], out timeoutMinutes);
                    }

                    logger.Trace("Process will shutdown after {0} minutes of inactivity.", timeoutMinutes);

                    serverFunctions = new ServerFunctions();

                    server.PipeMessage += Server_PipeMessage;

                    //startup what we need to startup

                    server.Listen(pipeName);

                    timer = new System.Threading.Timer(TimerCallback, null, 1000, Timeout.Infinite);

                    ev.WaitOne();

                    //dispose what we need to dispose
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }

            return true;
        }

        private void Server_PipeMessage(string message, ref string reply)
        {
            logger.Trace("Message recieved: {0}", message);
            lastKeepAlive = DateTime.Now;

            ResponseMessage response = null;

            try
            {
                var msg = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestMessage>(message);

                response = ProcessRequest(msg, message);
            }
            catch (Exception ex)
            {
                response = new ErrorResponseMessage(ex.ToString());
                logger.Error(ex);
            }

            reply = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            logger.Trace("Response: {0}", reply);
        }


        private ResponseMessage ProcessRequest(RequestMessage message, string messageSource)
        {
            //process the request

            return new ResponseMessage();
        }
    }
}
