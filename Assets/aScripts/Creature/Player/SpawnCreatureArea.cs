using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCreatureArea : MonoBehaviour
{
    [Header("Range")]
    [SerializeField] private SphereCollider nonSpawnRange;
    private SphereCollider spawnRange;

    private void Awake()
    {
        spawnRange = GetComponent<SphereCollider>();
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        StartCoroutine(CreatureDensity());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    [SerializeField] private List<Creature> creatures = new List<Creature>();
    private List<Creature> ListCreature
    {
        get
        {
            for (int i = 0; i < creatures.Count; i++)
            {
                if (creatures[i] == null)
                {
                    creatures.RemoveAt(i);
                }
            }

            return creatures;
        }
    }

    [SerializeField] private int limitDensity = 5;
    [SerializeField, Min(10)] private float spawnTime = 10f;

    #region trigger
    private void OnTriggerEnter(Collider other)
    {
        CreatureSetActive creatureActive = other.GetComponent<CreatureSetActive>();

        if (creatureActive == null) return;

        creatures.Add(creatureActive.creature);
    }

    private void OnTriggerExit(Collider other)
    {
        CreatureSetActive creatureActive = other.GetComponent<CreatureSetActive>();

        if (creatureActive == null) return;

        creatures.Remove(creatureActive.creature);
    }



    #endregion trigger


    #region creature spawn

    public static event Action<Vector3> OnPositionSpawn;

    private IEnumerator CreatureDensity()
    {
        while (true)
        {
            if(ListCreature.Count < limitDensity)
            {
                float x = RandomSpawnPosition();
                float y = 0;
                float z = RandomSpawnPosition();

                Vector3 randomPos = transform.position + new Vector3(x,y,z);

                randomPos.y = 0;

                OnPositionSpawn?.Invoke(randomPos);
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }

    private float RandomSpawnPosition()
    {
        float result = RandomSystem.RandomFloat(-spawnRange.radius, spawnRange.radius);

        while (MathF.Abs(result) < nonSpawnRange.radius)
        {
            result = RandomSystem.RandomFloat(-spawnRange.radius, spawnRange.radius);
        }

        return result;
    }

    #endregion creature spawn
}
