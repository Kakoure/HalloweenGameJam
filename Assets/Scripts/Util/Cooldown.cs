using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CooldownTimer
{
    [Serializable]
    public struct Cooldown
    {
        public float UseTime { get; private set; }

        [SerializeField]
        private float cooldownTime;
        public float CooldownTime => cooldownTime;

        public float StartTime => UseTime - cooldownTime;

        public bool IsReady { get => Time.time > UseTime; }
        public void Use()
        {
            UseTime = Time.time + CooldownTime;
        }
        public void Use(float cooldownTime)
        {
            this.cooldownTime = cooldownTime;
            Use();
        }

        public Cooldown(float cooldownTime)
        {
            this.cooldownTime = cooldownTime;
            this.UseTime = 0;
        }

    }
}
