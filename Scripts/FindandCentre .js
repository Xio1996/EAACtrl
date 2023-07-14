/* Java Script */
/* Socket Start Packet */
var ObjectName = "";var RA = 8.23109102576319;var Dec = -5.750485752558516;var FOV = 1.0;

var Out="";
var ObjectFound = false;

/* Find target and centre or if not found set chart centre position */
try {
	sky6StarChart.Find(ObjectName);
	/* Centre target */
	TheSkyXAction.execute("TARGET_CENTER");
	ObjectFound=true;
}
catch(err){	Out = err.message;}

try {
	if (!ObjectFound) {
		/* set RA and Dec in decimal degrees */
		sky6StarChart.RightAscension = RA;
		sky6StarChart.Declination = Dec;
		ObjectFound = true;
	}
}
catch (err) { Out += err.message; }

if (ObjectFound) {
	/* Set FOV degrees */
	if (FOV > 0) {
		sky6StarChart.FieldOfView = FOV;
	}


}

/* Socket End Packet */
