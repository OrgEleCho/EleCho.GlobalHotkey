using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EleCho.GlobalHotkey.Windows.Wpf
{
    internal class KeyHelper
    {
        public static EleCho.GlobalHotkey.ModifierKeys GetModifierKeysFromWpf(System.Windows.Input.ModifierKeys modifiers)
        {
            return (EleCho.GlobalHotkey.ModifierKeys)modifiers;
        }

        public static EleCho.GlobalHotkey.Key GetKeyFromWpf(System.Windows.Input.Key key)
        {
            return (EleCho.GlobalHotkey.Key)key;
        }
    }
}
