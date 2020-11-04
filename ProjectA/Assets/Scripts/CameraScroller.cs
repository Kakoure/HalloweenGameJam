using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroller : MonoBehaviour
{
    private Camera c;
    [SerializeField]
    private float playerWeight = 1;
    [SerializeField]
    private float mouseWeight = 1;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        c = CameraReference.Instance.camera;
    }

    // Update is called once per frame
    void Update()
    {
        //weighted average
        Vector2 player = Player.Instance.transform.position;
        Vector2 mouse = c.ScreenToWorldPoint(Input.mousePosition);
        Vector2 avg = player * playerWeight + mouse * mouseWeight;
        avg /= playerWeight + mouseWeight;
        c.transform.position = (Vector3)avg + 10 * Vector3.back;
    }
}
