using UnityEngine;
using System.Collections;

/*        
        Player Interaction / Interactable
        Author: Tristan 'Kennyist' Cunningham - www.tristanjc.com
        Date: 31/01/2014
        License: Creative Commons ShareAlike 3.0 - https://creativecommons.org/licenses/by-sa/3.0/
*/


/* ------------------ In development ---------------- */
// Early version

public class Interaction : MonoBehaviour {
    public enum Type { Interactable, Caster }

    [System.Serializable]
    public class Caster
    {
        public LayerMask layerMask;
        public Transform rayStartLocation;
        public float castDistance = 1f;
    }

    [System.Serializable]
    public class Interactable
    {
        public enum Type { Press, HoldForTime, HoldForTimeSlowReset }
        public Type type; 
        public float holdTime = 3f;
        public string InputButton = "f";
        public float slowResetTime = 0.1f;
    }

    public Type type;
    public Caster caster = new Caster();
    public Interactable interactable = new Interactable();

    private GameObject hitOBJ;
    private GameObject lastOBJ;
    private bool isHit;
    
    [HideInInspector]
    public float holdTime;

    void start()
    {
        hitOBJ = null;
        holdTime = 0.0f;
    }
	
	void Update () {
        if (type == Type.Caster)
        {
            CastRay();
        }
        else
        {
            if ((interactable.type == Interactable.Type.HoldForTime || interactable.type == Interactable.Type.HoldForTimeSlowReset) && holdTime >= interactable.holdTime)
            {
                gameObject.SendMessage("InteractableComplete", SendMessageOptions.DontRequireReceiver);
            }
            else if (interactable.type == Interactable.Type.Press && isHit && Input.GetKeyDown(interactable.InputButton))
            {
                gameObject.SendMessage("InteractableComplete", SendMessageOptions.DontRequireReceiver);
            }

            if (isHit && Input.GetKey(interactable.InputButton))
            {
                holdTime += Time.deltaTime;

                if (holdTime > interactable.holdTime)
                {
                    holdTime = interactable.holdTime;
                }

                Debug.Log(holdTime);
            }
            else
            {
                if (interactable.type != Interactable.Type.HoldForTimeSlowReset)
                {
                    holdTime = 0.0f;
                }
                else
                {
                    if (holdTime > 0.0f)
                    {
                        holdTime -= Time.deltaTime * interactable.slowResetTime;
                    }
                }
            }
        }
	}

    void InteractionHit(bool hit)
    {
        isHit = hit;
    }

    RaycastHit hit;

    void CastRay()
    {

        if (Physics.Raycast(caster.rayStartLocation.position, caster.rayStartLocation.forward, out hit, caster.castDistance, caster.layerMask))
        {
            hitOBJ = hit.collider.gameObject;

            if (hitOBJ.GetComponent<Interaction>() != null && hitOBJ != gameObject)
            {
                if (hitOBJ.GetComponent<Interaction>().type == Type.Interactable)
                {
                    hitOBJ.SendMessage("InteractionHit", true, SendMessageOptions.DontRequireReceiver);
                }
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
