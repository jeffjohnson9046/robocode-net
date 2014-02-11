using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Robocode;


namespace Ogres.Detection
{
    interface IScannerPackage
    {
        void Scan(ScannedRobotEvent evnt);
    }
}
