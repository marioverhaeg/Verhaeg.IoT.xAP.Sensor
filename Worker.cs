using System.Diagnostics;
using Verhaeg.IoT.Configuration.MQTT;
using Verhaeg.IoT.xAP.Sensor.Managers;

namespace Verhaeg.IoT.xAP.Sensor
{
    public class Worker : BackgroundService
    {
        // Logging
        private Serilog.ILogger Log;
        private Mosquito mos_configuration;

        public Worker(ILogger<Worker> logger)
        {
            Log = Processor.Log.CreateLog("Worker");
            mos_configuration = Mosquito.Instance("Mosquito.json");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            xAP.Client.Managers.EventManager.Instance(3639, "192.168.0.10").xap_event += Worker_xap_event;

            // Start connection to MQTT broker
            MQTT.Client.Managers.EventManager.Start(mos_configuration.ipaddress, mos_configuration.port, mos_configuration.username, mos_configuration.password, "#");

            while (!stoppingToken.IsCancellationRequested)
            {
                
                await Task.Delay(5000, stoppingToken);
            }
        }

        private void Worker_xap_event(object? sender, string e)
        {
            Log.Debug("Received xAP event, forwarding event to ForwardingManager.");
            ForwardingManager.Instance().Write(e);
        }
    }
}
