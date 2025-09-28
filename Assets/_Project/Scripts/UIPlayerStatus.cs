using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerStatus : UIPlayerControl
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image manaBar;
    [SerializeField] private Image expStatus;
    [SerializeField] private TextMeshProUGUI levelText;
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

        healthBar.fillAmount = currentScaleHealth;
        manaBar.fillAmount = currentScaleMana;
        expStatus.fillAmount = levelController.LevelProgress;
        levelText.text = levelController.Level.ToString();
    }
}
