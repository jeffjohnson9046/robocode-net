using System;

using Robocode;
using Robocode.Util;
using Ogres.Helpers;


namespace Ogres.Detection
{
    /// <summary>
    /// A scanner that implements the logic for a "Width Lock Scanner" as described on the Robocode Wiki.
    /// </summary>
    class WidthLockScanner : IScannerPackage
    {
        readonly AdvancedRobot _robot;

        public WidthLockScanner(AdvancedRobot robot)
        {
            _robot = robot;
        }

        /// <summary>
        /// Figure out how far to turn the robot's radar, making sure the radar stays "locked" on the target.  To do
        /// that, we:
        /// 1.  Calculate the target's absolute bearing.
        /// 2.  Determine how far the radar has to turn in order to stay locked on our target.
        /// 3.  Add in some padding (the "extra turn") in an attempt to account for the target's movement.
        /// </summary>
        /// <param name="evnt">The <c>ScannedRobotEvent</c> arguments from the <c>OnScannedRobot</c> event.</param>
        public void Scan(ScannedRobotEvent evnt)
        {
            var enemyAbsoluteBearing = Utilities.GetAbsoluteBearing(_robot.HeadingRadians, evnt.BearingRadians);
            var radarTurn = Utils.NormalRelativeAngle(enemyAbsoluteBearing - _robot.RadarHeadingRadians);
            var extraTurn = Math.Min(Math.Atan(Utilities.ROBOT_WIDTH / evnt.Distance), Rules.RADAR_TURN_RATE_RADIANS);

            // if radarTurn is negative, then it's turning to the left, so we'll need to push it more to the left.  Otherwise, 
            // keep turning it to the right.
            radarTurn += (radarTurn < 0D ? -extraTurn : extraTurn);

            _robot.SetTurnRadarRightRadians(radarTurn);
        }
    }
}