using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Robocode;
using Robocode.Util;
using Ogres.Helpers;


namespace Ogres.Detection
{
    class TurnMultiplierLockScanner : IScannerPackage
    {
        AdvancedRobot _robot;

        public TurnMultiplierLockScanner(AdvancedRobot robot)
        {
            _robot = robot;
        }

        public void Scan(ScannedRobotEvent evnt)
        {
            double radarTurn = Utilities.GetAbsoluteBearing(_robot.HeadingRadians, evnt.BearingRadians) - _robot.RadarHeadingRadians;

            _robot.TurnRadarRightRadians(2D * Utils.NormalRelativeAngle(radarTurn));
        }
    }
}
