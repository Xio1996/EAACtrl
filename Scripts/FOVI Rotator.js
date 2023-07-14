/* Java Script */
/* Socket Start Packet */
var Out;
/* Find centre of chart and calculate PA for rotator */
try
{
	if (ccdsoftCamera.rotatorIsConnected() == 0)
	{
		ccdsoftCamera.rotatorConnect();
		Out = "Rotator Connected,"
	}

	if (ccdsoftCamera.rotatorIsConnected() > 0)
	{
		var cRA = sky6StarChart.RightAscension;
		var cDec = sky6StarChart.Declination;

		sky6Utils.ConvertRADecToAzAlt(cRA, cDec)
		sky6Utils.ConvertAzAltToRADec(sky6Utils.dOut0, sky6Utils.dOut1 - 0.25)

		sky6Utils.ComputePositionAngle(cRA, cDec, sky6Utils.dOut0, sky6Utils.dOut1)

		ccdsoftCamera.rotatorGotoPositionAngle(sky6Utils.dOut0);

		Out = Out + " RA:" + cRA + " Dec:" + cDec + " PA:" + sky6Utils.dOut0;
	}
	else {
		Out = "-2";
	}
}
catch (err) { Out = "-1"; }
/* Socket End Packet */
