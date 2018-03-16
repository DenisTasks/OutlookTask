using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BLL.EntitesDTO;
using ViewModel.Models;

namespace TestWpf.Administration.Groups.TreeVievModel
{
    public class TreeClasses : ListView
    {
        internal ObservableCollectionAdv Rows
        {
            get;
            private set;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeListItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeListItem;
        }
    }

    public class TreeListItem : TreeViewItem
    {

    }

    public class ObservableCollectionAdv : ObservableCollection<GroupModel>
    {
        public void RemoveRange(int index, int count)
        {
            this.CheckReentrancy();
            var items = this.Items as List<GroupModel>;
            items.RemoveRange(index, count);
        }

        public void InsertRange(int index, IEnumerable<GroupModel> collection)
        {
            this.CheckReentrancy();
            var items = this.Items as List<GroupModel>;
            items.InsertRange(index, collection);
        }
    }

    public class TreeNode : ITreeModel
    {
        public IEnumerable<GroupModel> GetChildren(object parent)
        {
            throw new NotImplementedException();
        }

        public bool HasChildren(object parent)
        {
            throw new NotImplementedException();
        }
    }
}
