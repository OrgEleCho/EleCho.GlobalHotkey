using System.Runtime.InteropServices;

namespace EleCho.GlobalHotkey.Windows
{
    public class GlobalHotkeyManager
    {
        public GlobalHotkeyManager(nint hwnd)
        {
            Hwnd = hwnd;
        }

        private readonly Dictionary<Hotkey, HotkeyHandler?> registeredHotkeyHandlers =
            new Dictionary<Hotkey, HotkeyHandler?>();

        public nint Hwnd { get; }

        /// <summary>
        /// Register a hotkey
        /// </summary>
        /// <param name="modifiers">Modifier keys</param>
        /// <param name="key">Key</param>
        /// <param name="handler">Hotkey handler</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        private void RegisterCore(ModifierKeys modifiers, Key key, HotkeyHandler? handler)
        {
            int hashCode =
                HashCode.Combine(modifiers, key);

            Hotkey hotkey =
                new Hotkey(modifiers, key);

            if (!NativeMethods.RegisterHotKey(Hwnd, hashCode,
                KeyHelper.GetNativeModifierKeys(modifiers),
                KeyHelper.GetNativeKey(key)))
            {
                int errorCode =
                    Marshal.GetLastWin32Error();

                switch (errorCode)
                {
                    case 1409:
                        throw new InvalidOperationException("Hotkey aready registered");
                    case 1400:
                        throw new InvalidOperationException("Invalid window handle");
                    case 87:
                        throw new ArgumentException("Invalid parameter");
                    default:
                        throw new Exception($"Faild to register hotkey, Error code: {errorCode}");
                }
            }

            registeredHotkeyHandlers[hotkey] = handler;
        }

        /// <summary>
        /// Unregister a hotkey
        /// </summary>
        /// <param name="modifiers">Modifier keys</param>
        /// <param name="key">Key</param>
        private void UnregisterCore(ModifierKeys modifiers, Key key, bool remove)
        {
            int hashCode =
                HashCode.Combine(modifiers, key);

            Hotkey hotkey =
                new Hotkey(modifiers, key);

            if (!NativeMethods.UnregisterHotKey(Hwnd, hashCode))
            {
                int errorCode =
                    Marshal.GetLastWin32Error();

                switch (errorCode)
                {
                    case 1419:
                        throw new InvalidOperationException("Hotkey is not registered");
                    case 1400:
                        throw new InvalidOperationException("Invalid window handle");
                    default:
                        throw new Exception($"Faild to unregister hotkey, Error code: {errorCode}");
                }
            }

            if (remove)
                registeredHotkeyHandlers.Remove(hotkey);
        }

        public void Register(ModifierKeys modifiers, Key key, HotkeyHandler handler) =>
            RegisterCore(modifiers, key, handler);

        public void Register(ModifierKeys modifiers, Key key) =>
            RegisterCore(modifiers, key, null);

        public void Register(Hotkey hotkey, HotkeyHandler handler) =>
            RegisterCore(hotkey.Modifier, hotkey.Key, handler);

        public void Register(Hotkey hotkey) =>
            RegisterCore(hotkey.Modifier, hotkey.Key, null);

        public void Unregister(ModifierKeys modifiers, Key key) =>
            UnregisterCore(modifiers, key, true);

        public void Unregister(Hotkey hotkey) =>
            UnregisterCore(hotkey.Modifier, hotkey.Key, true);

        public void UnregisterAll()
        {
            foreach (var key in registeredHotkeyHandlers.Keys)
                UnregisterCore(key.Modifier, key.Key, false);

            registeredHotkeyHandlers.Clear();
        }

        /// <summary>
        /// Process hotkey detect (call this method in WndProc)
        /// </summary>
        /// <returns></returns>
        public bool Process(nint hwnd, int msg, nint wParam, nint lparam)
        {
            if (msg != 0x0312)
                return false;

            int id = (int)wParam;

            ModifierKeys modifier = KeyHelper.GetModifierKeysFromNative((uint)lparam & 0xFFFF);
            Key key = KeyHelper.GetKeyFromNative(((uint)lparam >> 16) & 0xFFFF);

            Hotkey hotkey = new Hotkey(modifier, key);

            if (registeredHotkeyHandlers.TryGetValue(hotkey, out var handler))
                handler?.Invoke(hotkey);

            HotkeyPressed?.Invoke(this, new HotkeyPressedEventArgs(hotkey));

            return true;
        }

        public event EventHandler<HotkeyPressedEventArgs>? HotkeyPressed;

    }
}