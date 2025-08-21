using System;
using System.Text;
using System.IO.Pipes;

namespace i4.Services
{
    public delegate void DelegateMessage(string message, ref string reply);

    class PipeServerAsync : IDisposable
    {
        private static NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();

        public event DelegateMessage PipeMessage;
        string pipeName;

        public void Listen(string pipeName)
        {
            // Set to class level var so we can re-use in the async callback method
            this.pipeName = pipeName;
            // Create the new async pipe 
            CreateNewServer();

        }
        NamedPipeServerStream lastPipeServer = null;

        public void Dispose()
        {
            if (lastPipeServer != null)
            {
                lastPipeServer.Dispose();
            }
        }
        private void CreateNewServer()
        {
            logger.Trace("Creating server...");

            lastPipeServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut,
                254, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

            // Recursively wait for the connection again and again....
            lastPipeServer.BeginWaitForConnection(
               new AsyncCallback(WaitForConnectionCallBack), lastPipeServer);
        }
        private async void WaitForConnectionCallBack(IAsyncResult iar)
        {
            logger.Trace("Client connected....");

            try
            {
                CreateNewServer();

                // Get the pipe
                using (NamedPipeServerStream pipeServer = (NamedPipeServerStream)iar.AsyncState)
                {
                    // End waiting for the connection
                    pipeServer.EndWaitForConnection(iar);


                    byte[] lengthBuffer = new byte[4];


                    int readByte = await pipeServer.ReadAsync(lengthBuffer, 0, lengthBuffer.Length);

                    if (readByte != 4)
                    {
                        throw new Exception("Invalid message length in pipe.");

                    }
                    int messageLength = BitConverter.ToInt32(lengthBuffer, 0);

                    var messageBuffer = new byte[messageLength];

                    await pipeServer.ReadAsync(messageBuffer, 0, messageBuffer.Length);

                    // Convert byte buffer to string
                    string stringData = Encoding.UTF8.GetString(messageBuffer, 0, messageBuffer.Length);

                    string reply = "";

                    if (PipeMessage != null)
                    {
                        PipeMessage.Invoke(stringData, ref reply);
                    }

                    var replyBytes = Encoding.UTF8.GetBytes(reply);

                    lengthBuffer = BitConverter.GetBytes(replyBytes.Length);

                    await pipeServer.WriteAsync(lengthBuffer, 0, lengthBuffer.Length);

                    await pipeServer.WriteAsync(replyBytes, 0, replyBytes.Length);
                }
            }
            catch (ObjectDisposedException)
            {
                // Ignore this exception, it is thrown when the server is disposed
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}