using UnityEngine;

public class MainUI : MonoBehaviour
{
    public Crosshair Crosshair;
    public PowerSliderScript PoweSlider;
    public RopeSliderScript RopeSlider;
    
    void Start()
    {
        GameManager.UI = this;
    }
}
