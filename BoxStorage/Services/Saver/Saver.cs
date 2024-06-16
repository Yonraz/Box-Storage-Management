using BoxStorage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxStorage.Models.Saver
{
    public class Saver
    {

        public Saver() 
        {
        }
        public static void Save(Box b, int amount)
        {
            Storage.Restock(b, amount);
            JsonHandler.Save();
        }
    }
}
