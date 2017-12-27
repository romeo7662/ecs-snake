using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.UI;

public class ScoreProcessing : IEcsSystem, IEcsInitSystem {
    [EcsWorld]
    EcsWorld _world;

    [EcsFilterInclude (typeof (Score))]
    EcsFilter _scoreFilter;

    [EcsIndex (typeof (Score))]
    int _scoreId;

    void IEcsInitSystem.Initialize () {
        foreach (var ui in GameObject.FindObjectsOfType<Text> ()) {
            var score = _world.AddComponent<Score> (_world.CreateEntity ());
            score.Amount = 0;
            score.Ui = ui;
            score.Ui.text = FormatText (score.Amount);
        }

        _world.AddEventAction<ScoreChanged> (OnScoreChanged);
    }

    string FormatText (int v) {
        return string.Format ("Score: {0}", v);
    }

    void OnScoreChanged (ScoreChanged obj) {
        foreach (var scoreEntity in _scoreFilter.Entities) {
            var score = _world.GetComponent<Score> (scoreEntity, _scoreId);
            score.Amount++;
            score.Ui.text = FormatText (score.Amount);
        }
    }

    void IEcsInitSystem.Destroy () {
        _world.RemoveEventAction<ScoreChanged> (OnScoreChanged);
    }
}