using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Creature_SpawnDeadbody : MonoBehaviour
{
    [SerializeField] private List<GameObject> _other;
    private List<GameObject> ListHideObj => _other;

    [SerializeField] private List<GameObject> _listBody;
    private List<GameObject> ListBody => _listBody;

    private void Start()
    {
        CreatureIsDead();
        EnableRagdoll();
    }

    private void CreatureIsDead()
    {
        Creature creature = this.GetComponent<Creature>();
        if (creature != null) creature.enabled = false;

        Collider collider = this.GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        Animator animator = this.GetComponent<Animator>();
        if (animator != null) animator.enabled = false;

        NavMeshAgent agent = this.GetComponent<NavMeshAgent>();
        if(agent != null) agent.enabled = false;

        foreach (GameObject member in ListHideObj)
        {
            member.SetActive(false);
        }
    }

    #region ragdoll

    private void EnableRagdoll()
    {
        foreach (GameObject body in ListBody)
        {
            NavMeshObstacle navMeshObstacle = body.GetComponent<NavMeshObstacle>();
            if (navMeshObstacle != null) EnableObstacle(navMeshObstacle);

            Resource resource = body.GetComponent<Resource>();
            if (resource != null) EnableResource(resource/*, body*/);

            Rigidbody rigidbody = body.GetComponent<Rigidbody>();
            if (rigidbody != null) EditRigidbody(rigidbody);
        }
    }

    private void EnableObstacle(NavMeshObstacle obstacle)
    {
        obstacle.enabled = true;
    }

    private void EnableResource(Resource resource/*, GameObject body*/)
    {
        resource.enabled = true;

        Collider collider = resource.gameObject.GetComponent<Collider>();
        if (collider != null) EditCollider(collider);
    }

    private void EditCollider(Collider collider)
    {
        collider.isTrigger = true;
    }

    private void EditRigidbody(Rigidbody rigidbody)
    {
        //rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
    }

    #endregion ragdoll


    #region load dead body data
    [Header("dead body")]
    [SerializeField] private Resource _deadBody;
    public Resource DeadBody => _deadBody;

    #endregion
}
