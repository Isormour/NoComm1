using UnityEngine;

public abstract class UIPlayerControl : MonoBehaviour
{
    protected PlayerController controller;
    protected LevelController levelController;
    public virtual void Initialize(PlayerController controller)
    {
        this.controller = controller;
        levelController = controller.GetComponent<LevelController>();
    }
}
