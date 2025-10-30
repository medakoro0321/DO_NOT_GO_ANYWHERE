using UdonSharp;
using UnityEngine;
using DoNotGoAnyWhere.Interfaces;
using DoNotGoAnyWhere.Systems;

namespace DoNotGoAnyWhere.Interact
{
    public class StartButton : UdonSharpBehaviour
    {
        private GameSystem _gameSystem;

        private void Start()
        {
            _gameSystem = Components.GetComponent<GameSystem>("GameSystem");
        }

        public override void Interact()
        {
            Debug.Log("<color=red>Interacted</color>");
            _gameSystem.ReadyGame();
        }
    }
}