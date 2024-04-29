using System;

namespace QuantumRevenant.PixelsinTheSpace
{
    public enum EntityStatus { Entering, PlayArea, Exiting }
    [Flags] public enum DamageTypes { Neutral = 0, Energy = 1, Kinetic = 2, Explosive = 4, Plasma = 8, Biological = 16, Antimatter = 32, AllTypes = -1 }
    [Serializable] public class Damage { public float value; public DamageTypes type; public Damage(float num, DamageTypes dtype) { value = num; type = dtype; } }
    [Flags] public enum PostMortemBulletAction { Nothing = 0, Summon = 1, Explode = 2, Pierce = 4, Alter = 8, All = -1 }
    [Serializable]
    public class StandarMultiplier
    {
        public float damage=1;
        public float speed=1;
        public float scale=1;
        public float reload=1;
        public float weakness=1;
        public StandarMultiplier(float damage=1, float speed=1, float scale=1, float reload=1,float weakness=1)
        {
            this.damage = damage;
            this.speed = speed;
            this.scale = scale;
            this.reload = reload;
            this.weakness=weakness;
        }
    }
}