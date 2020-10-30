using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ArcherBehaviour : EntityBehaviour
{
    public float seekDist = 5;

    public override Func<Vector2> GetBehaviour(Transform target)
    {
        return () => ((Vector2)transform.position - (Vector2)target.position).normalized * seekDist + (Vector2)target.position;
    }
}