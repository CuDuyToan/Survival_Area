using System.Collections;
using UnityEngine;

public interface IDangerCreatureState
{
    void Enter(DangerCreature creature);
    void Update(DangerCreature creature);
    void Exit(DangerCreature creature);
}

public interface IFriendlyCreatureState
{
    void Enter(FriendlyCreature creature);
    void Update(FriendlyCreature creature);
    void Exit(FriendlyCreature creature);
}

public interface INeutralCreatureState
{
    void Enter(NeutralCreature creature);
    void Update(NeutralCreature creature);
    void Exit(NeutralCreature creature);
}

public interface ITimidCreatureState
{
    void Enter(TimidCreature creature);
    void Update(TimidCreature creature);
    void Exit(TimidCreature creature);
}