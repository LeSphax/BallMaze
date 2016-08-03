

namespace BallMaze
{
    class Paths
    {
        public const string FOLDER_SEPARATOR = "/";
        public const string PARTICLES = "Prefabs"+ FOLDER_SEPARATOR + "Particles"+ FOLDER_SEPARATOR;
        public const string FAIRY_DUST = PARTICLES + "FairyDust";
        public const string BRICK_SMOKE = PARTICLES + "BrickSmoke";
        private const string MECHANICS = "Prefabs" + FOLDER_SEPARATOR + "Mechanics" + FOLDER_SEPARATOR;
        public const string TILE = MECHANICS + "Tile";
        public const string NO_OBJECTIVE_BALL = MECHANICS + "NoObjectiveBall";
        public const string WALL = MECHANICS + "Wall";
        public const string OBJECTIVE1_BALL = MECHANICS + "Objective1Ball";
        public const string OBJECTIVE2_BALL = MECHANICS + "Objective2Ball";
        public const string OBJECTIVE1_TILE = MECHANICS + "Objective1Tile";
        public const string OBJECTIVE2_TILE = MECHANICS + "Objective2Tile";
        public const string LEVEL_FILES = "LevelFiles" + FOLDER_SEPARATOR;
    }
}
