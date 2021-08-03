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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyMusic.Wpf.Controls
{
    /// <summary>
    /// Tab control is extended to place the search box on Tab Strip Panel
    /// </summary>
    public class TabControlEx : TabControl
    {
        private WatermarkTextbox watermarkTextbox;
        static TabControlEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabControlEx), new FrameworkPropertyMetadata(typeof(TabControlEx)));
        }


        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(TabControlEx), new PropertyMetadata(null, (obj, args) => 
            {
                if(obj is TabControlEx tabControl && args.NewValue is string val && !string.IsNullOrEmpty(val) && val.Length > 0)
                {
                    if(tabControl.Items.Count > 1)
                    {
                        if(tabControl.SelectedItem == null || (tabControl.SelectedItem != null && tabControl.SelectedItem != tabControl.Items[1]))
                        {
                            tabControl.Focusable = false;
                            tabControl.SelectedIndex = 1;                            
                            tabControl.watermarkTextbox?.Focus();
                            tabControl.Focusable = true;
                        }                       
                    }
                }
            }));



        public bool EnableSearchBox
        {
            get { return (bool)GetValue(EnableSearchBoxProperty); }
            set { SetValue(EnableSearchBoxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EnableSearchBox.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableSearchBoxProperty =
            DependencyProperty.Register("EnableSearchBox", typeof(bool), typeof(TabControlEx), new PropertyMetadata(true));




        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            watermarkTextbox = GetTemplateChild("Part_SearchBox") as WatermarkTextbox;
            if(watermarkTextbox != null)
            {
                watermarkTextbox.SetBinding(WatermarkTextbox.TextProperty, new Binding("SearchText") { Source = this, Mode = BindingMode.TwoWay });
                watermarkTextbox.SetBinding(WatermarkTextbox.IsEnabledProperty, new Binding("EnableSearchBox") { Source = this, Mode = BindingMode.TwoWay });
            }            
        }
    }
}
