using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class collision : MonoBehaviour
{

    /*
    public AudioSource audio_source;
    public AudioClip collision_sound;
    public AudioClip trap_sound;
    */

    private Slider slider;
    private Data_Saver data_saver;

    public GameObject prephab;
    public ParticleSystem _particle;




    private void OnCollisionEnter(Collision col)
    {
        slider = GameObject.FindGameObjectWithTag("Lenghth_slider").GetComponent<Slider>();
        data_saver = GameObject.FindGameObjectWithTag("Data_Saver").GetComponent<Data_Saver>();


        Vector3 velocity = col.relativeVelocity;

        if (col.gameObject.CompareTag("anchor") && col.gameObject != data_saver.last_ancor)
        {

            data_saver.last_ancor = col.gameObject;

            GameObject newTire = (GameObject)Instantiate(prephab, (col.transform.position + new Vector3(0, 0, 0)), (col.transform.rotation));

            Destroy(transform.parent.gameObject);

            slider.value = 0;

  
        }

        if (col.gameObject.CompareTag("Trap"))
        {

            GameObject newTire = (GameObject)Instantiate(prephab, (data_saver.last_ancor.transform.position + new Vector3(0, 0, 0)), (data_saver.last_ancor.transform.rotation));
            Destroy(transform.parent.gameObject);
            slider.value = 0;

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

            ParticleSystem particle = Instantiate(_particle, contact.point, Quaternion.identity);

            StartCoroutine(Destroy_After_Delay(particle, 5f));
        }
    }



    private IEnumerator Destroy_After_Delay(ParticleSystem particle, float dealy)
    {

        particle.Play();

        yield return new WaitForSeconds(dealy);

        Destroy(particle.gameObject);
    }
}


