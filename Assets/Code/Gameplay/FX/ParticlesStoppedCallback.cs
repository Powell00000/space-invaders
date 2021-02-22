using UnityEngine;

public class ParticlesStoppedCallback : MonoBehaviour
{
    public System.Action OnParticlesStopped;

    public void OnParticleSystemStopped()
    {
        OnParticlesStopped.Invoke();
    }
}
