using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.Space
{
    [BindProto]
    public class Star
    {
        [BindProto(true)]
        public float Size;
        [BindProto(true)]
        public float Brightness;
        [BindProto(true)]
        public Color Color;
    }
}
