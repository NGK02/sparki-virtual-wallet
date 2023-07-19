using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumSystem.DataAccess.Exceptions
{
    public class EntityNotBlockedException:ApplicationException
    {
        public EntityNotBlockedException(string message):base(message) { }

    }
}
