using Harmonic.Hosting;
using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Harmonic.Networking.Rtmp;
using Harmonic;

namespace demo
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {


            RtmpServer server = new RtmpServerBuilder()
                    .UseStartup<Startup>()
                    .CheckNameAndPwd((username, pwd) =>
                    {

                        if (!"123456".Equals(pwd))
                        {
                            OnlineObj.ClinetIOPipeLines[username].Disconnect();
                            return false;
                        }
                        else
                        {
                            return true;
                        }

                    })
                    .BackIOPipeLine((username, x) =>
                    {
                        if (OnlineObj.ClinetIOPipeLines.ContainsKey(username))
                        {
                            OnlineObj.ClinetIOPipeLines[username].Disconnect();
                            OnlineObj.ClinetIOPipeLines[username] = x;
                        }
                        else
                        {
                            OnlineObj.ClinetIOPipeLines.Add(username, x);
                        }
                        return true;
                    })
                    .UseWebSocket(c =>
                    {
                        c.BindEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.12"), 8080);
                    })
                    .Build();
            var tsk = server.StartAsync();
            tsk.Wait();
            // Pipe pipe = new Pipe();

            // await WriteSomeDataAsync(pipe.Writer);
            // pipe.Writer.Complete();
            // await ReadSomeDataAsync(pipe.Reader);
        }
        static async ValueTask WriteSomeDataAsync(PipeWriter writer)
        {
            Memory<byte> workspace = writer.GetMemory(512);
            int bytes = Encoding.ASCII.GetBytes(
                "hello, world!", workspace.Span);
            writer.Advance(bytes);
            await writer.FlushAsync();
        }
        static async ValueTask ReadSomeDataAsync(PipeReader reader)
        {
            while (true)
            {
                ReadResult read = await reader.ReadAsync();
                ReadOnlySequence<byte> buffer = read.Buffer;
                if (buffer.IsEmpty && read.IsCompleted)
                    break;
                if (buffer.IsSingleSegment)
                {
                    string s = Encoding.ASCII.GetString(buffer.First.ToArray());
                    // Console.Write(s);
                }
                else
                {
                    foreach (ReadOnlyMemory<byte> segment in buffer)
                    {
                        string s = Encoding.ASCII.GetString(segment.Span);
                        // Console.Write(s);
                    }
                }
                reader.AdvanceTo(buffer.End);
            }
        }
    }
}
