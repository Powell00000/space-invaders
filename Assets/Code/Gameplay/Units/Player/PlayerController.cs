using UnityEngine;

namespace Game.Gameplay
{
    //no pooling, we die like real men
    public class PlayerController
    {
        [Zenject.Inject] UnitDefinitionsProvider unitDefProvider = null;
        [Zenject.Inject] PlayableArea playableArea = null;

        public System.Action OnPlayerDied;

        public Vector3 PlayerPosition => spawnedPlayer.transform.position;

        Player spawnedPlayer;

        public void MaintainPlayerSpawn()
        {
            DespawnPlayer();
            spawnedPlayer = GameObject.Instantiate(unitDefProvider.Player.Prefab) as Player;

            spawnedPlayer.transform.position = playableArea.PlayerSpawnPosition;

            ConnectEvents();
        }

        void ConnectEvents()
        {
            spawnedPlayer.OnDamageReceived += PlayerReceivedDamage;
            spawnedPlayer.OnDeath += PlayerDied;
        }

        void DisconnectEvents()
        {
            spawnedPlayer.OnDamageReceived -= PlayerReceivedDamage;
            spawnedPlayer.OnDeath -= PlayerDied;
        }

        void PlayerReceivedDamage()
        {

        }

        void PlayerDied(Unit deadUnit)
        {
            DespawnPlayer();
            if (OnPlayerDied != null)
                OnPlayerDied();
        }

        public void DespawnPlayer()
        {
            if (spawnedPlayer)
            {
                DisconnectEvents();
                GameObject.Destroy(spawnedPlayer.gameObject);
            }
        }
    }
}