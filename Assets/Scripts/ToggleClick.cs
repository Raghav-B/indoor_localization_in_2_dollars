using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject to_toggle;

    void Start() {
        to_toggle.SetActive(true);
    }

    public void OnPointerClick(PointerEventData click) {
        if (click.button == PointerEventData.InputButton.Left) {
            if (to_toggle.activeInHierarchy == false) {
                to_toggle.SetActive(true);
            } else {
                to_toggle.SetActive(false);
            }
        }
    }
}
