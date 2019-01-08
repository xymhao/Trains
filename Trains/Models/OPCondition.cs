using System;
using System.Collections.Generic;
using System.Text;

namespace Trains.Models
{
    public class OPCondition
    {
        public virtual bool GetConditionResult()
        {
            return false;
        }
    }

    public class LessThanCondtion : OPCondition
    {

    }
}

