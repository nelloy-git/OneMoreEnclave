using Godot;

namespace Utils {
    [Tool]
    [GlobalClass]
    public abstract partial class AbstractParam : Resource {
        public abstract string Name { get; set; }
    }
}