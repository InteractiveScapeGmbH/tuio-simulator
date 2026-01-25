namespace TuioSimulator.Utils
{
    public static class KeyUtils
    {
        public static bool IsCtrlPressed()
        {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            var keyboard = UnityEngine.InputSystem.Keyboard.current;
            if (keyboard == null) return false;

#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            return keyboard.leftCtrlKey.isPressed || keyboard.rightCtrlKey.isPressed;
#else
            return keyboard.ctrlKey.isPressed;
#endif

#elif ENABLE_LEGACY_INPUT_MANAGER
        return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
#else
        return false;
#endif
        }

#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        public static bool IsCommandPressed()
        {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            var keyboard = UnityEngine.InputSystem.Keyboard.current;
            if (keyboard == null) return false;
            return keyboard.leftCommandKey.isPressed || keyboard.rightCommandKey.isPressed;
#elif ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);
#else
            return false;
#endif
        }
#endif
    }
}
