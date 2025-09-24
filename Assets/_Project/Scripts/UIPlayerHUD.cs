using UnityEngine;

public class UIPlayerHUD : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Transform healthBar;
    [SerializeField] Transform manaBar;

    float currentScaleHealth;
    float currentScaleMana;

    [SerializeField] float lerpSpeed = 100;
    // Update is called once per frame
    void Update()
    {
        float healthNormalized = playerController.CurrentHealth / playerController.MaxHealth;
        float manaNormalized = playerController.CurrentMana / playerController.MaxMana;

        currentScaleMana = Mathf.Lerp(currentScaleMana, manaNormalized, lerpSpeed * Time.deltaTime);
        currentScaleHealth = Mathf.Lerp(currentScaleHealth, healthNormalized, lerpSpeed * Time.deltaTime);

        healthBar.transform.localScale = new Vector3(1, currentScaleHealth, 1);
        manaBar.transform.localScale = new Vector3(1, currentScaleMana, 1);
    }
}
