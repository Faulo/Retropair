using UnityEngine;
using Ink.Runtime;
using System.Collections;
using System;

public sealed class CharacterMonologPlayer : MonoBehaviour
{
    public float lineDelaySeconds = 3.0f;

    public static event Action<CharacterMood> onMoodChanged;
    public static event Action<string> onLineChanged;
    public static event Action onMonologFinished;

    Story currentStory = default;

    Coroutine currentMonologCoroutine = default;

    public void Start() {
        onMoodChanged?.Invoke(CharacterMood.Sad);
    }

    public void PlayMonolog(CharacterMonologProvider monologProvider) {
        if (currentStory == monologProvider.GetStory()) {
            return;
        }
        currentStory = monologProvider.GetStory();

        if (currentMonologCoroutine != null) {
            StopCoroutine(currentMonologCoroutine);
        }
        currentMonologCoroutine = StartCoroutine(PlayLines());
    }

    public void JumpToSection(CharacterMonologSection section) {
        currentStory?.ChoosePathString(section.ToString());
    }

    IEnumerator PlayLines() {
        while (currentStory.canContinue) {

            string currentLine = currentStory.Continue();
            onLineChanged?.Invoke(currentLine);

            yield return new WaitForSeconds(lineDelaySeconds);
        }
        onMonologFinished?.Invoke();
    }
}
