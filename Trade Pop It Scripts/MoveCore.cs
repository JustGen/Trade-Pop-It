using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveCore : MonoBehaviour
{
    public static MoveCore S;

    public List<GameObject> listOfParts;
    public List<GameObject> listOfProjectors;
    public float scaleFactorForBubble;
    public bool modelIsReady = false;
    //public bool bonusLevel;

    private Vector3 offset;
    private float distance;
    private Vector3 startPosition;
    private GameObject activePart;
    private Vector2 snapZone;
    private bool _found;
    private int countFinal = 0;

    public bool lookModel;

    public int countBubble = 0;

    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        _found = false;
        lookModel = false;
        activePart = null;
        snapZone = new Vector2(-0.5f, 0.5f);
    }

    public void DecrementCountBubble()
    {
        if (PlayerPrefs.GetInt("Vibro") == 1)
            Vibration.Vibrate(100);
            //Handheld.Vibrate();

        --countBubble;

        if (countBubble == 0)
        {
            CoreUI.S.FinishLevelAddLevel(1);
        }
    }
     
    private void Update()
    {
        if (Input.touchCount == 1 && !modelIsReady && !CoreUI.S.Blocking3DModelsBool)
        {
            Touch touch = Input.GetTouch(0);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (!_found)
            {
                if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Draggable")
                {
                    activePart = hit.collider.gameObject;
                    startPosition = activePart.transform.localPosition;
                    _found = true;
                }
            }

            if (activePart != null && !lookModel)
            {
                Transform target = activePart.transform;

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (Physics.Raycast(ray, out hit))
                        {
                            target = hit.transform;
                            offset = target.localPosition - hit.point;
                            distance = hit.distance;
                        }

                        //MenuController.S.LeftRightButtons(false);
                        break;

                    case TouchPhase.Moved:
                        target.localPosition = ray.origin + ray.direction * distance + offset;
                        target.localPosition = new Vector3(target.localPosition.x, target.localPosition.y, -0.5f);
                        break;

                    case TouchPhase.Ended:
                        if (target.localPosition.x > snapZone.x && target.localPosition.x < snapZone.y && target.localPosition.y > snapZone.x && target.localPosition.y < snapZone.y)
                        {
                            target.localPosition = Vector3.zero;
                            target.tag = "NonDraggable";
                            countFinal++;
                        }
                        else
                        {
                            target.localPosition = startPosition;
                        }

                        _found = false;
                        activePart = null;

                        if (countFinal == listOfParts.Count)
                        {
                            modelIsReady = true;
                        }

                        break;
                }
            }
        }
    }
}
