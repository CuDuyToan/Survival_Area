using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionCollider : MonoBehaviour
{
    [SerializeField] private PlayerController _ownerPlayer;

    //private void OnTriggerEnter(Collider other)
    //{
    //    GameObject gObj = other.gameObject;
    //    if (gObj != null && _ownerPlayer.Target == gObj)
    //    {
    //        _ownerPlayer.CanInteract = true;
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        GameObject gObj = other.gameObject;
        if (gObj != null && _ownerPlayer.Target == gObj)
        {
            _ownerPlayer.CanInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject gObj = other.gameObject;
        if (gObj != null && _ownerPlayer.Target == gObj)
        {
            _ownerPlayer.CanInteract = false;
        }
    }
}
