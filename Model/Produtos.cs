﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SATCARGO.ClassesGerais;
using Android.Database.Sqlite;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TESTEWL.Model
{
    public class Produtos
    {
        public class CarredaDadosProdutos
        {
            public string Id { get; set; }
            public string Produto { get; set; }
            public string Valor { get; set; }
            public string Data { get; set; }
            public string Hora { get; set; }
            private SQLiteDatabase sqldTemp;
            private string sSQLQuery;
            //BancoDados mdTemp;
        }
    }
}