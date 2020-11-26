using Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//Loads the game on awake instead of on a loading screen.
//this may cause lag.
//used for running tests on the game without opening a loading screen
class DebugLoadOnAwake : MonoBehaviour
{
    //will cause lag at the start of the game. Loading using LoadAssets on the loading screen is prefered.
    private void Awake()
    {
        Item.LoadItems();
    }
}
