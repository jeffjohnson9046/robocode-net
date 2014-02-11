using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Robocode;
using Robocode.Util;


namespace Ogres.Helpers
{
    class Utilities
    {
        public const double MAX_BULLET_VELOCITY = 20D;
        public const double ROBOT_WIDTH = 36D;
        public const double NINETY_DEGREES_IN_RADIANS = Math.PI / 2D;

        /// <summary>
        /// Determine a target's absolute bearing to this <c>Robot</c>.
        /// </summary>
        /// <param name="myHeading">This <c>Robot</c>'s heading.</param>
        /// <param name="enemyBearing">The enemy <c>Robot</c>'s bearing.</param>
        /// <returns>The enemy <c>Robot</c>'s absolute bearing.</returns>
        /// <remarks>
        /// Sooo... this doesn't really seem to calculatea the absolute bearing at all, meaning this
        /// method is pretty poorly named.  However, it seems to be working *fairly* well, so I'm going
        /// to leave it for now.
        /// </remarks>
        public static double GetAbsoluteBearing(double myHeading, double enemyBearing)
        {
            return myHeading + enemyBearing;
        }
        
        /// <summary>
        /// Calculate a <c>Bullet</c>'s velocity based on its power.
        /// </summary>
        /// <param name="bulletPower">The power/strength of the <c>Bullet</c>.</param>
        /// <returns>The <c>Bullet</c>'s velocity.</returns>
        /// <seealso cref="http://robowiki.net/wiki/Robocode/Game_Physics"/>
        public static double GetBulletVelocity(double bulletPower)
        {
            return MAX_BULLET_VELOCITY - (Rules.MAX_BULLET_POWER * bulletPower);
        }
    }
}
