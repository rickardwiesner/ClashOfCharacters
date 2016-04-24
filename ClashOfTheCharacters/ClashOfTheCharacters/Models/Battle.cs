using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ClashOfTheCharacters.Helpers;

namespace ClashOfTheCharacters.Models
{
    public class Battle
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public bool Aired { get { return StartTime.CompareTo(DateTime.Now) < 0; } }

        public bool Calculated { get; set; }

        public string WinnerId { get; set; }

        public int WinnerRankingPoints { get; set; }

        public int LoserRankingPoints { get; set; }

        public virtual ICollection<Attack> Attacks { get; set; }

        public int ChallengeId { get; set; }

        public virtual Challenge Challenge { get; set; }


        private ApplicationDbContext db = new ApplicationDbContext();

        private List<BattleCharacter> challengerTeam;

        private List<BattleCharacter> receiverTeam;

        private Effect effect;

        private bool challengerTeamIsDead { get { return challengerTeam.All(ct => ct.Dead); } }
        private bool receiverTeamIsDead { get { return receiverTeam.All(rt => rt.Dead); } }


        public void CalculateBattle()
        {
            challengerTeam = new List<BattleCharacter>();
            receiverTeam = new List<BattleCharacter>();

            foreach (var teamMember in Challenge.Challenger.TeamMembers.ToList())
            {
                challengerTeam.Add(new BattleCharacter { TeamMemberId = teamMember.Id, Hp = teamMember.Hp });
            }

            foreach (var teamMember in Challenge.Receiver.TeamMembers.ToList())
            {
                receiverTeam.Add(new BattleCharacter { TeamMemberId = teamMember.Id, Hp = teamMember.Hp });
            }

            //Så länge inget av lagen har slut på gubbar...
            while (!challengerTeamIsDead && !receiverTeamIsDead)
            {
                //så plockar vi fram första karatären i båda lagen som inte är död.
                var challengerCharacter = challengerTeam.First(ct => !ct.Dead);
                var receiverCharacter = receiverTeam.First(rt => !rt.Dead);

                //Så länge ingen av dom är döda...
                while (!challengerCharacter.Dead && !receiverCharacter.Dead)
                {
                    int damage = Convert.ToInt32(CalculateDamage(challengerCharacter.TeamMemberId, receiverCharacter.TeamMemberId));

                    db.Attacks.Add(new Attack
                    {
                        BattleId = Id,
                        AttackerId = challengerCharacter.TeamMemberId,
                        DefenderId = receiverCharacter.TeamMemberId,
                        Damage = damage,
                        HpRemaining = receiverCharacter.Hp - damage,
                        Effect = effect
                    });

                    receiverCharacter.Hp -= damage;

                    if (!receiverCharacter.Dead)
                    {
                        damage = Convert.ToInt32(CalculateDamage(receiverCharacter.TeamMemberId, challengerCharacter.TeamMemberId));

                        db.Attacks.Add(new Attack
                        {
                            BattleId = Id,
                            AttackerId = receiverCharacter.TeamMemberId,
                            DefenderId = challengerCharacter.TeamMemberId,
                            Damage = damage,
                            HpRemaining = challengerCharacter.Hp - damage,
                            Effect = effect
                        });

                        challengerCharacter.Hp -= damage;
                    }
                }

                if (challengerCharacter.Dead)
                {
                    db.TeamMembers.Find(receiverCharacter.TeamMemberId).Xp += 10;
                    db.TeamMembers.Find(challengerCharacter.TeamMemberId).Xp += 5;
                }

                else
                {
                    db.TeamMembers.Find(challengerCharacter.TeamMemberId).Xp += 10;
                    db.TeamMembers.Find(receiverCharacter.TeamMemberId).Xp += 5;
                }
            }

            WinnerRankingPoints = 10;
            LoserRankingPoints = -5;

            if (challengerTeamIsDead)
            {
                WinnerId = Challenge.ReceiverId;
                Challenge.Receiver.Rank += WinnerRankingPoints;
                Challenge.Challenger.Rank += LoserRankingPoints;
            }

            else
            {
                WinnerId = Challenge.ChallengerId;
                Challenge.Challenger.Rank += WinnerRankingPoints;
                Challenge.Receiver.Rank += LoserRankingPoints;
            }

            db.SaveChanges();

        }

        float CalculateDamage(int attackerId, int defenderId)
        {
            var attacker = db.TeamMembers.Find(attackerId);
            var defender = db.TeamMembers.Find(defenderId);

            float elementBonus = 1;
            effect = Effect.Normal;

            if (attacker.Character.Element == Element.Gravity && defender.Character.Element != Element.Gravity)
            {
                elementBonus = 1.25f;
                effect = Effect.GravityAttack;
            }

            else if ((int)defender.Character.Element - (int)attacker.Character.Element == -2 || (int)defender.Character.Element - (int)attacker.Character.Element == 6)
            {
                elementBonus = 0.5f;
                effect = Effect.VeryBad;
            }

            else if ((int)defender.Character.Element - (int)attacker.Character.Element == -1 || attacker.Character.Element == Element.Fire && defender.Character.Element == Element.Polution)
            {
                elementBonus = 0.75f;
                effect = Effect.Bad;
            }

            else if ((int)defender.Character.Element - (int)attacker.Character.Element == 1 || attacker.Character.Element == Element.Polution && defender.Character.Element == Element.Fire)
            {
                elementBonus = 1.5f;
                effect = Effect.Good;
            }

            else if ((int)defender.Character.Element - (int)attacker.Character.Element == 2 || (int)defender.Character.Element - (int)attacker.Character.Element == -6)
            {
                elementBonus = 2.0f;
                effect = Effect.VeryGood;
            }

            Random instance = new Random();

            float random = 0;

            while (random < 0.85)
            {
                random = (float)instance.NextDouble();
            }

            //float a = (2 * (float)attacker.Level + 10) / 250;
            //float b = a * ((float)attacker.Damage / (float)defender.Defense);
            //float c = b * (float)attacker.Character.BaseAttack + 2;
            //float d = c * (1.5f * elementBonus * random);

            return (((2 * (float)attacker.Level + 10) / 250) * ((float)attacker.Damage / (float)defender.Defense) * (float)attacker.Character.BaseAttack + 2) * (1.5f * elementBonus * random);
        }
    }
}