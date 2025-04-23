using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VRGrabInputHandler : MonoBehaviour
{
    public InputActionProperty triggerAction; // Assign XRI RightHand/Activate
    public PowerLogic powerLogic;             // Assign the PowerLogic script manually

    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        Debug.Log("VRGrabInputHandler: Awake() - Grab Interactable found: " + (grabInteractable != null));
    }

    private void OnEnable()
    {
        Debug.Log("VRGrabInputHandler: OnEnable()");

        if (triggerAction != null && triggerAction.action != null)
        {
            triggerAction.action.Enable();
            triggerAction.action.performed += OnTriggerPressed;
            Debug.Log("VRGrabInputHandler: Trigger action enabled.");
        }
        else
        {
            Debug.LogError("VRGrabInputHandler: Trigger action is null or unassigned!");
        }

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }
    }

    private void OnDisable()
    {
        Debug.Log("VRGrabInputHandler: OnDisable()");

        if (triggerAction != null && triggerAction.action != null)
        {
            triggerAction.action.performed -= OnTriggerPressed;
            triggerAction.action.Disable();
        }

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isHeld = true;
        Debug.Log("VRGrabInputHandler: Object grabbed.");
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isHeld = false;
        Debug.Log("VRGrabInputHandler: Object released.");
    }

    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        Debug.Log("VRGrabInputHandler: Trigger pressed. IsHeld = " + isHeld);

        if (isHeld)
        {
            if (powerLogic != null)
            {
                Debug.Log("VRGrabInputHandler: Calling HandleGrabAction().");
                powerLogic.HandleGrabAction();
            }
            else
            {
                Debug.LogError("VRGrabInputHandler: PowerLogic reference not set!");
            }
        }
    }

    private void Update()
    {
        // Temp keyboard fallback for testing without VR headset
        if (isHeld && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Simulated trigger press with keyboard (E key)");
            if (powerLogic != null)
            {
                powerLogic.HandleGrabAction();
            }
        }
    }
    private void Start()
    {
        isHeld = true; // simulate cable is being held for debug
    }
}