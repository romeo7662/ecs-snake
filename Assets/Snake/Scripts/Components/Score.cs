using LeopotamGroup.Ecs;
using UnityEngine.UI;

sealed class Score : IEcsComponent {
    public int Amount;
    public Text Ui;
}