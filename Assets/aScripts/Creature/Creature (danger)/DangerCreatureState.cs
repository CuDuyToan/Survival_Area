using FriendlyCreatureState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace DangerCreatureState
{
    public class IdleState : IDangerCreatureState
    {
        private float idleTime = 5f;

        public void Enter(DangerCreature creature)
        {
            if (creature.ReachedDestination()) creature.Idle();
        }

        public void Update(DangerCreature creature)
        {
            if(creature.Target != null)
            {
                if (creature.DetectCreature() && creature._groupType == EGroupingType.PackBased) creature.SwitchState(new ReadyForBattle());
                else if (creature.DetectCreature() && creature._groupType != EGroupingType.SoloBased) creature.SwitchState(new ChaseState());

                if (creature.IsPerception)
                {
                    creature.SwitchState(new ChaseState());
                }
            }

            if (idleTime > 0) idleTime -= Time.deltaTime;
            else if (idleTime <= 0) creature.SwitchState(new Wadering());
        }

        public void Exit(DangerCreature creature)
        {

        }

    }

    public class Wadering : IDangerCreatureState
    {

        public void Enter(DangerCreature creature)
        {
            creature.Walk();
            creature.RandomDestination();
        }

        public void Update(DangerCreature creature)
        {
            if (creature.DetectCreature() && creature._groupType == EGroupingType.PackBased) creature.SwitchState(new ReadyForBattle());
            else if (creature.DetectCreature() && creature._groupType != EGroupingType.SoloBased) creature.SwitchState(new ChaseState());

            if (creature.IsPerception)
            {
                creature.SwitchState(new ChaseState());
            }

            if (creature.ReachedDestination()) creature.SwitchState(new IdleState());
        }

        public void Exit(DangerCreature creature)
        {
        }
    }

    public class SearchState : IDangerCreatureState
    {
        float timeSearch = 60f;
        float timeCount = 1f;
        Vector3 lostLocation = Vector3.zero;
        public void Enter(DangerCreature creature)
        {
            lostLocation = creature.Target.transform.position;
        }

        public void Update(DangerCreature creature)
        {
            timeSearch -= Time.deltaTime;
            if (creature.DetectCreature() && creature._groupType == EGroupingType.PackBased) creature.SwitchState(new ReadyForBattle());
            else if (creature.DetectCreature() && creature._groupType != EGroupingType.SoloBased) creature.SwitchState(new ChaseState());

            if (creature.IsPerception)
            {
                creature.SwitchState(new ChaseState());
            }

            else if (creature.ReachedDestination()) //khi không di chuyển nhưng vẫn trong trạng thái tìm kiếm
            {
                timeCount -= Time.deltaTime;
                if (timeCount <= 0) //khi dừng đủ lâu
                {
                    timeCount = 1f;
                    if (Vector3.Distance(creature.SearchInArea(lostLocation), creature.transform.position) <= creature._creatureSO.WalkSpeed) // nếu vị trí tìm kiếm mới đủ gần thì chỉ (walk)
                    {
                        creature.Walk();
                    }
                    else if (Vector3.Distance(creature.SearchInArea(lostLocation), creature.transform.position) > creature._creatureSO.WalkSpeed) // nếu vị trí tìm kiếm mới quá xa thì (sprint)
                    {
                        creature.Sprint();
                    }
                }
                else if (timeCount == 1)
                {
                    creature.Idle();
                }

            }
            if (timeSearch <= 0) creature.SwitchState(new IdleState());
        }

        public void Exit(DangerCreature creature)
        {
        }
    }

    public class ReadyForBattle : IDangerCreatureState
    {
        public void Enter(DangerCreature creature)
        {
            if (!creature.IsPerception && creature._groupType == EGroupingType.PackBased) creature.Howl();
            creature.SwitchState(new ChaseState());
        }

        public void Update(DangerCreature creature) 
        {

        }

        public void Exit(DangerCreature creature)
        {
        }
    }

    public class ChaseState : IDangerCreatureState
    {
        Vector3 lastPos = Vector3.zero;

        public void Enter(DangerCreature creature)
        {
            creature.Sprint();

            if(creature.Target != null)
            {
                lastPos = creature.Target.transform.position;
            }
        }

        public void Update(DangerCreature creature)
        {
            TargetIsDie(creature);

            if(creature.Target != null)
            {
                if (creature.Agitated()) lastPos = creature.Target.transform.position;
                creature.MoveToPoint(lastPos);
                // nếu đủ tầm tấn công thì tấn công
                if (creature.IsPerception || creature.CanInteract) creature.SwitchState(new AttackState());
                // nếu đã đến điểm dừng nhưng không thấy mục tiêu thì tìm kiếm 
                else if (creature.ReachedDestination() && !creature.Agitated()) creature.SwitchState(new SearchState());
            }
            else
            {
                creature.SwitchState(new IdleState());
            }
        }

        private void TargetIsDie(DangerCreature creature)
        {
            if (creature.Target == null) return;
            Creature target = creature.Target.GetComponent<Creature>();

            if (target.IsDead) creature.Target = null;
        }

        public void Exit(DangerCreature creature)
        {
        }
    }
    public class AttackState : IDangerCreatureState
    {
        public void Enter(DangerCreature creature)
        {
            creature.Agent.isStopped = true;
        }

        public void Update(DangerCreature creature)
        {
            TargetIsDie(creature);
            creature.RotateToTarget();
            if (creature.Target != null)
            {
                if (!creature.CanInteract) // không đủ tầm 
                {
                    creature.SwitchState(new ChaseState());
                }
                else if (creature.CanInteract) //đủ tầm
                {
                    creature.Attack();
                }
                else if (creature.ReachedDestination() && !creature.Agitated()) // tới điểm dừng và không thấy mục tiêu
                {
                    creature.SwitchState(new SearchState());
                }
            }
            else
            {
                creature.SwitchState(new IdleState());
            }
        }

        private void TargetIsDie(DangerCreature creature)
        {
            if (creature.Target == null) return;
            Creature target = creature.Target.GetComponent<Creature>();

            if (target.IsDead) creature.Target = null;
        }

        public void Exit(DangerCreature creature)
        {
            //creature.Attack(false);
            creature.Agent.isStopped = false;
        }
    }
}

