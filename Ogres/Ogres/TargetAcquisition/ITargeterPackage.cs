using Robocode;


namespace Ogres.TargetAcquisition
{
    interface ITargeterPackage
    {
        void Aim(ScannedRobotEvent evnt);
        void Fire(double bulletPower);
        double GetBulletPower(ScannedRobotEvent evnt);
    }
}
