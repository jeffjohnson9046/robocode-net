using System;
using System.Drawing;

using Robocode;
using Ogres.Detection;
using Ogres.TargetAcquisition;
using Ogres.Movement;


namespace Ogres
{
    class Ogre_MkIII : AdvancedRobot
    {
        IScannerPackage _sensors;
        ITargeterPackage _gun;
        IMovementPackage _treads;

        double _enemyPreviousEnergy = 100D;

        /// <summary>
        /// The main game loop:
        /// 1.  Decorate our robot.
        /// 2.  Set the various robot components to move independently - body, gun and radar all turn independently.
        /// 3.  Wire up the various packages (movement, radar and targeting).
        /// 4.  Start turning the radar infinitely.
        /// 5.  Execute all queued actions.
        /// </summary>
        public override void Run()
        {
            SetColors(Color.DarkOliveGreen, Color.Khaki, Color.DarkSlateGray, Color.DeepPink, Color.PaleGreen);

            IsAdjustGunForRobotTurn = true;
            IsAdjustRadarForGunTurn = true;
            IsAdjustRadarForRobotTurn = true;

            _sensors = new WidthLockScanner(this);
            _gun = new OgreTargeter(this);
            _treads = new DefensiveMovement(this);

            while (true)
            {
                if (RadarTurnRemaining == 0D)
                {
                    TurnRadarRightRadians(Double.PositiveInfinity);
                }
                Execute();
            }
        }

        /// <summary>
        /// Determine what to do when another robot is detected by our radar.
        /// </summary>
        /// <param name="evnt">The <c>ScannedRobotEvent</c> args containing data about the robot that
        /// was detected.</param>
        public override void OnScannedRobot(ScannedRobotEvent evnt)
        {
            _treads.MakeDefensiveMove(evnt, _enemyPreviousEnergy);

            _sensors.Scan(evnt);
            
            _gun.Aim(evnt);

            var bulletPower = _gun.GetBulletPower(evnt);
            _gun.Fire(bulletPower);
        }

        /// <summary>
        /// Handle getting hit by a bullet (i.e. start moving in hopes of getting out of the way of the
        /// next bullet).
        /// </summary>
        /// <param name="evnt">The <c>HitByBulletEvent</c> args containing data about the bullet that 
        /// hit us.</param>
        public override void OnHitByBullet(HitByBulletEvent evnt)
        {
            _treads.ReactToBulletHit(evnt);
        }

        /// <summary>
        /// Handle running into a wall (basically, reverse direction and keep going).
        /// </summary>
        /// <param name="evnt">The <c>HitWallEvent</c> args containing data about the wall that was hit.</param>
        public override void OnHitWall(HitWallEvent evnt)
        {
            _treads.ReactToWall(evnt);
        }

        /// <summary>
        /// Handle hitting another target with a bullet from our gun.  In this case, recording the enemy's current
        /// energy state.
        /// </summary>
        /// <param name="evnt">The <c>BulletHitEvent</c> args describing the target's state when our bullet hit.</param>
        public override void OnBulletHit(BulletHitEvent evnt)
        {
            _enemyPreviousEnergy = evnt.VictimEnergy;
        }

        /// <summary>
        /// Handle crashing into another robot.  Record the enemy robot's energy state (<c>AdvancedRobot</c>s take 
        /// damage from collisions) and attempt to move away from the enemy robot.
        /// </summary>
        /// <param name="evnt">The <c>HitRobotEvent</c> args containing data about the collision.</param>
        public override void OnHitRobot(HitRobotEvent evnt)
        {
            _enemyPreviousEnergy = evnt.Energy;
            _treads.ReactToRobotImpact(evnt);
        }
    }
}