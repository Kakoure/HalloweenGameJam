using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DistanceTrigger : MonoBehaviour
{
    public float dist;
    public Transform target;
    public UnityEvent onTrigger;

    public bool allChildren;
    [Tooltip("If allChildren is on then this is irrelevent")]
    public GameObject[] objects;

    // Update is called once per frame
    void Update()
    {
        if ((Player.Instance.transform.position - target.position).magnitude <= dist)
        {
            if (!allChildren)
            {
                foreach (var g in objects)
                {
                    g.SetActive(true);
                }
            }
            else
            {
                foreach(Transform t in transform)
                {
                    t.gameObject.SetActive(true);
                }
            }

            onTrigger.Invoke();

            Destroy(this);
        }
    }
}
