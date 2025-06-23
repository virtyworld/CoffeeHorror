using UnityEngine;
using System.Collections;

/// <summary>
/// PaperCup - Coffee Cup State Management System
/// 
/// Main Logic:
/// This script manages the visual state and coffee filling animation of paper cups.
/// It controls the coffee appearance, provides snap points for caps, and handles
/// the coffee brewing animation with smooth scaling and positioning transitions.
/// The system tracks various states including coffee completion and cap attachment.
/// 
/// Key Features:
/// - Coffee filling animation with smooth transitions
/// - Snap point management for cap attachment
/// - State tracking for coffee completion
/// - Visual feedback through coffee appearance
/// - Sound effect integration during coffee brewing
/// - Configurable animation parameters
/// </summary>
public class PaperCup : MonoBehaviour
{
    [SerializeField] private GameObject paperCup;
    [SerializeField] private GameObject coffee;
    [SerializeField] public Transform SnapPoint;
    [SerializeField] private SFX sfx;

    private Material cupMaterial;
    private MeshRenderer coffeeMeshRenderer;
    public bool IsTransparentCap { get; private set; } = false;
    public bool IsTransparentCapEnabled { get; private set; } = false;
    public bool IsTransparentTube { get; private set; } = false;
    public bool IsTransparentTubeEnabled { get; private set; } = false;
    public bool IsCoffeeDone { get; private set; } = false;
    public Vector3 CapBasePosition { get; private set; } = new Vector3(0, 0, 0.01647285f);
    public Vector3 CapRotatedPosition { get; private set; } = new Vector3(-90, 0, 0);
    public Vector3 TubeBasePosition { get; private set; } = new Vector3(-0.08791149f, 6.94942f, 1.113803f);
    public Vector3 TubeRotatedPosition { get; private set; } = new Vector3(2.206f, 0.684f, 89.387f);
    private Vector3 initialCoffeeScale = new Vector3(0.01550759f, 0.002295122f, 0.01550758f);
    private Vector3 targetCoffeeScale = new Vector3(0.0185f, 0.002922148f, 0.0185f);
    private Vector3 initialCoffeePosition = new Vector3(0f, 0f, -0.00962f);
    private Vector3 targetCoffeePosition = new Vector3(0, 0f, 0.0155f);
    private float coffeeChangeDuration = 10f;
    public bool isCofeeInProgress = false;
    private BoxCollider boxCollider;

    /// <summary>
    /// Initializes the paper cup system
    /// Sets up components and starts with coffee disabled
    /// </summary>
    void Start()
    {
        coffeeMeshRenderer = coffee.GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        DisableCoffee();
    }

    /// <summary>
    /// Hides the coffee visual element
    /// Used when the cup is empty
    /// </summary>
    private void DisableCoffee()
    {
        coffee.SetActive(false);
    }

    /// <summary>
    /// Shows the coffee visual element
    /// Used when coffee brewing starts
    /// </summary>
    private void EnableCoffee()
    {
        coffee.SetActive(true);
    }

    /// <summary>
    /// Coroutine that handles the coffee filling animation
    /// Smoothly scales and positions the coffee visual from empty to full
    /// </summary>
    /// <returns>IEnumerator for coroutine execution</returns>
    private IEnumerator ChangeCoffeeCoroutine()
    {
        if (coffee == null)
        {
            Debug.LogError("Coffee object is null!");
            yield break;
        }

        if (IsCoffeeDone)
        {
            Debug.Log("Coffee change already done");
            yield break;
        }
        isCofeeInProgress = true;

        coffee.transform.localScale = initialCoffeeScale;
        coffee.transform.localPosition = initialCoffeePosition;
        EnableCoffee();

        float elapsedTime = 0f;

        while (elapsedTime < coffeeChangeDuration)
        {
            try
            {
                if (coffee == null)
                {
                    Debug.LogError("Coffee object became null during animation!");
                    yield break;
                }

                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / coffeeChangeDuration);

                coffee.transform.localScale = Vector3.Lerp(initialCoffeeScale, targetCoffeeScale, t);
                coffee.transform.localPosition = Vector3.Lerp(initialCoffeePosition, targetCoffeePosition, t);

                if (elapsedTime % 1f < Time.deltaTime)
                {
                    Debug.Log($"Coffee animation progress: {t * 100:F1}%");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error in ChangeCoffeeCoroutine: {e.Message}\n{e.StackTrace}");
                yield break;
            }

            yield return null;
        }


        IsCoffeeDone = true;
        isCofeeInProgress = false;
    }

    /// <summary>
    /// Starts the coffee brewing process
    /// Initiates the coffee filling animation and plays sound effects
    /// </summary>
    public void StartCoffeeChange()
    {
        Debug.Log("StartCoffeeChange");
        if (coffee != null && !IsCoffeeDone)
        {
            Debug.Log("StartCoffeeChange2");
            StartCoroutine(ChangeCoffeeCoroutine());
            sfx.PlayCoffeeDreepSound();
        }
    }


}
