using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RussianCube
{
    public class Cube
    {
        public Cube() { }

        public Cube(Cube _cube)
        {
            this.Component = _cube.Component;
            this.CubeType = _cube.CubeType;
            this.Orientation = _cube.Orientation;
            this.TurnTimes = _cube.TurnTimes;
            this.X = _cube.X;
            this.Y = _cube.Y;
        }

        public int CubeType { get; set; }
        public int Orientation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int[,] Component { get; set; }
        public int TurnTimes { get; set; }
        public int Color { get; set; }
    }
}
