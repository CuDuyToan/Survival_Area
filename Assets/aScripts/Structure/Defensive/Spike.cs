using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Structure
{
    public override void TakeDame(float amount, GameObject source)
    {
        base.TakeDame(amount, source);

        Creature creature = source.GetComponent<Creature>();

        if(creature != null)
        {
            creature.TakeDame(amount * 0.1f, this.gameObject);
        }
    }
}
