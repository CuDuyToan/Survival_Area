using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour
{
    [SerializeField] private List<RecipeFurnaceSO> listRecipe;

    [SerializeField] private bool isActive = false;
    public bool IsActive
    {
        set
        {
            isActive = value;
        }
        get
        {
            return isActive;
        }
    }

    private ItemStorage storage;

    private float burnTime = 0;
    private float timeComplete = 10;
    private float timeCount = 0;
    private float TimeCount
    {
        set
        {
            timeCount = value;
            if(timeCount > timeComplete)
            {
                timeCount = timeComplete;
            }
            else if(timeCount < 0)
            {
                timeCount = 0;
            }
        }
        get
        {
            return timeCount;
        }
    }

    [Header("Effect")]
    [SerializeField] private ParticleSystem fire;
    [SerializeField] private ParticleSystem smoke;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        storage = GetComponent<ItemStorage>();
    }

    private void Update()
    {
        FinishedProduct();
    }

    public void OnStartBaking()
    {
        ConsumeFuel();

        if(!this.fire.isPlaying && isActive) this.fire.Play();
        if(!this.smoke.isPlaying && isActive) this.smoke.Play();
        if(!this.audioSource.isPlaying && isActive) this.audioSource.Play();    

    }

    public void OnEndBaking()
    {
        IsActive = false;

        TimeCount = 0;
        burnTime = 0;

        if(!this.fire.isStopped) this.fire.Stop();
        if(!this.smoke.isStopped) this.smoke.Stop();
        if (this.audioSource.isPlaying) this.audioSource.Stop();
    }

    private void FinishedProduct()
    {
        if(IsActive)
        {
            TimeCount += Time.deltaTime;
            burnTime -= Time.deltaTime;

            if (burnTime <= 0)
            {
                ConsumeFuel();
            }

            if (TimeCount >= timeComplete)
            {
                OutputItem();
            }
        }

        
    }

    private void ConsumeFuel()
    {
        foreach (ItemStack item in storage.ItemList)
        {
            if (item._Item is MaterialSO material)
            {
                if (material.IsFuel)
                {
                    storage.ConsumeThisItem(item);
                    burnTime = material.BurnTime;
                    isActive = true;
                    return;
                }
            }
        }
        this.OnEndBaking();
    }

    private void OutputItem()
    {
        TimeCount = 0;
        foreach (RecipeFurnaceSO recipe in listRecipe)
        {
            if(storage.TotalItemInList(recipe.InputItem) >= recipe.InputQuantity)
            {
                storage.ConsumeItem(recipe.InputItem, recipe.InputQuantity);
                storage.AddItem(recipe.OutputItem, recipe.OutputQuantity);
                return;
            }
        }

    }

}
