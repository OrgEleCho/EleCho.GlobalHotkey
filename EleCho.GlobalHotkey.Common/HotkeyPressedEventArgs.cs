namespace EleCho.GlobalHotkey
{
    public class HotkeyPressedEventArgs : EventArgs
    {
        public HotkeyPressedEventArgs(Hotkey hotkey)
        {
            Hotkey = hotkey;
        }

        public Hotkey Hotkey { get; }
    }

    public delegate void HotkeyHandler(Hotkey hotkey);
}