using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime {
    sealed class HoverText : MonoBehaviour {
        [SerializeField]
        DeviceSelector selector;
        [SerializeField]
        Player player;
        [SerializeField]
        RectTransform container;
        [SerializeField]
        TMP_Text infoBox;
        [SerializeField]
        TMP_Text rightClickBox;
        [SerializeField]
        TMP_Text leftClickBox;

        void LateUpdate() {
            if (Pointer.current is { position: { value: Vector2 position } }) {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(container.parent as RectTransform, position, default, out var rectPosition)) {
                    container.anchoredPosition = rectPosition;
                }
            }

            string infoText = GetInfoText();
            string rightClickText = GetRightClickText();
            string leftClickText = GetLeftClickText();

            infoBox.text = infoText;
            infoBox.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(infoText));

            rightClickBox.text = rightClickText;
            rightClickBox.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(rightClickText) && rightClickText != leftClickText);

            leftClickBox.text = leftClickText;
            leftClickBox.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(leftClickText));
        }

        string GetInfoText() {
            return string.Empty;
        }

        string GetLeftClickText() {
            if (player.heldDevice) {
                return selector
                    .selectedSlots
                    .Where(s => s.CanFit(player.heldDevice))
                    .Select(s => s.displayName)
                    .FirstOrDefault();
            }

            return selector.outerSelection
                ? selector.outerSelection.displayName
                : string.Empty;
        }

        string GetRightClickText() {
            if (player.heldDevice) {
                return string.Empty;
            }

            return selector.innerSelection
                ? selector.innerSelection.displayName
                : string.Empty;
        }
    }
}