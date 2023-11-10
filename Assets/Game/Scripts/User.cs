using UnityEngine;

public static class User {
    private const string NAME_KEY = "9bb11fb473874d04ad052302c7627965";
    private const string VFX_KEY = "317fe3ef55e84e3f96d320719e19b521";

    public static event System.Action<string> OnNameChange = delegate { };
    public static event System.Action<int> OnVFXChange = delegate { };

    public static string Name {
        get => PlayerPrefs.GetString(NAME_KEY, string.Empty);
        set { PlayerPrefs.SetString(NAME_KEY, value); OnNameChange(value); }
    }

    public static int VFX {
        get => PlayerPrefs.GetInt(VFX_KEY, 0);
        set { PlayerPrefs.SetInt(VFX_KEY, value); OnVFXChange(value); }
    }
}