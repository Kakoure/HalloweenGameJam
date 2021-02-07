using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//TODO: merge with PlayerMove
/// <summary>
/// A click checker for UI elements.
/// </summary>
public class CheckClicks : MonoBehaviour
{
    // Normal raycasts do not work on UI elements, they require a special kind
    //TODO: set the raycast layer of the raycaster
    GraphicRaycaster raycaster;

    void Awake()
    {
        // Get both of the components we need to do this
        this.raycaster = GetComponent<GraphicRaycaster>();
    }

    void Update()
    {
        //Check if the left Mouse button is clicked
        if (Input.GetButtonDown("Interact"))
        {
            //Set up the new Pointer Event
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            pointerData.position = Input.mousePosition;

            //raycaster should raycast on a specific layer
            this.raycaster.Raycast(pointerData, results);

            foreach(RaycastResult r in results)
            {
                //just keep null propagating it'll be fine
                var g = r.gameObject;
                if (g != null)
                    g.GetComponent<IClickable>()?.OnClick();
            }
        }
    }
}