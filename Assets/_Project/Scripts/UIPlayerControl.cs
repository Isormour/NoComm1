using UnityEngine;

public abstract class UIPlayerControl : MonoBehaviour
{
    protected PlayerController controller;
    public virtual void Initialize(PlayerController controller)
    {
        this.controller = controller;
    }
}
