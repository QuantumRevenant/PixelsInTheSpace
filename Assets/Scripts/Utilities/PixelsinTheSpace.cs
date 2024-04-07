namespace QuantumRevenant
{
    namespace PixelsinTheSpace
    {
        public enum EntityStatus { Entering, PlayArea, Exiting }
        [System.Flags]
        public enum DamageTypes { Standard = 0, Energy = 1, Kinetic = 2, Explosive = 4, Plasma = 8, Biological = 16, Antimatter = 32, AllTypes = -1 }
        public struct Damage { public int value; public DamageTypes type; }

    }
}