using BallMaze.Saving;
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

        public MoveCommand(SaveManager saveManager, Direction direction) : base(saveManager)
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
