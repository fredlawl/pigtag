using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Globalization;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public static readonly string DURATION_30_MINUTES = "PT30M";
    public static readonly string TIMESPAN_30_MINUTES = "00:30:00";

    [SerializeField]
    private TMPro.TextMeshProUGUI display;

    /*
     * RFC 3339 "Appendix A. ISO 8601 Collected ABNF" durations
     * https://www.ietf.org/rfc/rfc3339.txt
     * https://en.wikipedia.org/wiki/ISO_8601#Durations
     * Is what I would want to use, but just stick to
     * C#'s time span format: 
     * https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-timespan-format-strings
     */
    [SerializeField]
    private string countdownAsString = TIMESPAN_30_MINUTES;

    private TimeSpan span;
    private float secondsRemaining;
    
    [SerializeField]
    private bool isEnabled = true;

    public UnityEvent onFinished;
    public UnityEvent<float> onTick;

    public TimeSpan StartTime => span;

    private void Awake()
    {
        Stop();
    }

    private void Start()
    {
        Play();
    }

    public void Stop()
    {
        // Always throw exception to let the programmer they
        // messed up here passing in a valid time :p
        TimeSpan ret = TimeSpan.Parse(countdownAsString);
        span = ret;
        secondsRemaining = (float) span.TotalSeconds;
        isEnabled = false;
    }

    public void Play()
    {
        isEnabled = true;
    }

    public void Pause()
    {
        isEnabled = false;
    }

    private void Update()
    {
        if (!isEnabled)
        {
            return;
        }

        if (secondsRemaining > 0)
        {
            secondsRemaining -= Time.deltaTime;
            onTick?.Invoke(secondsRemaining);
        } else {
            Pause();
            secondsRemaining = 0f;
            onFinished?.Invoke();
        }
    }

    private void LateUpdate()
    {
        display.text = new TimeSpan(0, 0, (int)secondsRemaining).ToString();
    }
}
