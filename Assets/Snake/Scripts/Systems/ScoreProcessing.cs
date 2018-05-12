using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.UI;

[EcsInject]
public class ScoreProcessing : IEcsRunSystem, IEcsInitSystem {
    EcsWorld _world;

    EcsFilter<Score> _scoreUiFilter = null;

    EcsFilter<ScoreChangeEvent> _scoreChangeFilter = null;

    void IEcsInitSystem.Initialize () {
        foreach (var ui in GameObject.FindObjectsOfType<Text> ()) {
            var score = _world.CreateEntityWith<Score> ();
            score.Amount = 0;
            score.Ui = ui;
            score.Ui.text = FormatText (score.Amount);
        }
    }

    void IEcsInitSystem.Destroy () { }

    string FormatText (int v) {
        return string.Format ("Score: {0}", v);
    }

    void IEcsRunSystem.Run () {
        for (var i = 0; i < _scoreChangeFilter.EntitiesCount; i++) {
            var amount = _scoreChangeFilter.Components1[i].Amount;
            for (var j = 0; j < _scoreUiFilter.EntitiesCount; j++) {
                var score = _scoreUiFilter.Components1[j];
                score.Amount += amount;
                score.Ui.text = FormatText (score.Amount);
            }
            _world.RemoveEntity (_scoreChangeFilter.Entities[i]);
        }
    }
}