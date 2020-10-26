using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//!! could try and make Timer serializable.
public class Timer
{
    public float passiveWait;
    private List<TimedAction> actions = new List<TimedAction>(4);
    public void AddAction(TimedAction action)
    {
        this.actions.Add(action);
    }

    [System.Serializable]
    public class TimedAction
    {
        public float cooldown;
        public float waitTime;

        [System.NonSerialized]
        public Func<bool> test;
        [System.NonSerialized]
        public Action action;
    }

    //warning: big boi spagaettii kode up ahead (or should i say down)
    public IEnumerator TimerCoroutine()
    {
        while(true)
        {
            TimedAction currentAction = null;
            float zeroTime = Time.time;

            while (true)
            {
                foreach(TimedAction action in actions)
                {
                    if (Time.time > zeroTime + action.waitTime && action.test())
                    {
                        currentAction = action;
                        break;
                    }
                }
                if (currentAction != null) break;
                if (passiveWait == 0) yield return new WaitForEndOfFrame();
                else yield return new WaitForSeconds(passiveWait);
            }
            currentAction.action();
            yield return new WaitForSeconds(currentAction.cooldown);
        }
    }

    public Timer()
    {

    }
}
