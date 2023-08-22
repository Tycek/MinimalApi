using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace MinimalApi;

public class Signal
{
    private static string hostId = "10.0.1.150";
    private static int hostPort = 80;

    public static void SendPickup(SetPickupModel pickupData)
    {
        string packet = 'P' + pickupData.Quantity.ToString() + 'E' + '\0';
        byte[] requestBytes = Encoding.ASCII.GetBytes(packet);

        using Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(hostId, hostPort);

        int bytesSent = 0;
        while (bytesSent < requestBytes.Length)
        {
            bytesSent += socket.Send(requestBytes, bytesSent, requestBytes.Length - bytesSent, SocketFlags.None);
        }
        socket.Disconnect(false);
    }

    public static GetPickupModel GetPickup()
    {
        string packet = "GE" + '\0';
        byte[] requestBytes = Encoding.ASCII.GetBytes(packet);

        using Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(hostId, hostPort);

        int bytesSent = 0;
        while (bytesSent < requestBytes.Length)
        {
            bytesSent += socket.Send(requestBytes, bytesSent, requestBytes.Length - bytesSent, SocketFlags.None);
        }
        // chvilku se pocka
        Thread.Sleep(50);

        // precte se vysledek
        byte[] resultBytes = new byte[4]; //GxyE
        int bytesRead = socket.Receive(resultBytes, 4, SocketFlags.None);
        socket.Disconnect(false);

        if (bytesRead == 4)
            return new GetPickupModel { Quantity = resultBytes[1], Status = resultBytes[2] == '2' ? "Confirm" : resultBytes[2] == '1' ? "Cancel" : "Waiting" };
        else
            return new GetPickupModel { Status = "Unknown" };
    }
}
