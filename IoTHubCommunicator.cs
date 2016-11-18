using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PT_UWP
{
    class IoTHubCommunicator
    {
        public event EventHandler<string> MessageReceivedEvent;
        private string _iotHubConnectionString =
            "HostName=PT-IoTHub.azure-devices.net;DeviceId=PT-raspberry_device;SharedAccessKey=x0Sg4wM4A7MYotU20//1ivodmeiTJh2T32vN8GJaSI0=";

        public async Task SendDataToAzure(string message)
        {
            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(_iotHubConnectionString, TransportType.Http1);
            var msg = new Message(Encoding.UTF8.GetBytes(message));
            await deviceClient.SendEventAsync(msg);
        }


        public async Task ReceiveDataFromAzure()
        {
            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(_iotHubConnectionString, TransportType.Http1);
            Message receivedMessage;
            string messageData;
            while (true)
            {
                receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage != null)
                {
                    messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    this.OnMessageReceivedEvent(messageData);
                    await deviceClient.CompleteAsync(receivedMessage);
                }
            }
        }

        protected virtual void OnMessageReceivedEvent(string s)
        {
            EventHandler<string> handler = MessageReceivedEvent;
            if (handler != null)
            {
                handler(this, s);
            }
        }
    }
}
