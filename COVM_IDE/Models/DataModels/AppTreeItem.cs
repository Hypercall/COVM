using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;

namespace COVM_IDE.Models.DataModels
{
    public class AppTreeItem : AppTreeNode
    {
        public AppTreeItem(string selectionGroup, string header, object dataContext)
            : base(selectionGroup, header)
        {
            DataContext = dataContext;
        }

        public AppTreeItem(string selectionGroup, string header, object dataContext, Image icon)
            : base(selectionGroup, header, icon)
        {
            DataContext = dataContext;
        }
    }
}
