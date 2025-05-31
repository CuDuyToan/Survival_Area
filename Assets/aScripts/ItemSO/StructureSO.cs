using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StructureSO ()", menuName = "Scriptable/Items/StructureSO")]
public class StructureSO : ItemSO
{
    [SerializeField] private RecipeSO recipeSO;
    public RecipeSO RecipeSO => recipeSO;

    [SerializeField] private bool canTakeDame = true;
    public bool CanTakeDame => canTakeDame;

    [SerializeField] private float maxHealth;
    public float MaxHealth => maxHealth;

    [SerializeField] private GameObject structurePrefab;
    public GameObject StructurePrefab => structurePrefab;

    [SerializeField] private GameObject objPreview;
    public GameObject ObjPreview => objPreview;
}
