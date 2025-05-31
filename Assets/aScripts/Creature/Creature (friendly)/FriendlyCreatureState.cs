using UnityEngine;

namespace FriendlyCreatureState
{
    public class Idle : IFriendlyCreatureState
    {
        float waitTime = 5;
        public void Enter(FriendlyCreature creature)
        {
            creature.Idle();
            if (creature.Target != null) waitTime *= 4;
        }

        public void Update(FriendlyCreature creature)
        {
            if(waitTime > 0)
            {
                waitTime -= Time.deltaTime;
            }else if(waitTime <= 0)
            {
                creature.Target = null;
                creature.SwitchState(new Wadering());
            }

            if(creature.Target != null)
            {
                if (Vector3.Distance(creature.Target.transform.position, creature.transform.position) <= creature.SafeDistance)
                {
                    creature.SwitchState(new Escape());
                }
            }
        }
        public void Exit(FriendlyCreature creature)
        {

        }
    }

    public class Wadering : IFriendlyCreatureState
    {
        public void Enter(FriendlyCreature creature)
        {
            creature.Walk();
            creature.RandomDestination();
        }

        public void Update(FriendlyCreature creature)
        {
            if (creature.IsPathComplete()) creature.SwitchState(new Idle());
        }
        public void Exit(FriendlyCreature creature)
        {

        }
    }

    public class Escape : IFriendlyCreatureState
    {
        public void Enter(FriendlyCreature creature)
        {
            creature.Run();
            creature.Escape();
        }

        public void Update(FriendlyCreature creature)
        {
            if (creature.IsPathComplete()) creature.SwitchState(new Idle());
        }
        public void Exit(FriendlyCreature creature)
        {
            creature._Animator.ResetTrigger("Run");
        }
    }
}