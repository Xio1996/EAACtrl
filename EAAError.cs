using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAACtrl
{
    internal class EAAError
    {
        private int ErrorNo = 0;

        public Dictionary<int, string> ErrorMapping = new Dictionary<int, string>()
        {
            {0,"OK"},
            {-1,"AstroPlanner, Cannot connect, is AstroPlanner running?" },
            {1,"AstroPlanner, Bad JSON payload"},{2,"AstroPlanner, Missing script name"},{3,"AstroPlanner, Script does not exist"},{4,"AstroPlanner, Authentication string error"},
            {5,"AstroPlanner, Command parameter missing"},{6,"AstroPlanner, Internal error"},{7,"AstroPlanner, Unknown command"},{8,"AstroPlanner, Error in script"},
            {9,"AstroPlanner, Cannot open script file"},{10,"AstroPlanner, Script not of general type"},{11,"AstroPlanner, Script is empty"},{12,"AstroPlanner, Script name illegal"}
        };

        public void Reset()
        {
            ErrorNo = 0;
        }

        public int ErrorNumber
        {
            get
            {
                return ErrorNo;
            }
            set
            {
                ErrorNo = value;
            }
        }

        public string Message
        {
            get
            {
                return ErrorMapping[ErrorNo];
            }
        }
    }
}
