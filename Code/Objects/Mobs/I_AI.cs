using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Objects.Mobs
{
    public interface I_AI
    {
        GameObject Owner { get; set; }
        void TakeTurn();
    }
}
