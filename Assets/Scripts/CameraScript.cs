using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private Transform character;
    private InputAction lookAction;
    private Vector3 cameraAngles;
    private Vector3 cameraPosition;
    //private Vector3 headPosition;

    void Start() {
        character = GameObject.FindWithTag("Character").transform;
        lookAction = InputSystem.actions.FindAction("Look");
        cameraAngles = this.transform.eulerAngles;
        cameraPosition = this.transform.position - character.position;
        //headPosition = GameObject.Find("HeadPosition").transform.position;
    }

    void Update() {
        Vector2 scrollWheel = Input.mouseScrollDelta;
        
        if(scrollWheel.y != 0) {
            if(cameraPosition.magnitude > GameState.minFpvDistance && cameraPosition.magnitude < GameState.maxFpvDistance) {
                float rr = cameraPosition.magnitude * (1 - scrollWheel.y / 10);
                if(rr <= GameState.minFpvDistance ) {
                    //cameraPosition.z *= 0.01f;
                    cameraPosition.z *= 1f;
                    //GameState.isFpv = true;
                }
                else if(rr >= GameState.maxFpvDistance) {
                    cameraPosition.z *= 1f;
                }
                else {
                    cameraPosition.z *= (1 - scrollWheel.y / 10);
                }
            }
            else if(scrollWheel.y < 0) {
                cameraPosition.z *= 100f;
                cameraPosition.z *= (1 - scrollWheel.y / 10);
                //GameState.isFpv = false;
            }
        }

        Vector2 lookValue = lookAction.ReadValue<Vector2>();
        if (lookValue != Vector2.zero) {
            cameraAngles.x += lookValue.y * Time.deltaTime * GameState.lookSensitivityY;
            cameraAngles.y += lookValue.x * Time.deltaTime * GameState.lookSensitivityX;

            cameraAngles.x = Mathf.Clamp(cameraAngles.x, 0, 35);

            //if(cameraPosition.y < 0.90f) {
            //    cameraAngles.x = Mathf.Clamp(cameraAngles.x, -10, 40);
            //}
            //else {
            //    cameraAngles.x = Mathf.Clamp(cameraAngles.x, 35, 75);
            //}
            this.transform.eulerAngles = cameraAngles;
        }
        this.transform.position = character.position + Quaternion.Euler(0, cameraAngles.y, 0) * cameraPosition;
    }
}
