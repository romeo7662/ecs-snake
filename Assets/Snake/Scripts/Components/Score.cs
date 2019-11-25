using Leopotam.Ecs;
using UnityEngine.UI;

namespace SnakeGame {
    sealed class Score : IEcsAutoReset {
        public int Amount;
        public Text Ui;

        void IEcsAutoReset.Reset () {
            Ui = null;
        }
    }
}