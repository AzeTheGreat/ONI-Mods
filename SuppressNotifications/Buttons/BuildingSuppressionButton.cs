using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuppressNotifications
{
    class BuildingSuppressionButton : SuppressionButton
    {
        [MyCmpAdd]
        private CopyBuildingSettings copyBuildingSettings;
    }
}
