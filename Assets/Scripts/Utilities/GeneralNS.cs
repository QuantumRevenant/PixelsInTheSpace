using System.Linq;

namespace QuantumRevenant
{
    namespace GeneralNS
    {
        public static class Tags
        {
            public const string NeutralTeam = "NeutralTeam";
            public const string PlayerTeam = "PlayerTeam";
            public const string EnemyTeam = "EnemyTeam";
            public const string PirateTeam = "PirateTeam";
            public const string OtherTeam = "OtherTeam";
            public static bool isTeamTag(string str)
            {
                string[] teams=new string[] {NeutralTeam, PlayerTeam, EnemyTeam,PirateTeam,OtherTeam};
                return teams.Contains(str);
            }
        }

        public enum Layers
        {
            Default = 0,
            TransparentFX = 1,
            IgnoreRaycast = 2,
            Water = 4,
            UI = 5,
            PlayerCharacter = 10,
            NonPlayerCharacter = 11,
            Projectile = 20,
        }
    }
}