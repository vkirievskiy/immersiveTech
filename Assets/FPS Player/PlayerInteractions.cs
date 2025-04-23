using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerInteractions : MonoBehaviour
{
    [Header("InteractableInfo")]
    public float sphereCastRadius = 0.5f;
    public int interactableLayerIndex;
    private Vector3 raycastPos;
    public GameObject lookObject;
    private FPSGrab physicsObject;
    [SerializeField] private Camera mainCamera;

    [Header("Pickup")]
    [SerializeField] private Transform pickupParent;
    public GameObject currentlyPickedUpObject;
    public GameObject lastItemHeld;
    private Rigidbody pickupRB;

    [Header("ObjectFollow")]
    [SerializeField] private float minSpeed = 0;
    [SerializeField] private float maxSpeed = 300f;
    [SerializeField] private float maxDistance = 10f;
    private float currentSpeed = 0f;
    private float currentDist = 0f;

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    Quaternion lookRot;

    public PowerLogic powerLogic;

    void Start()
    {
        if (powerLogic == null)
            powerLogic = FindObjectOfType<PowerLogic>();
    }

    void Update()
    {
        raycastPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        Debug.DrawRay(raycastPos, mainCamera.transform.forward, Color.green);

        if (Physics.SphereCast(raycastPos, sphereCastRadius, mainCamera.transform.forward, out hit, maxDistance, 1 << interactableLayerIndex))
        {
            lookObject = hit.collider.transform.root.gameObject;
        }
        else
        {
            lookObject = null;
        }
    }

    private void FixedUpdate()
    {
        if (currentlyPickedUpObject != null)
        {
            currentDist = Vector3.Distance(pickupParent.position, pickupRB.position);
            currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, currentDist / maxDistance);
            currentSpeed *= Time.fixedDeltaTime;
            Vector3 direction = pickupParent.position - pickupRB.position;
            pickupRB.velocity = direction.normalized * currentSpeed;

            lookRot = Quaternion.LookRotation(mainCamera.transform.position - pickupRB.position);
            lookRot = Quaternion.Slerp(mainCamera.transform.rotation, lookRot, rotationSpeed * Time.fixedDeltaTime);
            pickupRB.MoveRotation(lookRot);
        }
    }

    public void BreakConnection()
    {
        lastItemHeld = currentlyPickedUpObject;
        pickupRB.constraints = RigidbodyConstraints.None;
        currentlyPickedUpObject = null;
        physicsObject.pickedUp = false;
        currentDist = 0;
    }
    public void PickUpObject()
    {
        if (lookObject == null)
        {
            Debug.LogWarning("PickUpObject: No lookObject found to pick up.");
            return;
        }

        physicsObject = lookObject.GetComponentInChildren<FPSGrab>();
        if (physicsObject == null)
        {
            Debug.LogWarning("PickUpObject: No FPSGrab script found on lookObject.");
            return;
        }

        pickupRB = lookObject.GetComponent<Rigidbody>();
        if (pickupRB == null)
        {
            Debug.LogWarning("PickUpObject: No Rigidbody found on lookObject.");
            return;
        }

        currentlyPickedUpObject = lookObject;
        pickupRB.constraints = RigidbodyConstraints.FreezeRotation;
        physicsObject.playerInteractions = this;
        StartCoroutine(physicsObject.PickUp());
    }
}