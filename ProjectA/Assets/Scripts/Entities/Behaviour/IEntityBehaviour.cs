using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class EntityBehaviour : MonoBehaviour
{
    public abstract Func<Vector2> GetBehaviour(Transform target);
}

