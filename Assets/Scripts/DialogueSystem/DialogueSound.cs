using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using AK.Wwise;

//Sound
public enum Intonation { Regular, EndPhrase, Question, Exclamation }
public enum EffectOnSyllab { Regular, Bolded, Big, Aparte }
public enum LetterType { vowel, consonant, punctuation, space }

public class DialogueSound
{
    public static void PlaySyllab(Intonation intonation, EffectOnSyllab effect, CharacterText speaker)
    {
        string eventName = "";
        eventName += speaker.characterName.ToLower();
        if (speaker.characterName == "Lepid")
            eventName += "e";

        switch (effect)
        {
            case EffectOnSyllab.Regular:
                {
                    eventName += "_regular";
                    break;
                }
            case EffectOnSyllab.Aparte:
                {
                    eventName += "_italicized";
                    break;
                }
            case EffectOnSyllab.Big:
                {
                    eventName += "_capslocked";
                    break;
                }
            case EffectOnSyllab.Bolded:
                {
                    eventName += "_bolded";
                    break;
                }
        }

        switch (intonation)
        {
            case Intonation.EndPhrase:
                {
                    eventName += "_endphrase";
                    break;
                }
            case Intonation.Question:
                {
                    eventName += "_question";
                    break;
                }
            case Intonation.Exclamation:
                {
                    eventName += "_exclamation";
                    break;
                }
        }
        //Debug.Log(eventName);
        //AkSoundEngine.PostEvent(eventName, SoundManager.Instance.gameObject);
    }
}
