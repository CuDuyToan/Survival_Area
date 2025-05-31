using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface ICreature
{
    void Idle();
    void IncreaseHunger();
    void Recovery();
    void RestoreHealth(float amount);
    void Eat(FoodSO food);
    void DecreaseStamina(float staminaDecrease);
    void RecoveryStamina(float recoverySpeed);
    void TakeDame(float amount, GameObject source);
    void Die();
}

public interface IPlayer
{
    void Walk();

    void Run();

    void Sprint();

    void Attack();

    void DealDamage();

    void Mining();

    void Gathering();

    void Exploit();
}

public interface IDangerCreature
{
    void RandomDestination();

    //void Attack(bool state);

    //void Walk(bool state);

    //void Sprint(bool state);
}

public interface IFriendlyCreature
{
    void Escape();

    void RandomDestination();

}