using UnityEngine;
using UnityEngine.Events;

public sealed class CharacterMonologPlayer : MonoBehaviour
{
    public UnityEvent<CharacterMood> onMoodChanged;

    public UnityEvent<string> onLineChanged;

    public UnityEvent onMonologFinished;

    int currentInkStory = 0;

    public void Start() {
        onMoodChanged?.Invoke(CharacterMood.Sad);
    }

    public void PlayMonolog(int inkStory) {
        if (currentInkStory == inkStory) {
            return;
        }
    }
}
