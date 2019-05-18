using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PersonData : MonoBehaviour
{
    public string person_name = "";
    public int signal_strength = 0;

    public Transform camera_transform;

    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.GetChild(1).GetChild(0).GetChild(3).GetComponent<Text>().text = person_name;
        gameObject.transform.GetChild(1).GetChild(0).GetChild(4).GetComponent<Text>().text = signal_strength.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.GetChild(1).GetChild(0).transform.rotation = camera_transform.rotation;
    }
}
