using UnityEngine.Events;
using UnityEngine.UI;

public static class GameManager
{
    public static UnityEvent<int> RopeLengthUpdated = new();
    public static UnityEvent<float> RopeStrengthReleased = new();

    public static MainUI UI;
    public static TireSwing TireSwing;
    public static Data_Saver DataSaver;
}
