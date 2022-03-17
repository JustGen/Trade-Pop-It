using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operations : MonoBehaviour
{
    [SerializeField] private int _idOperation;

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position); //Создаем луч в точке прикосновения
            RaycastHit hit; //Регистрируем переменную в которой будет инфа о предмете, в который мы попали
            Physics.Raycast(ray, out hit); //Выпускаем луч и записываем всю инфу в hit
            
            if (hit.collider == this.gameObject.GetComponent<Collider>()) //Если мы попали в объект, на котором висит этот скрипт
            {
                switch(_idOperation)
                {
                    case 1:
                        CoreGame.S.DoneClick();
                        break;

                    case 2:
                        CoreGame.S.AddClick();
                        break;

                    case 3:
                        CoreGame.S.CancelClick();
                        break;
                }
            }
        }
    }
}
