using Godot;
using Utils;

namespace Event {
    [GlobalClass]
    public partial class EventHandler : Node, Singleton<EventHandler> {
        public static EventHandler Inst {
            get { return Singleton<EventHandler>.Inst; }
        }

        public EventHandler() {
            Singleton<EventHandler>.Init(this);
        }

        ~EventHandler() {
            Singleton<EventHandler>.Free(this);
        }
        
        [Export]
        public CursorDetector CursorDetector;


    }
}