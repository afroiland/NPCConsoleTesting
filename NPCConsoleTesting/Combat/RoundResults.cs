using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    public class RoundResults
    {
        public List<Character> characters;
        public List<string> roundLog;

        public RoundResults(List<Character> chars, List<string> log)
        {
            characters = chars;
            roundLog = log;
        }
    }
}
