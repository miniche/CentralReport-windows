using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace CR_winserv
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        static void Main()
        {
            #if (!DEBUG)
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
			    { 
				    new CRService() 
			    };
                ServiceBase.Run(ServicesToRun);
            #else
                CRService service = new CRService();
                service.myCustomTest();
            #endif
        }
    }
}
