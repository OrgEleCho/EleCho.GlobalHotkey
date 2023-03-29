Windows implementation of EleCho.GlobalHotkey and WPF helper

1. Using namespace in your XAML file

    ```xaml
    <Window x:Class="TestWpf.MainWindow"
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
            xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
            xmlns:local="clr-namespace:TestWpf"
            xmlns:hotkey="clr-namespace:EleCho.GlobalHotkey.Windows.Wpf;assembly=EleCho.GlobalHotkey.Windows.Wpf"
    
    </Window>
    ```

2. Add `hotkey:GlobalHotkey.IsRegistered="True"` on `KeyBinding`.

    ```xaml
    <Window x:Class="TestWpf.MainWindow"
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
            xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
            xmlns:local="clr-namespace:TestWpf"
            xmlns:hotkey="clr-namespace:EleCho.GlobalHotkey.Windows.Wpf;assembly=EleCho.GlobalHotkey.Windows.Wpf"
    
        <Window.InputBindings>
            <KeyBinding Modifiers="Ctrl+Shift" Key="P" Command="{Binding TestCommand}"
                        hotkey:GlobalHotkey.IsRegistered="True"/>
        </Window.InputBindings>

    </Window>
    ```