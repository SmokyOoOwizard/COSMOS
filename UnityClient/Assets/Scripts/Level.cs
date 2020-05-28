using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS
{
    public class LevelObject
    {
        public LevelObject Parent;
        public LevelObject[] Children;

    }
    public class Level
    {
        public LevelObject[] LevelObjects;
    }
}
