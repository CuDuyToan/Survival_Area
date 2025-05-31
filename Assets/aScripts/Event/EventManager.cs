using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public event Action<bool> OnRecipeFeasible;

    public void CraftItem(bool active)
    {
        OnRecipeFeasible?.Invoke(active);
    }
}
