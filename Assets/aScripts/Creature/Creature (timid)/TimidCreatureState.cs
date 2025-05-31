using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimidCreatureState
{
    public class Idle : ITimidCreatureState
    {
        float waitTime = 5;
        public void Enter(TimidCreature creature)
        {
            creature.Idle();
            if (creature.Target != null) waitTime *= 4;
        }

        public void Update(TimidCreature creature)
        {
            if(waitTime > 0)
            {
                waitTime -= Time.deltaTime;
            }

            if (waitTime <= 0)
            {
                creature.Target = null;
                creature.SwitchState(new Wadering());
            }

            if(creature.Target != null && !creature.isSafe)
            {
                creature.SwitchState(new Escape());
            }
        }
        public void Exit(TimidCreature creature)
        {

        }
    }

    public class Wadering : ITimidCreatureState
    {
        public void Enter(TimidCreature creature)
        {
            creature.RandomDestination();
            creature.Walk();
        }

        public void Update(TimidCreature creature)
        {
            if (creature.IsPathComplete()) creature.SwitchState(new Idle());
            
            if (creature.Target != null && !creature.isSafe)
            {
                creature.SwitchState(new Escape());
            }
        }
        public void Exit(TimidCreature creature)
        {

        }
    }

    public class Escape : ITimidCreatureState
    {
        public void Enter(TimidCreature creature)
        {
            creature.Escape();
            creature.Run();
        }

        public void Update(TimidCreature creature)
        {
            if(creature.IsPathComplete())
            {
                creature.SwitchState(new Idle());
            }
        }
        public void Exit(TimidCreature creature)
        {

        }
    }
}
