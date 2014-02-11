using System;

using Robocode;
using Robocode.Util;
using Ogres.Helpers;


namespace Ogres.TargetAcquisition
{
    class OgreTargeter : ITargeterPackage
    {
        readonly AdvancedRobot _robot;

        public OgreTargeter(AdvancedRobot robot)
        {
            _robot = robot;
        }

        /// <summary>
        /// Aim the gun at the target:
        /// 1.  Get the absolute bearing to the target.
        /// 2.  Figure out the target's lateral velocity (i.e. how fast it's moving in parallel to us).
        /// 3.  Determine how powerful the bullet should be (based on the target's range).
        /// 4.  Figure out how fast the bullet will travel (according to Robocode: 20 - [MAX POWER] * power).
        /// 5.  Determine the additional angle offset, so we can fire where the target will be, not where it
        ///     is currently.
        /// </summary>
        /// <param name="evnt">The <c>ScannedRobotEvent</c> args from the <c>OnScannedRobot</c> event.</param>
        public void Aim(ScannedRobotEvent evnt)
        { 
            var absoluteBearing = Utilities.GetAbsoluteBearing(_robot.HeadingRadians, evnt.BearingRadians);
            var enemyLateralVelocity = evnt.Velocity * Math.Sin(evnt.HeadingRadians - absoluteBearing);
            var bulletPower = GetBulletPower(evnt);
            var bulletVelocity = Utilities.GetBulletVelocity(bulletPower);
            var angleOffset = enemyLateralVelocity / bulletVelocity;

            _robot.TurnGunRightRadians(Utils.NormalRelativeAngle(absoluteBearing - _robot.GunHeadingRadians + angleOffset));
        }

        /// <summary>
        /// Shoot a bullet at our target.
        /// </summary>
        /// <param name="bulletPower">The strength of the bullet to shoot.</param>
        public void Fire(double bulletPower)
        {
            if (_robot.GunHeat == 0D && Math.Abs(_robot.GunTurnRemaining) < 10D)
            {
                _robot.SetFire(bulletPower);
            }
        }

        /// <summary>
        /// Figure out how poweful the bullet should be based on the distance to the target.
        /// </summary>
        /// <param name="evnt">The <c>ScannedRobotEvent</c> args from the <c>OnScannedRobot</c> event.</param>
        /// <returns>The power of the bullet.</returns>
        public double GetBulletPower(ScannedRobotEvent evnt)
        {
            return Math.Min(400D / evnt.Distance, Rules.MAX_BULLET_POWER);
        }
    }
}
