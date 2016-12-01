using UnityEngine;
using System;

/// Time iteration class.
///
/// Component of the sky dome parent game object.

public class TOD_Time : MonoBehaviour
{
	/// Length of one day in minutes.
	[Tooltip("Length of one day in minutes.")]
	public float DayLengthInMinutes = 30;

	/// Set the time to the current device time on start.
	[Tooltip("Set the time to the current device time on start.")]
	public bool UseDeviceTime = false;

	/// Apply the time curve when progressing time.
	[Tooltip("Apply the time curve when progressing time.")]
	public bool UseTimeCurve = false;

	/// Time progression curve.
	[Tooltip("Time progression curve.")]
	public AnimationCurve TimeCurve = AnimationCurve.Linear(0, 0, 24, 24);

	/// Fired whenever the minute value is incremented.
	internal event Action OnMinute;

	/// Fired whenever the hour value is incremented.
	internal event Action OnHour;

	/// Fired whenever the day value is incremented.
	internal event Action OnDay;

	/// Fired whenever the month value is incremented.
	internal event Action OnMonth;

	/// Fired whenever the year value is incremented.
	internal event Action OnYear;

	private TOD_Sky sky;
	private AnimationCurve timeCurve;
	private AnimationCurve timeCurveInverse;

	/// Apply changes made to TimeCurve.
	internal void RefreshTimeCurve()
	{
		TimeCurve.preWrapMode  = WrapMode.Clamp;
		TimeCurve.postWrapMode = WrapMode.Clamp;

		ApproximateCurve(TimeCurve, out timeCurve, out timeCurveInverse);

		timeCurve.preWrapMode  = WrapMode.Loop;
		timeCurve.postWrapMode = WrapMode.Loop;

		timeCurveInverse.preWrapMode  = WrapMode.Loop;
		timeCurveInverse.postWrapMode = WrapMode.Loop;
	}

	/// Apply the time curve to a time span.
	/// \param deltaTime The time span to adjust.
	/// \return The adjusted time span.
	internal float ApplyTimeCurve(float deltaTime)
	{
		float time = timeCurveInverse.Evaluate(sky.Cycle.Hour) + deltaTime;
		deltaTime = timeCurve.Evaluate(time) - sky.Cycle.Hour;

		if (time >= 24)
		{
			deltaTime += ((int)time / 24) * 24;
		}
		else if (time < 0)
		{
			deltaTime += ((int)time / 24 - 1) * 24;
		}

		return deltaTime;
	}

	/// Add hours and fractions of hours to the current time.
	/// \param hours The hours to add.
	/// \param adjust Whether or not to apply the time curve.
	internal void AddHours(float hours, bool adjust = true)
	{
		if (UseTimeCurve && adjust) hours = ApplyTimeCurve(hours);

		var dateTimeOld = sky.Cycle.DateTime;
		var dateTimeNew = dateTimeOld.AddHours(hours);

		if (dateTimeNew.Year > dateTimeOld.Year)
		{
			if (OnYear   != null) OnYear();
			if (OnMonth  != null) OnMonth();
			if (OnDay    != null) OnDay();
			if (OnHour   != null) OnHour();
			if (OnMinute != null) OnMinute();
		}
		else if (dateTimeNew.Month > dateTimeOld.Month)
		{
			if (OnMonth  != null) OnMonth();
			if (OnDay    != null) OnDay();
			if (OnHour   != null) OnHour();
			if (OnMinute != null) OnMinute();
		}
		else if (dateTimeNew.Day > dateTimeOld.Day)
		{
			if (OnDay    != null) OnDay();
			if (OnHour   != null) OnHour();
			if (OnMinute != null) OnMinute();
		}
		else if (dateTimeNew.Hour > dateTimeOld.Hour)
		{
			if (OnHour   != null) OnHour();
			if (OnMinute != null) OnMinute();
		}
		else if (dateTimeNew.Minute > dateTimeOld.Minute)
		{
			if (OnMinute != null) OnMinute();
		}

		sky.Cycle.DateTime = dateTimeNew;
	}

	/// Add seconds and fractions of seconds to the current time.
	/// \param seconds The seconds to add.
	/// \param adjust Whether or not to apply the time curve.
	internal void AddSeconds(float seconds, bool adjust = true)
	{
		AddHours(seconds / 3600f);
	}

	private void CalculateLinearTangents(Keyframe[] keys)
	{
		for (int i = 0; i < keys.Length; i++)
		{
			var key = keys[i];

			if (i > 0)
			{
				var prev = keys[i-1];
				key.inTangent = (key.value - prev.value) / (key.time - prev.time);
			}

			if (i < keys.Length-1)
			{
				var next = keys[i+1];
				key.outTangent = (next.value - key.value) / (next.time - key.time);
			}

			keys[i] = key;
		}
	}

	private void ApproximateCurve(AnimationCurve source, out AnimationCurve approxCurve, out AnimationCurve approxInverse)
	{
		const float minstep = 0.01f;

		var approxCurveKeys   = new Keyframe[25];
		var approxInverseKeys = new Keyframe[25];

		float time = -minstep;
		for (int i = 0; i < 25; i++)
		{
			time = Mathf.Max(time + minstep, source.Evaluate(i));

			approxCurveKeys[i]   = new Keyframe(i, time);
			approxInverseKeys[i] = new Keyframe(time, i);
		}

		CalculateLinearTangents(approxCurveKeys);
		CalculateLinearTangents(approxInverseKeys);

		approxCurve   = new AnimationCurve(approxCurveKeys);
		approxInverse = new AnimationCurve(approxInverseKeys);
	}

	protected void Awake()
	{
		sky = GetComponent<TOD_Sky>();

		if (UseDeviceTime) sky.Cycle.DateTime = DateTime.Now;

		RefreshTimeCurve();
	}

	protected void FixedUpdate()
	{
		const float oneDayInMinutes = 60 * 24;

		float timeFactor = oneDayInMinutes / DayLengthInMinutes;

		AddSeconds(Time.deltaTime * timeFactor);
	}
}
