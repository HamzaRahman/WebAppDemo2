using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppDemo2.Models
{
    public class ValidateFever
    {
        public static bool IsNull(float? value)
        {
            if (value==null)
            {
                return true;
            }
            return false;
        }
    }
}
