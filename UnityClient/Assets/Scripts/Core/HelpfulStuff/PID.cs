using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Core.HelpfulStuff
{
    public class PID
    {
        public float Kp = 1;
        public float Ki = 0;
        public float Kd = 0.1f;

        public float Power = 1;

        private float P, I, D;
        private float prevError;
        public float GetOutput(float currentError, float deltaTime)
        {
            P = currentError;
            I += P * deltaTime;
            D = (P - prevError) / deltaTime;
            prevError = currentError;

            return P * (Kp * Power) + I * (Ki * Power) + D * (Kd * Power);
        }
    }
}

