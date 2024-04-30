using System;
using System.Diagnostics;
using QuantumRevenant.Timer;

namespace QuantumRevenant.PixelsinTheSpace
{
    public enum EntityStatus { Entering, PlayArea, Exiting }
    [Flags] public enum DamageTypes { Neutral = 0, Energy = 1, Kinetic = 2, Explosive = 4, Plasma = 8, Biological = 16, Antimatter = 32, AllTypes = -1 }
    [Serializable] public class Damage { public float value; public DamageTypes type; public Damage(float num, DamageTypes dtype) { value = num; type = dtype; } }
    [Flags] public enum PostMortemBulletAction { Nothing = 0, Summon = 1, Explode = 2, Pierce = 4, Alter = 8, All = -1 }
}
namespace QuantumRevenant.PixelsinTheSpace.Multiplier
{
    [Serializable]
    public class StandarMultiplier
    {
        private float damage, speed, scale, reload, resistance;
        public static StandarMultiplier EmptyMultiplier() { return new StandarMultiplier(0, 0, 0, 0, 0); }
        public StandarMultiplier(float damage = 1, float speed = 1, float scale = 1, float reload = 1, float resistance = 1)
        {
            this.damage = damage;
            this.speed = speed;
            this.scale = scale;
            this.reload = reload;
            this.resistance = resistance;
        }
        public float Damage { get => damage; set => damage = value; }
        public float Speed { get => speed; set => speed = value; }
        public float Scale { get => scale; set => scale = value; }
        public float Reload { get => reload; set => reload = value; }
        public float Resistance { get => resistance; set => resistance = value; }
        public StandarMultiplier ThisObj { get => this; }
        public StandarMultiplier Clone() { return new StandarMultiplier(damage, speed, scale, reload, resistance); }
        public StandarMultiplier SetStandarMultiplier(float damage = 1, float speed = 1, float scale = 1, float reload = 1, float resistance = 1)
        {
            this.damage = damage;
            this.speed = speed;
            this.scale = scale;
            this.reload = reload;
            this.resistance = resistance;
            return this;
        }
        public StandarMultiplier AddMultiplier(float damage = 0, float speed = 0, float scale = 0, float reload = 0, float resistance = 0)
        {
            return SetStandarMultiplier(this.damage + damage, this.speed + speed, this.scale + scale, this.reload + reload, this.resistance + resistance);
        }
        public StandarMultiplier AddMultiplier(StandarMultiplier m) { return AddMultiplier(m.damage, m.speed, m.scale, m.reload, m.resistance); }
        public static StandarMultiplier AddMultiplier(StandarMultiplier a, StandarMultiplier b) { return a.Clone().AddMultiplier(b); }
        public StandarMultiplier SubstractMultiplier(float damage = 0, float speed = 0, float scale = 0, float reload = 0, float resistance = 0)
        {
            return SetStandarMultiplier(this.damage - damage, this.speed - speed, this.scale - scale, this.reload - reload, this.resistance - resistance);
        }
        public StandarMultiplier SubstractMultiplier(StandarMultiplier m) { return SubstractMultiplier(m.damage, m.speed, m.scale, m.reload, m.resistance); }
        public static StandarMultiplier SubstractMultiplier(StandarMultiplier a, StandarMultiplier b) { return a.Clone().SubstractMultiplier(b); }

        public void AddTimedMultiplier(StandarMultiplier multiplier, float time)
        {
            AddMultiplier(multiplier);
            FunctionTimer.Create(() => SubstractMultiplier(multiplier), time, "Multiplier Timer - Temporal Buff");
        }

        public void SubstractTimedMultiplier(StandarMultiplier multiplier, float time)
        {
            SubstractMultiplier(multiplier);
            FunctionTimer.Create(() => AddMultiplier(multiplier), time, "Multiplier Timer - Temporal Debuff");
        }



        public static StandarMultiplier operator +(StandarMultiplier a, StandarMultiplier b)
        {
            return AddMultiplier(a, b);
        }
        public static StandarMultiplier operator -(StandarMultiplier a, StandarMultiplier b)
        {
            return SubstractMultiplier(a, b);
        }
    }
}