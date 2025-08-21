using System.Text;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace i4.Services
{

    public class PipeClientAsync
    {
        string pipeName;
        public PipeClientAsync(string pipeName)
        {
            this.pipeName = pipeName;
        }

        public async Task<string> SendAsync(string sendStr, int timeOut = 1000)
        {
            using (var pipeStream = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut, PipeOptions.Asynchronous))
            {
                pipeStream.Connect(timeOut);

                var lengthBuffer = new byte[4];

                var writeBuffer = Encoding.UTF8.GetBytes(sendStr);

                lengthBuffer = BitConverter.GetBytes(writeBuffer.Length);

                await pipeStream.WriteAsync(lengthBuffer, 0, lengthBuffer.Length);


                await pipeStream.WriteAsync(writeBuffer, 0, writeBuffer.Length);

                int readByte = await pipeStream.ReadAsync(lengthBuffer, 0, lengthBuffer.Length);

                if (readByte != 4)
                {
                    throw new Exception("Invalid message length in pipe.");

                }
                int messageLength = BitConverter.ToInt32(lengthBuffer, 0);

                var messageBuffer = new byte[messageLength];

                readByte = await pipeStream.ReadAsync(messageBuffer, 0, messageBuffer.Length);

                // Convert byte buffer to string
                return Encoding.UTF8.GetString(messageBuffer, 0, messageBuffer.Length);

            }
        }
    }
}