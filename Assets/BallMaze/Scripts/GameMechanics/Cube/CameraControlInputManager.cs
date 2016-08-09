using BallMaze.GameMechanics;

namespace BallMaze.Inputs
{
    public class CameraControlInputManager : InputManager
    {
        public ACameraTurnAround cameraController;

        protected override void Update()
        {
            Direction direction = GetDirection();
            cameraController.TurnInDirection(direction);
        }
        
    }
}
