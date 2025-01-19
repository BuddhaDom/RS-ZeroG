using UnityEngine;

public class MainUI : MonoBehaviour
{
    public Crosshair Crosshair;
    public PowerSliderScript PoweSlider;
    public RopeSliderScript RopeSlider;
    [SerializeField] private TireSwing tireSwingPrefab;
    
    void Start()
    {
        GameManager.UI = this;
    }

    public void RestartTire()
    {
        Destroy(GameManager.TireSwing.gameObject);
        Instantiate(tireSwingPrefab, GameManager.DataSaver.last_ancor.transform);
    }
}
