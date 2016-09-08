using BallMaze.GameManagement;
using System.Xml.Serialization;

namespace BallMaze.Inputs
{
    public class MoveCommand : BoardInputCommand
    {
        [XmlAttribute]
        public Direction direction;

        public MoveCommand()
        {

        }

        public MoveCommand(Direction direction, SaveManager saveManager = null ) : base(saveManager)
        {
            this.direction = direction;
        }

        public override void Execute()
        {
            base.Execute();
            model.PlayTurn(direction);
        }
    }
}
