﻿#region License
// Copyright 2009-2011 Josh Close
// This file is a part of CsvHelper and is licensed under the MS-PL
// See LICENSE.txt for details or visit http://www.opensource.org/licenses/ms-pl.html
// http://csvhelper.com
#endregion
using System;
using System.IO;
using CsvHelper.Configuration;

namespace CsvHelper
{
	/// <summary>
	/// Reads to and writes from CSV files.
	/// </summary>
	public class CsvHelper : IDisposable
	{
		private readonly CsvConfiguration configuration = new CsvConfiguration();
		private bool disposed;

		/// <summary>
		/// The configuration used for reading and writing CSV files.
		/// </summary>
		public CsvConfiguration Configuration
		{
			get { return configuration; }
		}

		/// <summary>
		/// Reads data from a CSV file.
		/// </summary>
		public ICsvReader Reader { get; protected set; }

		/// <summary>
		/// Writes data to a CSV file.
		/// </summary>
		public ICsvWriter Writer { get; protected set; }

		/// <summary>
		/// Creates a new instance of <see cref="CsvHelper"/>
		/// using defaults.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> attached to a CSV file.</param>
		public CsvHelper( Stream stream )
		{
			Reader = new CsvReader( new CsvParser( new StreamReader( stream ), configuration ) );
			Writer = new CsvWriter( new StreamWriter( stream ), configuration );
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public virtual void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing">True if the instance needs to be disposed of.</param>
		protected virtual void Dispose( bool disposing )
		{
			if( !disposed )
			{
				if( disposing )
				{
					Reader.Dispose();
					Writer.Dispose();
				}

				disposed = true;
				Reader = null;
				Writer = null;
			}
		}

		/// <summary>
		/// Checks if the instance has been disposed of.
		/// </summary>
		/// <exception cref="ObjectDisposedException" />
		protected virtual void CheckDisposed()
		{
			if( disposed )
			{
				throw new ObjectDisposedException( GetType().ToString() );
			}
		}
	}
}
