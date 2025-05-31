using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreatureData
{
    public string name_SO;

    public float health;

    public float deadBody_Hp;
    public float deadBody_Timecount;

    public float food;

    public PositionData position;

    public RotationData rotation;

    public CreatureData(Creature creature, Transform structureTransform, Creature_SpawnDeadbody deadbody)
    {
        this.name_SO = creature._creatureSO.Name;
        this.health = creature._Health;
        this.food = creature._Food;

        this.position = new PositionData(structureTransform.position);
        this.rotation = new RotationData(structureTransform.eulerAngles);

        this.deadBody_Hp = deadbody.DeadBody._Health;
        this.deadBody_Timecount = deadbody.DeadBody._TimeCount;

        //this.name_SO = name_SO;
        //this.health = health;
        //this.food = food;

        //this.position = position;
        //this.rotation = rotation;

        //this.deadBody_Hp = deadBody_Hp;
        //this.deadBody_Timecount = deadBody_Timecount;
    }

}
