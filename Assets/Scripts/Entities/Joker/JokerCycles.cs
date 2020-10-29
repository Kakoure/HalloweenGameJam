using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Boomerang;

public partial class Joker
{
    #region Paths

    public Path path1; //assigned in starts
    public Path path2; //start
    public Path Path3 => Negy(path1);
    public Path Path4 => Negy(path2);
    public Path Path5 => Flipxy(path1);
    public Path Path6 => Negy(Path5);
    public Path Path7 => Negx(Path5);
    public Path Path8 => Negx(Path6);

    private const float magnitudeScalar = 2;
    public Path wiggle = Add(LinePath(SinC(2), new Vector2(-1, 1) * magnitudeScalar), LinePath(f => 7 * f, new Vector2(1, 1)));
    public Path wiggle2 => Flipxy(wiggle);
    public Path wiggle3 => Negy(wiggle5);
    public Path wiggle4 => Negy(wiggle6);
    public Path wiggle5 => Negx(wiggle);
    public Path wiggle6 => Negx(wiggle2);
    public Path wiggle7 => Negy(wiggle);
    public Path wiggle8 => Negy(wiggle2);

    #endregion
}

