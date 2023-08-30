using System.IO.Ports;
using System.Text;

namespace MinimalApi;
public class SerialSignal
{
    private static string comPort = "COM5";
    private static int baudRate = 9600;

    public static void SendPickup(SetPickupModel pickupData)
    {
        string packet = 'P'+pickupData.Quantity.ToString()+'E'+'\0';
        byte[] requestBytes = Encoding.ASCII.GetBytes(packet);

        using (SerialPort serialPort = new SerialPort(comPort, baudRate))
        {
            serialPort.Open();
            serialPort.Write(requestBytes, 0, requestBytes.Length);
            serialPort.Close();
        }
    }

    public static GetPickupModel GetPickup()
    {
        string packet = "GE" + '\0';
        byte[] requestBytes = Encoding.ASCII.GetBytes(packet);
        byte[] resultBytes = new byte[4]; //GxyE
        int bytesRead = 0;

        using (SerialPort serialPort = new SerialPort(comPort, baudRate))
        {
            serialPort.Open();
            serialPort.Write(requestBytes, 0, requestBytes.Length);

            // chvilku se pocka
            Thread.Sleep(50);

            bytesRead = serialPort.Read(resultBytes, 0, 4);
            serialPort.Close();
        }
        if (bytesRead == 4)
            return new GetPickupModel { Quantity = resultBytes[1]-'0', Status = resultBytes[2] == '2' ? "Confirm" : resultBytes[2] == '1' ? "Cancel" : "Waiting" };
        else
            return new GetPickupModel { Status = "Unknown" };
    }
}
