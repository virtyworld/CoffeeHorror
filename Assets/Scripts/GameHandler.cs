using UnityEngine;
using UnityEngine.Events;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private LightSwitcher lightSwitcher;
    [SerializeField] private GuestReplacer guestReplacer;
    [SerializeField] private PlayerInteraction playerInteraction;
    private UnityEvent onPlayerSwitchingLight;
    private UnityEvent onTurnOffLight;
    private UnityEvent onTurnAllLightsRed;
    private UnityEvent onTurnAllLightsWhite;
    void Awake()
    {
        InitEvents();
        InitLightSwitcher();
        InitGuestReplacer();
        InitPlayerInteraction();
    }

    private void InitEvents()
    {
        Debug.Log("InitEvents");
        onPlayerSwitchingLight = new UnityEvent();
        onTurnOffLight = new UnityEvent();
        onTurnAllLightsRed = new UnityEvent();
        onTurnAllLightsWhite = new UnityEvent();
    }

    private void InitLightSwitcher()
    {
        lightSwitcher.Setup(onTurnOffLight, onTurnAllLightsRed, onTurnAllLightsWhite);
    }

    private void InitGuestReplacer()
    {
        guestReplacer.Setup(onPlayerSwitchingLight, onTurnOffLight, onTurnAllLightsRed, onTurnAllLightsWhite);
    }

    private void InitPlayerInteraction()
    {
        playerInteraction.Setup(onPlayerSwitchingLight);
    }

}
