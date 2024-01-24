using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ZaemService;

public partial class Choise : Window
{
    public Choise()
    {
        InitializeComponent();
    }
    public void Zaemi(object? sender, RoutedEventArgs e)
    {
        ZaemService.Zaemi zaem = new ZaemService.Zaemi();
        Close();
        zaem.Show(); 
    }
        
    public void Zaemshik(object? sender, RoutedEventArgs e)
    {
        ZaemService.Zaemshik zaemsh = new ZaemService.Zaemshik();
        Close();
        zaemsh.Show(); 
    }

    private void Exit_OnClick(object? sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }
}