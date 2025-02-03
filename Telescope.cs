using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using ASCOM.DriverAccess;

namespace EAACtrl
{
    internal class EAATelescope
    {
        private Telescope Telescope = null;
        private Util Util = new Util();
        private string TelescopeID = "";

        public EAATelescope()
        {
            TelescopeID = Properties.Settings.Default.ASCOMTelescope;
        }

        ~EAATelescope()
        {
            if (Telescope != null)
            {
                if (Telescope.Connected)
                {
                    Telescope.Connected = false;    
                    Telescope = null;
                }
            }
        }
        
        private string sMsg = "";

        public string Message
        {
            get
            {
                return sMsg;
            }
        }

        public string EquatorialSystem 
        {
            get
            {
                try
                {
                    string result = "";
                    if (Connected)
                    {
                        EquatorialCoordinateType ECT = Telescope.EquatorialSystem;
                        switch (ECT)
                        {
                            case EquatorialCoordinateType.equTopocentric:
                                result = "JNOW";
                                break;
                            case EquatorialCoordinateType.equJ2000:
                                result = "J2000";
                                break;
                        }
                    }
                    return result;
                }
                catch (Exception e)
                {
                    sMsg = "Telescope: Get EquatorialSystem failed - " + e.Message;
                }

                return "";
            }
        }

        public double RightAscension
        {
            get
            {
                sMsg = "";
                try 
                { 
                    return Telescope.RightAscension; 
                }
                catch (Exception e)
                {
                    sMsg = "Telescope: Get RA failed - " + e.Message;
                    return 0.0;
                }
            }
        }

        public double Declination
        {
            get
            {
                sMsg = "";
                try 
                { 
                    return Telescope.Declination; 
                }
                catch (Exception e)
                {
                    sMsg = "Telescope: Get Dec failed - " + e.Message;
                    return 0.0;
                }
            }
        }

        public double Azimuth
        {
            get
            {
                sMsg = "";
                try
                {
                    return Telescope.Azimuth;
                }
                catch (Exception e)
                {
                    sMsg = "Telescope: Get Az failed - " + e.Message;
                    return 0.0;
                }
            }
        }

        public double Altitude
        {
            get
            {
                sMsg = "";
                try
                {
                    return Telescope.Altitude;
                }
                catch (Exception e)
                {
                    sMsg = "Telescope: Get Alt failed - " + e.Message;
                    return 0.0;
                }
            }
        }

        public bool Slewing
        {
            get
            {
                sMsg = "";
                try 
                { 
                    return Telescope.Slewing; 
                }
                catch (Exception e ) 
                {
                    sMsg = "Telescope: Get Slewing failed - " + e.Message;
                    return false; 
                }
            }
        }

        public bool Tracking
        {
            get
            {
                sMsg = "";
                try 
                { 
                    return Telescope.Tracking ; 
                }
                catch (Exception e)
                {
                    sMsg = "Telescope: Get Tracking failed - " + e.Message;
                }
                return false;
            }
            set
            {
                sMsg = "";
                try
                {
                    Telescope.Tracking = value;
                }
                catch (Exception e)
                {
                    sMsg = "Telescope: Set Tracking failed - " + e.Message;
                }
            }
        }

        public bool CanSetTracking
        {
            get
            {
                sMsg = "";
                try
                {
                    return Telescope.CanSetTracking;
                }
                catch (Exception e)
                {
                    sMsg = "Telescope: Get CanSetTracking failed - " + e.Message;
                }
                return false;
            }
        }

        public bool Connected
        {
            get
            {
                sMsg = "";
                try 
                { 
                    return Telescope.Connected; 
                }
                catch (Exception e)
                {
                    sMsg = "Telescope: Get connection failed - " + e.Message;
                }
                return false;
            }
            set
            {
                sMsg = "";
                try
                {
                    Telescope.Connected = value;    
                }
                catch (Exception e)
                {
                    sMsg = "Telescope: set connection failed - " + e.Message;
                }
            }
        }

        public string DriverVersion
        {
            get
            {
                sMsg = "";
                try
                {
                    return Telescope.DriverVersion;
                }
                catch
                {
                    return "";
                }
            }
        }

        public bool CanSlew
        {
            get
            {
                sMsg = "";
                try
                {
                    return Telescope.CanSlew;
                }
                catch ( Exception e ) 
                {
                    sMsg = "Telescope: Get CanSlew failed - " + e.Message;
                }
                return false;
            }
        }

        public bool CanPark
        {
            get
            {
                sMsg = "";
                try
                {
                    return Telescope.CanPark;
                }
                catch( Exception e )
                {
                    sMsg = "Telescope: Get CanUnPark failed - " + e.Message;
                }
                return false;
            }
        }

        public bool AtPark
        {
            get
            {
                sMsg = "";
                try
                {
                    return Telescope.AtPark;
                }
                catch (Exception e)
                {
                    sMsg = "Telescope: Get AtPark failed - " + e.Message;
                }
                return false;
            }
        }

