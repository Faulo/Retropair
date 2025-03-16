using System.Collections;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime {
    public sealed class CharacterMonologLineDisplayer : MonoBehaviour {
        [SerializeField]
        TMPro.TMP_Text textMesh = default;

        [SerializeField]
        TMPro.TMP_Text buttonHintMesh = default;

        [SerializeField]
        Image buttonHintImage = default;

        [SerializeField]
        Image background = default;

        public float buttonHintDelay = 2.0f;

        public float typeSpeed = 0.05f;

        string fullText = default;

        bool isButtonHintShown = true;

        LTDescr LtDescr = default;

        void Start() {
            DisplayLine("", true);
            CharacterMonologPlayer.onLineChanged += DisplayLine;
            CharacterMonologPlayer.onTryCompleteLine += TryCompleteLine;
        }

        void OnDestroy() {
            CharacterMonologPlayer.onLineChanged -= DisplayLine;
            CharacterMonologPlayer.onTryCompleteLine -= TryCompleteLine;
        }

        void DisplayLine(string line, bool shouldHideHint) {
            background.SetAlpha(line.IsNullOrEmpty() ? 0f : 1f);
            fullText = line;

            if (LtDescr != null) {
                LeanTween.cancel(LtDescr.id);
                LtDescr = null;
            }

            int totalCharacters = fullText.Length;
            LtDescr = LeanTween
                .value(gameObject, 0, totalCharacters, totalCharacters * typeSpeed)
                .setOnUpdate((float progress) => {
                    int charCount = Mathf.Clamp(Mathf.FloorToInt(progress), 0, totalCharacters);
                    textMesh.text = fullText[..charCount];
                })
                .setEase(LeanTweenType.linear);

            if (shouldHideHint || (isButtonHintShown && line.IsNullOrEmpty())) {
                SetButtonHintShown(false);
            } else if (!isButtonHintShown) {
                StartCoroutine(ShowButtonHintWithDelay());
            }
        }

        bool TryCompleteLine() {
            if (textMesh.text == fullText) {
                return false;
            } else {
                if (LtDescr != null) {
                    LeanTween.cancel(LtDescr.id);
                    LtDescr = null;
                }

                textMesh.text = fullText;
                return true;
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
}