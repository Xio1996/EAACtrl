# SharpCap MQTT Control Script
#
# The script requires an MQTT server to be specified in the parameters below.
# The script subscribes to the topic 'SharpCap/command' to receive commands from external applications.
# Data/output is sent to the topic 'SharpCap/out'.

import sys
import random
import time
from SharpCap.Interfaces import PlateSolvePurpose
from System.Threading import CancellationToken

CurrentTargetObject = ''

# The location of the paho-mqtt python package
sys.path.append(r"C:\Users\peter\AppData\Local\MyPythonPackages")

from paho.mqtt import client as mqtt_client

#TerraNova (telescope laptop)
#broker = '192.168.0.143'
#Odin (Mini PC)
broker = '192.168.50.126'
port = 1883

CommandTopic = "SharpCap/command"
ResultTopic = "SharpCap/out"

# Generate a Client ID with the subscribe prefix.
client_id = f'subscribe-{random.randint(0, 100)}'

# username = 'emqx'
# password = 'public'

#Set the camera we are going to use
sCameraName="Test Camera 1 (Deep Sky)"
#sCameraName="AA294CPRO"
#sCameraName="ZWO ASI Cameras::ZWO ASI533MM"
#sCameraName="Player One Cameras::Ares-C PRO (IMX533)"

gExposure = -1

def selectFindMode():

	# Check that the camera is connected
	if not SharpCap.IsCameraSelected:
		# If not then attempt to connect to the camera
		SharpCap.SelectedCamera = SharpCap.Cameras.Find( lambda x:x.DeviceName == sCameraName)
		
		if not SharpCap.IsCameraSelected:
			SharpCap.ShowNotification("Find Mode - Cannot find specified camera.") 
			return

	# If Live Stacking then stop the process
	if SharpCap.IsLiveStacking or SharpCap.LiveStacking.IsPaused:
		SharpCap.LiveStacking.Reset()
		SharpCap.Transforms.SelectedTransform = None

	# Load FindMode profile
	sProfileName = "FindMode (" + SharpCap.SelectedCamera.DeviceName + ")"
	if not SharpCap.SelectedCamera.LoadCaptureProfile(sProfileName):
		SharpCap.ShowNotification("Find Mode - Cannot load profile: " + sProfileName)
		return

	# Reset the Zoom to Auto. So we can see the whole image
	if SharpCap.ZoomPercent != 0:
		SharpCap.ZoomPercent=0
		
	# Display a reticule if one is not already selected
	if not SharpCap.Reticules.IsVisible:
		SharpCap.Reticules.SelectedReticule = SharpCap.Reticules[1]		

def selectCaptureMode(CaptureMode):
	global gExposure
	sTargetName=""

	# Make sure live stacking is off and reset from last object
	if SharpCap.IsLiveStacking or SharpCap.LiveStacking.IsPaused:
			SharpCap.LiveStacking.Reset()
			SharpCap.Transforms.SelectedTransform = None

	# Check that the camera is connected
	if not SharpCap.IsCameraSelected:
		# If not then attempt to connect to the camera
		SharpCap.SelectedCamera = SharpCap.Cameras.Find( lambda x:x.DeviceName == sCameraName)
		
		if not SharpCap.IsCameraSelected:
			SharpCap.ShowNotification("Capture Mode - Cannot find specified camera.") 
			return
			
	# Reset the Zoom to Auto. So we can see the whole image
	if SharpCap.ZoomPercent != 0:
		SharpCap.ZoomPercent=0
		
	# Turn off reticule if one is already selected
	if SharpCap.Reticules.IsVisible:
		SharpCap.Reticules.SelectedReticule = SharpCap.Reticules[0]			

	# Load CaptureMode profile
	sProfileName = str(CaptureMode) + " (" + SharpCap.SelectedCamera.DeviceName + ")"
	
	if not SharpCap.SelectedCamera.LoadCaptureProfile(sProfileName):
		SharpCap.ShowNotification("Capture Mode - Cannot load profile: " + sProfileName)
		return

	time.sleep(1)
	
	gExposure = SharpCap.SelectedCamera.Controls.Exposure.ExposureMs/1000;
	print("Sub Exposure=" + str(gExposure));

	SharpCap.LiveStacking.Activate()


