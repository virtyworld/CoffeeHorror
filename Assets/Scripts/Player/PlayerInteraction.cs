using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// PlayerInteraction - First-Person Interaction System
/// 
/// Main Logic:
/// This script handles all player interactions in the coffee shop game, including
/// object pickup/drop, coffee machine operation, light switching, and customer service.
/// It uses raycasting to detect interactive objects and provides visual feedback
/// through UI text. The system includes object physics manipulation, sound effects,
/// and integration with the lighting system for horror atmosphere.
/// 
/// Key Features:
/// - Raycast-based object detection and interaction
/// - Object pickup/drop system with physics
/// - Coffee machine interaction and cup management
/// - Light switch control with horror integration
/// - Customer service (throwing coffee to customers)
/// - Visual feedback through UI text
/// - Sound effect integration
/// - Money earning system
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private GameObject holdPoint;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float rayDistance = 2f;
    [SerializeField] private float throwRayDistance = 4f;
    [SerializeField] private GameObject panelWithText;
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private Money money;
    [SerializeField] private SFX sfx;
    [SerializeField] private LightSwitcher lightSwitcher;

    private GameObject heldObject;
    private bool isHoldingObject;
    private UnityEvent onPlayerSwitchingLight;

    public GameObject HeldObject
    {
        get { return heldObject; }
        set { heldObject = value; }
    }
    public bool IsHoldingObject
    {
        get { return isHoldingObject; }
        set { isHoldingObject = value; }
    }

    /// <summary>
    /// Initializes the player interaction system with light switching event
    /// Connects to the light switching system for horror scenario integration
    /// </summary>
    /// <param name="onPlayerSwitchingLight">Event triggered when player switches lights</param>
    public void Setup(UnityEvent onPlayerSwitchingLight)
    {
        this.onPlayerSwitchingLight = onPlayerSwitchingLight;
    }

    /// <summary>
    /// Initializes the interaction system and validates required components
    /// Sets up UI elements and checks for required dependencies
    /// </summary>
    private void Start()
    {
        InitializeUI();
        ValidateDependencies();
    }

    /// <summary>
    /// Main interaction loop that handles all player interactions
    /// Uses raycasting to detect interactive objects and processes input
    /// </summary>
    private void Update()
    {
        HandleMainInteraction();
        HandleThrowInteraction();
        CheckCupState();
        HandleDarknessWarning();
    }

    /// <summary>
    /// Initializes UI elements to hidden state
    /// </summary>
    private void InitializeUI()
    {
        if (panelWithText != null)
        {
            panelWithText.SetActive(false);
        }
    }

    /// <summary>
    /// Validates that required components are assigned
    /// </summary>
    private void ValidateDependencies()
    {
        if (money == null)
        {
            Debug.LogError("Money component is not assigned in PlayerInteraction!");
        }
    }

    /// <summary>
    /// Handles the main raycast interaction system
    /// </summary>
    private void HandleMainInteraction()
    {
        RaycastHit hit;
        Ray ray = CreatePlayerRay();
        DrawDebugRay(ray);

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            ProcessHitObject(hit);
        }
        else
        {
            HideInteractionUI();
        }
    }

    /// <summary>
    /// Creates a ray from player camera position forward
    /// </summary>
    /// <returns>Ray from camera position forward</returns>
    private Ray CreatePlayerRay()
    {
        return new Ray(playerCamera.transform.position, playerCamera.transform.forward);
    }

    /// <summary>
    /// Draws debug ray for visualization
    /// </summary>
    /// <param name="ray">Ray to draw</param>
    private void DrawDebugRay(Ray ray)
    {
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);
    }

    /// <summary>
    /// Processes the hit object based on its tag
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void ProcessHitObject(RaycastHit hit)
    {
        if (IsUntaggedObject(hit))
        {
            HideInteractionUI();
            return;
        }

        if (IsGlassInteraction(hit))
        {
            HandleGlassInteraction(hit);
        }
        else if (IsPlaceInteraction(hit))
        {
            HandlePlaceInteraction(hit);
        }
        else if (IsCapInteraction(hit))
        {
            HandleCapInteraction(hit);
        }
        else if (IsCoffeeButtonInteraction(hit))
        {
            HandleCoffeeButtonInteraction(hit);
        }
        else if (IsLightSwitchInteraction(hit))
        {
            HandleLightSwitchInteraction(hit);
        }
        else if (IsSurfaceInteraction(hit))
        {
            HandleSurfaceInteraction(hit);
        }
    }

    /// <summary>
    /// Checks if hit object is untagged
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    /// <returns>True if object is untagged</returns>
    private bool IsUntaggedObject(RaycastHit hit)
    {
        return hit.collider.CompareTag("Untagged");
    }

    /// <summary>
    /// Checks if this is a glass interaction
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    /// <returns>True if glass interaction</returns>
    private bool IsGlassInteraction(RaycastHit hit)
    {
        return hit.collider.CompareTag("Glass") && !isHoldingObject && lightSwitcher.IsTurnedOn;
    }

    /// <summary>
    /// Handles glass pickup interaction
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void HandleGlassInteraction(RaycastHit hit)
    {
        ShowInteractionText("E - Get");

        if (Input.GetKeyDown(KeyCode.E))
        {
            PickupGlass(hit);
        }
    }

    /// <summary>
    /// Picks up a glass object
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void PickupGlass(RaycastHit hit)
    {
        heldObject = hit.collider.gameObject;
        PaperCup paperCup = heldObject.GetComponent<PaperCup>();

        if (!paperCup.isCofeeInProgress && lightSwitcher.IsTurnedOn)
        {
            SetupHeldObject(hit.point);
            sfx.PlayPaperCollectSound();
        }
    }

    /// <summary>
    /// Checks if this is a place interaction
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    /// <returns>True if place interaction</returns>
    private bool IsPlaceInteraction(RaycastHit hit)
    {
        return hit.collider.CompareTag("Place") && isHoldingObject && lightSwitcher.IsTurnedOn;
    }

    /// <summary>
    /// Handles placing object on designated place
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void HandlePlaceInteraction(RaycastHit hit)
    {
        ShowInteractionText("E - Place");

        if (Input.GetKeyDown(KeyCode.E) && lightSwitcher.IsTurnedOn)
        {
            PlaceObject(hit);
        }
    }

    /// <summary>
    /// Places the held object on the designated place
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void PlaceObject(RaycastHit hit)
    {
        ReleaseHeldObject();
        PositionObjectOnPlace(hit);
        EnableObjectCollision();
        ResetHeldObject();
    }

    /// <summary>
    /// Checks if this is a cap interaction
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    /// <returns>True if cap interaction</returns>
    private bool IsCapInteraction(RaycastHit hit)
    {
        return hit.collider.CompareTag("Cap") && lightSwitcher.IsTurnedOn;
    }

    /// <summary>
    /// Handles cap pickup and placement interactions
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void HandleCapInteraction(RaycastHit hit)
    {
        if (!isHoldingObject)
        {
            HandleCapPickup(hit);
        }
        else
        {
            HandleCapPlacement(hit);
        }
    }

    /// <summary>
    /// Handles picking up a cap
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void HandleCapPickup(RaycastHit hit)
    {
        ShowInteractionText("E - Get");

        if (Input.GetKeyDown(KeyCode.E))
        {
            PickupCap(hit);
        }
    }

    /// <summary>
    /// Picks up a cap object
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void PickupCap(RaycastHit hit)
    {
        heldObject = hit.collider.gameObject;
        SetupHeldObject(hit.point);
        sfx.PlayPaperCollectSound();
    }

    /// <summary>
    /// Handles placing a cap on a cup
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void HandleCapPlacement(RaycastHit hit)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlaceCapOnCup(hit);
        }
    }

    /// <summary>
    /// Places a cap on a coffee cup
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void PlaceCapOnCup(RaycastHit hit)
    {
        PaperCup paperCup = hit.collider.GetComponent<PaperCup>();

        if (paperCup != null && !paperCup.isCofeeInProgress)
        {
            AttachCapToCup(paperCup);
        }
    }

    /// <summary>
    /// Attaches cap to the coffee cup
    /// </summary>
    /// <param name="paperCup">The paper cup component</param>
    private void AttachCapToCup(PaperCup paperCup)
    {
        heldObject.transform.SetParent(paperCup.gameObject.transform);
        heldObject.transform.position = paperCup.SnapPoint.position;
        heldObject.transform.rotation = Quaternion.Euler(paperCup.CapRotatedPosition);
        isHoldingObject = false;
        panelWithText.SetActive(false);
    }

    /// <summary>
    /// Checks if this is a coffee button interaction
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    /// <returns>True if coffee button interaction</returns>
    private bool IsCoffeeButtonInteraction(RaycastHit hit)
    {
        return hit.collider.CompareTag("StartCoffeeButton") && !isHoldingObject && lightSwitcher.IsTurnedOn;
    }

    /// <summary>
    /// Handles coffee machine button interaction
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void HandleCoffeeButtonInteraction(RaycastHit hit)
    {
        ShowInteractionText("E - Start Coffee");

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoffeeMachine(hit);
        }
    }

    /// <summary>
    /// Starts the coffee machine
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void StartCoffeeMachine(RaycastHit hit)
    {
        Debug.Log("StartCoffeeButton");
        hit.collider.GetComponentInParent<CoffeeMachine>().ToggleState();
        panelWithText.SetActive(false);
    }

    /// <summary>
    /// Checks if this is a light switch interaction
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    /// <returns>True if light switch interaction</returns>
    private bool IsLightSwitchInteraction(RaycastHit hit)
    {
        return hit.collider.CompareTag("Lights");
    }

    /// <summary>
    /// Handles light switch interaction
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void HandleLightSwitchInteraction(RaycastHit hit)
    {
        ShowLightSwitchText();

        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchLights();
        }
    }

    /// <summary>
    /// Shows appropriate text for light switch
    /// </summary>
    private void ShowLightSwitchText()
    {
        if (interactionText != null)
        {
            panelWithText.SetActive(true);
            if (lightSwitcher.IsTurnedOn)
            {
                interactionText.text = "Turn off the lights";
            }
            else
            {
                interactionText.text = "Turn on the lights";
            }
        }
    }

    /// <summary>
    /// Switches the lights on/off
    /// </summary>
    private void SwitchLights()
    {
        onPlayerSwitchingLight.Invoke();
        lightSwitcher.SwitchLight();
        Debug.Log("SwitchLight");
        panelWithText.SetActive(false);
    }

    /// <summary>
    /// Checks if this is a surface interaction
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    /// <returns>True if surface interaction</returns>
    private bool IsSurfaceInteraction(RaycastHit hit)
    {
        return hit.collider.CompareTag("Surface") && isHoldingObject && lightSwitcher.IsTurnedOn;
    }

    /// <summary>
    /// Handles placing object on surface
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void HandleSurfaceInteraction(RaycastHit hit)
    {
        ShowInteractionText("E - Put it down");

        if (Input.GetKeyDown(KeyCode.E))
        {
            PlaceObjectOnSurface(hit);
        }
    }

    /// <summary>
    /// Places object on surface
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void PlaceObjectOnSurface(RaycastHit hit)
    {
        ReleaseHeldObject();
        PositionObjectOnSurface(hit);
        EnableObjectCollision();
        ResetHeldObject();
    }

    /// <summary>
    /// Shows interaction text with specified message
    /// </summary>
    /// <param name="message">Text to display</param>
    private void ShowInteractionText(string message)
    {
        if (interactionText != null)
        {
            panelWithText.SetActive(true);
            interactionText.text = message;
        }
    }

    /// <summary>
    /// Hides the interaction UI
    /// </summary>
    private void HideInteractionUI()
    {
        if (interactionText != null)
        {
            panelWithText.SetActive(false);
        }
    }

    /// <summary>
    /// Sets up a held object with proper positioning and physics
    /// </summary>
    /// <param name="hitPoint">Point where object was hit</param>
    private void SetupHeldObject(Vector3 hitPoint)
    {
        holdPoint.transform.position = hitPoint;
        heldObject.transform.position = holdPoint.transform.position;
        heldObject.transform.SetParent(holdPoint.transform);
        heldObject.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        heldObject.GetComponent<Rigidbody>().isKinematic = true;
        heldObject.GetComponent<BoxCollider>().enabled = false;
        isHoldingObject = true;
    }

    /// <summary>
    /// Releases the currently held object from parent
    /// </summary>
    private void ReleaseHeldObject()
    {
        heldObject.GetComponent<Rigidbody>().isKinematic = false;
        heldObject.transform.SetParent(null);
    }

    /// <summary>
    /// Positions object on place with specific coordinates
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void PositionObjectOnPlace(RaycastHit hit)
    {
        heldObject.transform.position = hit.collider.transform.position;
        heldObject.transform.position = new Vector3(heldObject.transform.position.x, 0.385f, heldObject.transform.position.z);
        heldObject.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    }

    /// <summary>
    /// Positions object on surface with offset
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void PositionObjectOnSurface(RaycastHit hit)
    {
        heldObject.transform.position = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
        heldObject.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    }

    /// <summary>
    /// Enables collision for the held object
    /// </summary>
    private void EnableObjectCollision()
    {
        if (heldObject.CompareTag("Glass"))
        {
            Debug.Log("Glass");
            heldObject.GetComponent<BoxCollider>().enabled = true;
        }
    }

    /// <summary>
    /// Resets held object state
    /// </summary>
    private void ResetHeldObject()
    {
        isHoldingObject = false;
        heldObject = null;
    }

    /// <summary>
    /// Handles throwing interaction with customers
    /// </summary>
    private void HandleThrowInteraction()
    {
        RaycastHit hit;
        Ray ray = CreatePlayerRay();
        DrawThrowDebugRay(ray);

        if (Physics.Raycast(ray, out hit, throwRayDistance))
        {
            ProcessThrowHit(hit);
        }
        else
        {
            HideInteractionUI();
        }
    }

    /// <summary>
    /// Draws debug ray for throw interaction
    /// </summary>
    /// <param name="ray">Ray to draw</param>
    private void DrawThrowDebugRay(Ray ray)
    {
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);
    }

    /// <summary>
    /// Processes throw interaction hit
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void ProcessThrowHit(RaycastHit hit)
    {
        if (CanThrowToCustomer(hit))
        {
            HandleCustomerThrow(hit);
        }
    }

    /// <summary>
    /// Checks if can throw to customer
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    /// <returns>True if can throw to customer</returns>
    private bool CanThrowToCustomer(RaycastHit hit)
    {
        return hit.collider.CompareTag("Client") && isHoldingObject && heldObject.CompareTag("Glass");
    }

    /// <summary>
    /// Handles throwing coffee to customer
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void HandleCustomerThrow(RaycastHit hit)
    {
        ShowInteractionText("E - Throw");

        if (Input.GetKeyDown(KeyCode.E))
        {
            ThrowCoffeeToCustomer(hit);
        }
    }

    /// <summary>
    /// Throws coffee cup to customer
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void ThrowCoffeeToCustomer(RaycastHit hit)
    {
        PaperCup paperCup = heldObject.GetComponent<PaperCup>();

        if (paperCup != null && paperCup.IsCoffeeDone)
        {
            ExecuteThrow(hit);
            AwardMoney(hit);
        }
    }

    /// <summary>
    /// Executes the throw action
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void ExecuteThrow(RaycastHit hit)
    {
        heldObject.transform.SetParent(null);
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;

        Vector3 throwDirection = playerCamera.transform.forward;
        rb.AddForce(throwDirection * (throwForce / 3f), ForceMode.Impulse);

        heldObject = null;
        isHoldingObject = false;
        panelWithText.SetActive(false);
    }

    /// <summary>
    /// Awards money for successful throw
    /// </summary>
    /// <param name="hit">Raycast hit information</param>
    private void AwardMoney(RaycastHit hit)
    {
        if (money != null)
        {
            money.ShowMoneyText("100", hit.point);
            sfx.PlayCoinsSound();
        }
    }

    /// <summary>
    /// Checks the state of held caps and removes them if they're attached to cups
    /// Prevents holding caps that are already attached to coffee cups
    /// </summary>
    private void CheckCupState()
    {
        if (IsHoldingCap())
        {
            RemoveAttachedCap();
        }
    }

    /// <summary>
    /// Checks if currently holding a cap
    /// </summary>
    /// <returns>True if holding cap</returns>
    private bool IsHoldingCap()
    {
        return heldObject != null && heldObject.CompareTag("Cap");
    }

    /// <summary>
    /// Removes cap if it's attached to a cup
    /// </summary>
    private void RemoveAttachedCap()
    {
        if (heldObject.GetComponent<Cap>().isOnTheCup)
        {
            heldObject = null;
        }
    }

    /// <summary>
    /// Handles darkness warning when lights are off
    /// </summary>
    private void HandleDarknessWarning()
    {
        if (!lightSwitcher.IsTurnedOn)
        {
            ShowDarknessWarning();
        }
    }

    /// <summary>
    /// Shows warning to turn on lights
    /// </summary>
    private void ShowDarknessWarning()
    {
        if (interactionText != null)
        {
            Debug.Log("Turn on the lights");
            panelWithText.SetActive(true);
            interactionText.text = "Turn on the lights";
        }
    }

    /// <summary>
    /// Releases the currently held object when right mouse button is pressed
    /// Resets object physics and parent relationships
    /// </summary>
    private void ReleaseItem()
    {
        if (ShouldReleaseItem())
        {
            ExecuteItemRelease();
        }
    }

    /// <summary>
    /// Checks if item should be released
    /// </summary>
    /// <returns>True if should release item</returns>
    private bool ShouldReleaseItem()
    {
        return heldObject != null && Input.GetMouseButtonDown(1) && lightSwitcher.IsTurnedOn;
    }

    /// <summary>
    /// Executes item release
    /// </summary>
    private void ExecuteItemRelease()
    {
        heldObject.transform.SetParent(null);
        heldObject = null;
        panelWithText.SetActive(false);
        heldObject.GetComponent<BoxCollider>().enabled = true;
        heldObject.GetComponent<Rigidbody>().isKinematic = false;
    }
}
