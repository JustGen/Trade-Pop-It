using UnityEngine;

public class BubbleClick : MonoBehaviour
{
    public bool turn;

    private SphereCollider sphereCollider;

    private void Start()
    {
        turn = false;
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (MoveCore.S.modelIsReady)
        {
            sphereCollider.enabled = true;

            foreach (Touch touch in Input.touches)
            {
                if ((touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved) && !turn)
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
                        MoveCore.S.DecrementCountBubble();
                        GameAudioSource.S.AudioPlayPopItOrSD();
                    }
                }
            }
        }

    }
}
