using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERIS.Process
{
    internal class ProcessMonster
    {
        //Reference to logger
        private static log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ProcessMonster()
        {

        }

        public void ProcessMonsterFile(string MonsterFile)
        {
            _log.Info("Processing Monster File");

            try
            {
                //TODO: Add code here to process monster file
            }
            catch (Exception ex)
            {
                _log.Error("Process Monster File Error:" + ex.Message + " " + ex.InnerException + " " + ex.StackTrace);
            }
        }
    }
}
