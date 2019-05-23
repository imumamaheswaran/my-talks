using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hero.Library
{
    interface IHeroQualities
    {
        void FightEvil();
        void SavePeople();
    }

    public class Superman : IHeroQualities
    {
        public void FightEvil()
        {
            string str = "";
        }

        public void SavePeople()
        {
            throw new NotImplementedException();
        }
    }
}
