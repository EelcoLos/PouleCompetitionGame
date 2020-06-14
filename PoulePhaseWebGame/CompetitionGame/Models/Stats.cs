namespace CompetitionGame
{
    public class Stats
    {
        public int GamesPlayed;
        public int GoalsMade;
        public int GoalsConceded;

        public float AttackStrength =>  (float)GoalsMade / (float)GamesPlayed;
        public float DefenseStrength => (float)GoalsConceded / (float)GamesPlayed;
    }

}