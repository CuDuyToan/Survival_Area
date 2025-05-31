using DangerCreatureState;
using UnityEngine;

namespace NeutralCreatureState
{
    public class Idle : INeutralCreatureState
    {
        float waitTime = 5;
        public void Enter(NeutralCreature creature)
        {
            creature.Idle();
            if (creature.Target != null && creature.isScared && creature.Target != null) waitTime *= 4;
        }

        public void Update(NeutralCreature creature)
        {
            if (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
            }
            else if (waitTime <= 0)
            {
                creature.Target = null;
                creature.SwitchState(new Wadering());
            }

            if (creature.Target != null && !creature.isScared)
            {
                creature.SwitchState(new Chase());
            }

            if (creature.isScared && creature.Target != null) creature.SwitchState(new Escape());
        }
        public void Exit(NeutralCreature creature)
        {

        }
    }

    public class Wadering : INeutralCreatureState
    {
        public void Enter(NeutralCreature creature)
        {
            creature.Walk();
            creature.RandomDestination();
        }

        public void Update(NeutralCreature creature)
        {
            if (creature.IsPathComplete()) creature.SwitchState(new Idle());
        }
        public void Exit(NeutralCreature creature)
        {

        }
    }

    public class Chase : INeutralCreatureState
    {
        Vector3 lastPos = Vector3.zero;
        private GameObject target;

        public void Enter(NeutralCreature creature)
        {
            if (creature.isScared) creature.SwitchState(new Escape());

            creature.Charge();
            target = creature.Target;
        }

        public void Update(NeutralCreature creature)
        {
            TargetIsDie(creature);
            //nếu vẫn nhìn thấy mục tiêu thì tiếp tục đuổi
            if (creature.Agitated())
            {
                if(target != null) lastPos = target.transform.position;
            }
            // nếu đủ tầm tấn công thì tấn công
            if (creature.CanInteract) creature.SwitchState(new Attack());
            // nếu đã đến điểm dừng nhưng không thấy mục tiêu thì tìm kiếm 
            else if (creature.IsPathComplete() && !creature.Agitated()) creature.SwitchState(new Search());

            creature.MoveToPoint(lastPos);
        }

        private void TargetIsDie(NeutralCreature creature)
        {
            if (creature.Target == null) return;
            Creature target = creature.Target.GetComponent<Creature>();

            if (target.IsDead) creature.Target = null;
        }

        public void Exit(NeutralCreature creature)
        {

        }
    }

    public class Search : INeutralCreatureState
    {
        float timeSearch = 60f;
        float timeCount = 1f;
        Vector3 lostLocation = Vector3.zero;
        public void Enter(NeutralCreature creature)
        {
            if (creature.isScared) creature.SwitchState(new Escape());

            if (creature.Target == null) creature.SwitchState(new Idle());
            else
            {
                lostLocation = creature.Target.transform.position;
            }
        }

        public void Update(NeutralCreature creature)
        {
            timeSearch -= Time.deltaTime;
            if (creature.Agitated() || creature.IsPerception) creature.SwitchState(new Chase());

            else if (creature.IsPathComplete()) //khi không di chuyển nhưng vẫn trong trạng thái tìm kiếm
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
                        creature.Run();
                    }
                }
                else if (timeCount == 1)
                {
                    creature.Idle();
                }

            }
            if (timeSearch <= 0) creature.SwitchState(new Idle());
        }
        public void Exit(NeutralCreature creature)
        {

        }
    }

    public class Attack : INeutralCreatureState
    {
        public void Enter(NeutralCreature creature)
        {
            creature.Attack();
            creature.Agent.isStopped = true;
            if (creature.isScared) creature.SwitchState(new Escape());
        }

        public void Update(NeutralCreature creature)
        {
            TargetIsDie(creature);

            if(creature.Target != null)
            {
                creature.RotateToTarget();
                if (!creature.CanInteract) // không đủ tầm 
                {
                    creature.SwitchState(new Chase());
                }
                //else if (creature.CanInteract) //đủ tầm
                //{
                //    creature.Attack();
                //}
                else if (creature.IsPathComplete() && !creature.Agitated()) // tới điểm dừng và không thấy mục tiêu
                {
                    creature.SwitchState(new Search());
                }

                if (creature.isScared) creature.SwitchState(new Escape());
            }
            else
            {
                creature.SwitchState(new Idle());
            }

            
        }

        private void TargetIsDie(NeutralCreature creature)
        {
            if (creature.Target == null) return;
            Creature target = creature.Target.GetComponent<Creature>();

            if (target.IsDead) creature.Target = null;
        }

        public void Exit(NeutralCreature creature)
        {
            creature.Agent.isStopped = false;
        }
    }

    public class Escape : INeutralCreatureState
    {
        public void Enter(NeutralCreature creature)
        {
            creature.Run();
            creature.Escape();
        }

        public void Update(NeutralCreature creature)
        {
            if (creature.IsPathComplete()) creature.SwitchState(new Idle());
        }
        public void Exit(NeutralCreature creature)
        {
            creature._Animator.ResetTrigger("Run");
        }
    }
}
