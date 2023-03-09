using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.Tables
{
    public class Table<TRows, TColumns, TValue>
    {
        public Dictionary<(TRows, TColumns), TValue> Dict;
        public HashSet<TRows> Rows;
        public HashSet<TColumns> Columns;
        public IndexOpen<TRows, TColumns, TValue> Open;
        public IndexExist<TRows, TColumns, TValue> Existed;


        public Table() 
        {
            Dict = new Dictionary<(TRows, TColumns), TValue>();
            Rows = new HashSet<TRows>();
            Columns = new HashSet<TColumns>();
            Open = new IndexOpen<TRows, TColumns, TValue>(this);
            Existed = new IndexExist<TRows, TColumns, TValue>(this);
        }

        public void AddColumn(TColumns columns)
        {
            if(!Columns.Contains(columns)) Columns.Add(columns);
        }

        public void AddRow(TRows rows)
        {
            if(!Rows.Contains(rows)) Rows.Add(rows);
        }
    }

    public class IndexOpen<TRows, TColumns, TValue>
    {
        Table<TRows, TColumns, TValue> table;

        public IndexOpen(Table<TRows, TColumns, TValue> table)
        {
            this.table = table;
        }

        public TValue this[TRows rows, TColumns columns]
        {
            get
            {
                if (!table.Dict.ContainsKey((rows, columns))) return default(TValue);
                return table.Dict[(rows, columns)];
            }
            set
            {
                if (!table.Rows.Contains(rows)) table.Rows.Add(rows);
                if (!table.Columns.Contains(columns)) table.Columns.Add(columns);
                table.Dict[(rows, columns)] = value;
            }
        }
    }

    public class IndexExist<TRows, TColumns, TValue>
    {
        Table<TRows, TColumns, TValue> table;

        public IndexExist(Table<TRows, TColumns, TValue> table)
        {
            this.table = table;
        }

        public TValue this[TRows rows, TColumns columns]
        {
            get
            {
                if (!table.Rows.Contains(rows)) throw new ArgumentException();
                if (!table.Columns.Contains(columns)) throw new ArgumentException();
                if(!table.Dict.ContainsKey((rows,columns))) table.Dict[(rows,columns)] = default(TValue);
                return table.Dict[(rows, columns)];
            }
            set
            {
                if (!table.Rows.Contains(rows)) throw new ArgumentException();
                if (!table.Columns.Contains(columns)) throw new ArgumentException();
                table.Dict[(rows, columns)] = value;
            }
        }
    }
}