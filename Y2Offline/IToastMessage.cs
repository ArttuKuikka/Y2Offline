using System;
using System.Collections.Generic;
using System.Text;

namespace Y2Offline
{
    public interface IToastMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}
