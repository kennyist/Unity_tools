using UnityEngine;
using System.Collections;

public class InteractObject : MonoBehaviour
{

    /*        
        Interact Object
        Author: Tristan 'Kennyist' Cunningham - www.tristanjc.com
        Date: 31/01/2014
        License: Creative Commons ShareAlike 3.0 - https://creativecommons.org/licenses/by-sa/3.0/
    */


    /* ------------------ In development ---------------- */
    // Early version

    public enum Type { Press, HoldForTime, HoldForTimeSlowReset }
    public Type type;
    public float TotalHoldTime = 3f;
    public string InputButton = "f";
    public float slowResetMultiplier = 0.1f;
    private bool isHit;
    private float holdTime;

    void Start()
    {
        holdTime = 0.0f;
    }

    void Update()
    {
        if ((type == Type.HoldForTime || type == Type.HoldForTimeSlowReset) && holdTime >= TotalHoldTime)
        {
            gameObject.SendMessage("InteractableComplete", SendMessageOptions.DontRequireReceiver);
        }
        else if (type == Type.Press && isHit && Input.GetKeyDown(InputButton))
        {
            gameObject.SendMessage("InteractableComplete", SendMessageOptions.DontRequireReceiver);
        }

        if (isHit && Input.GetKey(InputButton))
        {
            holdTime += Time.deltaTime;

            if (holdTime > TotalHoldTime)
            {
                holdTime = TotalHoldTime;
            }
        }
        else
        {
            if (type != Type.HoldForTimeSlowReset)
            {
                holdTime = 0.0f;
            }
            else
            {
                if (holdTime > 0.0f)
                {
                    holdTime -= Time.deltaTime * slowResetMultiplier;
                }
            }
        }

        Debug.Log(holdTime);
    }

    void InteractionHit(bool hit)
    {
        isHit = hit;
    }

    /// <summary>
    /// Get the time the button has been held for.
    /// </summary>
    /// <returns>Time button has been held for while on this object</returns>
    public float CurrentHeldTime()
    {
        return holdTime;
    }
}
