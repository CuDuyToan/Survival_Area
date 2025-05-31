using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void Enter(PlayerController player);
    void Update(PlayerController player);
    void Exit(PlayerController player);
}

namespace PlayerState
{
    public class Die : IPlayerState
    {
        public void Enter(PlayerController player)
        {
            Debug.Log("die event");
        }

        public void Update(PlayerController player)
        {
            if (player._Health > 0)
            {
                player.SwitchState(new Idle());
            }
        }

        public void Exit(PlayerController player)
        {
            player.ResetTrigger();
        }
    }

    public class Idle : IPlayerState
    {
        public void Enter(PlayerController player)
        {
            player.StopMovement(true);
            player.Idle();
        }

        public void Update(PlayerController player)
        {
            player.Idle();
            player._Animator.SetBool("Run", false);
            player.RecoveryStamina(100 * Time.deltaTime);
        }

        public void Exit(PlayerController player)
        {
            player.StopMovement(false);
            player.ResetTrigger();
        }
    }

    public class Move : IPlayerState
    {
        private Vector3 destination = Vector3.zero;

        public void Enter(PlayerController player)
        {
            player.RaycastToDestination();
            destination = player._MoveOffset;
            player.MoveToPoint(destination);
        }

        public void Update(PlayerController player)
        {

            if (player._Stamina <= 0 && player.WeightRate >= 85) player.Walk();

            if (player.IsPathComplete()) player.SwitchState(new Idle());

            SwitchSprint(player);
        }

        private void SwitchSprint(PlayerController player)
        {
            if (player._creatureSO is PlayerSO playerData)
            {
                if (player._MovementSpeed >= playerData.SprintSpeed) player.SwitchState(new Sprint());
            }
        }

        public void Exit(PlayerController player)
        {
        }
    }

    public class FindTarget : IPlayerState
    {
        private GameObject target = null;

        public void Enter(PlayerController player)
        {
            player.Walk();
        }

        public void Update(PlayerController player)
        {

            target = player.Target;

            if (player._Stamina <= 0 && player.WeightRate >= 85) player.Walk();

            if (target != null) player.MoveToPoint(target.transform.position);

            if (target != null && player.CanInteract) player.SwitchState(new InteractTarget());
            else if (target == null) player.SwitchState(new Idle());
        }

        public void Exit(PlayerController player)
        {
        }

    }

    public class InteractTarget : IPlayerState
    {
        Resource _resource = null;
        Creature _creature = null;
        Structure _structure = null;

        public void Enter(PlayerController player)
        {
            player.Idle();

            _resource = player.Target.GetComponent<Resource>();
            _creature = player.Target.GetComponent<Creature>();
            _structure = player.Target.GetComponent<Structure>();

            /*if(player._Stamina>0) */Interaction(player);

            if (player.IsPathComplete() && player.Target == null) player.SwitchState(new Idle());
            else if (player.Target != null && !player.CanInteract) player.SwitchState(new FindTarget());

            //if (Vector3.Distance(player.transform.position, player.Target.transform.position) > 2) player.CanInteract = false;
        }

        private void Interaction(PlayerController player)
        {
            if (player._Stamina > 0)
            {
                if (_resource != null)
                {
                    ResourceInteraction(player);
                }
                else if (_creature != null)
                {
                    CreatureInteraction(player);
                }
            }
            
            if (_structure != null)
            {
                WorkStationInteraction(player);
            }
        }

        private void WorkStationInteraction(PlayerController player)
        {
            _structure.PlayerInteraction();
            player._Animator.SetBool("Run", false);
        }

        private void CreatureInteraction(PlayerController player)
        {
            player.Attack();
        }

        private void ResourceInteraction(PlayerController player)
        {
            player._Animator.SetBool("Run", false);
            if (_resource.ExploitType == EExploitType.Gather)
            {
                player.Gathering();
            }
            else if (_resource.ExploitType == EExploitType.Mine)
            {
                player.Mining();
            }
        }

        public void Update(PlayerController player)
        {
            if (player.Target != null && !player.CanInteract) player.SwitchState(new FindTarget());
            else if (player.Target == null) player.SwitchState(new Idle());

            if(player._Stamina <= 0 && _structure == null)
            {
                Debug.Log("met roi nghi chut da");
                player.Target = null;
                player.SwitchState(new Idle());

            }

            player.RotateToTarget();
        }

        public void Exit(PlayerController player)
        {
            //player.StopMovement(false);
            AudioSource audioSource = player.GetComponent<AudioSource>();

            if (player._Stamina <= 20)
            {
                audioSource.Play();
            }

        }
    }

    public class Sprint : IPlayerState
    {
        private Vector3 destination = Vector3.zero;

        private bool isSprint = true;

        public void Enter(PlayerController player)
        {
            player.Sprint();
        }

        public void Update(PlayerController player)
        {

            player.RaycastToDestination();

            destination = player._MoveOffset;
            player.MoveToPoint(destination);

            if (player.IsPathComplete()) player.SwitchState(new Idle());

            SwitchMovementMode(player);

            //SwitchMove(player);
        }

        private void SwitchMovementMode(PlayerController player)
        {
            bool cantSprint = player._Stamina <= 0 || player.WeightRate >= 85;

            if(isSprint == true && cantSprint == true)
            {
                player.Walk();
                isSprint = false;
            }
            else if(isSprint == false && cantSprint == false)
            {
                player.Sprint();
                isSprint = true;
            }
        }

        private void SwitchMove(PlayerController player)
        {
            if(player._creatureSO is PlayerSO playerData)
            {
                if (player._MovementSpeed < playerData.SprintSpeed) player.SwitchState(new Move());
            }
        }

        public void Exit(PlayerController player)
        {
            AudioSource audioSource = player.GetComponent<AudioSource>();

            if (player._Stamina <= 20)
            {
                audioSource.Play();
            }
        }
    }

}
