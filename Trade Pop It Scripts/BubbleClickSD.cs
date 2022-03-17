using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BubbleClickSD : MonoBehaviour
{
    public bool turn;

    private void Start()
    {
        turn = false;
    }

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began && !turn && !CoreUI.S.Blocking3DModelsBool)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position); //Создаем луч в точке прикосновения
                RaycastHit hit; //Регистрируем переменную, в которой будет инфа о предмете, в который мы попали
                Physics.Raycast(ray, out hit); //Выпускаем луч и записываем всю инфу в hit
                if (hit.collider == this.gameObject.GetComponent<Collider>()) //Если мы попали в объект, на котором висит этот скрипт
                {
                    Vector3 scaleBubble = Vector3.one;
                    scaleBubble.z = this.transform.localScale.z * -1;
                    this.transform.localScale = scaleBubble;
                    turn = true;
                    GameAudioSource.S.AudioPlayPopItOrSD();
                    SD_BaseSrarter.S.DecrementCountBubbleSD();
                }
            }
        }
    }
}
