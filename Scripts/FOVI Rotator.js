/* Java Script */
/* Socket Start Packet */
var Out;
try {
	if (ccdsoftCamera.rotatorIsConnected() == 0) {
		ccdsoftCamera.rotatorConnect();
		Out = "Rotator Connected,"
	}

	if (ccdsoftCamera.rotatorIsConnected() > 0) {
		var Angle = sky6StarChart.Rotation
		ccdsoftCamera.rotatorGotoPositionAngle(Angle);
		Out = "  PA:" + Angle.toFixed(3);
	}
	else {
		Out = "-2";
	}
}
catch (err) { Out = "-1"; }
/* Socket End Packet */
