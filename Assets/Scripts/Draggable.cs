using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class Draggable : MonoBehaviour, IClickable
{
    private Vector2 snapback;
    private bool isActive = false;
    private static PointerEventData pointer;

    private void Awake()
    {
        pointer = new PointerEventData(EventSystem.current);
    }

    private void Start()
    {
        snapback = this.transform.position;
        
    }

    public void Update()
    {
        if (!isActive) return;

        //if active drag the transform with the mouse
        var screenpoint = Input.mousePosition; 
        // aperently mouse position is screen point
        this.transform.position = screenpoint; 
        //this should work

        if (Input.GetMouseButtonUp(0))
        //release
        {
            isActive = false;

            //use drop position as raycast position
            pointer.position = transform.position;

            //snap back
            this.transform.position = snapback;
            List<RaycastResult> res = new List<RaycastResult>();

            CameraReference.Instance.canvas.GetComponent<GraphicRaycaster>().Raycast(pointer, res);

            GameObject gameObject = null;
            Transform other = null;
            bool success = false;

            foreach (RaycastResult result in res)
            {
                if ((gameObject = result.gameObject).GetComponent<IClickable>() != null)
                {
                    success = true;
                    break;
                }
            }

            if (success)
                other = gameObject.transform;

            //call the UnityEvent
            onRelease?.Invoke(this.transform, other);
        }
    }

    public void OnClick()
    {
        isActive = true;
    }

    [Tooltip("Accepts two arguments the first being its own transform and the second being the result from a raycast detecting any items beneth it")]
    public OnRelease onRelease;
}

[Serializable]
class OnRelease : UnityEvent<Transform, Transform> { }