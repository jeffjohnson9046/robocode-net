using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ogres.Helpers;
using Robocode;
using Robocode.Util;

namespace Ogres.TargetAcquisition
{
    class AnotherTargeter : ITargeterPackage
    {
        private readonly AdvancedRobot _robot;

        public AnotherTargeter(AdvancedRobot robot)
        {
            _robot = robot;
        }

        public void Aim(ScannedRobotEvent evnt)
        {
            var enemyAbsoluteBearing = Utilities.GetAbsoluteBearing(_robot.HeadingRadians, evnt.BearingRadians);
            var enemyX = _robot.X + evnt.Distance * Math.Sin(enemyAbsoluteBearing);
            var enemyY = _robot.Y + evnt.Distance * Math.Cos(enemyAbsoluteBearing);
            var enemyVelocity = evnt.Velocity;
            var enemyHeading = evnt.HeadingRadians;

            var A = (enemyX - _robot.X) / enemyVelocity;
            var B = enemyVelocity/enemyVelocity * Math.Sin(enemyHeading);
            var C = (enemyY - _robot.Y) / enemyVelocity;
            var D = enemyVelocity/enemyVelocity * Math.Cos(enemyHeading);

            var a = A * A + C * C;
            var b = 2d * (A * B + C * D);
            var c = (B * B + D * D - 1d);
            var discriminator = b*b - 4*a*c;

            if (discriminator > 0d)
            {
                var t1 = 2 * a / (-b - Math.Sqrt(discriminator));
                var t2 = 2 * a / (-b + Math.Sqrt(discriminator));
                var t = Math.Min(t1, t2) >= 0d ? Math.Min(t1, t2) : Math.Max(t1, t2);

                var valueX = enemyX + enemyVelocity * t * Math.Sin(enemyHeading);
                var endX = Limit(valueX, Utilities.ROBOT_WIDTH/2d, 800d - Utilities.ROBOT_WIDTH/2d);

                var valueY = enemyY + enemyVelocity * t * Math.Cos(enemyHeading);
                var endY = Limit(valueY, Utilities.ROBOT_WIDTH / 2d, 800d - Utilities.ROBOT_WIDTH / 2d);

                var gunTurn = Math.Atan2(endX - _robot.X, endY - _robot.Y) - _robot.GunHeadingRadians;
                _robot.SetTurnGunRightRadians(Utils.NormalRelativeAngle(gunTurn));
            }

        }

        public void Fire(double bulletPower)
        {
            if (_robot.GunHeat == 0d && Math.Abs(_robot.GunTurnRemaining) < 10d)
            {
                _robot.SetFire(bulletPower);
            }
        }

        public double GetBulletPower(Robocode.ScannedRobotEvent evnt)
        {
            return Math.Min(400D / evnt.Distance, Rules.MAX_BULLET_POWER);
        }

        private double Limit(double value, double min, double max)
        {
            return Math.Min(max, Math.Max(min, value));
        }
    }
}
