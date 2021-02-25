using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Unit;

namespace Assets.Scripts.Util.Comparers
{
    public class UnitComparer: IEqualityComparer<UnitBase>
    {
        public bool Equals(UnitBase x, UnitBase y)
        {
            return x?.UnitId == y?.UnitId;
        }

        public int GetHashCode(UnitBase obj)
        {
            return obj.GetHashCode();
        }
    }
}
