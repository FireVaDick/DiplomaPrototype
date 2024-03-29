﻿using System;
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

namespace DiplomaProrotype
{
    /// <summary>
    /// Interaction logic for MachineTile.xaml
    /// </summary>
    public partial class MachineTile : UserControl
    {
        #region Свойства
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image",
            typeof(ImageSource), typeof(MachineTile), new PropertyMetadata(null));

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }


        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string), typeof(MachineTile), new PropertyMetadata(null));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id",
            typeof(int), typeof(MachineTile), new PropertyMetadata(null));

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        public static readonly DependencyProperty ProcessesProperty = DependencyProperty.Register("Processes",
            typeof(int), typeof(MachineTile), new PropertyMetadata(null));

        public int Processes
        {
            get { return (int)GetValue(ProcessesProperty); }
            set { SetValue(ProcessesProperty, value); }
        }
        #endregion


        public MachineTile()
        {
            InitializeComponent();
            DataContext = this;
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
            }
        }
    }
}
