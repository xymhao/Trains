using System;
using System.Collections.Generic;
using System.Text;

namespace Trains
{
    //表头结点（表示图的顶点）
    public class Station
    {
        public bool Visited { get; set; }

        public Route FirstRoute { get; set; }

        public string Name { get; }

        public Station(string value)
        {
            Name = value;
        }

        //重写equals方法
        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Station vertex = obj as Station;
                return (Name == vertex.Name);
            }
        }

        public bool ContainsNode(string nodeData)
        {
            var curruntNode = FirstRoute;
            do
            {
                if (curruntNode.GetDestinationStation().Equals(nodeData))
                {
                    return true;
                }
                curruntNode = curruntNode.NextRoute;
            } while (curruntNode != null);
            return false;
        }

        public override int GetHashCode() => Name.GetHashCode();
    }
}
