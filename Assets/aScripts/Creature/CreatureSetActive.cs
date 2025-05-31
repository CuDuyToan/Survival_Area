using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureSetActive : MonoBehaviour
{
    [HideInInspector] public Creature creature;
    private CapsuleCollider capsuleCollider;
    private Animator animator;
    private NavMeshAgent agent;

    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private Canvas canvas;

    private void SetActive(bool active)
    {
        if(creature.IsDead == false)
        {
            creature.enabled = active;
            capsuleCollider.enabled = active;
            animator.enabled = active;
            agent.isStopped = !active;

            canvas.enabled = active;
        }


        skinnedMeshRenderer.enabled = active;
    }

    private void GetAllComponent()
    {
        creature = GetComponentInParent<Creature>();
        capsuleCollider = GetComponentInParent<CapsuleCollider>();
        animator = GetComponentInParent<Animator>();
        agent = GetComponentInParent<NavMeshAgent>();
    }


    private void Awake()
    {
        GetAllComponent();
    }


    private void OnTriggerEnter(Collider other)
    {
        SpawnCreatureArea area = other.GetComponent<SpawnCreatureArea>();

        if (area == null) return;

        SetActive(true);

    }

    private void OnTriggerExit(Collider other)
    {
        SpawnCreatureArea area = other.GetComponent<SpawnCreatureArea>();

        if (area == null) return;

        SetActive(false);
    }
}
