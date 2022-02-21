using PSniffGUI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSniffGUI.Model.Parsing
{
    [System.Diagnostics.DebuggerDisplay("{ToString()}")]
    internal class HandAction
    {
        public HandAction()
        {
            FinalChipCommit = true;
            ActionStatFlags = ActionStatType.None;
            OpportunityStatFlags = ActionOpportunityStatFlags.None;
        }

        public HandPlayer Player { get; set; }
        public Hand Hand { get; set; }
        public PlayerAction Action { get; set; }
        public int Amount { get; set; }
        public bool FinalChipCommit { get; set; }
        public ActionStatType ActionStatFlags { get; set; }
        public ActionOpportunityStatFlags OpportunityStatFlags { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1} {{{2}}} -- {{{3}}}", Player.Name, Action, ActionStatFlags.ToString(), OpportunityStatFlags.ToString());
        }
    }
}
