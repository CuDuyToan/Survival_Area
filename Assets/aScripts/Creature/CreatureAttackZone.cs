using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAttackZone : MonoBehaviour
{
    [SerializeField]
    private Creature owner;

    private void OnTriggerEnter(Collider other)
    {
        Creature creature = other.GetComponent<Creature>();
        if (creature != null && creature._creatureSO.Name != owner._creatureSO.Name)
        {
            creature.TakeDame(owner._creatureSO.Damage, owner.gameObject);
            return;
        }

        Structure structure = other.GetComponent<Structure>();
        if (structure != null)
        {
            Debug.Log(structure.name);
            structure.TakeDame(owner._creatureSO.Damage, owner.gameObject);
            return;
        }
    }
}
