using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Engine.Core
{
    public struct Line
    {
        public Vector3 v1;
        public Vector3 v2;

        public Line(Vector3 v1, Vector3 v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }
    }


    public class LineString
    {
        public List<Vector3> Vertices;

        public LineString(List<Vector3> vertices)
        {
            Vertices = vertices;
        }
    }
}
