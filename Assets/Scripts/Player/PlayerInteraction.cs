using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private GameObject heldObject;
    private bool isHoldingObject;

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
    private void Start()
    {
        if (panelWithText != null)
        {
            panelWithText.SetActive(false);
        }

        if (money == null)
        {
            Debug.LogError("Money component is not assigned in PlayerInteraction!");
        }
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("Untagged")) return;
            if (hit.collider.CompareTag("Glass") && !isHoldingObject)
            {
                if (interactionText != null)
                {
                    panelWithText.SetActive(true);
                    interactionText.text = "Get";
                }
                //можно ли взять стакан если кофе в процессе приготовления
                if (Input.GetKeyDown(KeyCode.E))
                {
                    heldObject = hit.collider.gameObject;

                    if (!heldObject.GetComponent<PaperCup>().isCofeeInProgress)
                    {
                        heldObject.GetComponent<Rigidbody>().isKinematic = true;
                        heldObject.GetComponent<BoxCollider>().enabled = false;
                        holdPoint.transform.position = hit.point;
                        heldObject.transform.position = holdPoint.transform.position;
                        heldObject.transform.SetParent(holdPoint.transform);
                        heldObject.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                        isHoldingObject = true;
                    }


                }
            }
            else if (hit.collider.CompareTag("Place") && isHoldingObject)
            {
                if (interactionText != null)
                {
                    panelWithText.SetActive(true);
                    interactionText.text = "Place";
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    heldObject.GetComponent<Rigidbody>().isKinematic = false;
                    heldObject.transform.SetParent(null);
                    heldObject.transform.position = hit.collider.transform.position;
                    heldObject.transform.position = new Vector3(heldObject.transform.position.x, 0.385f, heldObject.transform.position.z);
                    heldObject.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                    isHoldingObject = false;
                    Debug.Log(heldObject.tag);
                    if (heldObject.CompareTag("Glass"))
                    {
                        Debug.Log("Glass");
                        heldObject.GetComponent<BoxCollider>().enabled = true;
                    }
                    heldObject = null;
                }
            }
            else if (hit.collider.CompareTag("Cap") && !isHoldingObject)
            {
                if (interactionText != null)
                {
                    panelWithText.SetActive(true);
                    interactionText.text = "Get";
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    heldObject = hit.collider.gameObject;
                    holdPoint.transform.position = hit.point;
                    heldObject.transform.position = holdPoint.transform.position;
                    heldObject.GetComponent<BoxCollider>().enabled = true;
                    heldObject.transform.SetParent(holdPoint.transform);
                    heldObject.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                    heldObject.GetComponent<Rigidbody>().isKinematic = true;
                    heldObject.GetComponent<BoxCollider>().enabled = false;
                    isHoldingObject = true;
                }
            }
            else if (hit.collider.CompareTag("StartCoffeeButton") && !isHoldingObject)
            {
                if (interactionText != null)
                {
                    panelWithText.SetActive(true);
                    interactionText.text = "E";
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("StartCoffeeButton");
                    hit.collider.GetComponentInParent<CoffeeMachine>().ToggleState();
                    panelWithText.SetActive(false);
                }
            }
            else if (hit.collider.CompareTag("Cap") && isHoldingObject)
            {
                if (interactionText != null)
                {
                    panelWithText.SetActive(true);
                    interactionText.text = "Put the lid on";
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PaperCup paperCup = hit.collider.GetComponent<PaperCup>();

                    if (paperCup != null && !paperCup.isCofeeInProgress)
                    {
                        heldObject.transform.SetParent(paperCup.gameObject.transform);
                        heldObject.transform.position = paperCup.SnapPoint.position;
                        heldObject.transform.rotation = Quaternion.Euler(paperCup.CapRotatedPosition);
                        isHoldingObject = false;
                        panelWithText.SetActive(false);
                    }
                }
                heldObject = hit.collider.gameObject;
                holdPoint.transform.position = hit.point;
                heldObject.transform.position = holdPoint.transform.position;
                heldObject.GetComponent<BoxCollider>().enabled = true;
                heldObject.transform.SetParent(holdPoint.transform);
                heldObject.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                isHoldingObject = true;
            }
            else
            {
                if (interactionText != null)
                {
                    panelWithText.SetActive(false);
                }
            }
        }
        else
        {
            if (interactionText != null)
            {
                panelWithText.SetActive(false);
            }
        }
        ThrowLogic();
        CupStateChecker();


    }

    private void ThrowLogic()
    {
        RaycastHit hit;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);

        if (Physics.Raycast(ray, out hit, throwRayDistance))
        {
            if (hit.collider.CompareTag("Client") && isHoldingObject && heldObject.CompareTag("Glass"))
            {
                if (interactionText != null)
                {
                    panelWithText.SetActive(true);
                    interactionText.text = "Throw";
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    heldObject.transform.SetParent(null);
                    Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                    rb.isKinematic = false;

                    Vector3 throwDirection = playerCamera.transform.forward;
                    rb.AddForce(throwDirection * (throwForce / 3f), ForceMode.Impulse);

                    heldObject = null;
                    isHoldingObject = false;
                    panelWithText.SetActive(false);

                    if (money != null)
                    {
                        money.ShowMoneyText("100", hit.point);
                    }
                }
            }
        }
    }

    private void CupStateChecker()
    {
        if (heldObject != null && heldObject.CompareTag("Cap"))
        {
            if (heldObject.GetComponent<Cap>().isOnTheCup)
            {
                heldObject = null;
            }
        }
    }
}
