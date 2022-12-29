using System;
using UnityEngine;

namespace Enemy
{
    [Serializable]
    public class AttackStats
    {
        [SerializeField]
        private float baseDamage = 100;

        [SerializeField]
        private int attacksPerSecond = 1;

        [SerializeField]
        private float attackRange = 1.1f;

        public int AttacksPerSecond => attacksPerSecond;
        public float BaseDamage => baseDamage;
        public float AttackRange => attackRange;

        public float CalculateDamageDelt() => baseDamage;
    }
}