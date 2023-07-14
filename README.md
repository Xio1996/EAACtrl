# EAACtrl - EAA Workflow App and Scripts
EAACtrl is my personalised c# app and associated scripts that orchestrates my EAA workflow with AstroPlanner, SharpCap and TheSky Professional. It was never intended for public consumption, but upon a request it is made available here. I hope it helps (and not hinders) any app or scripts you might be producing. <b>There is no support or warranty and it is definitely not fit for any purpose!</b>

My EAA configuration is a desktop running EAACtrl, AstroPlanner and TheSky Professional. A laptop connected to the scope running SharpCap. The laptop has a network drive that exposes the SharpCap Captures folder. This folder is also used to communicate with SharpCap (more later).

The EAACtrl C# app communicates with the apps using the following methods.

<b>AstroPlanner (2.4b22+)</b> - Uses the built in Web Services interface. Make sure you enable the interface in Preferences->Web. The script EAAControl (named exactly) must be present in AstroPlanner. The script is in the Script folder.

<b>TheSky Professional</b> - Must be the Professional version as that allows scripting. The script is passed via TCP. Make sure that TheSky's TCP Server is enabled, Tools->TCP Server

<b>SharpCap</b> - Unfortunately, SharpCap has no interface that allows external control. Talking to the author (Robin Glover), there is an interface but its not available currently. I did try and implement a web server in Sharpcap but that seemed to intefere with SharpCap and would never shutdown cleanly (big badda boom). So, the code uses a couple of files for the purpose of inter-process (machine) communication, via the exposed network drive.

The Scripts folder contains the AstroPlanner and SharpCap scripts. The SharpCap script is executed at startup and waits until EAACtrl issues a command by creating a file in the SharpCap Captures folder. TheSky Professional script is held in a string in the EAACtrl app itself.

Have fun!

