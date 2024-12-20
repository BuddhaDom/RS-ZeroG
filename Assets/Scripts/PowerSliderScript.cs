using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PowerSliderScript : MonoBehaviour, IEndDragHandler
{
    public Slider slider;
    private float savedSliderValue;
    public TMP_Text uiValue;

    public GameObject fill;

    private void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        uiValue.SetText(slider.value.ToString());
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        savedSliderValue = slider.value;
        GameManager.RopeStrengthReleased.Invoke(slider.value);
        slider.value = 0f;
    }

    private void OnSliderValueChanged(float value)
    {
        savedSliderValue = slider.value;

        if (Mathf.Approximately(value, 0f))
        {
            uiValue.SetText("0");
        }
        else
        {
            uiValue.SetText(slider.value.ToString("F2"));
        }

        fill.GetComponent<Image>().color = UpdatePowerColor(savedSliderValue);
    }

    private Color UpdatePowerColor(float value)
    {
        float normalizedValue = (value - slider.minValue) / (slider.maxValue - slider.minValue);

        if (normalizedValue <= 0.25f)
        {
        // Transition from White to Green (0-0.25)
            return Color.Lerp(Color.white, Color.green, normalizedValue * 4);
        }
        else if (normalizedValue <= 0.5f)
        {
        // Transition from Green to Yellow (0.25-0.5)
            return Color.Lerp(Color.green, Color.yellow, (normalizedValue - 0.25f) * 4);
        }
        else if (normalizedValue <= 0.75f)
        {
        // Transition from Yellow to Orange (0.5-0.75)
            return Color.Lerp(Color.yellow, new Color(1f, 0.647f, 0f), (normalizedValue - 0.5f) * 4);
        }
        else
        {
        // Transition from Orange to Red (0.75-1.0)
        return Color.Lerp(new Color(1f, 0.647f, 0f), Color.red, (normalizedValue - 0.75f) * 4);
        }
    }

    public float GetSavedValue()
    {
        return savedSliderValue;
        // Retutn and update the main struct
    }
}