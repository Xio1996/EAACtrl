# EAACtrl - EAA Workflow App and Scripts
EAACtrl is my personalised c# app and associated scripts that orchestrates my EAA workflow with AstroPlanner, SharpCap and TheSky Professional. It was never intended for public consumption, but upon a request it is made available here. I hope it helps (and not hinders) any app or scripts you might be producing. <b>There is no support or warranty and it is definitely not fit for any purpose!</b> UPDATE - I have added the NOLICENCE file to this project, in the hope it allows any code you find useful to be freely used.

My EAA configuration is a desktop running EAACtrl, AstroPlanner and TheSky Professional. A laptop connected to the scope running SharpCap. The laptop has a network drive that exposes the SharpCap Captures folder. This folder is also used to communicate with SharpCap (more later).

The EAACtrl C# app communicates with the apps using the following methods.

<b>AstroPlanner (2.4b22+)</b> - Uses the built in Web Services interface. Make sure you enable the interface in Preferences->Web. The script EAAControl (named exactly) must be present in AstroPlanner. The script is in the Script folder.

<b>TheSky Professional</b> - Must be the Professional version as that allows scripting. The script is passed via TCP. Make sure that TheSky's TCP Server is enabled, Tools->TCP Server

<b>Stellarium</b> - Uses the Remote Control plugin on port 8090. The plugin must be enabled and 'Enabled automatically on startup' must be checked. The Remote Control Plugin uses HTTP get and post commands to expose the  Stellarium object model as a web service.

<b>SharpCap</b> - Unfortunately, SharpCap has no interface that allows external control. Talking to the author (Robin Glover), there is an interface but its not available currently. This could be implemented using a http server or in this case I used MQTT. A bit weird, but I had a mosquitto server running on a Raspberry Pi. Hence, it was easy to implement communication.

<p><b>Skychart</b> - Uses Skychart's in-built TCP server. As Skychart does not play nicely with PowerToys the config tab also has a way of specifying an IP address and port of Skychart's location. In my case on another PC.</p>
<p><b>SAMP Client</b> - A non-callable SAMP client interface is also implemented that allows EAACtrl to broadcast SAMP messages to clients connected to the same hub. This is useful for synchronising Aladin's view. A SAMP tab specifies the profile to use and provides other configuration and SAMP setup.</p>
<p><b>KStars</b> - Uses KStars DBus interface and the dbus-send.exe.</p>

The Scripts folder contains the AstroPlanner and SharpCap scripts. The SharpCap script is executed at startup and waits until EAACtrl issues a command by creating a file in the SharpCap Captures folder. TheSky Professional script is held in a string in the EAACtrl app itself.

Have fun!

Pete

