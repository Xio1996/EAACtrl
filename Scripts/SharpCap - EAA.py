# Find and Observe Modes
# The script moves between each mode of observing, finding and frameing an object and then live stacking
# or observing the object. Each mode change configures SharpCap to my required settings.
#

import clr
clr.AddReference("System")
clr.AddReference("System.IO")

from System import *
from System.IO import *

import time

#Set the camera we are going to use
sCameraName="Test Camera 1 (Deep Sky)"
#sCameraName="AA294CPRO"

#SharpCap Capture Folder
def selectSetupFindMode():

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
 	
 	# Background subtraction Off - might cause flashing image.
 	#SharpCap.SelectedCamera.Controls[32].Value = "Off"
 	
	# Change the Find and Frame expsoure to 2000ms
	#SharpCap.SelectedCamera.Controls.Exposure.ExposureMs = 500
	 

def selectSetupObserveMode(CaptureMode):
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
			SharpCap.ShowNotification("Find Mode - Cannot find specified camera.") 
			return
			
	
	# Reset the Zoom to Auto. So we can see the whole image
	if SharpCap.ZoomPercent != 0:
		SharpCap.ZoomPercent=0
		
	# Turn off reticule if one is already selected
	if SharpCap.Reticules.IsVisible:
		SharpCap.Reticules.SelectedReticule = SharpCap.Reticules[0]			
	
	#Fetch target name from Shared file.
	TargetFileName=SharpCap.CaptureFolder + "\\ObjectInfo.txt"
	
	if File.Exists(TargetFileName):
		sTargetName = File.ReadAllText(TargetFileName) 
	
		#If the clipboard has text then use that as the target name
		#if Clipboard.ContainsText():
		SharpCap.TargetName=sTargetName			
	
	# Load CaptureMode profile
	sProfileName = "CaptureMode" + str(CaptureMode) + " (" + SharpCap.SelectedCamera.DeviceName + ")"
	if not SharpCap.SelectedCamera.LoadCaptureProfile(sProfileName):
		SharpCap.ShowNotification("Capture Mode - Cannot load profile: " + sProfileName)
		return
		
	time.sleep(1)

	SharpCap.LiveStacking.Activate()

	
def selectSaveAsSeen():
	if SharpCap.IsLiveStacking:
		
		# Save the image
		SharpCap.LiveStacking.SaveImageWithDisplayStretch()
		
		# Get stack info (StackedFrames, Total Exposure, Last Exposure)
		sStackInfo = str(SharpCap.LiveStacking.StackedFrames) + "," + str(SharpCap.LiveStacking.TotalExposure) + "," + str(SharpCap.SelectedCamera.Controls.Exposure.ExposureMs) + "\n"
		
		# Find the path and name of the last stacked image
		sLastImageFile = SharpCap.CaptureFolder + "\\" + SharpCap.LiveStacking.GetSavedFiles()[len(SharpCap.LiveStacking.GetSavedFiles())-1]
		
		sLastSettingsFile = sLastImageFile[:sLastImageFile.rfind('_')]
		sLastSettingsFile = sLastSettingsFile + ".CameraSettings.txt"
		
		File.WriteAllText(SharpCap.CaptureFolder + "\\CaptureInfo.txt",SharpCap.TargetName + "\n" + sStackInfo + sLastImageFile + "\n" + sLastSettingsFile)
		
	else:
		SharpCap.ShowMessageBox("Not Live Stacking!")
		
def ResetIPC():
	
	sSCIPCFile = SharpCap.CaptureFolder + "\\SCIPC.txt"
	if File.Exists(sSCIPCFile):
		File.Delete(sSCIPCFile)
		
	sCaptureInfoFile = SharpCap.CaptureFolder + "\\CaptureInfo.txt"
	if File.Exists(sCaptureInfoFile):
		File.Delete(sCaptureInfoFile)
		
	sObjectInfoFile = SharpCap.CaptureFolder + "\\ObjectInfo.txt"
	if File.Exists(sObjectInfoFile):
		File.Delete(sObjectInfoFile)
		
def CommandIPCFile():
	try:
		sSCIPCFile =SharpCap.CaptureFolder + "\\SCIPC.txt"
	
		if File.Exists(sSCIPCFile):
			sCommand = File.ReadAllText(sSCIPCFile) 

			# Find: Sharpcap to find mode
			if sCommand=="Find":
				selectSetupFindMode()
				File.Delete(sSCIPCFile)
			# Capture: SharpCap to LiveStacking
			elif sCommand=="CaptureMode1":
				selectSetupObserveMode(1)
				File.Delete(sSCIPCFile)
			elif sCommand=="CaptureMode2":
				selectSetupObserveMode(2)
				File.Delete(sSCIPCFile)
			elif sCommand=="CaptureMode3":
				selectSetupObserveMode(3)
				File.Delete(sSCIPCFile)			
			# Log: Save as seen and pass the path back for copying and attaching to AP observation.
			elif sCommand=="Log":
				selectSaveAsSeen()
				File.Delete(sSCIPCFile)
			elif sCommand=="LogAndFind":
				selectSaveAsSeen()
				File.Delete(sSCIPCFile)
				selectSetupFindMode()
			elif sCommand=="ResetIPC":
				resetIPC()
	except:
		SharpCap.ShowMessageBox("SharpCap - Cmd Err")
		
	time.sleep(2)
	

#SharpCap.AddCustomButton("Find Mode", None, "Configure for finding and framing objects", selectSetupFindMode)
#SharpCap.AddCustomButton("Observe Mode", None, "Configure for live stacking objects", selectSetupObserveMode)
#SharpCap.AddCustomButton("Add Observation", None, "Record Obs in AstroPlanner", selectSaveAsSeen)

#Remove any files used for IPC from previous session
ResetIPC()

while True:
	CommandIPCFile()

#SharpCap.RemoveCustomButton(SharpCap.CustomButtons[0])
#SharpCap.Sequencer.RunSequenceFile("C:\Users\pete:r\OneDrive\Astronomy\Sharpcap\Find+Frame.scs")