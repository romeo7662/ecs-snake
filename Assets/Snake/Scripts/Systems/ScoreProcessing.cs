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
        _world.CreateEntityWith<ScoreChangeEvent> ();
    }

    void IEcsInitSystem.Destroy () { }

    string FormatText (int v) {
        return string.Format ("Score: {0}", v);
    }

    public override EcsFilter GetReactFilter () {
        return _scoreChangeFilter;
    }

    public override EcsReactSystemType GetReactSystemType () {
        return EcsReactSystemType.OnUpdate;
    }

    public override void RunReact (int[] entities, int count) {
        // Debug.Log ("score updated " + Time.time);
        // no need to repeat update for all events - we can do it once.
        for (var i = 0; i < _scoreUiFilter.EntitiesCount; i++) {
            var score = _world.GetComponent<Score> (_scoreUiFilter.Entities[i]);
            score.Amount++;
            score.Ui.text = FormatText (score.Amount);
        }

        // and remove all received events.
        // foreach (var entity in entities) {
        //     _world.RemoveEntity (entity);
        // }
    }
}