using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace SnakeGame {
    sealed class ScoreProcessing : IEcsInitSystem, IEcsDestroySystem, IEcsRunSystem {
        readonly EcsFilter<ScoreChangeEvent> _scoreChangeFilter = null;
        readonly EcsFilter<Score> _scoreUiFilter = null;
        readonly EcsWorld _world = null;

        void IEcsDestroySystem.Destroy () {
            foreach (var i in _scoreUiFilter) _scoreUiFilter.Entities[i].Destroy ();
        }

        void IEcsInitSystem.Init () {
            foreach (var ui in Object.FindObjectsOfType<Text> ()) {
                _world.NewEntityWith<Score> (out var score);
                score.Amount = 0;
                score.Ui = ui;
                score.Ui.text = FormatText (score.Amount);
            }
        }

        void IEcsRunSystem.Run () {
            foreach (var scoreChangeIdx in _scoreChangeFilter) {
                var amount = _scoreChangeFilter.Get1[scoreChangeIdx].Amount;
                foreach (var scoreUiIdx in _scoreUiFilter) {
                    var score = _scoreUiFilter.Get1[scoreUiIdx];
                    score.Amount += amount;
                    score.Ui.text = FormatText (score.Amount);
                }
            }
        }

        string FormatText (int v) {
            return $"Score: {v.ToString ()}";
        }
    }
}