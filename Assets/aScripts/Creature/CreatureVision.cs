using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureVision : MonoBehaviour
{
    [SerializeField]
    private Creature owner;

    private void OnTriggerEnter(Collider other)
    {
        Creature creature = other.GetComponent<Creature>();
        if (creature != null && creature._creatureSO.Name != owner._creatureSO.Name && creature._Health > 0)
        {
            owner.TargetList.Add(creature.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Creature creature = other.GetComponent<Creature>();
        if (creature != null)
        {
            if (owner.TargetList.Remove(creature.gameObject))
            {
                //Debug.Log("creature has been deleted!");
            }
            else
            {
                //Debug.Log("That creature is no longer on the list!");
            }
        }
    }
}
