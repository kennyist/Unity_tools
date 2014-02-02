using UnityEngine;
using System.Collections;

public class ObjectiveBriefcase : MonoBehaviour {


    /// <summary>
    /// A basic example script, using the interaction script to create a "Steal the briefcase" interation object
    /// </summary>
    
    
    public Texture2D barTexture;
    public Texture2D bgTexture;

    InteractableObject interact;
    bool isHit = false, complete = false;

    void Awake()
    {
        InteractPlayer.OnChange += HitEvent;
        interact = gameObject.GetComponent<InteractableObject>();
        interact.Complete += InteractableComplete;
    }

    void HitEvent(bool hit, GameObject g)
    {
        if (g == gameObject)
        {
            isHit = hit;
        }
    }

    void InteractableComplete()
    {
        complete = true;
        interact.Complete -= InteractableComplete;
    }

    void OnGUI()
    {
        float length = interact.CurrentHeldTime() / interact.TotalHoldTime;

        if(isHit && !complete){
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 30), "hold "+ interact.InputButton);
        }

         if (interact.CurrentHeldTime() > 0.0f && !complete)
        {
            GUI.DrawTexture(new Rect(Screen.width / 2 - 102, Screen.height / 2 + 48, 204, 24), bgTexture);
            GUI.DrawTexture(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 50, 200 * length, 20), barTexture);
        }

        if (complete)
        {
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 30), " Briefcase stolen");
            gameObject.renderer.enabled = false;
            gameObject.collider.enabled = false;
        }
    }
}
