using System.Runtime.InteropServices;

namespace EleCho.GlobalHotkey.Windows
{
    internal static class NativeMethods
    {
        // 定义方法用于注册热键
        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        // 定义方法用于注销热键
        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}