using Microsoft.Azure.Devices.Client;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace PT_UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
            //txtblkMessages.Text = "test";
            Start();
        }

        private const string DeviceConnectionString = "HostName=PT-IoTHub.azure-devices.net;DeviceId=PT-raspberry_device;SharedAccessKey=x0Sg4wM4A7MYotU20//1ivodmeiTJh2T32vN8GJaSI0=";
        public async Task Start()
        {
            try
            {
                DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString, TransportType.Http1);
                //await SendEvent(deviceClient);
                await ReceiveCommands(deviceClient);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in sample: {0}", ex.Message);
            }
        }

        async Task SendEvent(DeviceClient deviceClient)
        {
            string dataBuffer;
            //for (int count = 0; count < MESSAGE_COUNT; count++)
            //{
            dataBuffer = "Hello Iot";
            Message eventMessage = new Message(Encoding.UTF8.GetBytes(dataBuffer));
            await deviceClient.SendEventAsync(eventMessage);
            // }
        }

        async Task ReceiveCommands(DeviceClient deviceClient)
        {
            Message receivedMessage;
            string messageData;
            while (true)
            {
                receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage != null)
                {
                    messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    txtblkMessages.Text = messageData + "\n" + txtblkMessages.Text;
                    await deviceClient.CompleteAsync(receivedMessage);
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}
