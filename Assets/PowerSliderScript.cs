using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PowerSliderScript : MonoBehaviour, IEndDragHandler
{

    public Slider slider;
    private float savedSliderValue;
    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log(slider.value);
        savedSliderValue = slider.value;
        slider.value = 0f;
    }

    public float GetSavedValue()
    {
        return savedSliderValue;
        // Retutn and update the main struct
    }
}