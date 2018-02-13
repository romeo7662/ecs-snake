using System.Collections.Generic;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.UI;

public class ScoreProcessing : EcsReactSystem, IEcsInitSystem {
    [EcsWorld]
    EcsWorld _world;

    [EcsFilterInclude (typeof (Score))]
    EcsFilter _scoreUiFilter;

    [EcsFilterInclude (typeof (ScoreChangeEvent))]
    EcsFilter _scoreChangeFilter;

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

    public override EcsFilter GetReactFilter () {
        return _scoreChangeFilter;
    }

    public override EcsReactSystemType GetReactSystemType () {
        return EcsReactSystemType.OnAdd;
    }

    public override void RunReact (List<int> entities) {
        // no need to repeat update for all events - we can do it once.
        foreach (var scoreEntity in _scoreUiFilter.Entities) {
            var score = _world.GetComponent<Score> (scoreEntity);
            score.Amount++;
            score.Ui.text = FormatText (score.Amount);
        }

        // and remove all received events.
        foreach (var entity in entities) {
            _world.RemoveEntity (entity);
        }
    }
}