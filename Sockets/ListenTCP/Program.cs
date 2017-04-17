using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ListenTCP
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

#if (!DEBUG)
            try
            {
                ServiceBase.Run(new ListenTCPService());
            }
            catch (Exception ex)
            {
                Util.Logger.LogController.Instance.GravarLogErro(ex);
                throw;
            }
#else
            var service = new ListenTCPService();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#endif
        }


    }
}
