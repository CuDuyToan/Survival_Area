using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BiomSpawnCreature : MonoBehaviour
{
    [SerializeField] private List<GameObject> creaturePrefabs;
    [SerializeField] private LayerMask terrainLayer;

    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        SpawnCreatureArea area = other.GetComponent<SpawnCreatureArea>();

        if (area == null) return;

        SpawnCreatureArea.OnPositionSpawn += SpawnCreature;
    }

    private void OnTriggerExit(Collider other)
    {
        SpawnCreatureArea area = other.GetComponent<SpawnCreatureArea>();

        if (area == null) return;

        SpawnCreatureArea.OnPositionSpawn -= SpawnCreature;
    }


    #region spawn creature
    //position-rotation
    public static event Action<GameObject, Vector3, Vector3> OnSpawnNewCreature;
    private void SpawnCreature(Vector3 position)
    {
        if (boxCollider == null) return;

        position.y = boxCollider.bounds.max.y / 2;

        if (!boxCollider.bounds.Contains(position)) return;

        position.y = boxCollider.bounds.max.y;

        Vector3 spawnPosition = GetPostionOnTerrain(position);

        float x = RandomRotation();
        float y = RandomRotation();
        float z = RandomRotation();

        Vector3 rotation = new Vector3(x, y, z);

        int gobjInList = RandomSystem.RandomInt(0, creaturePrefabs.Count);

        Debug.Log("Spawn", creaturePrefabs[gobjInList]);


        OnSpawnNewCreature?.Invoke(creaturePrefabs[gobjInList], spawnPosition, rotation);
    }

    private Vector3 GetPostionOnTerrain(Vector3 startRay)
    {
        Ray rayDown = new Ray(startRay, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(rayDown, out hit, Mathf.Infinity, terrainLayer))
        {
            return hit.point;
        }
        return startRay;
    }

    private float RandomRotation()
    {
        return RandomSystem.RandomFloat(-360, 360);
    }

    #endregion spawn creature
}
