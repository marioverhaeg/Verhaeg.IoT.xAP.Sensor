# Verhaeg.IoT.xAP.Sensor
Verhaeg.IoT.xAP.Sensor converts xAP messages generated by MCS XAP to MQTT: https://forums.homeseer.com/forum/lighting-primary-technology-plug-ins/lighting-primary-technology-discussion/mcsxap-michael-mcsharry.

# Configuration
Create a configuration directory in the root of the folder and add Mosquitto.json with the following JSON structure:

```json
{
  "MQTT": {
    "URI": "mqtt://x.x.x.x:1883",
    "IPAddress": "x.x.x.x",
    "Port":  1883,
    "Username": "xxx",
    "Password": "xxx"
  }
}

```