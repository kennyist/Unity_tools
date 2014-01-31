﻿using UnityEngine;
using System.Collections;

public class ObjectiveBreifcase : MonoBehaviour {

    Interaction interact;
    
    public Camera cam;
    public Texture2D barTexture;
    public Texture2D bgTexture;

    bool isHit = false, complete = false;

    void Start()
    {
        interact = gameObject.GetComponent<Interaction>();
    }

    void InteractionHit(bool hit)
    {
        isHit = hit;
    }

    void InteractableComplete()
    {
        complete = true;
    }

    void OnGUI()
    {
        float length = interact.holdTime / interact.interactable.holdTime;

        if(isHit && !complete){
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 30), "hold "+ interact.interactable.InputButton);
        }

         if (interact.holdTime > 0.0f && !complete)
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
