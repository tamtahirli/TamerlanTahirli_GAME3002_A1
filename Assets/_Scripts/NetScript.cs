using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetScript : MonoBehaviour
{
    public ParticleSystem goalParticle;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Wow great");
        Sound.Instance.PlaySound(Sound.Instance.goal);
        CameraMovement.cam.UpdateScore(1);
        goalParticle.transform.position = collision.transform.position;
        goalParticle.Play();
    }
}
