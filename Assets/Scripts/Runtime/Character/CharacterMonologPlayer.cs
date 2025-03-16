using UnityEngine;
using Ink.Runtime;
using System.Collections;
using System;

public sealed class CharacterMonologPlayer : MonoBehaviour
{
    public float lineDelaySeconds = 3.0f;

    public static event Action<string, bool> onLineChanged;
    public static event Func<bool> onTryCompleteLine;
    public static event Action onMonologFinished;

    Story currentStory = default;
    Story currentHighPrioStory = default;

    Coroutine currentMonologCoroutine = default;

    bool lineAdvanceRequested = false;

    bool shouldHideHint = false;

    bool isHighPrioMonologPlaying = false;

    void Start() {
        Runtime.Player.onDialogueLineAdvanceIntent += HandleLineAdvanceIntent;
    }

    void OnDestroy() {
        Runtime.Player.onDialogueLineAdvanceIntent -= HandleLineAdvanceIntent;
    }

    public void Clear() {
        currentStory = currentHighPrioStory = null;
        StopCoroutine(currentMonologCoroutine);
        lineAdvanceRequested = false;
        onLineChanged?.Invoke("", true);
    }

    public void SetMonolog(CharacterDefinition monologProvider) {
        if (currentStory == monologProvider.GetStory()) {
            return;
        }
        currentStory = monologProvider.GetStory();
        currentHighPrioStory = monologProvider.GetHighPrioStory();
    }

    public void SetHideHint(bool newShouldHideHint) {
        shouldHideHint = newShouldHideHint;
    }

    public void PlayMonologLowPrioParallel(CharacterMonologSection section) {
        currentStory?.ChoosePathString(section.ToString());

        if (currentMonologCoroutine != null) {
            StopCoroutine(currentMonologCoroutine);
        }
        currentMonologCoroutine = StartCoroutine(PlayLines(false));
    }

    public IEnumerator PlayMonologHighPrioBlocking(CharacterMonologSection section) {
        currentHighPrioStory?.ChoosePathString(section.ToString());
        isHighPrioMonologPlaying = true;
        yield return PlayLines(true);
        isHighPrioMonologPlaying = false;
    }

    IEnumerator PlayLines(bool isHighPrioMonolog) {
        Story story = isHighPrioMonolog ? currentHighPrioStory : currentStory;

        while (story.canContinue) {
            if (!isHighPrioMonolog && isHighPrioMonologPlaying) {
                yield return new WaitUntil(() => !isHighPrioMonologPlaying);
            }

            string currentLine = story.Continue();
            onLineChanged?.Invoke(currentLine, shouldHideHint);

            yield return new WaitUntil(() => lineAdvanceRequested);
            lineAdvanceRequested = false;
        }
        onMonologFinished?.Invoke();
    }

    void HandleLineAdvanceIntent() {
        if (!onTryCompleteLine.Invoke()) {
            lineAdvanceRequested = true;
        }
    }
}
