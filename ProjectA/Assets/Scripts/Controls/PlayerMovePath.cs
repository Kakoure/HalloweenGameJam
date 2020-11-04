using CooldownTimer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Boomerang;

public partial class PlayerMove
{
    static Converter rollConv = f => f < 3.0f / 5 ? 4.0f / 3 : 1.0f / 2;
    static Converter c = f => f < 0.6f ? 4 * f / 3 : f / 2 + 0.5f;
    public static Path RollPath(Vector2 dir) => LinePath(rollConv, dir);

    //float pathStart = 0;
    //float pathDuration;
    Cooldown pathDuration;

    public bool PathEnd => pathDuration.IsReady;
    public Path path = f => Vector2.zero;

    /// <summary>
    /// Sets the path for a duration
    /// </summary>
    /// <param name="path">velocity path [0, 1]</param>
    /// <param name="duration">duration of path, higher is slower</param>
    public void SetPath(Path path, float duration)
    {
        this.pathDuration.Use(duration);
        this.path = path;
    }
    Vector2 GetPathVel()
    {
        float t = Time.time - pathDuration.StartTime;
        t /= pathDuration.CooldownTime;
        return path(t);
    }
}
