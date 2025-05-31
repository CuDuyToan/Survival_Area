using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAttackRange : MonoBehaviour
{
    [SerializeField]
    private Creature owner;

    private void OnTriggerEnter(Collider other)
    {
        //Creature creature = other.GetComponent<Creature>();
        //if (creature != null && owner.Target == creature.gameObject)
        //{
        //    owner.CanInteract = true;
        //}

        if (owner.Target == other.gameObject)
        {
            owner.CanInteract = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(owner.Target == other.gameObject && owner.CanInteract == false)
        {
            owner.CanInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Creature creature = other.GetComponent<Creature>();

        //if (creature != null && owner.Target == creature.gameObject)
        //{
        //    owner.CanInteract = false;
        //}

        if (owner.Target == other.gameObject)
        {
            owner.CanInteract = false;
        }
    }
}
