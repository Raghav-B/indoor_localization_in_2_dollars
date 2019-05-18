using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PersonClick : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData click) {
        if (click.button == PointerEventData.InputButton.Left) {

        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
