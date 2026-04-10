using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    public StaminaController staminaController;
    public Image staminaRing;

    void Update()
    {
        if (staminaController != null && staminaRing != null)
        {
            // Calculate the percentage of stamina left
            float percent = staminaController.currentStamina / staminaController.maxStamina;
            staminaRing.fillAmount = percent;

            // Flash red if stamina is below 20%
            if (percent <= 0.2f)
            {
                staminaRing.color = Color.red;
            }
            else
            {
                staminaRing.color = Color.white;
            }
        }
    }
}