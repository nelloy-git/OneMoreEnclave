using Godot;

namespace Utils {
    [Tool]
    [GlobalClass]
    public partial class ConstParam : AbstractParam {
        [Export]
        public override string Name {
            get { return _name; }
            set { _name = value; }
        }
        [Export]
        public float Second;

        private string _name;
    }
}