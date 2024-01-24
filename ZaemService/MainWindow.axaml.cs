using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mime;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace ZaemService;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    string connectionString = "server=localhost;database=ZaemiService;port=3306;User Id=root;password=Qwerty_123456";
    public void Authorization(object? sender, RoutedEventArgs e)
    {
        string username = Login.Text;
        string password = Password.Text;
        if (IsUserValid(username, password))
        {
            var form2 = new ZaemService.Choise();
            Hide();
            form2.Show();
        }
        else
        {
            Console.Write("Неверный логин или пароль");
        }
    }
    
    private bool IsUserValid(string username, string password) //проверка пользователей по БД
    {
        bool isValid = false;
    
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT COUNT(1) FROM Users WHERE login = @Username AND pass = @Password";
    
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
    
                connection.Open();
    
                object result = command.ExecuteScalar();
                int count = Convert.ToInt32(result);
    
                if (count == 1)
                {
                    isValid = true;
                }
            }
        }
    
        return isValid;
    }



    public void Exit_PR(object? sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }
}