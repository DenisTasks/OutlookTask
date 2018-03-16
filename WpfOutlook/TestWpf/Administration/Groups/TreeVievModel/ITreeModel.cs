using BLL.EntitesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Models;

namespace TestWpf.Administration.Groups.TreeVievModel
{
    interface ITreeModel
    {
        IEnumerable<GroupModel> GetChildren(object parent);
        bool HasChildren(object parent);
    }
}
