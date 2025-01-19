using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class GameManager
{
    public static UnityEvent<int> RopeLengthUpdated = new();
    public static UnityEvent<float> RopeStrengthReleased = new();

    public static MainUI UI;
    public static TireSwing TireSwing;
    public static Data_Saver DataSaver;
    
    public static void Destroy_After_Delay(ParticleSystem particle, float lifetime)
    {
        particle.Play();
        Object.Destroy(particle, lifetime);
    }
}
