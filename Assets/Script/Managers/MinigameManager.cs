using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    [SerializeField]
    AudioClip MinigameManagerAudioClip;

    public GameObject NumberSlotObject;
    public GameObject MinigameStartButton;

    public Sprite[] NumberSprite;
    public Image[] SlotSprite;

    public List<int> StartList = new List<int>();
    public int ResultIndex;
    public bool isMinigameOver = false;

    private AudioSource MinigameManagerAudioSource;
    int ItemCnt = 10;

    private void Start()
    {
        MinigameManagerAudioSource = GetComponent<AudioSource>();
    }

    private void NumberCardInitialize()
    {
        for (int i = 0; i < ItemCnt; i++)
        {
            StartList.Add(i);
        }

        for (int i = 0; i < ItemCnt; i++)
        {
            int randomIndex = Random.Range(0, StartList.Count);
            if (i == 0)
            {
                SlotSprite[ItemCnt].sprite = NumberSprite[StartList[randomIndex]];
            }
            if (i == 1)
            {
                ResultIndex = 1 + StartList[randomIndex];
            }
            SlotSprite[i].sprite = NumberSprite[StartList[randomIndex]];
            StartList.RemoveAt(randomIndex);
        }
    }

    IEnumerator StartSlot()
    {
        NumberCardInitialize();//숫자 카드 무작위로 섞어놓기
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i < 21; i++)
        {
            NumberSlotObject.transform.localPosition += new Vector3(0, 100f, 0);
            if (NumberSlotObject.transform.localPosition.y > 1000f)
            {
                NumberSlotObject.transform.localPosition -= new Vector3(0, 1000f, 0);
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2f);
        isMinigameOver = true;
        yield return new WaitForSeconds(1f);
        NumberSlotObject.transform.localPosition = new Vector3(0, 0, 0);
        MinigameStartButton.SetActive(true);
    }

    public void StartMinigame()
    {
        if (!MinigameManagerAudioSource.isPlaying)
        {
            MinigameManagerAudioSource.PlayOneShot(MinigameManagerAudioClip);
        }

        StartCoroutine(StartSlot());
        MinigameStartButton.SetActive(false);
    }
}
