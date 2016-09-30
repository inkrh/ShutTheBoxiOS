﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Mono.Data.Sqlite;
namespace ShutTheBox
{
	public class ScoreDB
	{
		public Dictionary<int, string> scoreDictionary = new Dictionary<int, string>();

		public ScoreDB ()
		{
		}


		public void CreateSQLiteDatabase (string databaseFile)
		{
			try
			{
				if (!File.Exists (databaseFile))
				{
					SqliteConnection.CreateFile (databaseFile);
					using (SqliteConnection sqlCon = new SqliteConnection (String.Format ("Data Source = {0};", databaseFile)))
					{
						sqlCon.Open ();
						using (SqliteCommand sqlCom = new SqliteCommand (sqlCon))
						{
							sqlCom.CommandText = "CREATE TABLE Scores (ID INTEGER PRIMARY KEY, Score VARCHAR(255))";
							sqlCom.ExecuteNonQuery ();
						}
						sqlCon.Close ();
					}
					Debug.WriteLine( "Database created!");
				} else {
					Debug.WriteLine( "Database already exists!");
				}
			} catch (Exception ex) {
				Debug.WriteLine( String.Format ("Sqlite error: {0}", ex.Message));
			}
		}



		public void InsertData(string databaseFile, String score) {
			score = score.Replace ("'", "");
			score = score.Replace("\"", "");
			score = score.Replace (",", "");
			score = score.Replace (".", "");

			try {
				if (File.Exists(databaseFile)) {
					using (SqliteConnection sqlCon = new SqliteConnection(String.Format("Data Source = {0};", databaseFile))) {
						sqlCon.Open();
						using (SqliteCommand sqlCom = new SqliteCommand(sqlCon)) {

							sqlCom.CommandText = String.Format("INSERT INTO Scores (Score) VALUES ('{0}')", score);
							sqlCom.ExecuteNonQuery();
						}
						sqlCon.Close();
					}
					Debug.WriteLine(String.Format("Inserted {0}", score));
				} else {
					Debug.WriteLine("Database file does not exist!");
				}
			} catch (Exception ex) {
				Debug.WriteLine( String.Format("Insert Error - Sqlite error: {0}", ex.Message));

			}
		}

		public void QueryData(string databaseFile) {
			scoreDictionary.Clear ();

			try {
				if (File.Exists(databaseFile)) {
					using (SqliteConnection sqlCon = new SqliteConnection(String.Format("Data Source = {0};", databaseFile))) {
						sqlCon.Open();
						using (SqliteCommand sqlCom = new SqliteCommand(sqlCon)) {
							sqlCom.CommandText = "SELECT * FROM Scores";
							using (SqliteDataReader dbReader = sqlCom.ExecuteReader()) {
								while (dbReader.Read()) {
									int recordId = Convert.ToInt32(dbReader["ID"]);
									string recordScore = (string) dbReader["Score"];

									if(!scoreDictionary.ContainsKey(recordId)) {
										scoreDictionary.Add(recordId, recordScore);
									}
									Debug.WriteLine(String.Format("ID {0}, Score {1}", recordId, recordScore));
								}

							}
							sqlCon.Close();
						}
					}
				} else {
					Debug.WriteLine( "Database file does not exist!");
				}
			} catch (Exception ex) {
				Debug.WriteLine(String.Format("Query Error - Sqlite error: {0}", ex.Message));
			}
		}

		public void DeleteData(string databaseFile) {
			try{
				if(File.Exists(databaseFile)) {
					using (SqliteConnection sqlCon = new SqliteConnection(String.Format("Data Source = {0};", databaseFile))) {
						sqlCon.Open();
						using (SqliteCommand sqlCom = new SqliteCommand(sqlCon)) {
							sqlCom.CommandText = "DELETE from Scores";
							sqlCom.ExecuteNonQuery();
						}
						sqlCon.Close();
					}
				}

			} catch{
				Debug.WriteLine ("Delete Failure");
			}
		}

	}
}

