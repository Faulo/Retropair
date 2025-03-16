using UnityEngine;
using Ink.Runtime;
using System.Collections;
using System;

public sealed class CharacterMonologPlayer : MonoBehaviour
{
    public float lineDelaySeconds = 3.0f;

    public static event Action<string, bool> onLineChanged;
    public static event Action onMonologFinished;

    Story currentStory = default;

    Coroutine currentMonologCoroutine = default;

    bool lineAdvanceRequested = false;

    bool shouldHideHint = false;

    void Start() {
        Runtime.Player.onDialogueLineAdvanceIntent += HandleLineAdvanceIntent;
    }

    void OnDestroy() {
        Runtime.Player.onDialogueLineAdvanceIntent -= HandleLineAdvanceIntent;
    }

    public void SetMonolog(CharacterDefinition monologProvider) {
        if (currentStory == monologProvider.GetStory()) {
            return;
        }
        currentStory = monologProvider.GetStory();
    }

    public void SetHideHint(bool newShouldHideHint) {
        shouldHideHint = newShouldHideHint;
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

            string currentLine = currentStory.Continue();
            onLineChanged?.Invoke(currentLine, shouldHideHint);

            yield return new WaitUntil(() => lineAdvanceRequested);
            lineAdvanceRequested = false;
        }
        onMonologFinished?.Invoke();
    }

    void HandleLineAdvanceIntent() {
        lineAdvanceRequested = true;
    }
}
