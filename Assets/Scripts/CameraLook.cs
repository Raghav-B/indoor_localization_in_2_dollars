using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraLook : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData click) {
        if (click.button == PointerEventData.InputButton.Left) {
            
        }
    }

    public GameObject SurvivorName;
    public GameObject Strength;

    Vector2 mouseLook;
	Vector2 smoothV;
	Vector2 FinalRot;

	#if UNITY_STANDALONE
	public float sensitivity = 2f;
	public float smoothing = 2f;
	#endif
	#if UNITY_ANDROID || UNITY_IOS
	public float sensitivity = 4f;
	public float smoothing = 1.0f;
	#endif


	public GameObject CameraGameObject;
	public GameObject ActualCamera;

    void Start() {
        cam_rotation();
        SurvivorName.transform.GetComponent<Text>().text = "";
        Strength.transform.GetComponent<Text>().text = "";
    }

    void Update() {
        if (Input.GetKey(KeyCode.Mouse1)) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cam_rotation();
        } else if (Input.GetKeyUp(KeyCode.Mouse1)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else if (Input.GetKeyDown(KeyCode.Mouse0)) {
            click();
        }
        if (Input.GetKey(KeyCode.Mouse2)) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Panning();
        } else if (Input.GetKeyUp(KeyCode.Mouse2)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else if (Input.GetKeyDown(KeyCode.Mouse0)) {
            click();
        }

        Zooming();
	}

    void click() {
        Ray MouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit HitPoint;

        Debug.Log("clicked");

        if (Physics.Raycast(MouseRay, out HitPoint, Mathf.Infinity)) {
            if (HitPoint.collider.tag == "PersonTag") {
                Debug.Log(HitPoint.collider.transform.GetComponent<PersonData>().person_name +
                    ", " + HitPoint.collider.transform.GetComponent<PersonData>().signal_strength);

                SurvivorName.transform.GetComponent<Text>().text = HitPoint.collider.transform.GetComponent<PersonData>().person_name;
                Strength.transform.GetComponent<Text>().text = HitPoint.collider.transform.GetComponent<PersonData>().signal_strength.ToString();

            } else {
                Debug.Log("wrong hit");
            }
        }
    }

    void Zooming() {
        var height = Input.mouseScrollDelta.y;
        int movement = 0;

        if (height > 0) {
            movement = 2;
        } else if (height < 0) {
            movement = -2;
        }

        CameraGameObject.transform.Translate(new Vector3(0, movement, 0));
        
    }

    void Panning() {
        var x = CrossPlatformInputManager.GetAxis("Mouse X");
        var y = CrossPlatformInputManager.GetAxis("Mouse Y");

        int new_x = 0;
        int new_y = 0;

        if (x > 1) {
            new_x = 2;
        } else if (x < -1) {
            new_x = -2;
        }

        if (y > 1) {
            new_y = 2;
        } else if (y < -1) {
            new_y = -2;
        }

        CameraGameObject.transform.Translate(new Vector3(new_x, 0, new_y));

        //Debug.Log("X: " + x + ", Y: " + y);
    }

    void cam_rotation() {
    #if UNITY_STANDALONE
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")); 
    #endif
    #if UNITY_ANDROID || UNITY_IOS
		var md = new Vector2 (VJoystick.Horizontal (), VJoystick.Vertical ());
    #endif

        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        mouseLook += smoothV;

        mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

        FinalRot.x = -mouseLook.y;
        FinalRot.y = mouseLook.x;

        ActualCamera.transform.localRotation = Quaternion.AngleAxis(FinalRot.x, Vector3.right);
        CameraGameObject.transform.localRotation = Quaternion.AngleAxis(FinalRot.y, CameraGameObject.transform.up);
    }
}
