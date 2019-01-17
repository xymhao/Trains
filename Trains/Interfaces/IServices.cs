using System;
using System.Collections.Generic;
using System.Text;

namespace Trains.Interfaces
{
    interface IServices
    {
        int GetDistanceOfRoutes(string routes);
        void GetNumberWithCondition(string start, Func<int, Stack<string>, bool> func = null, int weight = 0);

        void GetPath(string start, Action<string, string, int> func = null);

        Station Find(string itemStr);
    }
}
