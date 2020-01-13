using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    private Button[] buttons;

    void Start()
    {
        buttons = GetComponentsInChildren<Button>();

        foreach(Button b in buttons)
        {
            b.onClick.AddListener(delegate { OnButtonClicked(b); });
        }
    }


    public void SetAllButtonsInteractable()
    {
        foreach (Button button in buttons)
            button.interactable = true;
    }

    public void OnButtonClicked(Button clickedButton)
    {
        int buttonIndex = System.Array.IndexOf(buttons, clickedButton);

        if (buttonIndex == -1)
            return;

        SetAllButtonsInteractable();

        clickedButton.interactable = false;
    }
}
