using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

using UnityEngine.UI;

public class collision : MonoBehaviour
{
    private Slider slider;
    private Data_Saver data_saver;

    public GameObject prephab;
    public ParticleSystem _particle;
    
    private void OnCollisionEnter(Collision col)
    {
        slider = GameManager.UI.RopeSlider.slider;
        data_saver = GameManager.DataSaver;

        Vector3 velocity = col.relativeVelocity;

        var swingAnchor = col.gameObject.GetComponentInParent<SwingAnchor>();
        
        if (col.gameObject.CompareTag("anchor") && swingAnchor != data_saver.last_ancor)
        {
            if (swingAnchor == null) return;
            
            data_saver.last_ancor = swingAnchor;

            Instantiate(prephab, swingAnchor.transform);
            
            Destroy(transform.parent.gameObject);

            slider.value = 1;
        }

        if (col.gameObject.CompareTag("Trap"))
        {
            Instantiate(prephab, data_saver.last_ancor.transform);
            Destroy(transform.parent.gameObject);
            slider.value = 1;
        }

        if (!col.gameObject.CompareTag("Rope") && (velocity.x > 0.1 || velocity.y > 0.1 || velocity.z > 0.1))
        {
            if (col.gameObject.CompareTag("Trap"))
            {
                data_saver.audio_source.clip = data_saver.trap_sound;
                data_saver.audio_source.Play();
            } else
            {
                data_saver.audio_source.clip = data_saver.collision_sound;
                data_saver.audio_source.Play();
            }

            ContactPoint contact = col.contacts[0];

            GameManager.Destroy_After_Delay(Instantiate(_particle, contact.point, Quaternion.identity), 2.5f);
        }
    }
}


