using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CameraReference : MonoBehaviour
{
    public static CameraReference Instance { get; private set; }

    new public Camera camera;
    public Canvas canvas;

    private void Start()
    {
        if (Instance != null) Debug.LogError("Multiple CameraReferences detected");
        Instance = this;
    }
}