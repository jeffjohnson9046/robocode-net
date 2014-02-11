using Robocode;
using Robocode.Util;
using Ogres.Helpers;


namespace Ogres.Detection
{
    class TurnMultiplierLockScanner : IScannerPackage
    {
        readonly AdvancedRobot _robot;

        public TurnMultiplierLockScanner(AdvancedRobot robot)
        {
            _robot = robot;
        }

        public void Scan(ScannedRobotEvent evnt)
        {
            var radarTurn = Utilities.GetAbsoluteBearing(_robot.HeadingRadians, evnt.BearingRadians) - _robot.RadarHeadingRadians;

            _robot.TurnRadarRightRadians(2D * Utils.NormalRelativeAngle(radarTurn));
        }
    }
}
