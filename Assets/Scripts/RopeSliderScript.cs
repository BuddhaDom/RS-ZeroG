using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RopeSliderScript : MonoBehaviour
{
    public Slider slider;
    private float savedSliderValue;
    public TMP_Text uiValue;

    void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        savedSliderValue = slider.value;
        uiValue.SetText(slider.value.ToString());
        GameManager.RopeLengthUpdated.Invoke((int)slider.value);
    }

    
    private void OnSliderValueChanged(float value)
    {
        uiValue.SetText(((int)value).ToString());
        savedSliderValue = slider.value;
        GameManager.RopeLengthUpdated.Invoke((int)value);
    }
    

    public float GetSavedValue()
    {
        return savedSliderValue;
        // Retutn and update the main struct
    }
}
