using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerCursor : MonoBehaviour
{
    public Texture2D cursor;
    public Texture2D cursorTwo;

    // Start is called before the first frame update
    void Start()
    {
        if (cursor != null)
            Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.ForceSoftware);
        if (cursorTwo == null) cursorTwo = cursor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) Cursor.SetCursor(cursorTwo, new Vector2(cursorTwo.width / 2, cursorTwo.height / 2), CursorMode.ForceSoftware);
        if (Input.GetButtonUp("Fire1")) Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.ForceSoftware);
    }
}
