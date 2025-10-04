using UnityEngine;

public class UIPlayerHUD : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] UIPlayerControl[] UIElements;
    private void Start()
    {
        foreach (var item in UIElements)
        {
            item.Initialize(playerController);
        }
    }
}
