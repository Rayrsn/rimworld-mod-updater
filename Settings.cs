using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RimworldModUpdater
{
    public static class Settings
    {
        public static bool ShouldBackupMods = true;
        public static bool UseModifiedDate = false;
        public static string Version = Utils.GetProgramVersion().ToString();
    }
}
