using Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class DebugLoadOnAwake : MonoBehaviour
{
    //will cause lag at the start of the game. Loading using LoadAssets on the loading screen is prefered.
    private void Awake()
    {
        Item.LoadItems();
    }
}
