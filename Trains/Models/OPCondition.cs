using System;
using System.Collections.Generic;
using System.Text;

namespace Trains
{
    public interface IOPCondition
    {
        bool GetConditionResult(int num, int val);
    }

    public class LessThanCondtion : IOPCondition
    {
        public bool GetConditionResult(int num, int val)
        {
            return val < num;
        }
    }

    public class LessThanAndEqualCondtion : IOPCondition
    {
        public bool GetConditionResult(int num, int val)
        {
            return val <= num;
        }
    }

    public class EqualCondition : IOPCondition
    {
        public bool GetConditionResult(int num, int val)
        {
            return num.Equals(val);
        }
    }
}

