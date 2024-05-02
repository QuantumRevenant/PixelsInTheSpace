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
    public class StandardMultiplier
    {
        public float damage, speed, scale, reload, resistance;
        public static StandardMultiplier ZeroMultiplier() { return new StandardMultiplier(0, 0, 0, 0, 0); }
        public static StandardMultiplier OneMultiplier() { return new StandardMultiplier(1, 1, 1, 1, 1); }
        public static StandardMultiplier CreateMultiplier(float damage = 1, float speed = 1, float scale = 1, float reload = 1, float resistance = 1)
        { return new StandardMultiplier(damage, speed, scale, reload, resistance); }

        public StandardMultiplier(float damage = 1, float speed = 1, float scale = 1, float reload = 1, float resistance = 1)
        {
            this.damage = damage;
            this.speed = speed;
            this.scale = scale;
            this.reload = reload;
            this.resistance = resistance;
        }
        public StandardMultiplier ThisObj { get => this; }
        public StandardMultiplier Clone() { return new StandardMultiplier(damage, speed, scale, reload, resistance); }
        public StandardMultiplier SetStandarMultiplier(float damage = 1, float speed = 1, float scale = 1, float reload = 1, float resistance = 1)
        {
            this.damage = damage;
            this.speed = speed;
            this.scale = scale;
            this.reload = reload;
            this.resistance = resistance;
            return this;
        }
        #region Addition
        public StandardMultiplier AddMultiplier(float damage = 0, float speed = 0, float scale = 0, float reload = 0, float resistance = 0)
        {
            return SetStandarMultiplier(this.damage + damage, this.speed + speed, this.scale + scale, this.reload + reload, this.resistance + resistance);
        }
        public StandardMultiplier AddMultiplier(StandardMultiplier m) { return AddMultiplier(m.damage, m.speed, m.scale, m.reload, m.resistance); }
        public static StandardMultiplier AddMultiplier(StandardMultiplier a, StandardMultiplier b) { return a.Clone().AddMultiplier(b); }
        #endregion
        #region Substraction
        public StandardMultiplier SubstractMultiplier(float damage = 0, float speed = 0, float scale = 0, float reload = 0, float resistance = 0)
        {
            return SetStandarMultiplier(this.damage - damage, this.speed - speed, this.scale - scale, this.reload - reload, this.resistance - resistance);
        }
        public StandardMultiplier SubstractMultiplier(StandardMultiplier m) { return SubstractMultiplier(m.damage, m.speed, m.scale, m.reload, m.resistance); }
        public static StandardMultiplier SubstractMultiplier(StandardMultiplier a, StandardMultiplier b) { return a.Clone().SubstractMultiplier(b); }
        #endregion

        #region Multiplication
        public StandardMultiplier MultiplyMultiplier(float damage = 1, float speed = 1, float scale = 1, float reload = 1, float resistance = 1)
        {
            return SetStandarMultiplier(this.damage * damage, this.speed * speed, this.scale * scale, this.reload * reload, this.resistance * resistance);
        }
        public StandardMultiplier MultiplyMultiplier(float val)
        {
            return SetStandarMultiplier(damage * val, speed * val, scale * val, reload * val, resistance * val);

        }
        public StandardMultiplier MultiplyMultiplier(StandardMultiplier m) { return MultiplyMultiplier(m.damage, m.speed, m.scale, m.reload, m.resistance); }
        public static StandardMultiplier MultiplyMultiplier(StandardMultiplier a, StandardMultiplier b) { return a.Clone().MultiplyMultiplier(b); }
        public static StandardMultiplier MultiplyMultiplier(StandardMultiplier a, float val) { return a.Clone().MultiplyMultiplier(val); }
        #endregion

        #region Division
        public StandardMultiplier DivideMultiplier(float damage = 1, float speed = 1, float scale = 1, float reload = 1, float resistance = 1)
        {
            if (damage * speed * scale * reload * resistance == 0)
                UnityEngine.Debug.Log($"Error, enviaste un valor 0 a dividir, se reemplazará con 1");

            return SetStandarMultiplier(this.damage / damage == 0 ? 1 : damage, this.speed / speed == 0 ? 1 : speed, this.scale / scale == 0 ? 1 : scale, this.reload / reload == 0 ? 1 : reload, this.resistance / resistance == 0 ? 1 : resistance);
        }
        public StandardMultiplier DivideMultiplier(float val)
        {
            return DivideMultiplier(val, val, val, val, val);

        }
        public StandardMultiplier DivideMultiplier(StandardMultiplier m) { return DivideMultiplier(m.damage, m.speed, m.scale, m.reload, m.resistance); }
        public static StandardMultiplier DivideMultiplier(StandardMultiplier a, StandardMultiplier b) { return a.Clone().DivideMultiplier(b); }
        public static StandardMultiplier DivideMultiplier(StandardMultiplier a, float val) { return a.Clone().DivideMultiplier(val); }
        #endregion
        #region Timed
        public void AddTimedMultiplier(StandardMultiplier multiplier, float time)
        {
            AddMultiplier(multiplier);
            FunctionTimer.Create(() => SubstractMultiplier(multiplier), time, "Multiplier Timer - Temporal Buff");
        }

        public void SubstractTimedMultiplier(StandardMultiplier multiplier, float time)
        {
            SubstractMultiplier(multiplier);
            FunctionTimer.Create(() => AddMultiplier(multiplier), time, "Multiplier Timer - Temporal Debuff");
        }
        #endregion

        public static StandardMultiplier operator +(StandardMultiplier a, StandardMultiplier b) { return AddMultiplier(a, b); }
        public static StandardMultiplier operator -(StandardMultiplier a, StandardMultiplier b) { return SubstractMultiplier(a, b); }

        public static StandardMultiplier operator *(StandardMultiplier a, StandardMultiplier b) { return MultiplyMultiplier(a, b); }
        public static StandardMultiplier operator *(StandardMultiplier a, float b) { return MultiplyMultiplier(a, b); }
        public static StandardMultiplier operator *(float b, StandardMultiplier a) { return MultiplyMultiplier(a, b); }
        public static StandardMultiplier operator *(StandardMultiplier a, int b) { return MultiplyMultiplier(a, b); }
        public static StandardMultiplier operator *(int b, StandardMultiplier a) { return MultiplyMultiplier(a, b); }

        public static StandardMultiplier operator /(StandardMultiplier a, StandardMultiplier b) { return DivideMultiplier(a, b); }
        public static StandardMultiplier operator /(StandardMultiplier a, float b) { return DivideMultiplier(a, b); }
        public static StandardMultiplier operator /(float b, StandardMultiplier a) { return DivideMultiplier(a, b); }
        public static StandardMultiplier operator /(StandardMultiplier a, int b) { return DivideMultiplier(a, b); }
        public static StandardMultiplier operator /(int b, StandardMultiplier a) { return DivideMultiplier(a, b); }

    }
}