using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Robocode;
using Robocode.Util;


namespace Ogres.TargetAcquisition
{
    interface ITargeterPackage
    {
        void Aim(ScannedRobotEvent evnt);
        void Fire(double bulletPower);
        double GetBulletPower(ScannedRobotEvent evnt);
    }
}
