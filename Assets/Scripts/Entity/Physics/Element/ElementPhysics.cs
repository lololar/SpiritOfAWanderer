using UnityEngine;
using System.Collections;

public class ElementPhysics : Moving {

    public override void Start()
    {
        base.Start();
    }

    protected override void MoveAction()
    {
        base.MoveAction();
        if (rigid.mass > 1.0f && _isFalling)
        {
            rigid.mass = 1.0f;
        }
        else if (rigid.mass <= 1.0f && !_isFalling)
        {
            rigid.mass = 100.0f;
        }
    }
}
