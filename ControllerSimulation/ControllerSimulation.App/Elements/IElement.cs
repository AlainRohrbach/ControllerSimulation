using ControllerSimulation.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace ControllerSimulation.App.Elements
{
    public class IElement : BindableBase
    {
        public double valueLast;

        public IElement(double initialValue = 0)
        {
            valueLast = initialValue;
        }

        public double Integrate(double value)
        {
            valueLast += value;
            if (valueLast < 0) valueLast = 0;
            else if (valueLast > 100) valueLast = 100;
            return valueLast;
        }
    }
}
