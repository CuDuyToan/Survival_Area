using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSensing : MonoBehaviour
{
    [SerializeField]
    private Creature owner;

    private void OnTriggerEnter(Collider other)
    {
        Creature creature = other.GetComponent<Creature>();
        if (creature != null && creature._creatureSO.Name != owner._creatureSO.Name)
        {
            if(owner.Target == null)
            {
                owner.IsPerception = true;
                owner.Target = creature.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Creature creature = other.GetComponent<Creature>();
        if (creature != null && creature._creatureSO.Name != owner._creatureSO.Name && creature.gameObject == owner.Target)
        {
            owner.IsPerception = false;
        }
    }
}
