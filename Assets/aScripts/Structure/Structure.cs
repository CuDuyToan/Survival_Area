using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public static event Action<Structure> OnInteractStructure;

    [SerializeField] private List<AudioClip> audioList;
    private AudioSource audioSource;

    #region protected member
    [SerializeField] protected StructureSO _structureSO;

    protected bool _CanTakeDame => _structureSO.CanTakeDame;


    #endregion

    #region public member

    public StructureSO _StructureSO => _structureSO;

    [SerializeField] private float _health;
    public float _Health
    {
        set
        {
            if (value > _structureSO.MaxHealth) _health = _structureSO.MaxHealth;
            else if (value < 0) _health = 0;
            else _health = value;

            if(_health <= 0) Destroy(this.gameObject);
        }
        get
        {
            return _health;
        }

    }

    public RecipeSO _RecipeSO => _structureSO.RecipeSO;

    #endregion

    private void Awake()
    {
        _Health = _structureSO.MaxHealth;
    }

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void PlayerInteraction()
    {
        OnInteractStructure?.Invoke(this);
    }

    public virtual void TakeDame(float amount, GameObject source)
    {
        this._Health -= amount;

        if(audioList.Count > 0)
        {
            audioSource.PlayOneShot(audioList[RandomSystem.RandomInt(audioList.Count - 1, 0)]);
        }
    }
}
