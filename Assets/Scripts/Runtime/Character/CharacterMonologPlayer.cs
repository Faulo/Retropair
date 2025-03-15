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

    bool isMonologPlaying = false;

    public void Start() {
        onMoodChanged?.Invoke(CharacterMood.Sad);
    }

    public void SetMonolog(CharacterDefinition monologProvider) {
        if (currentStory == monologProvider.GetStory()) {
            return;
        }
        currentStory = monologProvider.GetStory();
    }

    public void PlayMonologParallel(CharacterMonologSection section) {
        currentStory?.ChoosePathString(section.ToString());

        if (currentMonologCoroutine != null) {
            StopCoroutine(currentMonologCoroutine);
        }
        currentMonologCoroutine = StartCoroutine(PlayLines());
    }

    public IEnumerator PlayMonologBlocking(CharacterMonologSection section) {
        currentStory?.ChoosePathString(section.ToString());

        if (currentMonologCoroutine != null) {
            StopCoroutine(currentMonologCoroutine);
        }
        yield return PlayLines();
    }

    IEnumerator PlayLines() {
        while (currentStory.canContinue) {

            isMonologPlaying = true;
            string currentLine = currentStory.Continue();
            onLineChanged?.Invoke(currentLine);

            yield return new WaitForSeconds(lineDelaySeconds);
        }

        isMonologPlaying = false;
        onMonologFinished?.Invoke();
    }
}
