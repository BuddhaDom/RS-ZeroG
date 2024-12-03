using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RopeSliderScript : MonoBehaviour
{
    public Slider slider;
    private float savedSliderValue;

    void Start()
    {
        //slider.onValueChanged.AddListener(OnSliderValueChanged);
        savedSliderValue = slider.value;
    }

    /*
    private void OnSliderValueChanged(float value)
    {
        Debug.Log("Slider value during drag: " + value);
    }
    */

    public float GetSavedValue()
    {
        return savedSliderValue;
        // Retutn and update the main struct
    }
}
