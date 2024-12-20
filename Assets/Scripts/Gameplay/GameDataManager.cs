using System.Collections.Generic;
using UnityEngine;

public static class GameDataManager {
    // Constantes para chaves
    public const string StageKey = "Stage";
    public const string DialogueKey = "Dialogue";
    public const string HerosJourneyKey = "HerosJourney";
    public const string ArchetypesKey = "Archetypes";

    // Inicializa as variáveis com valores padrão, se necessário
    public static void Initialize() {

        if (!PlayerPrefs.HasKey(StageKey)) {
            PlayerPrefs.SetInt(StageKey, 0);
        }

        if (!PlayerPrefs.HasKey(DialogueKey)) {
            PlayerPrefs.SetInt(DialogueKey, 0);
        }

        if (!PlayerPrefs.HasKey(HerosJourneyKey)) {
            PlayerPrefs.SetInt(HerosJourneyKey, 0);
        }

        if (!PlayerPrefs.HasKey(ArchetypesKey)) {
            PlayerPrefs.SetString(ArchetypesKey, "");
        }

        PlayerPrefs.Save();
    }

    // Métodos para Gerenciar Stage
    public static int GetStage() {
        return PlayerPrefs.GetInt(StageKey, 0);
    }

    public static void SetStage(int value) {
        PlayerPrefs.SetInt(StageKey, value);
        PlayerPrefs.Save();
    }

    // Métodos para Gerenciar Stage
    public static int GetDialogue() {
        return PlayerPrefs.GetInt(DialogueKey, 0);
    }

    public static void SetDialogue(int value) {
        PlayerPrefs.SetInt(DialogueKey, value);
        PlayerPrefs.Save();
    }

    // Métodos para Gerenciar HerosJourney
    public static int GetHerosJourney() {
        return PlayerPrefs.GetInt(HerosJourneyKey, 0);
    }

    public static void ProcessHerosJourney(string flag) {
        if (int.TryParse(flag.Substring(3), out int value)) {
            int currentHerosJourney = GetHerosJourney();
            if (value > currentHerosJourney) {
                 SetHerosJourney(value);
                Debug.Log($"Updated HerosJourney to: {value}");
            } else {
                Debug.Log($"HerosJourney ({currentHerosJourney}) is greater than or equal to: {value}. No update.");
            }
        } else {
            Debug.LogWarning($"Invalid HJ flag: {flag}");
        }
    }

    public static void SetHerosJourney(int value) {
        PlayerPrefs.SetInt(HerosJourneyKey, value);
        PlayerPrefs.Save();
    }

    // Métodos para Gerenciar Archetypes
    public static List<string> GetArchetypes() {
        string savedData = PlayerPrefs.GetString(ArchetypesKey, "");
        return new List<string>(savedData.Split(',', System.StringSplitOptions.RemoveEmptyEntries));
    }

    public static void AddArchetype(string archetype) {
        List<string> archetypes = GetArchetypes();

        if (!archetypes.Contains(archetype)) {
            archetypes.Add(archetype);
            PlayerPrefs.SetString(ArchetypesKey, string.Join(",", archetypes));
            PlayerPrefs.Save();
        }
    }
}
