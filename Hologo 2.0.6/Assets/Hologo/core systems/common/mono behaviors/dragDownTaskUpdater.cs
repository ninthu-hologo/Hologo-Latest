using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class dragDownTaskUpdater : MonoBehaviour
{
    [Header("UI ELEMENTS")]
    [SerializeField]
    MyScrollRect myCustomScrollRect;
    public RectTransform ScrollRect;
    public Transform LoaderRect;
    public Animator LoadAnimator;
    [Header("UPDATER CONFIG")]
    [SerializeField]
    float timeMultiplier = 1f;
    [SerializeField]
    float dragDownDistance = 125f;
    private LayoutElement loadRectLE;
    [SerializeField]
    float AnimationDivider = 2f;
    [SerializeField]
    float minimumRectSize = 1f;
    [SerializeField]
    float minimumRectSizeSnapBuffer = 10f;
    [Header("UPDATER EVENT")]
    [SerializeField]
    public UnityEvent UpdateEvent;
    [SerializeField]
    float scrollerStartPosition;
    float currentPosition;
    float size = 0;
    bool start = true;


    bool updateDrag = true;
    bool lerpMeBack = false;
    bool startedUpdating = false;

    

    void Start()
    {
        scrollerStartPosition = ScrollRect.transform.InverseTransformPoint(LoaderRect.position).y;
        loadRectLE = LoaderRect.gameObject.GetComponent<LayoutElement>();
       // myCustomScrollRect = ScrollRect.gameObject.GetComponent<MyScrollRect>();
        if (myCustomScrollRect != null)
        {
            myCustomScrollRect.onValueChanged.RemoveAllListeners();
            myCustomScrollRect.onValueChanged.AddListener(OnScrollUpdate);
        }
    }


    private void OnEnable()
    {
        ////if (myCustomScrollRect == null)
        ////{
        //    myCustomScrollRect = ScrollRect.gameObject.GetComponent<MyScrollRect>();
        //    if (myCustomScrollRect != null)
        //    {
        //        myCustomScrollRect.onValueChanged.RemoveAllListeners();
        //        myCustomScrollRect.onValueChanged.AddListener(OnScrollUpdate);
        //    }
        ////}
    }

    void OnScrollUpdate(Vector2 scrollVector)
    {
        if (start)
        {
            scrollerStartPosition = ScrollRect.transform.InverseTransformPoint(LoaderRect.position).y;
            start = false;
            updateDrag = true;
            startedUpdating = true;
        }
        if (updateDrag)
        {
           
            currentPosition = ScrollRect.transform.InverseTransformPoint(LoaderRect.position).y;
            if (currentPosition < scrollerStartPosition)
            {
                //if (!LoadAnimator.gameObject.activeSelf)
                //{
                bool active = size > minimumRectSizeSnapBuffer;
                // LoadAnimator.gameObject.SetActive(active); // shariz changing update pull down appear oct 21st
                //}


                if (size < dragDownDistance)
                {
                    size = scrollerStartPosition - currentPosition;
                    loadRectLE.preferredHeight = size;
                    if (LoadAnimator.gameObject.activeSelf)
                        LoadAnimator.Play("load_animation", 0, size / AnimationDivider);
                }
                else
                {
                    if (startedUpdating)
                    {
                        // Debug.Log("started updating");
                        startedUpdating = false;
                        StartCoroutine(StartUpdate());
                        LoadAnimator.gameObject.SetActive(active); // shariz changing update pull down appear oct 21st
                    }
                }
            }
        }

    }


    IEnumerator StartUpdate()
    {
        iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);
        UpdateEvent.Invoke();
        yield return new WaitForSeconds(2);
        lerpMeBack = true;

    }

    // Update is called once per frame
    void Update()
    {

        if (!lerpMeBack)
        {
            return;
        }
        float k = timeMultiplier * Time.deltaTime;
        if (loadRectLE.preferredHeight > minimumRectSizeSnapBuffer)
        {
            loadRectLE.preferredHeight = Mathf.Lerp(loadRectLE.preferredHeight, minimumRectSize, timeMultiplier * Time.deltaTime);
        }
        else
        {
            LoadAnimator.gameObject.SetActive(false);
            loadRectLE.preferredHeight = minimumRectSize;
            size = 0f;
            lerpMeBack = false;
            start = true;
        }

    }
}
