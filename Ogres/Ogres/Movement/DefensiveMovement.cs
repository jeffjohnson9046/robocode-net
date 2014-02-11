using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Robocode;
using Ogres.Helpers;


namespace Ogres.Movement
{
    class DefensiveMovement : IMovementPackage
    {
        AdvancedRobot _robot;
        double _movingForward = 1D;

        public DefensiveMovement(AdvancedRobot robot)
        {
            _robot = robot;
        }

        /// <summary>
        /// When hit by a bullet, try to turn ninety degrees from the origin's bearing and move ahead.
        /// </summary>
        /// <param name="evnt">The <c>HitByBulletEvent</c> args from the <c>OnHitByBullet</c> event.</param>
        public void ReactToBulletHit(HitByBulletEvent evnt)
        {
            double enemyAbsoluteBearing = Helpers.Utilities.GetAbsoluteBearing(_robot.HeadingRadians, evnt.BearingRadians);
            double turnRadians = GetTurnRadians(enemyAbsoluteBearing);

            if (enemyAbsoluteBearing < 0D)
            {
                Console.WriteLine("Hit By Bullet: enemyAbsoluteBearing: {0}: Turn RIGHT {1} Radians.", enemyAbsoluteBearing, turnRadians);
                _robot.SetTurnRightRadians(turnRadians);
            }
            else
            {
                Console.WriteLine("Hit By Bullet: enemyAbsoluteBearing: {0}: Turn LEFT {1} Radians.", enemyAbsoluteBearing, turnRadians);
                _robot.SetTurnLeftRadians(turnRadians);
            }

            _robot.SetAhead(100D * _movingForward);
        }

        /// <summary>
        /// After running into a wall, reverse direction and try to turn away from the wall.
        /// </summary>
        /// <param name="evnt">the <c>HitWallEvent</c> args from the <c>OnHitWall</c> event.</param>
        public void ReactToWall(HitWallEvent evnt)
        {
            ReverseDirection();

            //double turnRadians = GetTurnRadians(evnt.BearingRadians);
            //if (evnt.BearingRadians < 0D)
            //{
            //    _robot.SetTurnRightRadians(turnRadians * _movingForward);
            //}
            //else
            //{
            //    _robot.SetTurnLeftRadians(turnRadians * _movingForward);
            //}

            _robot.SetAhead(100D * _movingForward);
        }

        /// <summary>
        /// After colliding with another robot, move away from it.
        /// </summary>
        /// <param name="evnt">The <c>HitRobotEvent</c> args from the <c>OnHitRobot</c> event.</param>
        public void ReactToRobotImpact(HitRobotEvent evnt)
        {
            if (evnt.IsMyFault)
            {
                ReverseDirection();
            }

            _robot.SetAhead(150D * _movingForward);
        }

        /// <summary>
        /// Move in an attempt to dodge the incoming bullet:
        /// 1.  Get the absolute bearing of the enemy robot.
        /// 2.  Determine if the enemy robot's energy has dropped.
        /// 3.  Attemt to turn perpendicular to the target and move.
        /// </summary>
        /// <param name="evnt">The <c>ScannedRobotEvent</c> args from the <c>OnScannedRobot</c> event.</param>
        /// <param name="currentEnemyEnergy">The enemy target's current <c>Energy</c> level.</param>
        /// <remarks>
        /// <para>
        /// This works by detecting an enemy's change in <c>Energy</c> level.  If the enemy's <c>Energy</c> level changes, odds are
        /// he's fired a bullet at us.  There are several other conditions that might cause an enemy robot's <c>Energy</c> level to
        /// drop (e.g. being hit by one of our bullets or hitting a wall), and this defensive move strategy will react to them all.
        /// Using some other events (<c>OnBulletHit</c> or <c>OnHitRobot</c>), we can determine a few conditions when the enemy's 
        /// <c>Energy</c> was changed.  However, it's better to be safe than sorry.
        /// </para>
        /// <para>
        /// There are probably about 42,000 ways to make this better/more accurate.  One way might be to only detect <c>Energy</c>
        /// changes between 0.1 and 3.0 (the min/max power a bullet can have).
        /// </para>
        /// </remarks>
        /// <seealso cref="http://robowiki.net/wiki/Energy_drop"/>
        public void MakeDefensiveMove(ScannedRobotEvent evnt, double currentEnemyEnergy)
        {
            double enemyAbsoluteBearing = Helpers.Utilities.GetAbsoluteBearing(_robot.HeadingRadians, evnt.BearingRadians);
            double energyDiff = currentEnemyEnergy - evnt.Energy;

            if (_robot.DistanceRemaining == 0D && energyDiff > 0D)
            {
                double turnRadians = GetTurnRadians(enemyAbsoluteBearing);
                if (enemyAbsoluteBearing < 0D)
                {
                    Console.WriteLine("Defensive Move: enemyAbsoluteBearing: {0}: Turn RIGHT {1} Radians.", enemyAbsoluteBearing, turnRadians);
                    _robot.SetTurnRightRadians(turnRadians);
                }
                else
                {
                    Console.WriteLine("Defensive Move: enemyAbsoluteBearing: {0}: Turn LEFT {1} Radians.", enemyAbsoluteBearing, turnRadians);
                    _robot.SetTurnLeftRadians(turnRadians);
                }

                _robot.SetAhead(_movingForward * Utilities.ROBOT_WIDTH * Math.Abs(energyDiff));
            }
        }

        /// <summary>
        /// No sort of movement is implemented.  This movement package strictly reacts to damage.
        /// </summary>
        public void MakeOffensiveMove()
        {
            throw new NotImplementedException();
        }

        private double GetTurnRadians(double bearingRadians)
        {
            return bearingRadians + Utilities.NINETY_DEGREES_IN_RADIANS;
        }

        /// <summary>
        /// Switch the movement direction.
        /// </summary>
        /// <remarks>
        /// 1.0 = forward, -1.0 = backward.
        /// </remarks>
        private void ReverseDirection()
        {
            _movingForward *= -1D;
        }
    }
}
