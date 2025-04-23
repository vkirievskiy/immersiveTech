using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class HMD_manager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private GameObject xrPlayer;
    [SerializeField] private GameObject fpsPlayer;

    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;

    void Start()
    {
        Debug.Log("Using device: " + XRSettings.loadedDeviceName);

        if (XRSettings.isDeviceActive)
        {
            fpsPlayer.SetActive(false);
            xrPlayer.SetActive(true);
        }
        else
        {
            fpsPlayer.SetActive(true);
            xrPlayer.SetActive(false);
        }
    }
}