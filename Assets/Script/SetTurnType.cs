using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class SetTurnType : MonoBehaviour
{
    public ActionBasedContinuousTurnProvider continousTurn;
    public ActionBasedSnapTurnProvider snapTurn;
    public TMP_Dropdown Dropdown;
    public TMP_Text textTurn;
    public Slider Slider;
    public TMP_Text textSensitivity;

    private void Start()
    {
        if (DataManager.Instance.togglePC)
        {
            Dropdown.gameObject.SetActive(false);
            textTurn.gameObject.SetActive(false);

            Slider.gameObject.SetActive(true);
            textSensitivity.gameObject.SetActive(true);

            // Set the initial value of the slider based on the current mouse sensitivity
            Slider.value = MapSensitivityToSlider(DataManager.Instance.mouseSensitivity);

            // Add listener for slider value changes
            Slider.onValueChanged.AddListener(OnSliderValueChanged);
            return;
        }
        int playerTurnSetting = DataManager.Instance.turnMethod.ToLower().Equals("continious") ? 0 : 1;
        SetTypeFromIndem(playerTurnSetting);
    }

    public void SetTypeFromIndem(int index)
    {
        if(index == 0)
        {
            snapTurn.enabled = false;
            continousTurn.enabled = true;
            DataManager.Instance.turnMethod = "continious";
        }
        else if (index == 1) 
        {
            snapTurn.enabled = true;
            continousTurn.enabled = false;
            DataManager.Instance.turnMethod = "snap";
        }
    }
    private void OnSliderValueChanged(float value)
    {
        // Map the slider value back to the sensitivity range and update DataManager
        DataManager.Instance.mouseSensitivity = MapSliderToSensitivity(value);
    }

    private float MapSliderToSensitivity(float sliderValue)
    {
        // Map slider value (0 to 1) to sensitivity (20 to 120)
        return Mathf.Lerp(20f, 120f, sliderValue);
    }

    private float MapSensitivityToSlider(float sensitivity)
    {
        // Map sensitivity (20 to 120) to slider value (0 to 1)
        return Mathf.InverseLerp(20f, 120f, sensitivity);
    }
}
