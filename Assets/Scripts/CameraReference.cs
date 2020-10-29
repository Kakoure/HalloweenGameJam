﻿using System;
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

    public void InstantiateHitMarker(int damage, Vector2 pos)
    {
        if (damage < 0) return; //just apply a hack to cover up a bug

        Vector2 rand = UnityEngine.Random.insideUnitCircle;
        rand.y /= 2;
        rand.y += 1;
        GameObject.Instantiate(hitMarker, pos, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = damageConstant * Mathf.Log(1 + damage) * rand;
    }

    private void Awake()
    {
        if (Instance != null) Debug.LogError("Multiple CameraReferences detected");
        Instance = this;
    }
}