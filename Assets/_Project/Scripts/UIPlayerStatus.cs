using UnityEngine;

public class UIPlayerStatus : UIPlayerControl
{
    [SerializeField] Transform healthBar;
    [SerializeField] Transform manaBar;
    [SerializeField] float lerpSpeed = 100;

    float currentScaleHealth;
    float currentScaleMana;

    // Update is called once per frame
    void Update()
    {
        float healthNormalized = controller.CurrentHealth / controller.MaxHealth;
        float manaNormalized = controller.CurrentMana / controller.MaxMana;

        currentScaleMana = Mathf.Lerp(currentScaleMana, manaNormalized, lerpSpeed * Time.deltaTime);
        currentScaleHealth = Mathf.Lerp(currentScaleHealth, healthNormalized, lerpSpeed * Time.deltaTime);

        healthBar.transform.localScale = new Vector3(1, currentScaleHealth, 1);
        manaBar.transform.localScale = new Vector3(1, currentScaleMana, 1);
    }
}
