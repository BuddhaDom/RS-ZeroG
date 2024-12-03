using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PowerSliderScript : MonoBehaviour, IEndDragHandler
{
    public Slider slider;
    private float savedSliderValue;
    public TMP_Text uiValue;

    private void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        uiValue.SetText(slider.value.ToString());
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log(slider.value);
        savedSliderValue = slider.value;
        GameManager.RopeStrengthReleased.Invoke(slider.value);
        uiValue.SetText(slider.value.ToString());
        slider.value = 0f;
    }

    private void OnSliderValueChanged(float value)
    {
        savedSliderValue = slider.value;
        uiValue.SetText(slider.value.ToString());
    }

    public float GetSavedValue()
    {
        return savedSliderValue;
        // Retutn and update the main struct
    }
}