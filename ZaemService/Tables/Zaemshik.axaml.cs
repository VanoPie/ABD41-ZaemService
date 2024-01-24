using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System;
using System.IO;
using System.Windows;

namespace ZaemService;

public partial class Zaemshik : Window
{
    public Zaemshik()
    {
        InitializeComponent();
        string fullTable = "SELECT * FROM zaemshik";
        ShowTable(fullTable);
    }
    private List<zaemshik> client;
    private string connStr = "server=localhost;database=ZaemiService;port=3306;User Id=root;password=Qwerty_123456";
    private MySqlConnection conn;
    
    public void ShowTable(string sql)
    {
        client = new List<zaemshik>();
        conn = new MySqlConnection(connStr);
        conn.Open();
        MySqlCommand command = new MySqlCommand(sql, conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var Clients = new zaemshik()
            {
                id = reader.GetInt32("id"),
                familiya = reader.GetString("familiya"),
                imya = reader.GetString("imya"),
                otchestvo = reader.GetString("otchestvo"),
                pasport = reader.GetInt32("pasport"),
                telephone = reader.GetString("telephone"),
                adres_raboti = reader.GetString("adres_raboti"),
                zdanie_raboti = reader.GetString("zdanie_raboti")
            };
            client.Add(Clients);
        }
        conn.Close();
        DataGrid.ItemsSource = client;
    }
    
    private void Search(object? sender, TextChangedEventArgs e)
    {
        var clients = client;
        clients = clients.Where(x => x.imya.Contains(NameSearch.Text)).ToList();
        DataGrid.ItemsSource = clients;
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        ZaemService.Choise back = new ZaemService.Choise();
        Close();
        back.Show(); 
    }

    private void Reset_OnClick(object? sender, RoutedEventArgs e)
    {
        string fullTable = "SELECT * FROM zaemshik";
        ShowTable(fullTable);
        NameSearch.Text = string.Empty;
    }

    private void DeleteData(object? sender, RoutedEventArgs e)
    {
        try
        {
            zaemshik currentClient = DataGrid.SelectedItem as zaemshik;
            if (currentClient == null)
            {
                return;
            }
            conn = new MySqlConnection(connStr);
            conn.Open();
            string sql = "DELETE FROM zaemshik WHERE id = " + currentClient.id;
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            client.Remove(currentClient);
            ShowTable("SELECT * FROM zaemshik");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    private void AddData(object? sender, RoutedEventArgs e)
    {
        zaemshik newClient = new zaemshik();
        ZaemService.CRUDZaemshiki add = new ZaemService.CRUDZaemshiki(newClient, client);
        add.Show();
        this.Close();
    }

    private void Edit(object? sender, RoutedEventArgs e)
    {
        zaemshik currentClient = DataGrid.SelectedItem as zaemshik;
        if (currentClient == null)
            return;
        ZaemService.CRUDZaemshiki edit = new  ZaemService.CRUDZaemshiki(currentClient, client);
        edit.Show();
        this.Close();
    }
    
    private void exportButton_Click(object sender, RoutedEventArgs e) //печать в Excel
    {
        string outputFile = @"C:\Users\VanoP\Desktop\print\otchet.xlsx";
        string query = "SELECT * FROM zaemshik";     
        MySqlCommand command = new MySqlCommand(query, conn);
        conn.Open();
        MySqlDataReader dataReader = command.ExecuteReader();
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Заемщики");
                int row = 1;
                
                for (int i = 1; i <= dataReader.FieldCount; i++)
                {
                    worksheet.Cells[row, i].Value = dataReader.GetName(i - 1);
                }

                while (dataReader.Read())
                {
                    row++;
                    for (int i = 1; i <= dataReader.FieldCount; i++)
                    {
                        worksheet.Cells[row, i].Value = dataReader[i - 1];
                    }
                }
                excelPackage.SaveAs(new FileInfo(outputFile));
            }
            dataReader.Close();
            conn.Close();
        Console.WriteLine("Данные успешно экспортированы в Excel файл.");
    }
}