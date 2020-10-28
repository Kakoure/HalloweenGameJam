using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CooldownTimer
{
    [Serializable]
    public struct Cooldown
    {
        private float useTime;
        public float UseTime => useTime;

        [SerializeField]
        private float cooldownTime;
        public float CooldownTime => cooldownTime;

        public bool IsReady { get => Time.time > UseTime; }
        public void Use()
        {
            useTime = Time.time + CooldownTime;
        }
        public void Use(float cooldownTime)
        {
            this.cooldownTime = cooldownTime;
            Use();
        }
    }
}
