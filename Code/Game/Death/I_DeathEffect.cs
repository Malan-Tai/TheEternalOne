using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Game.Death
{
    public interface I_DeathEffect
    {

        string Name { get; set; }
        string Description { get; set; }

        bool CheckIfPossible();
        void Apply();

    }
}
