using UnityEngine;

public class CofeeMachineCupChecker : MonoBehaviour
{

    public bool isCupInPlace = false;

    public PaperCup paperCup;

    void Start()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.gameObject.CompareTag("Glass"))
        {
            isCupInPlace = true;
            paperCup = other.gameObject.GetComponent<PaperCup>();
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Glass"))
        {
            isCupInPlace = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Glass"))
        {
            isCupInPlace = false;
            paperCup = null;
        }
    }

}
