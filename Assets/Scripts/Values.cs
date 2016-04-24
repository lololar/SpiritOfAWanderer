using UnityEngine;
using System.Collections;

public class Values {

    public static float EPSYLON = 1e-5f;
    public static float PI = 3.141956f;


    public static Vector3 Multiply(Vector3 l, Vector3 r)
    {
        return new Vector3(l.x * r.x, l.y * r.y, l.z * r.z);
    }

    public static Vector3 Absolute(Vector3 nabs)
    {
        return new Vector3(Mathf.Abs(nabs.x), Mathf.Abs(nabs.y), Mathf.Abs(nabs.z));
    }

}
