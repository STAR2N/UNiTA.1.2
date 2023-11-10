using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LetterMaster : NetworkBehaviour
{
    public static LetterMaster Instance { get; private set; }

    [SerializeField] public Sprite[] LetterFront_BgImg;
    [SerializeField] public Sprite[] LetterBack_BgImg;

    public GameObject LetterUIPrefab;
    public GameObject CompleteLetterUIPrefab;
    [HideInInspector] public GameObject LetterUI;
    [HideInInspector] public GameObject CompleteLetterUI;

    public GameObject LetterCardPrefab;
    public List<GameObject> LetterCards = new List<GameObject>();

    public int ShowIndex;
    public int RandomImage = 0;

    [SyncVar] public int CurrentIndex = 0;
    [SyncVar] public int CompleteIndex = 0;
    public readonly SyncList<int> ImgIdxList = new SyncList<int>();

    public bool _IsServer = false;
    public bool _HasAuthority = false;
    public bool IsDone = false;
    public bool IsWriting = false;

    public override void OnStartServer()
    {
        base.OnStartServer();
        for(int i = 0; i < 8; i++)
        {
            ImgIdxList.Add(i);
        }

        for (int i = 0; i < 8; ++i)
        {
            var LetterCardObject = Instantiate(LetterCardPrefab,
            this.transform.GetChild(i).transform.position,
            this.transform.GetChild(i).transform.rotation);
            NetworkServer.Spawn(LetterCardObject);
        }
    }
    private void Start()
    {
        ShowIndex = 0;
        LetterUI = Instantiate(LetterUIPrefab);
        CompleteLetterUI = Instantiate(CompleteLetterUIPrefab);
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        _IsServer = isServer;
        _HasAuthority = hasAuthority;
    }

    [Command(requiresAuthority = false)]
    public void CmdSetLetter(string content, int imageIdx, LetterSecurityLevelType securityLevelType, string email)
    {
        GameObject LetterCard = LetterCards[CompleteIndex];

        LetterCard.GetComponent<Letter>().Content = content;
        LetterCard.GetComponent<Letter>().ImgIdx = imageIdx;
        LetterCard.GetComponent<Letter>().SecurityLevel = securityLevelType;
        LetterCard.GetComponent<Letter>().Email = email;

        ++CompleteIndex;
    }

    [Command(requiresAuthority = false)]
    public void CmdChangeRandImage(NetworkConnectionToClient sender = null)
    {
        ++CurrentIndex;

        string t = "";
        for(int i = 0; i < ImgIdxList.Count; i++)
        {
            t += ImgIdxList[i].ToString();
            t += " ";
        }

        int idx = Random.Range(0, ImgIdxList.Count);
        RandomImage = ImgIdxList[idx];
        ImgIdxList.Remove(ImgIdxList[idx]);
        RpcSetLocalImageIndex(sender, RandomImage);
    }

    [TargetRpc]
    public void RpcSetLocalImageIndex(NetworkConnection target, int idx)
    {
        RandomImage = idx;
        LetterUI.GetComponentInChildren<Image>().sprite = LetterFront_BgImg[RandomImage];

        LetterUI.GetComponentInChildren<CanvasGroup>().alpha = 1.0f;
        LetterUI.GetComponentInChildren<CanvasGroup>().interactable = true;
        LetterUI.GetComponentInChildren<CanvasGroup>().blocksRaycasts = true;

        IsWriting = true;
    }

    [Command(requiresAuthority = false)]
    public void CmdBackupLetterVar(int idx)
    {
        CurrentIndex -= 1;
        ImgIdxList.Add(idx);

        string t = "";
        for (int i = 0; i < ImgIdxList.Count; i++)
        {
            t += ImgIdxList[i].ToString();
            t += " ";
        }
    }

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
    }
    private void OnDisable()
    {
        if (Instance == this)
            Instance = null;
    }
}
