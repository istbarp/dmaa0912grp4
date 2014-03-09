using UnityEngine;
using System.Collections;

public class RTSCameraController : MonoBehaviour
{
    public float sensitivityDistance = 50;
    public float damping = 5;
    public float minFOV = 40;
    public float maxFOV = 120;
    public float minY = 10;
    public float maxY = 100;
    public float CamSpeed = 0.5f;
    public float ScreenEdgeOffset = 5;
    public float yMinLimit = -90;
    public float yMaxLimit = 90;
    public float mouseSentivity = 0.1f;
    public Rect playArea;

    public InputManager inputManager;

    float xDeg = 0;
    float yDeg = 0;
    private float distance;
    Rect recdown;
    Rect recup;
    Rect recleft;
    Rect recrigh;

    void Start()
    {
        inputManager = new InputManager();
        recdown = new Rect(0, 0, Screen.width, ScreenEdgeOffset);
        recup = new Rect(0, Screen.height - ScreenEdgeOffset, Screen.width, ScreenEdgeOffset);
        recleft = new Rect(0, 0, ScreenEdgeOffset, Screen.height);
        recrigh = new Rect(Screen.width - ScreenEdgeOffset, 0, ScreenEdgeOffset, Screen.height);
    }

    void LateUpdate()
    {
        float dif = (maxY - minY) / (maxFOV - minFOV);
        distance -= Input.GetAxis("ScrollWheel") * sensitivityDistance;
        distance = Mathf.Clamp(distance, minY, maxY);
        //camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, distance, Time.deltaTime * damping);
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, distance, Time.deltaTime * damping), transform.position.z);
        inputManager.CheckControleKeys();
        if (inputManager.CheckInput("Rotate", KeyStages.Press))
        {
            xDeg += Input.GetAxis("MouseX") * StaticValues.AxisxSpeed * 0.1f * StaticValues.invertX;
            yDeg -= Input.GetAxis("MouseY") * StaticValues.AxisySpeed * 0.1f * StaticValues.invertY;
            if (Input.GetAxis(StaticValues.MouseAxisX) < -StaticValues.deadZone || Input.GetAxis(StaticValues.MouseAxisX) > StaticValues.deadZone)
            {
                xDeg += Input.GetAxis(StaticValues.MouseAxisX) * StaticValues.AxisxSpeed * 0.1f * StaticValues.invertX;
            }
            if (Input.GetAxis(StaticValues.MouseAxisY) < -StaticValues.deadZone || Input.GetAxis(StaticValues.MouseAxisY) > StaticValues.deadZone)
            {
                yDeg += Input.GetAxis(StaticValues.MouseAxisY) * StaticValues.AxisySpeed * 0.1f * StaticValues.invertY;
            }
            yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
            transform.rotation = Quaternion.Euler(yDeg, xDeg, 0);
        }
        else if (inputManager.CheckInput("Deselect/Quick Move", KeyStages.Press))
        {
            float xmov = Input.GetAxis("MouseX") * StaticValues.AxisxSpeed * 0.1f * StaticValues.invertX * mouseSentivity;
            float ymov = Input.GetAxis("MouseY") * StaticValues.AxisySpeed * 0.1f * StaticValues.invertY * mouseSentivity;
            if (Input.GetAxis(StaticValues.MouseAxisX) < -StaticValues.deadZone || Input.GetAxis(StaticValues.MouseAxisX) > StaticValues.deadZone)
            {
                xmov = Input.GetAxis(StaticValues.MouseAxisX) * StaticValues.AxisxSpeed * 0.1f * StaticValues.invertX * mouseSentivity;
            }
            if (Input.GetAxis(StaticValues.MouseAxisY) < -StaticValues.deadZone || Input.GetAxis(StaticValues.MouseAxisY) > StaticValues.deadZone)
            {
                ymov = Input.GetAxis(StaticValues.MouseAxisY) * StaticValues.AxisySpeed * 0.1f * StaticValues.invertY * mouseSentivity;
            }
            Vector3 mov = transform.TransformDirection(xmov, ymov, ymov);
            if (mov.x < 0 && transform.position.x < -10)
            {
                mov.x = 0;
            }
            if (mov.z < 0 && transform.position.z < -10)
            {
                mov.z = 0;
            }
            if (mov.x > 0 && transform.position.x > 110)
            {
                mov.x = 0;
            }
            if (mov.z > 0 && transform.position.z > 110)
            {
                mov.z = 0;
            }
            transform.Translate(mov.x, 0, mov.z, Space.World);
        }
        else
        {
            Vector3 mov = new Vector3();
            if (recdown.Contains(Input.mousePosition))
                mov.z -= CamSpeed;

            if (recup.Contains(Input.mousePosition))
                mov.z += CamSpeed;

            if (recleft.Contains(Input.mousePosition))
                mov.x -= CamSpeed;

            if (recrigh.Contains(Input.mousePosition))
                mov.x += CamSpeed;

            mov.y = mov.z;
            mov = transform.TransformDirection(mov);
            if (mov.x < 0 && transform.position.x < -10)
            {
                mov.x = 0;
            }
            if (mov.z < 0 && transform.position.z < -10)
            {
                mov.z = 0;
            }
            if (mov.x > 0 && transform.position.x > 110)
            {
                mov.x = 0;
            }
            if (mov.z > 0 && transform.position.z > 110)
            {
                mov.z = 0;
            }
            transform.Translate(mov.x, 0, mov.z, Space.World);
        }
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}