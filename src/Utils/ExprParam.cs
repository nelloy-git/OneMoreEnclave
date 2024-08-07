using Godot;

namespace Utils {
    [Tool]
    [GlobalClass]
    public partial class ExprParam : AbstractParam {
        [Export]
        public override string Name {
            get { return _name; }
            set { _name = value; }
        }
        [Export]
        public string Value;

        private string _name;
    }
}