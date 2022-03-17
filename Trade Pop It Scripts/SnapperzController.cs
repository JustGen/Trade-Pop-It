using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class SnapperzController : MonoBehaviour
{
    [SerializeField] private float _speedRotate;
    [SerializeField] private int countClickLevel;

    private Animator _animator;
    private Rigidbody _rigidbody;
    private bool _statusAnimation = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.angularDrag = 5;
        _rigidbody.useGravity = false;

        CoreUI.S.SliderActivator(true, countClickLevel, false);

        //countClickLevel = CoreGame.S.listOfCountClickers[CoreGame.S.CountLevelHidden];
        // MenuController.S.LeftRightButtons(false);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (!CoreUI.S.Blocking3DModelsBool)
                        StartTouch();
                    break;

                case TouchPhase.Ended:
                    EndedTouch();
                    break;

                case TouchPhase.Moved:
                    if (!CoreUI.S.Blocking3DModelsBool)
                    {
                        float torque = touch.deltaPosition.x * Time.deltaTime * _speedRotate;
                        _rigidbody.AddTorque(Vector3.up * torque);
                    }
                    break;
            }
        }
    }

    private void StartTouch()
    {
        _animator.SetBool("StartAnim", true);
        _statusAnimation = true;
        DecrementCountClickers();
    }

    public void EndedTouch()
    {
        _animator.SetBool("StartAnim", false);
        _statusAnimation = false;
        GameAudioSource.S.AudioPlaySnapperz();
    }

    public void DecrementCountClickers()
    {
        --countClickLevel;
        CoreUI.S.ProgressBarUpdate();

        if (countClickLevel == 0)
        {
            CoreUI.S.FinishLevelAddLevel(3);
        }
    }
}
