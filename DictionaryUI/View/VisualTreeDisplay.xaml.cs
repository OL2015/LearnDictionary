using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DictionaryUI.View
{
    /// <summary>
    /// Interaction logic for VisualTreeDisplay.xaml
    /// </summary>
    public partial class VisualTreeDisplay : Window
    {
        public VisualTreeDisplay()
        {
            InitializeComponent();
        }

        public void ShowVisualTree(DependencyObject element)
        {
            // Clear the tree.
            VisualTreeElements.Items.Clear();
            LogicalTreeElements.Items.Clear();
            // Start processing elements, begin at the root.
            ProcessVisualElement(element, null);
            ProcessLogicalElement(element, null);
        }

        private void ProcessVisualElement(DependencyObject element, TreeViewItem previousItem)
        {
            // Create a TreeViewItem for the current element.
            TreeViewItem item = new TreeViewItem();
            item.Header = element.GetType().Name;
            item.IsExpanded = true;
            // Check whether this item should be added to the root of the tree
            //(if it's the first item), or nested under another item.
            if (previousItem == null)
            {
                VisualTreeElements.Items.Add(item);
            }
            else
            {
                previousItem.Items.Add(item);
            }
            // Check whether this element contains other elements.
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                // Process each contained element recursively.
                ProcessVisualElement(VisualTreeHelper.GetChild(element, i), item);
            }
        }

        private void ProcessLogicalElement(DependencyObject element, TreeViewItem previousItem)
        {
            // Create a TreeViewItem for the current element.
            TreeViewItem item = new TreeViewItem();
            item.Header = element.GetType().Name;
            item.IsExpanded = true;
            // Check whether this item should be added to the root of the tree
            //(if it's the first item), or nested under another item.
            if (previousItem == null)
            {
                LogicalTreeElements.Items.Add(item);
            }
            else
            {
                previousItem.Items.Add(item);
            }
            // Check whether this element contains other elements.
            var children =LogicalTreeHelper.GetChildren(element);
            foreach (var z in children)
            {
                DependencyObject c1 = z as DependencyObject;
                if (c1!=null)
                    ProcessLogicalElement(c1, item);
                FrameworkElement c2 = z as FrameworkElement;
                if (c2 != null)
                    ProcessLogicalElement(c2, item);
                FrameworkContentElement c3 = z as FrameworkContentElement;
                if (c3 != null)
                    ProcessLogicalElement(c3, item);
            }

        }
 
    }
}
