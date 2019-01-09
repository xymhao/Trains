using System;
using System.Collections.Generic;
using System.Text;

namespace Trains
{
    public abstract class OPCondition
    {
        public abstract bool GetConditionResult(int num, int val);
    }

    public class LessThanCondtion : OPCondition
    {
        public override bool GetConditionResult(int num, int val)
        {
            return val < num;
        }
    }

    public class LessThanAndEqualCondtion : OPCondition
    {
        public override bool GetConditionResult(int num, int val)
        {
            return val <= num;
        }
    }

    public class EqualCondition : OPCondition
    {
        public override bool GetConditionResult(int num, int val)
        {
            return num.Equals(val);
        }
    }
}

