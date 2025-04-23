using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Cable : MonoBehaviour
{
    public bool moveAble = true;
    public bool isPlugged = false;
    public bool isInElectricBoxTrigger;

    [SerializeField] private float velocityThreshold = 2f;
    [SerializeField] private float snapSpeed = 10f;

    private Rigidbody rb;
    private Collider col;
    private FPSGrab fpsGrab;
    private XRGrabInteractable grabInteractable;

    private bool isSnapping = false;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        fpsGrab = GetComponent<FPSGrab>();
    }

    private void Update()
    {
        if (isSnapping)
        {
            // Smoothly move and rotate the cable towards the plug position and rotation
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * snapSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * snapSpeed);

            // Check if the cable has reached the target position
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ElectricBox"))
        {
            // Check if the cable is moving slowly enough when entering the electric box trigger
            if (rb != null && rb.velocity.magnitude < velocityThreshold)
            {
                isInElectricBoxTrigger = true;
                Debug.Log("Cable: Entered ElectricBox trigger and is moving slowly enough.");

                // Notify PowerLogic to try activating power
                PowerLogic powerLogic = FindObjectOfType<PowerLogic>();
                if (powerLogic != null)
                {
                    powerLogic.HandleGrabAction();
                }
            }
            else
            {
                Debug.Log("Cable: Too fast on trigger enter. Velocity: " + (rb != null ? rb.velocity.magnitude.ToString() : "No Rigidbody"));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ElectricBox"))
        {
            isInElectricBoxTrigger = false;
            Debug.Log("Cable: Exited ElectricBox trigger.");
        }
    }

    public void PlugInto(Vector3 plugPosition, Quaternion plugRotation)
    {
        // Stop the cable's movement and freeze it in place
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        if (col != null)
        {
            col.enabled = false;
        }

        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }

        moveAble = false;
        isPlugged = true;
        isSnapping = true;

        targetPosition = plugPosition;
        targetRotation = plugRotation;

        // ? BREAK FPSGrab CONNECTION HERE
        if (fpsGrab != null && fpsGrab.playerInteractions != null)
        {
            fpsGrab.playerInteractions.BreakConnection();
            Debug.Log("Cable: FPSGrab connection broken.");
        }

        // Optionally disable FPSGrab if needed
        if (fpsGrab != null)
        {
            fpsGrab.enabled = false;
        }

        Debug.Log("Cable: Starting plug animation.");
    }

    private void CompletePlug()
    {
        isSnapping = false;
        transform.position = targetPosition;
        transform.rotation = targetRotation;

        // Re-enable the collider now that the cable is plugged in (optional)
        if (col != null)
        {
            col.enabled = true;
        }

        Debug.Log("Cable: Plug complete and frozen.");
    }
}