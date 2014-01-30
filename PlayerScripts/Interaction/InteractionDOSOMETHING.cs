using UnityEngine;
using System.Collections;

public class InteractionDOSOMETHING : MonoBehaviour {

    // Exmaple of how you can add any tpye of action on Interaction complete
    // Attach to the same object as the Interaction Interactable, Once its type (Such as button pressed) "InteractableComplete()" will fire

    bool complete = false;

    void InteractableComplete()
    {
        gameObject.GetComponent<Interaction>().enabled = false;
        // Do something
        complete = true;
    }

    void OnGUI()
    {
        if (complete)
        {
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 30), "Interaction complete");
        }
    }
}
