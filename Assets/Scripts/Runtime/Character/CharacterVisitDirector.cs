using System;
using System.Collections;
using Runtime;
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
    public static event Action<CharacterMood> onMoodChanged;

    uint currentVisitorIndex = 0;
    CharacterDefinition currentVisitorDefinition = default;
    GameObject currentVisitorObject = default;
    GameObject currentVisitorConsoleObject = default;

    bool wasDeviceGrabbed = false;

    IEnumerator Start() {
        while (true) {
            yield return new WaitForSeconds(visitorArrivalDelay);

            // next visitor arrival
            ChooseNextVisitor();
            SpawnVisitor();
            monologPlayer.SetMonolog(currentVisitorDefinition);

            // arrival monolog
            onMoodChanged?.Invoke(CharacterMood.Initial);
            yield return monologPlayer.PlayMonologBlocking(CharacterMonologSection.Arrival);

            // spawn console and trigger main monolog
            SpawnVisitorConsole();
            yield return WaitForVisitorConsolePickedUp();
            monologPlayer.PlayMonologParallel(CharacterMonologSection.Main);

            // while device not successfully returned
            while (!WasDeviceReturned() || !AreVisitorRequirementsMet()) {

                // fail monolog if device was returned, but requirements not met
                if (WasDeviceReturned()) {
                    onMoodChanged?.Invoke(CharacterMood.Deny);
                    yield return monologPlayer.PlayMonologBlocking(CharacterMonologSection.Failure);
                }
                yield return null;
            }

            // device was returned successfully
            SetInteractionWithVisitorConsoleAllowed(false);
            onMoodChanged?.Invoke(CharacterMood.Success);
            yield return monologPlayer.PlayMonologBlocking(CharacterMonologSection.Success);

            // success monolog done, cleanup scene
            DespawnVisitor();
            DespawnVisitorConsole();
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

    bool WasDeviceReturned() {
        // TODO
        return false;
    }

    bool AreVisitorRequirementsMet() {
        // TODO
        return false;
    }

    void SetInteractionWithVisitorConsoleAllowed(bool newAllowed) {
        currentVisitorConsoleObject.GetComponent<Device>().isTangible = newAllowed;
    }

    IEnumerator WaitForVisitorConsolePickedUp() {
        Player.onDeviceGrabbed += HandleDevicePickedUp;
        yield return new WaitUntil(() => wasDeviceGrabbed);
        Player.onDeviceGrabbed -= HandleDevicePickedUp;
        wasDeviceGrabbed = false;
    }

    void HandleDevicePickedUp(Device pickedUpDevice) {
        if (pickedUpDevice == currentVisitorConsoleObject.GetComponent<Device>()) {
            wasDeviceGrabbed = true;
        }
    }
}