using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CharTween;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(UIShake))]
public class DialogueManager : MonoBehaviour
{
    public DialogueText thisDialogue;


    [Header("Characteristics")]
    [SerializeField] private float delayBetweenLetters = 1.0f;
    [SerializeField] private bool autoPass = true;
    [SerializeField] private float autoPassDuration = 3.0f;
    [SerializeField] private float portraitShakeIntensity = 1.0f;


    private List<DialogueData> dialogueData;
    private bool textIsPrinted = false;
    private float counter = 0.0f;
    private CharTweener _tweener;
    private Sequence _sequence;
    private Sequence _sequencePortrait;
    private bool isTwinkling = false;
    private Coroutine _twinkle;
    private float scalePortrait;

    [Header("Fields")]
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text speakerName;
    [SerializeField] private Image portrait;
    [SerializeField] private Image endDialogueSign;
    [SerializeField] private RectTransform leftAnchor;
    [SerializeField] private RectTransform rightAnchor;
    [SerializeField] private Material shaderNormal;
    private List<Intonation> intonations;
    private List<EffectOnSyllab> effectOnSyllabs;

    public string SceneAfterDialog = null;
    public bool inJojo = false;
    public Animator jojoAnim;


    private void Start()
    {
        if(GameManager.instance.marave != null)
            GameManager.instance.marave.state = MaraveState.Cutscene;
        GetComponentInParent<Canvas>().worldCamera = Camera.main;
        scalePortrait = portrait.rectTransform.localScale.x;
        //GameManager.Instance.dialogueManager = this;
        StartCoroutine(TimerBeforeBegin(1.6f));
    }

    public void RemoveListeners()
    {
        /*
        if (GameManager._instance != null)
        {
            GameManager.Instance.dialogueManager = null;
            if (GameManager.Instance.lepide != null)
            {
                GameManager.Instance.lepide.actionMap["PassDialogue"].started -= PassAction;
            }
        }*/
    }

    private void OnDestroy()
    {
        /*if(GameManager._instance != null)
        {
            GameManager.Instance.dialogueManager = null;
            if (GameManager.Instance.lepide != null)
            {
                GameManager.Instance.lepide.actionMap["PassDialogue"].started -= PassAction;
            }
        }
            */
    }

    private IEnumerator TimerBeforeBegin(float duration)
    {
        transform.DOLocalMoveY(-1000f, 1.8f).From().SetEase(Ease.InOutQuad);
        StartDialogue(thisDialogue);
        yield return new WaitForSeconds(duration);
        UpdateDisplay();
    }

