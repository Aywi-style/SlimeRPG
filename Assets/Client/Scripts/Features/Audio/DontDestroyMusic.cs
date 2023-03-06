using UnityEngine;

public class DontDestroyMusic : MonoBehaviour
{
    public static DontDestroyMusic Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
            return;
        }
    }
}
