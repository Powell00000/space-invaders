using UnityEngine;
using Zenject;

public abstract class PoolableParticlesBase<SpawnContextOfT, T> : MonoBehaviour, IPoolable<SpawnContextOfT, T>
{
    [SerializeField] protected ParticleSystem particles = null;
    [SerializeField] protected ParticleSystemRenderer particlesRenderer = null;
    [SerializeField] ParticlesStoppedCallback stoppedCallback = null;

    public virtual void OnDespawned()
    {
        ClearParticles();
    }

    public virtual void OnSpawned(SpawnContextOfT context, T pool)
    {
        ConnectToParticlesCallback();
        PlayParticles();
    }

    protected void PlayParticles()
    {
        particles.Play();
    }

    protected void StopParticles()
    {
        particles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
    }

    protected void ClearParticles()
    {
        particles.Clear();
    }

    void ConnectToParticlesCallback()
    {
        if (stoppedCallback == null)
            return;
        stoppedCallback.OnParticlesStopped += Destroy;
    }

    protected virtual void Destroy()
    {
        if (stoppedCallback != null)
            stoppedCallback.OnParticlesStopped -= Destroy;
    }
}
