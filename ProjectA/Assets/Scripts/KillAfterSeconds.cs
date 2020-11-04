using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAfterSeconds : MonoBehaviour
{
    public float seconds;
    private float finalTime;

    // Start is called before the first frame update
    void Start()
    {
        finalTime = Time.time + seconds;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > finalTime) GameObject.Destroy(gameObject);
    }
}
