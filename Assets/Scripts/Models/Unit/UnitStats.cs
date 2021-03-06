using System;

namespace Assets.Scripts.Models.Unit
{
    [Serializable]
    public class UnitStats
    {
        public float Speed;
        public float Health;
        public float Attack;
        public float Armor;
        public float Defence;
        public float AttackRange;
        public float AttackSpeed;

        public static UnitStats MakeCopy(UnitStats obj)
        {
            return new UnitStats
            {
                Armor = obj.Armor,
                Attack = obj.Attack,
                Defence = obj.Defence,
                Health = obj.Health,
                Speed = obj.Speed,
                AttackRange = obj.AttackRange,
                AttackSpeed = obj.AttackSpeed
            };
        }

        public override bool Equals(object obj)
        {
            return obj is UnitStats stats &&
                   Speed == stats.Speed &&
                   Health == stats.Health &&
                   Attack == stats.Attack &&
                   Armor == stats.Armor &&
                   Defence == stats.Defence &&
                   AttackRange == stats.AttackRange &&
                   AttackSpeed == stats.AttackSpeed;
        }

        public override int GetHashCode()
        {
            int hashCode = -1714822693;
            hashCode = hashCode * -1521134295 + Speed.GetHashCode();
            hashCode = hashCode * -1521134295 + Health.GetHashCode();
            hashCode = hashCode * -1521134295 + Attack.GetHashCode();
            hashCode = hashCode * -1521134295 + Armor.GetHashCode();
            hashCode = hashCode * -1521134295 + Defence.GetHashCode();
            return hashCode;
        }
    }
}