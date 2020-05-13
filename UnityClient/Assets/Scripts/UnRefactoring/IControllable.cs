using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.Player
{
    public interface IControllable
    {
        Vector3 GetPos();
        void Move(Vector2 dir);
        void Rotate(float angle);
        /// <summary>
        /// Execute every fixedUpdate if selected
        /// </summary>
        void Selected();
        bool Exist();
    }
}
