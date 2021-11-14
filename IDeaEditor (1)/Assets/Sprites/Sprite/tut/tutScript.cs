using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class tutScript : MonoBehaviour
{
    
    public GameObject[] popups;
    public GameObject[] avatars;
    Animator avatarAnimator;
    public int step = 0;
    public GameObject helpAvatar;
    //From Brandon Script--------------------------------
    public GameObject leftPanel;
    private GameObject container;
    public SideToggleMenu EX;
    [Header("Animation")]
    [SerializeField] private float expandDuration;
    [SerializeField] private float collapseDuration;
    [SerializeField] private Ease expandEase;
    [SerializeField] private Ease collapseEase;

    [Header("Rotation")]
    //[SerializeField] private float rotationDuration;
    //[SerializeField] private Ease rotationEase;


    //[SerializeField] private Vector3 rotstart;
    //[SerializeField] private Vector3 rotend;

    [SerializeField] private Vector3 EndPosition;
    [SerializeField] private Vector3 StartPosition;
    //---------------------------------------------------

    public GameObject cam;
    public GameObject campos;

    // Start is called before the first frame update
    void Start()
    {
        container = leftPanel.transform.GetChild(0).gameObject;
        Button clickNext = transform.GetChild(0).GetComponent <Button>();
        Button skip = transform.GetChild(1).GetComponent<Button>();
        clickNext.onClick.AddListener(TaskOnClick);
        skip.onClick.AddListener(TaskOnClickSkip);
    }

    void TaskOnClick()
    {
        
        if (step == 0)
        {
            Debug.Log("step1");
            avatarAnimator = avatars[0].GetComponent<Animator>();
            avatarAnimator.SetTrigger("Done");
            popups[0].SetActive(false);
            step = 1;
            popups[1].SetActive(true);
            avatars[1].SetActive(true);

        }
        else if (step == 1)
        {
            Debug.Log("step2");
            avatarAnimator = avatars[1].GetComponent<Animator>();
            avatarAnimator.SetTrigger("Done");
            popups[1].SetActive(false);
            step = 2;
            popups[2].SetActive(true);
            avatars[2].SetActive(true);
            container.transform.DOMove(EndPosition, expandDuration).From(StartPosition).SetEase(expandEase);
            EX.isexpanded = true;
        }
        else if (step == 2)
        {
            Debug.Log("step2");
            avatarAnimator = avatars[2].GetComponent<Animator>();
            avatarAnimator.SetTrigger("Done");
            popups[2].SetActive(false);
            container.transform.DOMove(StartPosition, collapseDuration).From(EndPosition).SetEase(collapseEase);
            step = 3;
            cam.transform.position = campos.transform.position;
            cam.transform.rotation = campos.transform.rotation;
            popups[3].SetActive(true);
            avatars[3].SetActive(true);
        }
        else if (step == 3)
        {
            Debug.Log("step2");
            avatarAnimator = avatars[3].GetComponent<Animator>();
            avatarAnimator.SetTrigger("Done");
            popups[3].SetActive(false);
            step = 4;
            popups[4].SetActive(true);
            avatars[4].SetActive(true);

        }
        else if (step == 4)
        {
            Debug.Log("step2");
            avatarAnimator = avatars[4].GetComponent<Animator>();
            avatarAnimator.SetTrigger("Done");
            popups[4].SetActive(false);
            step = 5;
            popups[5].SetActive(true);
            avatars[5].SetActive(true);

        }
        else if (step == 5)
        {
            Debug.Log("step2");
            avatarAnimator = avatars[5].GetComponent<Animator>();
            avatarAnimator.SetTrigger("Done");
            popups[5].SetActive(false);
            step = 6;
            popups[6].SetActive(true);
            avatars[6].SetActive(true);

        }
        else if (step == 6)
        {
            Debug.Log("step2");
            avatarAnimator = avatars[5].GetComponent<Animator>();
            avatarAnimator.SetTrigger("Done");
            popups[6].SetActive(false);
            for (int i = 0; i< avatars.Length; i++)
            {
                avatarAnimator = avatars[i].GetComponent<Animator>();
                avatarAnimator.ResetTrigger("Done");
                avatars[i].SetActive(false);
                
            }
            transform.GetChild(0).transform.gameObject.SetActive(false);
            transform.GetChild(1).transform.gameObject.SetActive(false);
            helpAvatar.SetActive(true);


        }



    }

    void TaskOnClickSkip()
    {
        for (int i=0; i < transform.childCount; i++)
        {
            step = 6;
            transform.GetChild(i).transform.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        

    }
    
}
