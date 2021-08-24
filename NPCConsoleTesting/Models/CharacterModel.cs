using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting.Models
{
    class CharacterModel
    {
        public string Name { get; set; }
        public string title { get; set; }
        public int level { get; set; }
        public string characterClass { get; set; }
        public string race { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
        public int currentHP { get; set; }
        public string hp_by_lvl { get; set; }
        public int ac_adj { get; set; }
        public int att_adj { get; set; }
        public string status { get; set; }
        public int str { get; set; }
        public int ex_str { get; set; }   //unsure if it's worth to make a child class for this one property
        public int intel { get; set; }
        public int dex { get; set; }
        public int con { get; set; }
        public int wis { get; set; }
        public int cha { get; set; }
        public int gold { get; set; }
        public string armor { get; set; }
        public string weapon { get; set; }
        public string items { get; set; }
        public int probity { get; set; }
        public string affiliation { get; set; }
        public string notes { get; set; }
    }
}