        public bool UnPark()
        {
            sMsg = "";
            try
            {
                Telescope.Unpark();
                return true;
            }
            catch (Exception e)
            {
                sMsg = "Telescope: Set UnPark failed - " + e.Message;
            }
            return false;
        }

        public bool Stabilise()
        {
            sMsg = "";
            if (Connected)
            {
                try
                {
                    if (Telescope.Azimuth >= 180)
                    {
                        Telescope.MoveAxis(TelescopeAxes.axisPrimary, -0.1);
                        Telescope.MoveAxis(TelescopeAxes.axisSecondary, 0.1);
                        Wait(3000);
                        Telescope.MoveAxis(TelescopeAxes.axisPrimary, 0);
                        Telescope.MoveAxis(TelescopeAxes.axisSecondary, 0);
                        Wait(1000);
                        Telescope.MoveAxis(TelescopeAxes.axisPrimary, 0.1);
                        Telescope.MoveAxis(TelescopeAxes.axisSecondary, -0.1);
                        Wait(2990);
                        Telescope.MoveAxis(TelescopeAxes.axisPrimary, 0);
                        Telescope.MoveAxis(TelescopeAxes.axisSecondary, 0);
                    }
                    else
                    {
                        Telescope.MoveAxis(TelescopeAxes.axisPrimary, -0.1);
                        Telescope.MoveAxis(TelescopeAxes.axisSecondary, -0.1);
                        Wait(3000);
                        Telescope.MoveAxis(TelescopeAxes.axisPrimary, 0);
                        Telescope.MoveAxis(TelescopeAxes.axisSecondary, 0);
                        Wait(1000);
                        Telescope.MoveAxis(TelescopeAxes.axisPrimary, 0.1);
                        Telescope.MoveAxis(TelescopeAxes.axisSecondary, 0.1);
                        Wait(2990);
                        Telescope.MoveAxis(TelescopeAxes.axisPrimary, 0);
                        Telescope.MoveAxis(TelescopeAxes.axisSecondary, 0);
                    }
                }
                catch (Exception e)
                {
                    sMsg = "Telescope: Stabilise failed - " + e.Message;
                }
            }
            
            return false;
        }

        public bool AddAlignmentPoint(double SolvedRA, double SolvedDec)
        {
            bool bResult = false;
            sMsg = "";

            if (Connected)
            {

                string Action = "Telescope:AddAlignmentReference";
                string ActionParams = SolvedRA.ToString() + ":" + SolvedDec.ToString();

                try
                {
                    Telescope.Action(Action, ActionParams);
                    sMsg = "Alignment Point Added " + SolvedRA.ToString() + " : " + SolvedDec.ToString();
                    bResult = true;
                }
                catch (Exception e)
                {
                    sMsg = "Alignment failed - " + e.Message;
                }
            }

            return bResult;
        }


        public string TrackingRate
        {
            get
            {
                sMsg = "";
                string Rate = "";
                if (Telescope != null && Telescope.Connected)
                {
                    switch (Telescope.TrackingRate)
                    {
                        case DriveRates.driveSidereal:
                            Rate = "Sidereal";
                            break;
                        case DriveRates.driveLunar:
                            Rate = "Lunar";
                            break;
                        case DriveRates.driveSolar:
                            Rate = "Solar";
                            break;
                    }
                }
                return Rate;
            }
        }

        public void Wait(int ms)
        {
            Util.WaitForMilliseconds(ms);
        }

        public bool Choose()
        {
            bool result = false;

            Util U = new Util();
            TelescopeID = Telescope.Choose("");
            if (TelescopeID != "")
            {
                Properties.Settings.Default.ASCOMTelescope = TelescopeID;
                result =true;
            }

            return result;
        }

        public bool Connect()
        {
            bool result = false;
            sMsg = "";

            if (Telescope == null)
            {
                Telescope = new Telescope(TelescopeID);
            }

            try
            {
                Telescope.Connected = true;
                result = true;
            }
            catch (Exception e)
            { 
                sMsg = "Telescope: Connect failed - " + e.Message;
            }

            return result ;
        }

        public bool Disconnect()
        {
            bool result = false;
            sMsg = "";

            if (Telescope != null)
            {
                try
                {
                    Telescope.Connected = false;
                    result = true;
                }
                catch (Exception e)
                { 
                    sMsg = "Telescope: Disconnect failed - " + e.Message;
                }
            }
            return result;
        }

        public bool Slew(double RA, double Dec)
        {
            bool result = false;
            sMsg = "";

            if (Connected)
            {
                try
                {
                    Telescope.SlewToCoordinatesAsync(RA, Dec);
                    result = true;
                }
                catch (Exception e)
                {
                    sMsg = "Telescope: Slew Failed - " + e.Message;
                }
            }
            return result;
        }

        public bool AbortSlew()
        {
            bool result = false;
            sMsg = "";

            if (Connected)
            {
                Telescope.AbortSlew();
                return true;
            }
            return result;
        }
    }
}
