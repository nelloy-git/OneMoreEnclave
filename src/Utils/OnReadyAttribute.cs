using System;
using Godot;

namespace Utils {
    static public class ExpandGodotNode {
        static public void OnceOnReady(this Node instance, Action action) {
            void once_action() {
                action.Invoke();
                instance.Ready -= once_action;
            }
            instance.Ready += once_action;
        }
    }

}