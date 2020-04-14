using System;
using System.Collections.Generic;
using System.Text;
using Wiser.DataObjects;

namespace Wiser
{
    public interface IWiserDataProvider
    {
        HeatHub GetData();
    }
}
