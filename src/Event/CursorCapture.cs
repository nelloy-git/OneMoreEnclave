using Godot;
using System.Collections.Generic;

namespace Event {

    public partial class CursorCapture : GodotObject {

        public CursorCapture(Dictionary<PhyLayer, CursorPhyLayer> map) {
            _Map = map;
        }

        public CursorPhyLayer this[PhyLayer layer] {
            get { return _Map[layer]; }
        }

        public int Count {
            get { return _Map.Count; }
        }

        private readonly Dictionary<PhyLayer, CursorPhyLayer> _Map = new();

    }

}