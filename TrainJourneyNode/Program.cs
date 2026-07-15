using Iot.Device.EPaper.Drivers.Ssd168x.Ssd1680;
using nanoFramework.Json;
using nanoFramework.Networking;
using System;
using System.Device.Gpio;
using System.Device.Spi;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;

namespace TrainJourneyNode
{
    public class JourneyStatus
    {
        public int StageId { get; set; }
        public string StageName { get; set; }
        public int SequencePosition { get; set; }
    }
    public class Program
    {
        private static int _currentStageId = -1;
        private const string ApiBaseUrl = "https://localhost:7227/api/journey/status";
        public static void Main()
        {
            Debug.WriteLine("Booting Train Journey Tracker...");

            // Connect to WiFi
            WifiNetworkHelper.ConnectDhcp("HUAWEI_E5577_F75F",
                "L94RNLA3M1M",
                requiresDateTime: true);
            Debug.WriteLine("Connected to WiFi.");

            var connectionSettings = new SpiConnectionSettings(1, 15)
            {
                ClockFrequency = 4_000_000,
                Mode = SpiMode.Mode0
            };

            var spiDevice = SpiDevice.Create(connectionSettings);

            var _gpioController = new GpioController();
            using var display = new Ssd1680(
                spiDevice,
                resetPin: 4,
                busyPin: 5,
                dataCommandPin: 2,
                width: 122,
                height: 250,
                gpioController: _gpioController,
                enableFramePaging: false
                );

            using var httpClient = new HttpClient();

            while (true)
            {
                try
                {
                    var response = httpClient.Get(ApiBaseUrl);
                    if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var json = response.Content.ReadAsString();
                        var status = (JourneyStatus)JsonConvert.DeserializeObject(json, typeof(JourneyStatus));

                        // Only refresh the E-ink screen if the station changed!
                        if (status.StageId != _currentStageId)
                        {
                            _currentStageId = status.StageId;
                            UpdateEinkDisplay(display, status);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Network Error: {ex.Message}");
                }
            }
        }

        private static void UpdateEinkDisplay(Ssd1680 display, JourneyStatus status)
        {
            display.Clear();

            // Here you would use the nanoFramework.Graphics library to draw your map.
            
            // For example, writing text to the Black memory channel:
            // graphics.DrawString($"Next Stop: {status.StageName}", font, Color.Black, 10, 10);

            // And drawing the train's current position to the Red memory channel:
            // graphics.FillCircle(Color.Red, currentX, currentY, 5);

            display.PerformFullRefresh();
            Debug.WriteLine($"Display Updated to: {status.StageName}");

            // Put display to sleep to prevent voltage damage
            display.PowerDown();
        }
    }
}
