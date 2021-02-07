using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CameraReference : MonoBehaviour
{
    public static CameraReference Instance { get; private set; }

    public static Vector2 MouseVec(Vector2 src)
    {
        return (Vector2)Instance.camera.ScreenToWorldPoint(Input.mousePosition) - src;
    }

    new public Camera camera;
    public Canvas canvas;
    public GameObject hitMarker;
    public GameObject bulletGeneric;
    public float damageConstant = 2;
    public GameObject itemGeneric;

    //utility function
    public void InstantiateHitMarker(int damage, Vector2 pos)
    {
        if (damage <= 0) return; //just apply a hack to cover up a bug

        Vector2 rand = UnityEngine.Random.insideUnitCircle;
        rand.y /= 2;
        rand.y += 1;
        GameObject g;
        g = GameObject.Instantiate(hitMarker, pos, Quaternion.identity);
        g.GetComponent<Rigidbody2D>().velocity = damageConstant * Mathf.Log(1 + damage) * rand;

        g.GetComponent<GetText>().text.text = "-" + damage;
    }

    private void Awake()
    {
        if (Instance != null) UnityEngine.Debug.LogError("Multiple CameraReferences detected");
        Instance = this;
    }
}