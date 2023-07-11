using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace MinimalApi;

public class Signal
{
    public static void Send(string data)
    {
        //Uri uri = new Uri("tcp://4.tcp.eu.ngrok.io");
        data += '\0';
        byte[] requestBytes = Encoding.ASCII.GetBytes(data);

        using Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        //socket.Connect(uri.Host, 17403);
        socket.Connect("localhost", 8889);

        // Send the request.
        // For the tiny amount of data in this example, the first call to Send() will likely deliver the buffer completely,
        // however this is not guaranteed to happen for larger real-life buffers.
        // The best practice is to iterate until all the data is sent.
        int bytesSent = 0;
        while (bytesSent < requestBytes.Length)
        {
            bytesSent += socket.Send(requestBytes, bytesSent, requestBytes.Length - bytesSent, SocketFlags.None);
        }
        socket.Disconnect(false);
    }
}
