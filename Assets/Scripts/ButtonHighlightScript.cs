using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHighlightScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    Color ogColor;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        ogColor = GetComponent<Image>().color;
        GetComponent<Image>().color = new Color(.2f, 0.2f, .2f, 1f);
    }

    public void OnButtonClick()
    {
        GetComponent<Image>().color = ogColor;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = ogColor;
    }
}
