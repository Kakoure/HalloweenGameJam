using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

//dummy struct alternative to void*
public struct state_ptr { }

public struct piece_code 
{
    
}

public class test : MonoBehaviour
{
    const string dll_loc = "generic_chess.dll.recipe";

    [DllImport(dll_loc, CallingConvention = CallingConvention.Cdecl)]
    state_ptr*

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
