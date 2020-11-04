using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

class ClickRaycaster : MonoBehaviour
{
    [SerializeField]
    private new Camera camera;

    private void Update()
    {
        //on mouse1
        if (Input.GetButtonDown("Interact"))
        {
            //get the transform of the hit
            Vector3 pt = camera.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(pt, Vector3.forward);
            Transform transform = hit.transform;
            
            //if the transform exists and if a clickable is attached then call its method
            transform?.GetComponent<IClickable>()?.OnClick();
        }
    }
}

