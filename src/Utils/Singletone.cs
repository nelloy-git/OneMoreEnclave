using Godot;

namespace Utils {
    public interface Singleton<T> where T : Node {
        public static T Inst {
            get { return _Inst; }
        }

        public static void Init(T self) {
            if (_Inst != null) {
                GD.PushError("Only one instance is allowed");
                self.QueueFree();
                return;
            }
            _Inst = self;
        }

        public static void Free(T self) {
            if (_Inst == self) {
                _Inst = null;
            }
        }

        private static T _Inst;
    }
}