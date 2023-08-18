# Purpose

SATEL INTEGRA along with ETHM-1 Plus module deliver a communication protocol that allows you to build your home automation. However, that protocol requires some knowledge about sockets, bytes, encoding, etc. Thus, I have decided to make a piece of code that would read and write SATEL inputs and outputs to boolean values (true/false), and send or receive them over MQTT.

The code is 100% .NET based. Feel free to create an issue, or just suggest a feature.

## Note about existing solution

Yes, there are already existing pieces of software that deliver connectivity to SATEL Integra through ETHM-1 Plus, e.g.:

- [Home Assistant Integration](https://www.home-assistant.io/integrations/satel_integra/)
- [Node-Red Nodes](https://flows.nodered.org/node/node-red-contrib-satel-integra-integration)
- [openHAB binding](https://www.openhab.org/addons/bindings/satel/)

But, they are specific for a given environment. I just wanted to make something independent from any specific "runtime". You can consume it anywhere if you know how to use MQTT.

# Supported features

The following read/write operations are currently supported:

- Read binary input state
- Read binary output state
- Change binary output state
- Read if alarm is armed
- Read if alarm is triggered


# NOT IN SCOPE / NOT SUPPORTED

This tool is not planned to deliver any sophisticated logic. It does simple work: it maps ETHM-1 Plus state to MQTT.
If you'd like to create some scenarios based on Integra's state, you can always create some piece of automation on your own (e.g. using Node-Red). 


## App to SATEL encryption

Satel ETHM-1 Plus support encryption, however this feature is not supported yet.

## App to MQTT encrytption

The App supports ca self-signed certificates. Read the documentation below to see how to use a custom certificate.

# Configuration
## Configuration blocks

### Loop

```json
"Loop": {
    "Interval": 100,
    "IterationCount": 8,
    "OnErrorDelayMiliseconds": 4000
  }
```
As it has been described above, the app works on background worker, that checks system state in given interval. The properties above allow to configure how the background worker checks system state.

- Interval - defines how long (in miliseconds) should be a delay between system checks.
- IterationCount - defines how many iterations the app performs before it resets its state and starts system check again
- OnErrorDelayMilisecnds - defines how long the app should wait to reconnect/restart when a critical error comes (for example when the connectivity to ETHM-1 Plus has been lost)

Tip: Leave it default.

### MQTT 
```json
"Mqtt": {
    "Host": "",
    "Port": 0,
    "User": "",
    "Password": "",
    "CrtPath": null
  }
```

This section contains needed to connect MQTT Broker. CrtPath is not required (if you accept that your trafic will not be encrypted).

### Satel

```json
"Satel": {
    "Address": "",
    "Port": 0,
    "UserCode": "",
    "OutgoingEventMappings": [{
      {
        "Type": "Output",
        "IOIndex": 121,
        "Topic": "satel/outputs/<output-name>/state"
      },
      {
        "Type": "Input",
        "IOIndex": 10,
        "Topic": "satel/inputs/<input-name>/state"
      },
    }],
    "IncomingEventMappings": [
      {
        "Type": "BinaryOutput",
        "Notify": false,
        "IOIndex": 121,
        "Topic": "satel/outputs/<output-name>/update"
      }
    ]
  },
```

- Address - this is IP Address that belongs to ETHM-1 Plus interface.
- Port - port that ETHM-1 Plus interface uses to integrate with 3rd party devices
- User - integration user's PIN
- OutgoingEventMapping - configuration of entities that send state change signals to outer world
- IncomingEventMapping - configuration of entities that can receiver signals from outer world.
