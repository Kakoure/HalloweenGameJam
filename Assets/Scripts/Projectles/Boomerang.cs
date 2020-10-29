using System;
using UnityEngine;
using static UnityEngine.Mathf;

public class Boomerang : MonoBehaviour
{
    public static Converter Sin = f => Mathf.Sin(f * 2 / PI);
    public static Converter Pow(float p) => f => Mathf.Pow(f, p);
    public static Converter Poly(float p) => f => -Pow(p)(2 * f - 1) + 1;
    public static Converter Dot(Path p1, Path p2) => f => Vector2.Dot(p1(f), p2(f));

    public static Path Polar180(Converter r) => f =>
    {
        float rad = r(f);
        float thet = f * PI;
        return new Vector2(rad * Sin(thet), rad * Cos(thet));
    };

    public static Path Negx(Path p) => f =>
    {
        Vector2 d = p(f);
        d.x *= -1;
        return d;
    };
    public static Path Negy(Path p) => f =>
    {
        Vector2 d = p(f);
        d.y *= -1;
        return d;
    };
    public static Path Flipxy(Path p) => f =>
    {
        Vector2 v = p(f);
        v.Set(v.y, v.x);
        return v;
    };
    public static Path Mult(float m, Path p) => f => m * p(f);
    public static Path LinePath(Converter c, Vector2 p) => f => p * c(f);

    public delegate Vector2 Path(float time);
    public delegate float Converter(float time);

    public float startTime;
    [NonSerialized]
    public float dur = 1;
    public float End => startTime + dur;

    public Path path = Polar180(Poly(6));
    public Vector2 getCurrent(float time)
    {
        return path(Clamp01((time - startTime)/dur));
    }

    public Boomerang()
    {

    }
}
