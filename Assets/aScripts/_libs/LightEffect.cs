using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem lightSource;
    [SerializeField] private PlayerController player;

    private void OnEnable()
    {
        lightSource.Play();
        if(player != null) StartCoroutine(DecreaseDurability());
    }

    private void OnDisable()
    {
        lightSource.Stop();
        if (player != null) StopCoroutine(DecreaseDurability());
    }

    private IEnumerator DecreaseDurability()
    {
        while (true)
        {
            if(player._CurrentItem != null)
            {
                player._CurrentItem._Durability -= 1;
            }


            yield return new WaitForSeconds(1);
        }
    }
}
