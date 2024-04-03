using System;
using System.Collections;
using UnityEngine;

public class ProgressRotate : MonoBehaviour
{
    [SerializeField] private float loadingTimer = 3f;
    [SerializeField] private float progressRPM = 1;
    
    private RectTransform _rectTransform;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnProgressRotate(Action action)
    {
        Debug.Log("On Progress");
        gameObject.SetActive(true);
        StartCoroutine(Circle(action));
    }

    IEnumerator Circle(Action action)
    {
        Debug.Log("Progress Circle");
        //Progress 유지 시간
        float timer = 0f;
        //회전 수에 따른 Value (+ 왼쪽 / - 오른쪽)
        float rpmValue = -progressRPM * 360;

        while (timer <= loadingTimer)
        {
            if (timer >= loadingTimer || GameManager.Instance.IsGameStart)
                break;

            timer += Time.deltaTime;

            _rectTransform.rotation = Quaternion.Euler(0, 0, timer * rpmValue);

            yield return new WaitForEndOfFrame();
        }

        if (action != null)
            action.Invoke();

        gameObject.SetActive(false);
    }
}
