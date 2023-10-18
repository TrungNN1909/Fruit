using System;
using System.Collections;
using System.Collections.Generic;
using Unicorn.Utilities;
using UnityEngine;

namespace Unicorn
{
    public class SkillsTab : MonoBehaviour
    {
        [SerializeField] public List<SkillButton> skills;

        public void Init()
        {
            for (int i = 1; i <= skills.Count; i++)
            {
                skills[i-1].id = i;
                skills[i-1].Init();
            }
        }

        public void NewPhaseStartAction()
        {
            foreach (SkillButton bt in skills)
            {
                if(!bt.isCoolingDown)
                    bt.NewPhaseStart();
            }
        }

        public void CheckFreeSkill()
        {
            foreach (SkillButton bt in skills)
            {
                bt.CheckNewDaySkill();
            }
        }
    }
}
