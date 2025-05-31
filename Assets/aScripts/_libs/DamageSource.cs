using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [Min(1)] public float dame = 1;

    private void OnTriggerStay(Collider other)
    {
        Creature creature = other.GetComponent<Creature>();

        if(creature != null)
        {
            if (creature is PlayerController) return;

            creature.TakeDame(1, this.gameObject);
        }
    }
}
