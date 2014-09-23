using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaoWindowsForms
{ 
    public class FaceInformation
    {
        public float Alpha { get; set; }
        public float Beta { get; set; }
        public float SizeX { get; set; }
        public float SizeY { get; set; }
        public FaceInformation(float alpha, float beta, float sizeX, float sizeY)
        {
            Alpha = alpha;
            Beta = beta;
            SizeX = sizeX;
            SizeY = sizeY;
        }
    }
}
