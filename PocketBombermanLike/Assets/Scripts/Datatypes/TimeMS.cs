using System;

[Serializable]
public struct TimeMS
{
    public uint Minutes;
    public uint Seconds;

    public float ToSeconds()
    {
        float totalSeconds = 0.0f;

        totalSeconds += Minutes > 0 ? (Minutes * 60) : 0;
        totalSeconds += Seconds > 0 ? Seconds : 0;

        return totalSeconds;
    }

    public TimeSpan ToTimeSpan() => TimeSpan.FromSeconds(this.ToSeconds());

    public override string ToString() => $"(Minutes: {Minutes}, Seconds: {Seconds}, Total Seconds: {this.ToSeconds()})";
}