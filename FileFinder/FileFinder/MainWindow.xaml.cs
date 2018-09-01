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
using System.IO;

namespace FileFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListBoxFoundFilesItem_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ListBoxItem lb = (ListBoxItem)sender;
            FileInfo fi = new FileInfo(lb.Content.ToString());
            System.Diagnostics.Process.Start("explorer",fi.Directory.ToString());
        }

        //
        private void listBoxFoundFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb != null)
            {
                lb.ScrollIntoView(lb.SelectedItem);
            }
        }
    }
}
