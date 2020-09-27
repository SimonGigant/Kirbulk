using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class SoundManager : MonoBehaviour
{
    enum Level { ZERO, ONE, TWO, THREE }

    [SerializeField] Level currentLevel = Level.ZERO;

    public static SoundManager soundManager = null;

    static uint[] playingIds = new uint[50];

    private void Awake()
    {
        if (soundManager == null)
            soundManager = this;
        else if (soundManager != this)
            Destroy(gameObject);
        
        AkSoundEngine.LoadBank("Kirbulk", out uint monCul);
    }

    void Start()
    {
        AkSoundEngine.PostEvent("Play_Music", gameObject);

        switch (currentLevel)
        {
            case Level.ONE:
                AkSoundEngine.PostEvent("Play_AMB_Centre_Ville", gameObject);
                break;

            case Level.TWO:
                AkSoundEngine.PostEvent("Play_AMB_Garden_Forest", gameObject);
                break;

            case Level.THREE:
                AkSoundEngine.PostEvent("Play_AMB_Parc_Enfant", gameObject);
                break;
        }
    }

    public static void PlaySound(string eventName, GameObject go)
    {
        AkSoundEngine.PostEvent(eventName, go);
    }


    public static bool IsEventPlayingOnGameObject(string eventName, GameObject go)
    {
        uint testEventId = AkSoundEngine.GetIDFromString(eventName);

        uint count = (uint)playingIds.Length;
        AKRESULT result = AkSoundEngine.GetPlayingIDsFromGameObject(go, ref count, playingIds);

        for (int i = 0; i < count; i++)
        {
            uint playingId = playingIds[i];
            uint eventId = AkSoundEngine.GetEventIDFromPlayingID(playingId);

            if (eventId == testEventId)
                return true;
        }

        return false;
    }
}
