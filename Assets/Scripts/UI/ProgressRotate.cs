using System;
using System.Collections;
using UnityEngine;

public class ProgressRotate : MonoBehaviour
{
    [SerializeField] private float loadingTimer = 3f;
    [SerializeField] private float progressRPM = 1;
    
    private RectTransform _rectTransform;
    private bool _isExit;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnProgressRotate(Action action)
    {
        gameObject.SetActive(true);
        StartCoroutine(Circle(action));
    }

    IEnumerator Circle(Action action)
    {
        //Progress 유지 시간
        float timer = 0f;
        //회전 수에 따른 Value (+ 왼쪽 / - 오른쪽)
        float rpmValue = -progressRPM * 360;

        _isExit = false;
        
        while (timer <= loadingTimer)
        {
            if (timer >= loadingTimer || _isExit)
                break;

            timer += Time.deltaTime;
            
            _rectTransform.rotation = Quaternion.Euler(0, 0, timer * rpmValue);

            yield return new WaitForEndOfFrame();
        }
        
        if(action != null)
            action.Invoke();
        
        gameObject.SetActive(false);
    }

    public bool IsExit
    {
        set => _isExit = value;
        get => _isExit;
    }
}
