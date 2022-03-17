using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class PopIt_Builder : MonoBehaviour
{
    public static PopIt_Builder S;

    [SerializeField] private Material _material;
    [SerializeField] private GameObject _bubble;
    [SerializeField] private GameObject[] _spawnPoints;
    //[SerializeField] private int idPart;
    //[SerializeField] private float _speedRotate;
    private int countBubble;

    private List<BubbleClick> _bubbleClicksOrder = new List<BubbleClick>();

    private void Awake()
    {
        S = this;
        _bubbleClicksOrder.Clear();
    }

    private void Start()
    {
        foreach (var point in _spawnPoints)
        {
            GameObject bubble = Instantiate(_bubble, point.transform.position, _bubble.transform.rotation);
            bubble.gameObject.GetComponent<Renderer>().material = _material;
            float scaleFactor = CoreGame.S.scaleFactorForBubble;
            bubble.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            bubble.transform.SetParent(this.transform);

            _bubbleClicksOrder.Add(bubble.GetComponent<BubbleClick>());
        }

        countBubble = _spawnPoints.Length;
    }

    //public IEnumerator Rotate(float angle)
    //{
    //    while (transform.rotation != Quaternion.Euler(0, angle, 0))
    //    {
    //        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, angle, 0), _speedRotate * Time.deltaTime);
    //        yield return null;
    //    }
    //}



    //private IEnumerator InizialiteAfterRotate(float angleRotate)
    //{
    //    yield return Rotate(angleRotate);

    //    countBubble = _spawnPoints.Length;

    //    foreach (var item in _bubbleClicksOrder)
    //    {
    //        item.turn = false;
    //    }
    //}
}
