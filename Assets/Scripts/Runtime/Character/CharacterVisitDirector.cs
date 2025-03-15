using UnityEngine;
using UnityEngine.Events;

public class CharacterVisitDirector : MonoBehaviour
{
    public GameObject[] characterPrefabs = default;

    public GameObject spawnOrigin = default;

    public UnityEvent<CharacterMonologProvider> onCharacterMonologRequested;
    public UnityEvent<CharacterMonologSection> onCharacterMonologSectionRequested;

    uint currentlyChosenCharacter = 0;

    void Start() {
        GameObject character = Instantiate(characterPrefabs[currentlyChosenCharacter], spawnOrigin.transform);
        var monologProvider = character.GetComponent<CharacterMonologProvider>();
        onCharacterMonologRequested?.Invoke(monologProvider);
        onCharacterMonologSectionRequested?.Invoke(CharacterMonologSection.Arrival);
    }

    void ChooseNextCharacter() {
        currentlyChosenCharacter = (uint)((currentlyChosenCharacter + 1) % characterPrefabs.Length);
    }
}
