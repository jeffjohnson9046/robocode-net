using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Robocode;


namespace Ogres.Movement
{
    public interface IMovementPackage
    {
        void ReactToBulletHit(HitByBulletEvent evnt);
        void ReactToWall(HitWallEvent evnt);
        void ReactToRobotImpact(HitRobotEvent evnt);
        void MakeDefensiveMove(ScannedRobotEvent evnt, double currentEnemyEnergy);
        void MakeOffensiveMove();
    }
}
