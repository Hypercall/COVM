using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace COVM_IDE.Models.DataModels
{
    public interface IAppTreeDataProvider
    {
        string Name { get; }
        string Path { get; }
        Bitmap Icon { get; }

        bool CanBeDisplayed { get; }
    }
}
