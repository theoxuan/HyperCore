﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HyperKore.IO;
using System.IO;
using HyperKore.Common;

namespace Mage
{
	public class MAGE : IDeckReader, IDeckWriter
	{
		public string FileExt
		{
			get { throw new NotImplementedException(); }
		}

		public string DeckType
		{
			get { throw new NotImplementedException(); }
		}

		public Deck Read(Stream input, IEnumerable<Card> database)
		{
			throw new NotImplementedException();
		}

		public string Description
		{
			get { throw new NotImplementedException(); }
		}

		public string Name
		{
			get { throw new NotImplementedException(); }
		}


		public bool Write(Deck deck, Stream output)
		{
			throw new NotImplementedException();
		}
	}
}
