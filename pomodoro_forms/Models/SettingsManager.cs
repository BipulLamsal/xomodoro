using Xamarin.Essentials;

public static class SettingsManager
{
    private const string FocusTimeKey = "FocusTime";
    private const string ShortBreakKey = "ShortBreak";
    private const string LongBreakKey = "LongBreak";
    private const string AutoResumeKey = "AutoResume";
    private const string SoundKey = "Sound";
    private const string NotificationsKey = "Notifications";

    public static int FocusTimeDefault = 26;
    public static int ShortBreakDefault = 5;
    public static int LongBreakDefault = 15;
    public static bool AutoResumeDefault = false;
    public static bool SoundDefault = true;
    public static bool NotificationsDefault = true;


    public static int FocusTime
    {
        get => Preferences.Get(FocusTimeKey, FocusTimeDefault);
        set => Preferences.Set(FocusTimeKey, value);
    }

    public static int ShortBreak
    {
        get => Preferences.Get(ShortBreakKey, ShortBreakDefault);
        set => Preferences.Set(ShortBreakKey, value);
    }

    public static int LongBreak
    {
        get => Preferences.Get(LongBreakKey, LongBreakDefault);
        set => Preferences.Set(LongBreakKey, value);
    }

    public static bool AutoResume
    {
        get => Preferences.Get(AutoResumeKey, AutoResumeDefault);
        set => Preferences.Set(AutoResumeKey, value);
    }

    public static bool Sound
    {
        get => Preferences.Get(SoundKey, SoundDefault);
        set => Preferences.Set(SoundKey, value);
    }

    public static bool Notifications
    {
        get => Preferences.Get(NotificationsKey, NotificationsDefault);
        set => Preferences.Set(NotificationsKey, value);
    }
}
