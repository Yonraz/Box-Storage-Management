using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxStorage.Services.Remover
{
    public static class Remover
    {
        public static void Remove(Box box, int amount)
        {
            Storage.RemoveBox(box, amount);
            JsonHandler.Save();
        }
        public static void ForceRemove(Box box, int amount)
        {
            Storage.ForceRemoveBox(box, amount);
        }
    }
}
