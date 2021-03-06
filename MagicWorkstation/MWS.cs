﻿using HyperKore.Common;
using HyperKore.IO;
using HyperKore.Utility;
using HyperKore.Xception;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MagicWorkstation
{
	public sealed class MWS : IDeckReader, IDeckWriter
	{
		public string DeckType
		{
			get { return "Magic Workstation"; }
		}

		public string Description
		{
			get { return "Import/Export MWS format deck"; }
		}

		public string FileExt
		{
			get { return "mwDeck"; }
		}

		public string Name
		{
			get { return "MWS"; }
		}

		public Deck Read(Stream input, IEnumerable<Card> database)
		{
			Deck deck = new Deck();
			try
			{
				var mdeck = Open(input);
				foreach (var item in mdeck.MainBoardLands)
				{
					for (int i = 0; i < item.Count; i++)
					{
						deck.MainBoard.Add(Convert(item, database));
					}
				}
				foreach (var item in mdeck.MainBoardSpells)
				{
					for (int i = 0; i < item.Count; i++)
					{
						deck.MainBoard.Add(Convert(item, database));
					}
				}
				foreach (var item in mdeck.SideBoard)
				{
					for (int i = 0; i < item.Count; i++)
					{
						deck.SideBoard.Add(Convert(item, database));
					}
				}

				deck.Name = mdeck.Name;
				deck.Comment = mdeck.Comment;

				return deck;
			}
			catch
			{
				throw;
			}
		}

		public bool Write(Deck deck, Stream output)
		{
			try
			{
				var mdeck = Convert(deck);
				Export(mdeck, output);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private Card Convert(MWSCard card, IEnumerable<Card> database)
		{
			var res = database.FirstOrDefault(c => card.SetCode == c.SetCode && card.Name == c.GetLegalName());
			if (res != null)
			{
				return res;
			}
			else
			{
				throw new CardMissingXception("Card not found when loading MWS deck.", card.Name, card.SetCode);
			}
		}

		private MWSDeck Convert(Deck deck)
		{
			try
			{
				MWSDeck mdeck = new MWSDeck();
				var lpM = deck.MainBoard.ToLookup(c => c.ID);
				foreach (var gp in lpM)
				{
					var tcard = gp.First();
					MWSCard mcard = new MWSCard() { Name = tcard.Name, SetCode = tcard.SetCode, Var = tcard.Var, Count = gp.Count() };
					if (tcard.TypeCode.Contains("L"))
					{
						mdeck.MainBoardLands.Add(mcard);
					}
					else
					{
						mdeck.MainBoardSpells.Add(mcard);
					}
				}

				var lpS = deck.SideBoard.ToLookup(c => c.ID);
				foreach (var gp in lpS)
				{
					var tcard = gp.First();
					MWSCard mcard = new MWSCard() { Name = tcard.Name, SetCode = tcard.SetCode, Var = tcard.Var, Count = gp.Count() };
					mdeck.SideBoard.Add(mcard);
				}

				mdeck.Name = deck.Name;
				mdeck.Comment = deck.Comment;

				return mdeck;
			}
			catch
			{
				throw;
			}
		}

		private void Export(MWSDeck deck, Stream stream)
		{
			try
			{
				var sw = new StreamWriter(stream);

				sw.WriteLine("// Comments\n");
				sw.WriteLine(deck.Name);
				sw.WriteLine(deck.Comment);

				sw.WriteLine("\r\n// Lands\n");
				deck.MainBoardLands.FindAll(c => !string.IsNullOrWhiteSpace(c.Var)).ForEach(c => sw.WriteLine(string.Format("{0} [{1}] {2} (1)", c.Count, c.SetCode, c.Name, c.Var)));
				deck.MainBoardLands.FindAll(c => string.IsNullOrWhiteSpace(c.Var)).ForEach(c => sw.WriteLine(string.Format("{0} [{1}] {2}", c.Count, c.SetCode, c.Name)));

				sw.WriteLine("\r\n// Spells\n");
				deck.MainBoardSpells.ForEach(c => sw.WriteLine("{0} [{1}] {2}", c.Count, c.SetCode, c.Name));

				sw.WriteLine("\r\n// Sideboard\n");
				deck.SideBoard.ForEach(c => sw.WriteLine("SB: {0} [{1}] {2}", c.Count, c.SetCode, c.Name));

				sw.Flush();
			}
			catch (Exception ex)
			{
				throw new IOXception("IO Error happended when exporting MWS file", ex);
			}
		}

		private MWSDeck Open(Stream input)
		{
			try
			{
				var sr = new StreamReader(input);
				sr.BaseStream.Seek(0L, SeekOrigin.Begin);
				MWSDeck deck = new MWSDeck();

				int partID = 1; // 0 - comment,1 - mainlands, 2 - mainspells, 3 - side

				var line = sr.ReadLine();
				while (line != null)
				{
					if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("//"))
					{
						if (line.StartsWith("SB:"))
						{
							partID = 3;
						}

						switch (partID)
						{
							case 0:
								if (deck.Comment == null)
								{
									deck.Comment = line;
								}
								else
								{
									deck.Comment += line;
								}
								break;

							case 1:
								var idxa = line.IndexOf("[");
								var idxb = line.IndexOf("]");

								if (idxa > 0 && idxb > idxa)
								{
									MWSCard card = new MWSCard();

									var count = line.Remove(idxa).Trim();
									card.Count = System.Convert.ToInt32(count);

									var setcode = idxb - idxa > 1 ? line.Substring(idxa + 1, idxb - idxa - 1) : string.Empty;
									card.SetCode = setcode.Trim();

									var name = idxb < line.Length ? line.Substring(idxb + 1) : string.Empty;
									card.Name = name.Trim();

									deck.MainBoardSpells.Add(card);
								}
								break;

							case 2:

								idxa = line.IndexOf("[");
								idxb = line.IndexOf("]");

								if (idxa > 0 && idxb > idxa)
								{
									MWSCard card = new MWSCard();

									var count = line.Remove(idxa).Trim();
									card.Count = System.Convert.ToInt32(count);

									var setcode = idxb - idxa > 1 ? line.Substring(idxa + 1, idxb - idxa - 1) : string.Empty;
									card.SetCode = setcode.Trim();

									var name = idxb < line.Length ? line.Substring(idxb + 1) : string.Empty;
									card.Name = name.Trim();

									var idxc = line.IndexOf("(");
									var idxd = line.IndexOf(")");
									if (idxc > 0 && idxd > idxc)
									{
										var var = idxd - idxc > 1 ? line.Substring(idxc + 1, idxd - idxc - 1) : string.Empty;
										card.Var = var.Trim();
										card.Name = name.Remove(name.IndexOf("(")).Trim();
									}

									deck.MainBoardLands.Add(card);
								}
								break;

							case 3:
								idxa = line.IndexOf("[");
								idxb = line.IndexOf("]");

								if (idxa > 0 && idxb > idxa)
								{
									MWSCard card = new MWSCard();

									var count = line.Remove(idxa).Replace("SB:", string.Empty).Trim();
									card.Count = System.Convert.ToInt32(count);

									var setcode = idxb - idxa > 1 ? line.Substring(idxa + 1, idxb - idxa - 1) : string.Empty;
									card.SetCode = setcode.Trim();

									var name = idxb < line.Length ? line.Substring(idxb + 1) : string.Empty;
									card.Name = name.Trim();

									var idxc = line.IndexOf("(");
									var idxd = line.IndexOf(")");
									if (idxc > 0 && idxd > idxc)
									{
										var var = idxd - idxc > 1 ? line.Substring(idxc + 1, idxd - idxc - 1) : string.Empty;
										card.Var = var.Trim();
										card.Name = name.Remove(name.IndexOf("(")).Trim();
									}

									deck.SideBoard.Add(card);
								}
								break;

							default:
								break;
						}
					}
					else
					{
						if (line.Contains("Comments"))
						{
							partID = 0;
						}
						else if (line.Contains("Sideboard"))
						{
							partID = 3;
						}
						else if (line.Contains("Lands"))
						{
							partID = 2;
						}
						else if (line.Contains("Spells"))
						{
							partID = 1;
						}
					}

					line = sr.ReadLine();
				}

				return deck;
			}
			catch (Exception ex)
			{
				throw new IOXception("IO Error happended when opening MWS file", ex);
			}
		}
	}

	internal class MWSCard
	{
		/// <summary>
		/// Initializes a new instance of the MWSCard class with parameters.
		/// </summary>
		public MWSCard(string setCode, string name, int count = 1, string @var = null)
		{
			Count = count;
			SetCode = setCode;
			Name = name;
			Var = @var;
		}

		/// <summary>
		/// Initializes a new instance of the MWSCard class.
		/// </summary>
		public MWSCard()
		{
			Count = 0;
			SetCode = String.Empty;
			Name = String.Empty;
			Var = String.Empty;
		}

		public int Count
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string SetCode
		{
			get;
			set;
		}

		public string Var
		{
			get;
			set;
		}
	}

	internal class MWSDeck
	{
		/// <summary>
		/// Initializes a new instance of the MWSDeck class with parameters.
		/// </summary>
		public MWSDeck(string name, IEnumerable<MWSCard> mainBoardLands, IEnumerable<MWSCard> mainBoardSpells, IEnumerable<MWSCard> sideBoard, string comment = "")
		{
			Name = name;
			Comment = comment;
			MainBoardLands = new List<MWSCard>(mainBoardLands);
			MainBoardSpells = new List<MWSCard>(mainBoardSpells);
			SideBoard = new List<MWSCard>(sideBoard);
		}

		/// <summary>
		/// Initializes a new instance of the MWSDeck class.
		/// </summary>
		public MWSDeck()
		{
			Name = String.Empty;
			Comment = String.Empty;
			MainBoardLands = new List<MWSCard>();
			MainBoardSpells = new List<MWSCard>();
			SideBoard = new List<MWSCard>();
		}

		public string Comment
		{
			get;
			set;
		}

		public List<MWSCard> MainBoardLands
		{
			get;
			set;
		}

		public List<MWSCard> MainBoardSpells
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public List<MWSCard> SideBoard
		{
			get;
			set;
		}
	}
}