def selectSaveAsSeen(Cmd):
    global gExposure
    ImageDetails=""
    
    if SharpCap.IsLiveStacking:
		
        # Save the image
        SharpCap.LiveStacking.SaveImageWithDisplayStretch()
		
        # Get stack info (StackedFrames, Total Exposure, Last Exposure)
        StackedFrames = SharpCap.LiveStacking.StackedFrames;
        #SubExposure = SharpCap.SelectedCamera.Controls.Exposure.ExposureMs/1000;
        TotalExposure = StackedFrames * gExposure;
        
        sStackInfo = str(StackedFrames) + "," + str(TotalExposure) + "," + str(gExposure) 

        # Find the path and name of the last stacked image
        sLastImageFile = SharpCap.CaptureFolder + "\\" + SharpCap.LiveStacking.GetSavedFiles()[len(SharpCap.LiveStacking.GetSavedFiles())-1]

        sLastSettingsFile = sLastImageFile[:sLastImageFile.rfind('_')]
        sLastSettingsFile = sLastSettingsFile + "_WithDisplayStretch.CameraSettings.txt"
        
        # Return image and image settings details.
        client = mqtt_client.Client(client_id)
        ImageDetails= Cmd + "|" + CurrentTargetObject + "|" + sStackInfo + "|" + sLastImageFile + "|" + sLastSettingsFile
        print (ImageDetails)
        
    else:
         SharpCap.ShowMessageBox("Not Live Stacking!")

    return ImageDetails

def connect_mqtt() -> mqtt_client:
    def on_connect(client, userdata, flags, rc):
        if rc == 0:
            print("SharpCap connected to MQTT Broker - " + str(broker) + " port " + str(port))
        else:
            print("Failed to connect, return code %d\n", rc)

    client = mqtt_client.Client(client_id)
    # client.username_pw_set(username, password)
    client.on_connect = on_connect
    client.connect(broker, port)
    return client


def subscribe(client: mqtt_client):
    def on_message(client, userdata, msg):
        global CurrentTargetObject

        print(f"Received `{msg.payload.decode()}` from `{msg.topic}` topic")

        SCMsg = msg.payload.decode()
        if len(SCMsg) == 0:
            # Nothing in the message!
            return
            
        params = SCMsg.split("|")
        print(params)
        
        if len(params) == 0:
            # No parameters! Need at least one command parameter.
            return
        
        # First parameter is the command.
        Cmd = params[0]
		
        if Cmd == "Exit":
            client.disconnect()
        elif Cmd == "Find":
            selectFindMode()
        elif Cmd == "Target":
            if len(params) == 2:        
                selectFindMode()
                # SharpCap will replace invalid filename characters with a '_'. Keep the original object ID for matching to AP object on logging.
                CurrentTargetObject = params[1]
                SharpCap.TargetName = params[1]
        elif Cmd == "Capture":
            # Check we have enough params - Command, Profile Name 
            if len(params) == 2:
                selectCaptureMode(params[1])
        elif Cmd == "Log" or Cmd == "LogAppend":
            if not CurrentTargetObject:
                ImageDetails = "FAILED|Logging No Target"
            else:
                ImageDetails = selectSaveAsSeen(Cmd)
            
                if ImageDetails == "":
                    ImageDetails = "FAILED|Logging"
                
            ret = client.publish(ResultTopic, ImageDetails)
        elif Cmd == "PlateSolve" or Cmd == "PlateSolveAlign":
            result = SharpCap.SafeGetAsyncResult(SharpCap.BlindSolver.SolveAsync(PlateSolvePurpose.Annotation, CancellationToken()))
            print("Platesolve: RA=" + str(result.RightAscension) + " Dec=" + str(result.Declination))
            ret = client.publish(ResultTopic, Cmd + "|" + str(result.RightAscension) + "|" + str(result.Declination))
			
    client.subscribe(CommandTopic)
    client.on_message = on_message
    print("SharpCap subscribed to topic - " + CommandTopic)

def run():
    print("EAACtrl Script v3")
    print("Camera = ", SharpCap.SelectedCamera)

    client = connect_mqtt()
    subscribe(client)
    client.loop_forever()


if __name__ == '__main__':
    run()