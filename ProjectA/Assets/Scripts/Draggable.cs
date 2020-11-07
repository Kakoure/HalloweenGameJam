using Items;
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

        if (Input.GetButtonUp("Interact"))
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
            bool hitSlot = false;
            bool hitInventory = false;

            foreach (RaycastResult result in res)
            {
                if (result.gameObject.transform.CompareTag("ItemSlot"))
                {
                    gameObject = result.gameObject;
                    hitSlot = true;
                }
                if (result.gameObject.transform.CompareTag("Inventory"))
                {
                    hitInventory = true;
                }
            }

            if (hitSlot)
            {
                //place the item in inventory
                Inventory.Swap(this.transform, gameObject.transform);
            }
            else if (!hitInventory)
            {
                //try to drop this item.
                Inventory.InventorySlot slot = Inventory.Instance.FindSlot(this.transform);
                Inventory.Drop(slot, CameraReference.Instance.camera.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }

    public void OnClick()
    {
        isActive = true;
    }
}

[Serializable]
class OnRelease : UnityEvent<Transform, Transform> { }