    private IEnumerator TimerBeforeEnd(float duration)
    {
        /*
        if (GameManager.Instance.lepide != null)
            GameManager.Instance.lepide.state = PlayerState.Idle;*/
        transform.DOLocalMoveY(-1000f, 1.8f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(duration);
        if (inJojo)
        {
            jojoAnim.SetBool("ActiveScene", true);
            yield return new WaitForSeconds(15f);
            if (SceneAfterDialog != null && !SceneAfterDialog.Equals(""))
                GameManager.instance.LoadLevel(SceneAfterDialog);
        }
        else
        {
            if (SceneAfterDialog != null && !SceneAfterDialog.Equals(""))
                GameManager.instance.LoadLevel(SceneAfterDialog);

            if(GameManager.instance.marave != null)
                GameManager.instance.marave.state = MaraveState.Idle;
        }
        Destroy(transform.parent.gameObject);
    }

    private void Update()
    {
        counter += Time.deltaTime;
        if (textIsPrinted && !isTwinkling)
        {
            _twinkle = StartCoroutine(Twinkle());
        }
        if (textIsPrinted && autoPass && counter > autoPassDuration)
        {
            PassDialogue();
        }
    }

    private DialogueData TranslateFX(DialogueData data)
    {
        DialogueData res = new DialogueData(data)
        {
            emphasis = new List<bool>(),
            size = new List<TextSize>(),
            effect = new List<TextEffect>(),
            speed = new List<TextSpeed>()
        };
        string thisText = "";
        bool isEmphased = false;
        TextSize currentSize = TextSize.Normal;
        TextEffect currentEffect = TextEffect.None;
        TextSpeed currentSpeed = TextSpeed.Normal;
        TextSpeed previousSpeed = TextSpeed.Normal;
        bool isPause = false;
        foreach(char c in res.text)
        {
            if (!isPause && currentSpeed == TextSpeed.Slow)
            {
                currentSpeed = previousSpeed;
            }
            switch (c)
            {
                case '*':
                    {
                        isEmphased = !isEmphased;
                        if (isEmphased)
                        {
                            thisText += "<b>";
                        }
                        else
                        {
                            thisText += "</b>";
                        }
                        continue;
                    }
                case '_':
                    {
                        currentSize = currentSize == TextSize.Small ? TextSize.Normal: TextSize.Small;
                        continue;
                    }
                case '^':
                    {
                        currentSize = currentSize == TextSize.Big ? TextSize.Normal : TextSize.Big;
                        continue;
                    }
                case '~':
                    {
                        currentEffect = currentEffect == TextEffect.Wave ? TextEffect.None : TextEffect.Wave;
                        continue;
                    }
                case '°':
                    {
                        currentEffect = currentEffect == TextEffect.Spooky ? TextEffect.None : TextEffect.Spooky;
                        continue;
                    }
                case '>':
                    {
                        currentSpeed = currentSpeed == TextSpeed.Normal ? TextSpeed.Fast : TextSpeed.Faster;
                        continue;
                    }
                case '<':
                    {
                        currentSpeed = currentSpeed == TextSpeed.Faster ? TextSpeed.Fast : TextSpeed.Normal;
                        continue;
                    }
                case '|':
                    {
                        previousSpeed = currentSpeed == TextSpeed.Slow ? TextSpeed.Normal : currentSpeed;
                        currentSpeed = TextSpeed.Slow;
                        isPause = true;
                        continue;
                    }
            }
            thisText += c;
            res.emphasis.Add(isEmphased);
            res.size.Add(currentSize);
            res.effect.Add(currentEffect);
            res.speed.Add(currentSpeed);
            isPause = false;
        }
        res.text = thisText;
        return res;
    }

    public void StartDialogue(DialogueText d)
    {
        if (d == null)
            return;
        dialogueData = new List<DialogueData>();
        foreach(DialogueData data in d.dialogues)
        {
            dialogueData.Add(TranslateFX(data));
        }
        portrait.sprite = dialogueData[0].portrait;
        speakerName.text = dialogueData[0].name;
        Material matName = speakerName.fontMaterial;
        matName.SetColor(ShaderUtilities.ID_UnderlayColor, dialogueData[0].character.color);
        speakerName.fontMaterial = matName;

        
        portrait.material = shaderNormal;

        if (dialogueData[0].leftSide)
        {
            portrait.transform.SetParent(leftAnchor, false);
        }
        else
        {
            portrait.transform.SetParent(rightAnchor, false);
        }
        if (dialogueData[0].mirror)
        {
            portrait.transform.localScale = new Vector2(-scalePortrait, scalePortrait);
        }
        else
        {
            portrait.transform.localScale = new Vector2(scalePortrait, scalePortrait);
        }
        text.SetText("");
    }

    private void PassDialogue()
    {
        if(textIsPrinted && dialogueData.Count > 0)
        {
            StopCoroutine(_twinkle);
            isTwinkling = false;
            endDialogueSign.enabled = false;
            dialogueData.RemoveAt(0);
            UpdateDisplay();
        }
        else if (!textIsPrinted)
        {
            _sequence.Complete();
            _sequencePortrait.Complete();
        }
    }

    private void PassAction(InputAction.CallbackContext ctx)
    {
            PassDialogue();
    }

    private IEnumerator Twinkle()
    {
        isTwinkling = true;
        for(; ; )
        {
            yield return new WaitForSeconds(0.5f);
            endDialogueSign.enabled = !endDialogueSign.enabled;
        }
    }

    private void UpdateDisplay()
    {
        if (dialogueData.Count <= 0)
        {
            StartCoroutine(TimerBeforeEnd(1.5f));
            return;
        }
        text.SetText(dialogueData[0].text);
        speakerName.text = dialogueData[0].name;
        portrait.sprite = dialogueData[0].portrait;
        Material matName = speakerName.fontMaterial;
        matName.SetColor(ShaderUtilities.ID_UnderlayColor, dialogueData[0].character.color);
        speakerName.fontMaterial = matName;
            portrait.material = shaderNormal;
        if (dialogueData[0].leftSide)
        {
            portrait.transform.SetParent(leftAnchor, false);
        }
        else
        {
            portrait.transform.SetParent(rightAnchor, false);
        }
        if (dialogueData[0].mirror)
        {
            portrait.transform.localScale = new Vector2(-scalePortrait, scalePortrait);
        }
        else
        {
            portrait.transform.localScale = new Vector2(scalePortrait, scalePortrait);
        }
        textIsPrinted = false;
        text.ForceMeshUpdate();
        FxShowTextCharByChar(text);
        text.ForceMeshUpdate();
    }

    private void Shake()
    {
        GetComponent<UIShake>().Shake();
    }

    private void FinishPrint()
    {
        textIsPrinted = true;
        counter = 0.0f;
    }

    private void FxShowTextCharByChar(TMP_Text text)
    {
        _sequence = DOTween.Sequence().SetAutoKill(true);
        _sequencePortrait = DOTween.Sequence().SetAutoKill(true);
        RectTransform portraitAnchor = portrait.GetComponent<RectTransform>();
        _tweener = text.GetCharTweener();
        int start = 0;
        int end = text.GetParsedText().Length-1;
        float offsetCumul = 0.0f;
        Color grey = new Color(0.4f, 0.4f, 0.4f);
        int numLetterSyllabe = 0;
        char previousChar = '\0';
        LetterType prevLetterType = LetterType.space;
        bool alreadyChangedLetterType = false;
        intonations = new List<Intonation>();
        effectOnSyllabs = new List<EffectOnSyllab>();


        for (int i = start; i <= end; ++i)
        {
            numLetterSyllabe++;
            float currentOffset = delayBetweenLetters * 0.05f;
            float speedMultiplier = 1f;
            Sequence portraitSequence = DOTween.Sequence().SetAutoKill(true);
            Sequence charSequence = DOTween.Sequence().SetAutoKill(true);
            bool normalMovePortrait = true;
            char thisChar = text.GetParsedText()[i];
            UnityEvent onComplete = new UnityEvent();
            switch (dialogueData[0].speed[i])
            {
                case TextSpeed.Slow:
                    {
                        speedMultiplier *= 10f;
                        break;
                    }
                case TextSpeed.Fast:
                    {
                        speedMultiplier *= 0.5f;
                        break;
                    }
                case TextSpeed.Faster:
                    {
                        speedMultiplier *= 0.2f;
                        break;
                    }
            }
            currentOffset *= speedMultiplier;



            if (i!=0)
                previousChar = text.GetParsedText()[i - 1];

            prevLetterType = GetType(previousChar);
            LetterType currentLetterType = GetType(thisChar);

            if ((prevLetterType == LetterType.space || prevLetterType == LetterType.punctuation) && (currentLetterType == LetterType.vowel || currentLetterType == LetterType.consonant))
            {
                numLetterSyllabe = 1;
                alreadyChangedLetterType = false;
            }
            else if(currentLetterType != LetterType.punctuation && currentLetterType != LetterType.space)
            {
                bool letterTypeDifferent = (prevLetterType == LetterType.vowel && currentLetterType == LetterType.consonant) || (prevLetterType == LetterType.consonant && currentLetterType == LetterType.vowel);
                if (letterTypeDifferent)
                {
                    if (alreadyChangedLetterType)
                    {
                        numLetterSyllabe = 1;
                        alreadyChangedLetterType = false;
                    }
                    else
                    {
                        alreadyChangedLetterType = true;
                    }
                }
                else if(currentLetterType == LetterType.vowel && numLetterSyllabe >= 3 && !alreadyChangedLetterType)
                {
                    numLetterSyllabe = 1;
                    alreadyChangedLetterType = false;
                }
                else if(currentLetterType == LetterType.vowel && alreadyChangedLetterType && (numLetterSyllabe > 1 && previousChar != thisChar))
                {
                    numLetterSyllabe = 1;
                    alreadyChangedLetterType = false;
                }
            }



            if (numLetterSyllabe == 1 && currentLetterType != LetterType.punctuation && currentLetterType != LetterType.space)
            {


                Intonation intonation = Intonation.Regular;
                EffectOnSyllab effect = EffectOnSyllab.Regular;
                char nextChar = CheckNextSyllableFirstChar(i);
                LetterType typeNextChar = GetType(nextChar);
                if (typeNextChar == LetterType.punctuation)
                {
                    if (nextChar == '?')
                    {
                        intonation = Intonation.Question;
                    }
                    else if (nextChar == '!')
                    {
                        intonation = Intonation.Exclamation;
                    }
                    else
                    {
                        intonation = Intonation.EndPhrase;
                    }
                }

                if (dialogueData[0].emphasis[i])
                {
                    effect = EffectOnSyllab.Bolded;
                }
                else if (dialogueData[0].size[i] == TextSize.Big)
                {
                    effect = EffectOnSyllab.Big;
                }
                else if (dialogueData[0].size[i] == TextSize.Small)
                {
                    effect = EffectOnSyllab.Aparte;
                }

                intonations.Add(intonation);
                effectOnSyllabs.Add(effect);
                onComplete.AddListener(BeginSyllable);
            }

            switch (dialogueData[0].size[i])
            {
                case TextSize.Small:
                    {
                        charSequence.Append(_tweener.DOLocalMoveY(i, 6f, 0.15f * speedMultiplier).From().SetEase(Ease.OutBounce)).OnPlay(onComplete.Invoke).Join(_tweener.DOScaleY(i, 0.65f, 0f)).Join(_tweener.DOScaleX(i, 0.65f, 0f)).Join(_tweener.DOFade(i, 0, 0.02f * speedMultiplier).From().SetEase(Ease.OutQuad)).Join(_tweener.DOColor(i, grey, 0.01f * speedMultiplier)).Join(_tweener.DOLocalMoveX(i, Random.Range(-1f, 1f), 0.45f * speedMultiplier).From().SetEase(Ease.OutBounce));
                        currentOffset *= 1.4f;
                        break;
                    }
                case TextSize.Big:
                    {
                        charSequence.Append(_tweener.DOLocalMoveY(i, 16f, 0.15f * speedMultiplier).From().SetEase(Ease.OutBounce)).OnPlay(onComplete.Invoke).Join(_tweener.DOScaleY(i, 1.5f, 0f)).Join(_tweener.DOScaleX(i, 1.2f, 0f)).Join(_tweener.DOFade(i, 0, 0.01f * speedMultiplier).From().SetEase(Ease.OutQuad)).Join(_tweener.DOColor(i, Color.black, 0.01f * speedMultiplier)).Join(_tweener.DOLocalMoveX(i, Random.Range(-1f, 1f), 0.45f * speedMultiplier).From().SetEase(Ease.OutBounce));
                        currentOffset *= 1.6f;
                        break;
                    }
                default:
                    {
                        charSequence.Append(_tweener.DOLocalMoveY(i, 12f, 0.15f * speedMultiplier).From().SetEase(Ease.OutBounce)).OnPlay(onComplete.Invoke).Join(_tweener.DOScaleY(i, 1f, 0f)).Join(_tweener.DOScaleX(i, 1f, 0f)).Join(_tweener.DOFade(i, 0, 0.01f * speedMultiplier).From().SetEase(Ease.OutQuad)).Join(_tweener.DOColor(i, Color.black, 0.01f * speedMultiplier)).Join(_tweener.DOLocalMoveX(i, Random.Range(-3f, 3f), 0.45f * speedMultiplier).From().SetEase(Ease.OutBounce));
                        break;
                    }
            }

            if (i != 0) {
                if (previousChar == '.' && thisChar != '.' || previousChar == ':' || previousChar == ';' || previousChar == '?')
                {
                    currentOffset *= 13f;
                }
                else if (previousChar == ',')
                {
                    currentOffset *= 11f;
                }
                else if (thisChar == '!' && !dialogueData[0].emphasis[i])
                {
                    charSequence.Join(_tweener.DOScaleY(i, 3f, 0.15f).From().SetEase(Ease.InOutQuad)).OnStart(Shake);
                    if (normalMovePortrait)
                    {
                        portraitSequence.Append(portraitAnchor.DOLocalRotate(Random.insideUnitSphere * 25.0f * portraitShakeIntensity, 0.03f).SetEase(Ease.OutBounce));
                        normalMovePortrait = false;
                    }
                }
                else if (dialogueData[0].size[i - 1] == TextSize.Small && dialogueData[0].size[i] != TextSize.Small)
                {
                    currentOffset *= 14f;
                }else if (previousChar == '!')
                {
                    currentOffset *= 13f;
                }
            }

            if (i != end)
            {
                char nextChar = text.GetParsedText()[i + 1];
                if (nextChar == '!' && thisChar == ' ')
                {
                    currentOffset *= 0.3f;
                }
                if(nextChar == ' ')
                {
                    portraitSequence.Append(portraitAnchor.DOLocalRotate(Random.insideUnitSphere * 10.0f * portraitShakeIntensity, 0.03f).SetEase(Ease.OutBounce));
                    portraitSequence.Append(portraitAnchor.DOLocalRotate(Vector3.zero, 0.1f).SetEase(Ease.OutBounce));
                    normalMovePortrait = false;
                }
            }
            else
            {
                portraitSequence.Append(portraitAnchor.DOLocalRotate(Random.insideUnitSphere * 10.0f * portraitShakeIntensity, 0.03f).SetEase(Ease.OutBounce));
                portraitSequence.Append(portraitAnchor.DOLocalRotate(Vector3.zero, 0.1f).SetEase(Ease.OutBounce));
                normalMovePortrait = false;
            }
            if (dialogueData[0].emphasis[i])
            {
                currentOffset *= 2f;
                charSequence.Join(_tweener.DOScaleY(i,5f, 0.15f * speedMultiplier).From().SetEase(Ease.InOutQuad)).OnStart(Shake);
                if (normalMovePortrait)
                {
                    portraitSequence.Append(portraitAnchor.DOLocalRotate(Random.insideUnitSphere * 25.0f * portraitShakeIntensity, 0.03f).SetEase(Ease.OutBounce));
                    normalMovePortrait = false;
                }
            }
            if (normalMovePortrait)
            {
                portraitSequence.Append(portraitAnchor.DOLocalRotate(Random.insideUnitSphere * 10.0f * portraitShakeIntensity, 0.03f).SetEase(Ease.OutBounce));
                normalMovePortrait = false;
            }
            if (dialogueData[0].effect[i] == TextEffect.Wave)
            {
                Tween sine = _tweener.DOLocalMoveY(i, 6f, 0.4f).SetEase(Ease.InOutSine)
                    .SetLoops(3, LoopType.Yoyo).SetAutoKill(true);
                Tween endSine = _tweener.DOLocalMoveY(i, 0f, 0.5f).SetEase(Ease.InOutSine).SetAutoKill(true);
                charSequence.Append(sine);
                charSequence.Append(endSine);
            }
            else if (dialogueData[0].effect[i] == TextEffect.Spooky)
            {
                Sequence spook = DOTween.Sequence().SetAutoKill(true);
                spook.Append(_tweener.DOShakePosition(i, 3f)).Join(_tweener.DOShakeRotation(i,3f,30f));
                charSequence.Append(spook);
            }

            _sequence.Insert(currentOffset + offsetCumul, charSequence);
            _sequencePortrait.Insert(currentOffset + offsetCumul, portraitSequence);
            offsetCumul += currentOffset;
        }
        _sequence.OnComplete(FinishPrint);
    }

    private LetterType GetType(char c)
    {
        switch (c)
        {
            case '.':
            case ',':
            case ':':
            case ';':
            case '?':
            case '!':
            case '(':
            case ')':
                {
                    return LetterType.punctuation;
                }
            case 'a':
            case 'e':
            case 'i':
            case 'o':
            case 'u':
            case 'y':
            case '-':
            case '\'':
                {
                    return LetterType.vowel;
                }
            case ' ':
            case '\t':
            case '\n':
                {
                    return LetterType.space;
                }
            default:
                {
                    return LetterType.consonant;
                }
        }
    }

    private char CheckNextSyllableFirstChar(int i)
    {
        int numLetterSyllabe = 1;
        int textSize = text.GetParsedText().Length;
        bool alreadyChangedLetterType = false;
        char previousChar = text.GetParsedText()[i];
        for (int j = i + 1; j <= i + 4 && j < textSize; ++j)
        {
            char thisChar = text.GetParsedText()[j];
            LetterType prevLetterType = GetType(previousChar);
            LetterType currentLetterType = GetType(thisChar);
            if ((prevLetterType == LetterType.space || prevLetterType == LetterType.punctuation) && (currentLetterType == LetterType.vowel || currentLetterType == LetterType.consonant))
            {
                return thisChar;
            }
            else if (currentLetterType != LetterType.punctuation && currentLetterType != LetterType.space)
            {
                bool letterTypeDifferent = (prevLetterType == LetterType.vowel && currentLetterType == LetterType.consonant) || (prevLetterType == LetterType.consonant && currentLetterType == LetterType.vowel);
                if (letterTypeDifferent)
                {
                    if (alreadyChangedLetterType)
                    {
                        return thisChar;
                    }
                    else
                    {
                        alreadyChangedLetterType = true;
                    }
                }
                else if (currentLetterType == LetterType.vowel && numLetterSyllabe >= 3 && !alreadyChangedLetterType)
                {
                    return thisChar;
                }
                else if (currentLetterType == LetterType.vowel && alreadyChangedLetterType && (numLetterSyllabe > 1 && previousChar != thisChar))
                {
                    return thisChar;
                }
            }
            previousChar = thisChar;
        }
        return ' ';
    }

    private void BeginSyllable()
    {
        if (intonations.Count <= 0f || effectOnSyllabs.Count <= 0f)
            return;
        BeginSyllable(intonations[0], effectOnSyllabs[0]);
        intonations.RemoveAt(0);
        effectOnSyllabs.RemoveAt(0);
    }

    private void BeginSyllable(Intonation intonation, EffectOnSyllab effect)
    {
        DialogueSound.PlaySyllab(intonation, effect, dialogueData[0].character);
    }
}
