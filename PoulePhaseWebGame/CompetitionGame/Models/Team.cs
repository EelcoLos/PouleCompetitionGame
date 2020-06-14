namespace CompetitionGame
{
    public class Team
    {
        public string TeamName;
        public Stats HomeStats;
        public Stats AwayStats;

        public (float AttackStrength, float DefenseStrength) CalculateAttackAndDefenseStrength(int homeOrAway)
        {
            // As homeOrAway == 0 will be covered by the line below, only 1 or 'other' needs to be described here
            var awayOrOther = homeOrAway == 1 ? (AwayStats.AttackStrength, AwayStats.DefenseStrength) : (1, 1);

            var strengths = homeOrAway == 0 ? (HomeStats.AttackStrength, HomeStats.DefenseStrength) : awayOrOther;
            return strengths;
        }
    }

}