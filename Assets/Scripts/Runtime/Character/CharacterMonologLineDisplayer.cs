using System.Collections;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

public sealed class CharacterMonologLineDisplayer : MonoBehaviour {
    [SerializeField]
    TMPro.TMP_Text textMesh = default;

    [SerializeField]
    TMPro.TMP_Text buttonHintMesh = default;

    [SerializeField]
    Image buttonHintImage = default;

    public float buttonHintDelay = 2.0f;

    bool isButtonHintShown = true;

    void Start() {
        DisplayLine("", true);
        CharacterMonologPlayer.onLineChanged += DisplayLine;
    }

    void OnDestroy() {
        CharacterMonologPlayer.onLineChanged -= DisplayLine;
    }

    void DisplayLine(string line, bool shouldHideHint) {
        textMesh.text = line;

        if (shouldHideHint || (isButtonHintShown && line.IsNullOrEmpty())) {
            SetButtonHintShown(false);
        } else if (!isButtonHintShown) {
            StartCoroutine(ShowButtonHintWithDelay());
        }
    }

    IEnumerator ShowButtonHintWithDelay() {
        yield return new WaitForSeconds(buttonHintDelay);
        SetButtonHintShown(true);

    }

    void SetButtonHintShown(bool shouldBecomeVisible) {
        isButtonHintShown = shouldBecomeVisible;
        buttonHintMesh.SetAlpha(shouldBecomeVisible ? 1f : 0f);
        buttonHintImage.SetAlpha(shouldBecomeVisible ? 1f : 0f);
    }
}
