using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Verhaeg.IoT.Fields.xAP;

namespace Verhaeg.IoT.xAP.Sensor.Managers
{
    public class ForwardingManager : Processor.QueueManager
    {

        // SingleTon
        private static ForwardingManager? _instance = default;
        private static readonly object padlock = new object();


        private ForwardingManager() : base("ForwardingManager")
        {


        }

        public static ForwardingManager Instance()
        {
            lock (padlock)
            {
                if (_instance == null)
                {
                    _instance = new ForwardingManager();
                }
                return _instance;
            }
        }

        protected override void Dispose()
        {
            
        }

        protected override void Process(object obj)
        {
            try
            {
                Log.Debug("Trying to parse xAP message.");
                string msg = (string)obj;
                Fields.xAP.Diagram d = new Fields.xAP.Diagram(msg);
                Log.Debug("Parsing xAP message from sensor " + d.header.str_source + " succesfull.");
                string topic;

                if (d.header.str_source.ToLower().Contains("temperature"))
                {
                    topic = "xAP/Temperature/" + d.header.str_source;
                }
                else if (d.header.str_source.ToLower().Contains("relay"))
                {
                    topic = "xAP/Relay/" + d.header.str_source;
                }
                else if (d.header.str_source.ToLower().Contains("switch"))
                {
                    topic = "xAP/Switch/" + d.header.str_source;
                }
                else
                {
                    topic = "xAP/Unknown/" + d.header.str_source;
                }

                string mqtt_message = JsonConvert.SerializeObject(d, Formatting.Indented);
                MQTT.Client.Managers.EventManager.Instance().Publish(mqtt_message, topic);
                Log.Debug("xAP data posted on MQTT broker.");

            }
            catch (Exception ex)
            {
                Log.Error("Could not parse xAP message.");
                Log.Debug(ex.ToString());
            }
        }

 
    }
}
