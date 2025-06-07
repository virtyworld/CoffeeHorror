using UnityEngine;
using System.Collections;

public class PaperCup : MonoBehaviour
{
    [SerializeField] private GameObject paperCup;
    [SerializeField] private GameObject coffee;
    [SerializeField] public Transform SnapPoint;

    private Material cupMaterial;
    private MeshRenderer coffeeMeshRenderer;
    public bool IsTransparentCap { get; private set; } = false;
    public bool IsTransparentCapEnabled { get; private set; } = false;
    public bool IsTransparentTube { get; private set; } = false;
    public bool IsTransparentTubeEnabled { get; private set; } = false;
    public Vector3 CapBasePosition { get; private set; } = new Vector3(0, 0, 0.01647285f);
    public Vector3 CapRotatedPosition { get; private set; } = new Vector3(-90, 0, 0);
    public Vector3 TubeBasePosition { get; private set; } = new Vector3(-0.08791149f, 6.94942f, 1.113803f);
    public Vector3 TubeRotatedPosition { get; private set; } = new Vector3(2.206f, 0.684f, 89.387f);
    private Vector3 initialCoffeeScale = new Vector3(0.01550759f, 0.002295122f, 0.01550758f);
    private Vector3 targetCoffeeScale = new Vector3(0.01974426f, 0.002922148f, 0.01974424f);
    private Vector3 initialCoffeePosition = new Vector3(0f, 0f, -0.00962f);
    private Vector3 targetCoffeePosition = new Vector3(0, 0f, 0.0155f);
    private float coffeeChangeDuration = 10f;
    public bool isCofeeInProgress = false;
    private bool isCoffeeDone = false;
    private BoxCollider boxCollider;

    void Start()
    {
        coffeeMeshRenderer = coffee.GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        DisableCoffee();
    }

    private void DisableCoffee()
    {
        coffee.SetActive(false);
    }
    private void EnableCoffee()
    {
        coffee.SetActive(true);
    }

    private IEnumerator ChangeCoffeeCoroutine()
    {
        if (coffee == null)
        {
            Debug.LogError("Coffee object is null!");
            yield break;
        }

        if (isCoffeeDone)
        {
            Debug.Log("Coffee change already done");
            yield break;
        }
        isCofeeInProgress = true;
        Debug.Log("Setting initial coffee state");
        coffee.transform.localScale = initialCoffeeScale;
        coffee.transform.localPosition = initialCoffeePosition;
        EnableCoffee();

        float elapsedTime = 0f;
        Debug.Log($"Starting coffee animation. Duration: {coffeeChangeDuration}");

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

        Debug.Log("Coffee animation completed");
        isCoffeeDone = true;
        isCofeeInProgress = false;
    }

    public void StartCoffeeChange()
    {
        Debug.Log("StartCoffeeChange");
        if (coffee != null && !isCoffeeDone)
        {
            Debug.Log("StartCoffeeChange2");
            StartCoroutine(ChangeCoffeeCoroutine());
        }
    }


}
