using UnityEngine;

public class MainUI : MonoBehaviour
{
    public Crosshair Crosshair;
    public PowerSliderScript PoweSlider;
    public RopeSliderScript RopeSlider;
    [SerializeField] private TireSwing tireSwingPrefab;
    public RectTransform victoryScreen, tutorialScreen;
    
    void Start()
    {
        GameManager.UI = this;
    }

    public void RestartTire()
    {
        Destroy(GameManager.TireSwing.gameObject);
        Instantiate(tireSwingPrefab, GameManager.DataSaver.last_ancor.transform);
    }

    public void RelocateMapHolder()
    {
        GameManager.MapAR.AnchorMap();
        RestartTire();
    }

    public void Victory()
    {
        victoryScreen.gameObject.SetActive(true);
    }

    public void HideTutorial()
    {
        tutorialScreen.gameObject.SetActive(false);
    }
}
