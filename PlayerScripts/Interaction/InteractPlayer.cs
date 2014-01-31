using UnityEngine;
using System.Collections;

public class InteractPlayer : MonoBehaviour
{

    /*        
        Interact Player
        Author: Tristan 'Kennyist' Cunningham - www.tristanjc.com
        Date: 31/01/2014
        License: Creative Commons ShareAlike 3.0 - https://creativecommons.org/licenses/by-sa/3.0/
    */


    /* ------------------ In development ---------------- */
    // Early version

    public LayerMask layerMask;
    public Transform rayStartLocation;
    public float castDistance = 1f;
    private GameObject hitOBJ;
    private GameObject lastOBJ;
    private RaycastHit hit;

    // Use this for initialization
    void Start()
    {
        hitOBJ = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(rayStartLocation.position, rayStartLocation.forward, out hit, castDistance, layerMask))
        {
            hitOBJ = hit.collider.gameObject;

            if (lastOBJ != null)
            {
                if (hitOBJ != lastOBJ)
                {
                    lastOBJ.SendMessage("InteractionHit", false, SendMessageOptions.DontRequireReceiver);
                    lastOBJ = null;
                }
            }

            if (hitOBJ.GetComponent<InteractableObject>() != null && hitOBJ != gameObject)
            {
                hitOBJ.SendMessage("InteractionHit", true, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                hitOBJ.SendMessage("InteractionHit", false, SendMessageOptions.DontRequireReceiver);
                hitOBJ = null;
            }
        }
        else
        {
            if (lastOBJ != null)
            {
                lastOBJ.SendMessage("InteractionHit", false, SendMessageOptions.DontRequireReceiver);
                lastOBJ = null;
            }
        }

        if (hitOBJ != null)
        {
            lastOBJ = hitOBJ;
        }
    }
}
