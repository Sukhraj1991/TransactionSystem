using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using TransactionSystem.Models;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace TransactionSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult OfficeTransactions()
        {
            return View();
        }

        public IActionResult AddTransaction()
        {

            ViewBag.TransactionType = new SelectList(GetTransactionType(), "Value", "Text");
            return View();
        }

        [HttpPost]
        public IActionResult AddTransaction(AddTransactionModel add)
        {
            if (ModelState.IsValid)
            {

                var path = @"DB/db.txt";
                string data = $"{add.TransactionType};{add.Amount};{add.Descreption};{DateTime.Now.ToShortDateString()};Balance;lastInsertedID {Environment.NewLine}";
                if (!System.IO.File.Exists(path))
                {

                    System.IO.File.AppendAllText(path, data);
                }
                else
                {
                    var lastBalance = GetLastBalance();
                    var lastInsertedID= GetID();
                    if (add.TransactionType.ToLower() == "credit")
                    {
                        lastBalance += add.Amount;
                    }
                    if (add.TransactionType.ToLower() == "debit")
                    {
                        lastBalance -= add.Amount;
                    }

                    if (lastBalance == 0 && add.TransactionType.ToLower() == "debit")
                    {
                            //debit not allowed
                    }
                    else
                    { 
                    data = data.Replace("Balance", lastBalance.ToString()).Replace("lastInsertedID", lastInsertedID.ToString());
                    System.IO.File.AppendAllText(path, data);
                }
                }
                return RedirectToAction("OfficeTransactions");
            }
            ViewBag.TransactionType = new SelectList(GetTransactionType(), "Value", "Text");
            return View();

        }
        [HttpPost]
        public string LoadData()
        {
            var path = @"DB/db.txt";
            List<OfficeTransactionModel> list = new List<OfficeTransactionModel>();
            if (!System.IO.File.Exists(path))
            {
                //""
            }
            else
            {
                var data = ReadDbFile();
                foreach (var line in data)
                {
                    OfficeTransactionModel obj = new OfficeTransactionModel();
                    var lineData = line.Split(';');
                    obj.TransactionID = Convert.ToInt32(lineData[5]);
                    obj.TransactionDate = lineData[3];
                    obj.Descreption = lineData[2];
                    if (lineData[0].ToLower() == "credit")
                    { obj.Credit = lineData[1].ToString(); 
                    }
                    
                    else { obj.Debit = lineData[1].ToString(); }

                    obj.RunningBalance = lineData[4].ToString(); ;

                    list.Add(obj);
                }
            }
            var newList = list.OrderByDescending(e => e.TransactionID);
            return JsonConvert.SerializeObject(new { data= newList });
        }

        public static List<ListModel> GetTransactionType()
        {
            List<ListModel> list = new List<ListModel>();
            ListModel ListModel = new ListModel();
            ListModel.Value = "Credit";
            ListModel.Text = "Credit";
            list.Add(ListModel);

            ListModel ListModel2 = new ListModel();
            ListModel2.Value = "Debit";
            ListModel2.Text = "Debit";

            list.Add(ListModel2);

            return list;
        }

        private string[] ReadDbFile()
        {
            var path = @"DB/db.txt";
            var data = System.IO.File.ReadAllLines(path);
            return data;
        }
        private double GetLastBalance()
        {
            
            var fileData = ReadDbFile();
            var lastBalance = 0.0;
            if (fileData.Length > 0)
            {
                lastBalance = Convert.ToDouble(fileData[fileData.Length - 1].Split(";")[4]);
            }
            return lastBalance;
        }

        private int GetID()
        {
            var fileData = ReadDbFile();
            var lastID = 0;
            if (fileData.Length > 0)
            {
                lastID = Convert.ToInt32(fileData[fileData.Length - 1].Split(";")[5]);
            }
            return lastID+1;
        }

    }
}
