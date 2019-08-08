using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuppressNotifications
{
    class StatusItemsSuppressed : KMonoBehaviour
    {
        public bool IsShown(StatusItem item)
        {
            return false;
        }
    }
}
