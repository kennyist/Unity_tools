using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour
{

    /*        
        Interact Object
        Author: Tristan 'Kennyist' Cunningham - www.tristanjc.com
        Date: 31/01/2014
        License: Creative Commons ShareAlike 3.0 - https://creativecommons.org/licenses/by-sa/3.0/
    */


    /* ------------------ In development ---------------- */
    // Early version

    public delegate void OnComplete();
    public event OnComplete Complete; 

    public enum Type { Press, HoldForTime, HoldForTimeSlowReset }
    public Type type;
    public float TotalHoldTime = 3f;
    public string InputButton = "f";
    public float slowResetMultiplier = 0.1f;
    private bool isHit;
    private float holdTime = 0.0f;

    void Awake() { InteractPlayer.OnChange += HitEvent; }

    void HitEvent(bool b, GameObject g){
        if (gameObject == g)
        {
            isHit = b;
        }
    }

    void Update()
    {
        if ((type == Type.HoldForTime || type == Type.HoldForTimeSlowReset) && holdTime >= TotalHoldTime)
        {
            Complete();
        }
        else if (type == Type.Press && isHit && Input.GetKeyDown(InputButton))
        {
            Complete();
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
