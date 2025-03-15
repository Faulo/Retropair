using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public sealed class CharacterVisitDirector : MonoBehaviour {

    [Header("Setup")]
    [SerializeField]
    GameObject[] characterPrefabs = default;
    [SerializeField]
    float visitorArrivalDelay = 5.0f;
    [SerializeField]
    CharacterMonologPlayer monologPlayer = default;

    [Header("World Regerences")]
    public GameObject characterSpawnOrigin = default;

    [Header("Events")]
    public UnityEvent<CharacterDefinition> onCharacterMonologRequested;
    public UnityEvent<CharacterMonologSection> onCharacterMonologSectionRequested;

    uint currentVisitorIndex = 0;
    CharacterDefinition currentVisitorDefinition = default;
    GameObject currentVisitorObject = default;
    GameObject currentVisitorConsoleObject = default;

    bool isDeviceReturned = false;

    IEnumerator Start() {
        while (true) {
            // next visitor
            ChooseNextVisitor();
            yield return new WaitForSeconds(visitorArrivalDelay);
            SpawnVisitor();
            monologPlayer.SetMonolog(currentVisitorDefinition);

            // arrival monolog
            yield return monologPlayer.PlayMonologBlocking(CharacterMonologSection.Arrival);

            // spawn console and trigger main monolog
            SpawnVisitorConsole();
            monologPlayer.PlayMonologParallel(CharacterMonologSection.Main);
            yield return WaitForVisitorConsolePickedUp();

            // while device not successfully returned
            while (!isDeviceReturned || !AreVisitorRequirementsMet()) {

                // fail monolog if device was returned, but requirements not met
                if (isDeviceReturned) {
                    yield return monologPlayer.PlayMonologBlocking(CharacterMonologSection.Failure);
                }

                yield return null;
            }

            // device was returned successfully
            DisallowInteractionWithVisitorConsole();
            yield return monologPlayer.PlayMonologBlocking(CharacterMonologSection.Success);

            // success monolog done, cleanup scene
            DespawnVisitor();
            DespawnVisitorConsole();
            isDeviceReturned = false;
        }
    }

    void ChooseNextVisitor() {
        currentVisitorIndex = (uint)((currentVisitorIndex + 1) % characterPrefabs.Length);
    }

    void SpawnVisitor() {
        currentVisitorObject = Instantiate(characterPrefabs[currentVisitorIndex], characterSpawnOrigin.transform);
        currentVisitorDefinition = currentVisitorObject.GetComponent<CharacterDefinition>();
        onCharacterMonologRequested?.Invoke(currentVisitorDefinition);
    }

    void DespawnVisitor() {
        if (currentVisitorObject != null) {
            Destroy(currentVisitorObject);
        }
    }

    void SpawnVisitorConsole() {
        currentVisitorConsoleObject = Instantiate(currentVisitorDefinition.consolePrefab, currentVisitorDefinition.consoleSpawnPoint.transform);
    }

    void DespawnVisitorConsole() {
        if (currentVisitorConsoleObject != null) {
            Destroy(currentVisitorConsoleObject);
        }
    }

    bool AreVisitorRequirementsMet() {
        // TODO
        return false;
    }

    void DisallowInteractionWithVisitorConsole() {
        // TODO
    }

    IEnumerator WaitForVisitorConsolePickedUp() {
        // TODO
        // yield return null;
        yield break;
    }
}