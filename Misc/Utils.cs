using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PLEXAPI.Utils
{
    public class Utils
    {

        // Crop left of string by slen chars and add ... if cropped
        public static string cropStr(string det, int? slen)
        {

            if (slen != null && (slen>0 && (det.Trim().Length > slen)))
            {
                return det.Trim().Substring(0, (int)slen - 1) + "...";
            }
            else
            {
                return det.Trim();
            }


        }

    }
}
