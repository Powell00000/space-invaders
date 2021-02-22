using UnityEngine;
using Zenject;

public abstract class ParticlesPoolBase<SpawnContextOfT, T> : MonoMemoryPool<SpawnContextOfT, T> where T : Component
{

}
