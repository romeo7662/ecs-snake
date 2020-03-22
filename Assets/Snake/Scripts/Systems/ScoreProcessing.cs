using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace SnakeGame {
    public class ScoreProcessing : IEcsInitSystem, IEcsDestroySystem, IEcsRunSystem {
        readonly EcsWorld _world = null;
        readonly EcsFilter<Score> _scoreUiFilter = null;
        readonly EcsFilter<ScoreChangeEvent> _scoreChangeFilter = null;

        void IEcsInitSystem.Init () {
            foreach (var ui in Object.FindObjectsOfType<Text> ()) {
                ref var score = ref _world.NewEntity ().Set<Score> ();
                score.Amount = 0;
                score.Ui = ui;
                score.Ui.text = FormatText (score.Amount);
            }
        }

        void IEcsDestroySystem.Destroy () {
            foreach (var i in _scoreUiFilter) {
                _scoreUiFilter.GetEntity (i).Destroy ();
            }
        }

        string FormatText (int v) {
            return $"Score: {v}";
        }

        void IEcsRunSystem.Run () {
            foreach (var scoreChangeIdx in _scoreChangeFilter) {
                ref var amount = ref _scoreChangeFilter.Get1 (scoreChangeIdx).Amount;
                foreach (var scoreUiIdx in _scoreUiFilter) {
                    ref var score = ref _scoreUiFilter.Get1 (scoreUiIdx);
                    score.Amount += amount;
                    score.Ui.text = FormatText (score.Amount);
                }
            }
        }
    }
}