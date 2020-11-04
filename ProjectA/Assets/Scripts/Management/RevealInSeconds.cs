using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealInSeconds : MonoBehaviour
{
    public float seconds;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        seconds += Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > seconds)
        {
            target.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
