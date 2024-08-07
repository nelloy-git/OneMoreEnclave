using System.Net;
using Godot;

namespace UI {

    public partial class LoadingScreen : Control {
        [Export]
        public ProgressBar ProgressBar = null;
        [Export(PropertyHint.File)]
        public string ScenePath {
            set {
                _ScenePath = value;
                ResourceLoader.LoadThreadedRequest(value);
            }
            get {
                return _ScenePath;
            }
        }

        public override void _Process(double delta) {
            Godot.Collections.Array progress = new();
            var status = ResourceLoader.LoadThreadedGetStatus(_ScenePath, progress);

            switch (status) {
                case ResourceLoader.ThreadLoadStatus.Loaded:
                    GetTree().ChangeSceneToPacked(ResourceLoader.LoadThreadedGet(_ScenePath) as PackedScene);
                    break;
                case ResourceLoader.ThreadLoadStatus.InProgress:
                    ProgressBar.Value = (progress[0].AsSingle()) * 100;
                    break;
                case ResourceLoader.ThreadLoadStatus.Failed:
                    goto case ResourceLoader.ThreadLoadStatus.InvalidResource;
                case ResourceLoader.ThreadLoadStatus.InvalidResource:
                    GD.PushError("Failed to load scene");
                    break;
            }
        }

        private string _ScenePath = null;
    }

}