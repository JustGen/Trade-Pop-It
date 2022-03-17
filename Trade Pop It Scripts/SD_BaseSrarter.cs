using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SD_BaseSrarter : MonoBehaviour
{
    public static SD_BaseSrarter S;

    [SerializeField] private GameObject[] _listOfBubble;
    [SerializeField] private float _speedRotate;
    [SerializeField] private int countClickLevel;

    private int countBubble;
    private List<BubbleClickSD> _bubbleClicksOrder = new List<BubbleClickSD>();
    

    private void Awake()
    {
        S = this;
        _bubbleClicksOrder.Clear();
    }

    private void Start()
    {
        foreach (var obj in _listOfBubble)
        {
            _bubbleClicksOrder.Add(obj.GetComponent<BubbleClickSD>());
        }

        countBubble = _listOfBubble.Length;

        CoreUI.S.SliderActivator(true, countClickLevel, false);

        //countClickLevel = CoreGame.S.listOfCountClickers[CoreGame.S.CountLevelHidden];
    }

    public IEnumerator Rotate(float angle)
    {
        while (transform.rotation != Quaternion.Euler(0, angle, 0))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, angle, 0), _speedRotate * Time.deltaTime);
            yield return null;
        }
    }

    public void DecrementCountBubbleSD()
    {
        --countBubble;
        countClickLevel--;
        CoreUI.S.ProgressBarUpdate();

        if (countBubble == 0)
        {
            if (transform.rotation.y == 0)
            {
                StartCoroutine(InizialiteAfterRotate(180.0f));
            }
            else
            {
                StartCoroutine(InizialiteAfterRotate(0.0f));
            }
        }

        if (countClickLevel == 0)
        {
            CoreUI.S.FinishLevelAddLevel(2);
        }
    }

    private IEnumerator InizialiteAfterRotate(float angleRotate)
    {
        yield return Rotate(angleRotate);

        countBubble = _listOfBubble.Length;

        foreach (var item in _bubbleClicksOrder)
        {
            item.turn = false;
        }
    }
}
