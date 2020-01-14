using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    Button[] buttons;
	bool[] enableds;

	public Color DisabledColor;
	public Color PressedColor;

	void Start()
    {
        buttons = GetComponentsInChildren<Button>();
		enableds = new bool[buttons.Length];

		int i = 0;
		foreach (Button b in buttons)
		{
			b.onClick.AddListener(delegate { OnButtonClicked(b); });
			enableds[i] = true;
			++i;
		}
    }

    public void SetAllButtonsInteractable()
    {
		for(int i = 0; i < buttons.Length; ++i)
			buttons[i].interactable = enableds[i];
    }

    public void OnButtonClicked(Button clickedButton)
    {
        int buttonIndex = Array.IndexOf(buttons, clickedButton);

        SetAllButtonsInteractable();

        clickedButton.interactable = false;
    }

	public void SetDisableds(int startDisabledIndex)
	{
		for(int i = 0; i < buttons.Length; ++i)
		{
			bool isEnabled = i < startDisabledIndex;

			enableds[i] = isEnabled;
			buttons[i].interactable = isEnabled;

			var myColorBlock = buttons[i].colors;
			myColorBlock.disabledColor = isEnabled ? PressedColor : DisabledColor;
			buttons[i].colors = myColorBlock;
		}
	}

	public void ClickToggle(int index)
	{
		if (index > 4)
			return;
		buttons[index].onClick.Invoke();
	}
}
