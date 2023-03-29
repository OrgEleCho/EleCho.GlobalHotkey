using EleCho.GlobalHotkey.Windows.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EleCho.GlobalHotkey.Windows.Wpf
{
    /// <summary>
    /// Register global hotkeys in WPF application.
    /// </summary>
    public class GlobalHotkey : DependencyObject
    {
        private readonly HwndSource hwndSource;


        /// <summary>
        /// 订阅初始化
        /// </summary>
        private GlobalHotkey()
        {
            HwndSourceParameters hwndSourceParameters =
                new HwndSourceParameters($"WND:{typeof(GlobalHotkey).FullName}")
                {
                    HwndSourceHook = Hook,
                    ParentWindow = (IntPtr)(-3),    // a magic window handle
                };

            hwndSource =
                new HwndSource(hwndSourceParameters);

            Manager =
                new GlobalHotkeyManager(hwndSource.Handle);
        }

        // hook 窗体消息用的
        private IntPtr Hook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // 让管理器处理消息
            if (Manager != null &&
                Manager.Process(hwnd, msg, wParam, lParam))
            {
                handled = true;
                return (IntPtr)1;
            }

            handled = false;
            return IntPtr.Zero;
        }

        public static GlobalHotkey Instance { get; } = new GlobalHotkey();

        public GlobalHotkeyManager Manager { get; }


        // 公开一个注册 KeyBinding 的方法
        public void Register(KeyBinding keyBinding)
        {
            Hotkey hotkey = new Hotkey(
                KeyHelper.GetModifierKeysFromWpf(keyBinding.Modifiers),
                KeyHelper.GetKeyFromWpf(keyBinding.Key));

            HotkeyHandler hotkeyHandler = hotkey =>
            {
                if (keyBinding.Command != null &&
                    keyBinding.Command.CanExecute(keyBinding.CommandParameter))
                    keyBinding.Command.Execute(keyBinding.CommandParameter);
            };

            Manager.Register(hotkey, hotkeyHandler);
        }

        // 公开注册一个取消注册 KeyBinding 的方法
        public void Unregister(KeyBinding keyBinding)
        {
            Manager.Unregister(
                KeyHelper.GetModifierKeysFromWpf(keyBinding.Modifiers),
                KeyHelper.GetKeyFromWpf(keyBinding.Key));
        }

        /// <summary>
        /// Get whether a KeyBinding is registered as global hotkey
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [AttachedPropertyBrowsableForType(typeof(KeyBinding))]
        public static bool GetIsRegistered(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsRegisteredProperty);
        }

        /// <summary>
        /// Register or Unregister a global hotkey from a KeyBinding
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        [AttachedPropertyBrowsableForType(typeof(KeyBinding))]
        public static void SetIsRegistered(DependencyObject obj, bool value)
        {
            obj.SetValue(IsRegisteredProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsRegistered.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRegisteredProperty =
            DependencyProperty.RegisterAttached("IsRegistered", typeof(bool), typeof(GlobalHotkey), new PropertyMetadata(false, IsRegisteredChanged));

        private static void IsRegisteredChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d))
                return;

            if (d is not KeyBinding keyBinding)
                throw new ArgumentException("Only KeyBinding can be registered global hotkey");
            if (e.NewValue is not bool b)
                throw new InvalidOperationException("Not a boolean value");

            if (b)
                Instance.Register(keyBinding);
            else
                Instance.Unregister(keyBinding);
        }
    }
}
