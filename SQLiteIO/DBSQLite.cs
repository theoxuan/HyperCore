﻿using HyperKore.Common;
using HyperKore.IO;
using HyperKore.Utility;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SQLite;
using System.Linq;

namespace SQLiteIO
{
	public class DBSQLite : IDBReader, IDBWriter
	{
		private const string buildCmd = "CREATE TABLE IF NOT EXISTS 'Card'('id' TEXT NOT NULL,'zid' TEXT,'var' TEXT,'name' TEXT NOT NULL,'zname' TEXT,'set' TEXT NOT NULL,'setcode' TEXT NOT NULL,'color' TEXT,'colorcode' TEXT,'cost' TEXT,'cmc' TEXT,'type' TEXT NOT NULL,'ztype' TEXT,'typecode' TEXT NOT NULL,'pow' TEXT,'tgh' TEXT,'loyalty' TEXT,'text' TEXT,'ztext' TEXT,'flavor' TEXT,'zflavor' TEXT,'artist' TEXT,'rarity' TEXT NOT NULL,'raritycode' TEXT NOT NULL,'number' TEXT NOT NULL,'rulings' TEXT,'legality' TEXT,'rating' TEXT,PRIMARY KEY('id'));CREATE TABLE IF NOT EXISTS 'Set'('SetName' TEXT NOT NULL,'SetCode' TEXT,'LastUpdate' TEXT,'Local' INTEGER,PRIMARY KEY('SetName'));CREATE TABLE IF NOT EXISTS 'File'('id' TEXT NOT NULL,'data' BLOB,'length' INTEGER,PRIMARY KEY('id'))";
		private const string connString = "data source=DATA.db;password=5AEB55D5-F169-4EB2-A768-B20EBD20151E";

		/// <summary>
		/// Single instance of SQLiteConnection
		/// </summary>
		private static readonly SQLiteConnection conn = new SQLiteConnection();

		/// <summary>
		/// Initializes a new instance of the SQLiteIO class.
		/// </summary>
		public DBSQLite()
		{
			conn.ConnectionString = connString;
			Create();
		}

		public string DBType
		{
			get { return "SQLite"; }
		}

		public string Description
		{
			get { return "Store Card/Image/Set database"; }
		}

		public string Name
		{
			get { return "DBSQLite"; }
		}

		public bool Delete(Card card)
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<Card>();
				var que = tab.Where(c => c.ID == card.ID);

				if (que.Count() == 0)
				{
					return false;
				}

				tab.DeleteAllOnSubmit(que);
				datacontext.SubmitChanges();

				return true;
			}
		}

		public bool Delete(string id)
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<Bin>();
				var que = tab.Where(i => i.ID == id);

				if (que.Count() == 0)
				{
					return false;
				}

				tab.DeleteAllOnSubmit(que);
				datacontext.SubmitChanges();

				return true;
			}
		}

		public bool Insert(Card card)
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<Card>();
				var que = tab.Where(c => c.ID == card.ID);

				if (que.Count() != 0)
				{
					return Update(card);
				}

				tab.InsertOnSubmit(card);
				datacontext.SubmitChanges();
				return true;
			}
		}

		public void Insert(IEnumerable<Card> cards)
		{
			foreach (var card in cards)
			{
				Insert(card);
			}
		}

		public void Insert(IEnumerable<Set> sets)
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<Set>();
				tab.InsertAllOnSubmit(sets);
				datacontext.SubmitChanges();
			}
		}

		public bool Insert(string id, byte[] data, ICompressor compressor)
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<Bin>();
				var compdata = compressor == null ? data : compressor.Compress(data);
				var que = tab.Where(i => i.ID == id);

				if (que.Count() != 0)
				{
					return Update(id, data, compressor);
				}

				tab.InsertOnSubmit(new Bin(compdata, id, data.Length));
				datacontext.SubmitChanges();
				return true;
			}
		}

		public IEnumerable<Card> LoadCards()
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<Card>();
				return tab.ToList();
			}
		}

		public byte[] LoadFile(string id, ICompressor compressor)
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<Bin>();
				var datas = tab.Where(i => i.ID == id).ToArray();

				if (datas.Count() != 1)
				{
					return null;
				}

				return compressor == null ? datas[0].Data : compressor.Decompress(datas[0].Data, datas[0].Length);
			}
		}

		public IEnumerable<Set> LoadSets()
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<Set>();
				return tab.ToList();
			}
		}

		public bool Update(Card card)
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<Card>();
				var que = tab.Where(c => c.ID == card.ID);

				if (que.Count() == 0)
				{
					return false;
				}

				foreach (var item in que)
				{
					item.CopyFrom(card);
				}
				datacontext.SubmitChanges();
				return true;
			}
		}

		public bool Update(string id, byte[] data, ICompressor compressor)
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<Bin>();
				var compdata = compressor == null ? data : compressor.Compress(data);
				var que = tab.Where(i => i.ID == id);

				if (que.Count() == 0)
				{
					return false;
				}

				foreach (var item in que)
				{
					item.Data = compdata;
					item.Length = data.Length;
				}
				datacontext.SubmitChanges();
				return true;
			}
		}

		public bool Update(Set set)
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<Set>();
				var que = tab.Where(c => c.SetName == set.SetName);

				if (que.Count() == 0)
				{
					return false;
				}

				foreach (var item in que)
				{
					item.SetCode = set.SetCode;
					item.LastUpdate = set.LastUpdate;
					item.Local = set.Local;
				}
				datacontext.SubmitChanges();
				return true;
			}
		}

		private void Create()
		{
			using (var datacontext = new DataContext(conn))
			{
				datacontext.ExecuteCommand(buildCmd);
			}
		}
	}
}