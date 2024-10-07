using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 센서값따로받기
{
    internal class sensorData
    {
        public DateTime dt;
        public int value;
        private string v1;
        private string v2;
        private int v3;

        //생성자
        public sensorData(DateTime dt, int value)
        {
            this.dt = dt;
            this.value = value;
        }

        public sensorData(string v1, string v2, int v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }
    }
}
