using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class inputController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Text toChangeText;
    public Color normalColor, highlightedColor;
    private bool selected = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.GetComponent<Button>().interactable)
        {
            toChangeText.color = highlightedColor;
            toChangeText.fontStyle = FontStyle.Bold;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (this.GetComponent<Button>().interactable && !selected)
        {
            toChangeText.color = normalColor;
            toChangeText.fontStyle = FontStyle.Normal;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        selected = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        toChangeText.color = normalColor;
        toChangeText.fontStyle = FontStyle.Normal;

        selected = false;
    }

    public void QuitApplication()
    {
        Debug.Log("Application has quit.");
        Application.Quit();
    }

    public void SwitchMenu()
    {
        toChangeText.color = normalColor;
        toChangeText.fontStyle = FontStyle.Normal;
        selected = false;
    }
}