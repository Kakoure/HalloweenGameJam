using System;
using UnityEngine;
using UnityEngine.Experimental.ParticleSystemJobs;
using static UnityEngine.Mathf;

public class Boomerang : MonoBehaviour
{
    public static Converter SinC(float halfcycles) => f => Mathf.Sin(f * PI * halfcycles);
    public static Converter Pow(float p) => f => Mathf.Pow(f, p);
    public static Converter Poly(float p) => f => -Pow(p)(2 * f - 1) + 1;
    public static Converter Dot(Path p1, Path p2) => f => Vector2.Dot(p1(f), p2(f));

    //turns converter into a polar function from theta = [0, pi] (180 degrees)
    public static Path Polar180(Converter r) => f =>
    {
        float rad = r(f);
        float thet = f * PI;
        return new Vector2(rad * Mathf.Sin(thet), rad * Cos(thet)); //im so dumb I cant change it though
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
    public static Path Add(Path p1, Path p2) => f => p1(f) + p2(f);

    //paths are vector valued functions usually with domain [0, 1]
    public delegate Vector2 Path(float time);
    //Converters are just functions from float to float
    public delegate float Converter(float time);

    public float startTime;
    [NonSerialized]
    public float dur = 1;
    public float End => startTime + dur;

    public Path path = Polar180(Poly(6));
    public Vector2 GetCurrent(float time)
    {
        return path(Clamp01((time - startTime)/dur));
    }

    public Boomerang()
    {

    }
}
