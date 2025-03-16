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

    bool isHoldingDevice => currentVisitorDefinition is { consoleSpawnPoint: { hasAttachedDevice: true } };
    Device currentHoldingDevice => isHoldingDevice
        ? currentVisitorDefinition.consoleSpawnPoint.attachedDevice
        : null;

    IEnumerator Start() {
        while (true) {
            yield return new WaitForSeconds(visitorArrivalDelay);

            // next visitor arrival
            ChooseNextVisitor();
            SpawnVisitor();
            monologPlayer.SetMonolog(currentVisitorDefinition);

            // arrival monolog
            onMoodChanged?.Invoke(CharacterMood.Initial);
            yield return monologPlayer.PlayMonologHighPrioBlocking(CharacterMonologSection.Arrival);

            // spawn console and trigger main monolog
            SpawnVisitorConsole();
            yield return new WaitWhile(() => isHoldingDevice);
            monologPlayer.PlayMonologLowPrioParallel(CharacterMonologSection.Main);

            // visitor waits while device not successfully returned
            while (true) {
                yield return new WaitUntil(() => isHoldingDevice);
                SetInteractionWithVisitorConsoleAllowed(false);

                // if requirements met, break out of visitor wait loop
                if (AreVisitorRequirementsMet()) {
                    break;
                }
                currentVisitorDefinition.ReportIncompletion();

                // fail monolog if device was returned, but requirements not met
                onMoodChanged?.Invoke(CharacterMood.Deny);
                yield return monologPlayer.PlayMonologHighPrioBlocking(CharacterMonologSection.Failure);

                onMoodChanged?.Invoke(CharacterMood.Initial);
                SetInteractionWithVisitorConsoleAllowed(true);
                yield return new WaitUntil(() => !isHoldingDevice);
            }

            // device was returned successfully
            onMoodChanged?.Invoke(CharacterMood.Success);
            yield return monologPlayer.PlayMonologHighPrioBlocking(CharacterMonologSection.Success);

            // success monolog done, cleanup scene
            monologPlayer.Clear();
            DespawnVisitorConsole();
            DespawnVisitor();
        }
    }

    void ChooseNextVisitor() {
        currentVisitorIndex = (uint)((currentVisitorIndex + 1) % characterPrefabs.Length);
    }

    void SpawnVisitor() {
        currentVisitorObject = Instantiate(characterPrefabs[currentVisitorIndex], characterSpawnOrigin.transform);
        currentVisitorDefinition = currentVisitorObject.GetComponent<CharacterDefinition>();
        currentVisitorDefinition.consoleSpawnPoint.gameObject.SetActive(false);
        onCharacterMonologRequested?.Invoke(currentVisitorDefinition);
    }

    void DespawnVisitor() {
        if (currentVisitorObject) {
            Destroy(currentVisitorObject);
            currentVisitorObject = null;
            currentVisitorDefinition = null;
        }
    }

    void SpawnVisitorConsole() {
        currentVisitorDefinition.consoleSpawnPoint.gameObject.SetActive(true);
    }

    void DespawnVisitorConsole() {
        currentVisitorDefinition.consoleSpawnPoint.gameObject.SetActive(false);
    }

    bool AreVisitorRequirementsMet() {
        return currentVisitorDefinition is { consoleSpawnPoint: { isCustomerComplete: true } };
    }

    void SetInteractionWithVisitorConsoleAllowed(bool newAllowed) {
        if (!isHoldingDevice) {
            Debug.LogWarning($"Can't set interaction, {this} is not holding a console!", this);
            return;
        }

        currentHoldingDevice.isTangible = newAllowed;
    }
}