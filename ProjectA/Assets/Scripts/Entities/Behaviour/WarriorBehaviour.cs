using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WarriorBehaviour : EntityBehaviour
{
    public override Func<Vector2> GetBehaviour(Transform target)
    {
        //just chase player
        return () => target.position;
    }
}
