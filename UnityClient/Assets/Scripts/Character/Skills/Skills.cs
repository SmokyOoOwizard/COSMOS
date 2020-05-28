using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.Skills
{
    public class Skills
    {
        public class SkillNode
        {
            public SkillNode[] Parents;
            public SkillNode[] Childs;

            public Skill Skill { get; protected set; }

            public Vector2Int Pos;
            public Vector2Int Size;

            public bool Active { get; protected set; } = false;

            public bool CanActive()
            {
                if(Parents != null)
                {
                    for (int i = 0; i < Parents.Length; i++)
                    {
                        if (!Parents[i].Active)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            public void Activate()
            {
                Active = true;
            }
        }

        List<SkillNode> StarNodes;
        List<SkillNode> AllNodes;

        

    }

}
