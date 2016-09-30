using System;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Foundation;
using UIKit;
using Mono.Data.Sqlite;
using CoreGraphics;

namespace ShutTheBox
{
	public partial class ShutTheBoxViewController : UIViewController
	{
		#region game controllers
		private Dictionary<int, bool> boxes = new Dictionary<int, bool>();
		private Boolean hasRolled;
		private int dieTotal;
		private int?[] last = new int?[2];
		private Boolean inProgress;
		private Random random = new Random ();
		private Dictionary<int, string> facts = new Dictionary<int, string>();
		private ScoreDB scoresDB;
		private String sqlitePath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "Scores.db3");
		private float screenHeight;
		private UILabel LastScore;

	//	private UILabel CurrentScore;
		private void GetNumbers()
		{
			getFacts();

		}

		private async void getFacts()
		{
			try{
			for (int i = 3; i < 51; i++)
			{

				string number = Convert.ToString(i);
				string url = "http://numbersapi.com/" + number + "/trivia?fragment";
				var request = HttpWebRequest.Create(url);
				request.Method = "GET";

				using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
				{
					if (response.StatusCode != HttpStatusCode.OK)
						Debug.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
						
					using (StreamReader reader = new StreamReader(response.GetResponseStream()))
					{
						var content = reader.ReadToEnd();
						if (content != null)
						{

							Debug.WriteLine(number + " - " + content.ToString());
							if (!facts.ContainsKey(Convert.ToInt32(number)))
							{
								facts.Add(Convert.ToInt32(number), content.ToString());
							}
						}
					}
				}



			}
			} catch(Exception ex) {
				Debug.WriteLine (ex.Message);
			}
		}
			

		private bool OwtLeft()
		{
			//return(!boxes.ContainsValue (true));
			if (!boxes.ContainsValue(true))
			{
				return false;
			}

			return true;
		}

		private void SetBoard()
		{
			inProgress = true;
			dieTotal = 0;
			hasRolled = false;
			DiceOne.Enabled = true;
			DiceTwo.Enabled = true;

			//Reset.Visibility = Visibility.Collapsed;
			//GoogleAnalytics.EasyTracker.GetTracker().SendView("Game Start");

			//total.Text = "roll to start";
			CurrentScore.Text = "roll to start";
			DiceOne.SetImage (UIImage.FromFile("Roll.png"), UIControlState.Normal);
			DiceTwo.SetImage (UIImage.FromFile ("Roll.png"), UIControlState.Normal);

			if (total.Text != null) {
				if (total.Text.Contains ("score: ")) {
					String temp = total.Text;
					temp = temp.Replace ("score: ", "last score: ");
					total.Text = temp;
				}
			}
			for (int i = 1; i <= 9; i++)
			{
				if (boxes.ContainsKey (i)) {
					boxes [i] = true;
				} else {
					boxes.Add (i, true);
				}
				numberToButton (i).Enabled = true;
				Debug.WriteLine ("i : "+i+"boxes[i] " + boxes [i]);
			}



		}

		private String numberToImage(int which) {
			if (which > 6 || which < 1) {
				return null;
			} else {
				return "d" + which;
			}
		}

		private UIButton numberToButton(int which) {
			switch (which) {
			case 1:
				return One;
			case 2:
				return Two;
			case 3:
				return Three;
			case 4:
				return Four;
			case 5:
				return Five;
			case 6:
				return Six;
			case 7:
				return Seven;
			case 8:
				return Eight;
			case 9:
				return Nine;
			default:
				return null;
			}
		}

			
//			<outlet property="One" destination="5" id="name-outlet-5"/>
//				<outlet property="Two" destination="11" id="name-outlet-11"/>
//				<outlet property="Three" destination="12" id="name-outlet-12"/>
//				<outlet property="Four" destination="13" id="name-outlet-13"/>
//				<outlet property="Six" destination="15" id="name-outlet-15"/>
//				<outlet property="Seven" destination="16" id="name-outlet-16"/>
//				<outlet property="Eight" destination="17" id="name-outlet-17"/>
//				<outlet property="Nine" destination="18" id="name-outlet-18"/>
//				<outlet property="DiceOne" destination="22" id="name-outlet-22"/>
//				<outlet property="DiceTwo" destination="23" id="name-outlet-23"/>
//				<outlet property="Five" destination="14" id="name-outlet-14"/>

		private int buttonToNumber(UIButton whichBtn) {
			int which = (int) whichBtn.Tag;
			Debug.WriteLine("buttonToNumber ("+whichBtn.Tag + ")");
			switch (which) {
			case 5:
				return 1;
			case 11:
				return 2;
			case 12:
				return 3;
			case 13:
				return 4;
			case 14:
				return 5;
			case 15:
				return 6;
			case 16:
				return 7;
			case 17:
				return 8;
			case 18:
				return 9;
			default:
				return 0;
			}
		}

		private bool NoMoreOptions(int rollValue)
		{
			Debug.WriteLine ("NoMoreOptions (" + rollValue + ")");
			int ble = 0;
			if (rollValue == 1)
			{
				if (boxes[1])
				{
					return false;
				}
			}

			if (dieTotal > 9)
			{
				rollValue = 9;
			}

			for (int i = 1; i <= rollValue; i++)
			{
				if (boxes[i])
				{
					ble = ble + 1;
				}
			}

			if (ble > 0)
			{
				return false;
			}

			return true;
		}

		private void GameEnd()
		{
			Debug.WriteLine ("GameEnd()");
			//initiate endOfGame animation
	//		newTimer = new DispatcherTimer();
	//		newTimer.Interval = TimeSpan.FromMilliseconds(250);
	//		newTimer.Tick += OnTimerTick;
	//		newTimer.Start();

			//save to scores table
	//		saveScore = new HS();
	//		saveScore.AddScore(Score());
			//saveScore.SaveFile(saveScore.filename);

			//show remaining die total
			CurrentScore.Text = "last rolled: " + last[0] + " and " + last[1] + " remaining: " + dieTotal.ToString() + "\nno moves left";
			//			total.Text = "last rolled: " + last[0] + " and " + last[1] + "\nremaining: " + dieTotal.ToString() + "\nno moves left";

			total.Text = "score: " + Score();
			scoresDB.InsertData (sqlitePath, Score ());

			//lastRoll.Visibility = Visibility.Collapsed;
			last[0] = null;
			last[1] = null;
			//set dice to invisible + show a reset button
			DiceOne.SetImage (UIImage.FromFile("Reset.png"), UIControlState.Normal);
			DiceTwo.SetImage (UIImage.FromFile ("Reset.png"), UIControlState.Normal);
			inProgress = false;
//			Die1.Visibility = Visibility.Collapsed;
//			Die2.Visibility = Visibility.Collapsed;
//			Reset.Visibility = Visibility.Visible;

			// track a custom event
//			GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Game Ended", "Game Over : " + Score(), null, 0);


		}

		private string Score()
		{


			string outpu = string.Empty;

			foreach (KeyValuePair<int, bool> pair in boxes)
			{
				if (pair.Value)
				{
					outpu = outpu + pair.Key.ToString();
				}
			}

			if (outpu == String.Empty)
			{
				outpu = "0";
			}

			int t = Convert.ToInt32(outpu) + dieTotal;

			if (t == 0)
			{
				return "0 - perfect game";

			}

			if (t == 42)
			{
					return "42 - the answer";
			}

			if (t < 3)
			{
				return Convert.ToString(t) + " - so close";
			}

			//get number facts
			if (facts.ContainsKey(t))
			{
				return Convert.ToString(t) + " - " + facts[t];
			}

			outpu = Convert.ToString(t);

			return outpu;
		}

		private void RollMe(object sender, EventArgs e)
		{
			if (!inProgress) {
				SetBoard ();
				return;
			}

			if (!OwtLeft())
			{
				GameEnd();
				return;
			}

			if (hasRolled)
			{
				if (dieTotal > 0)
				{
					return;
				}
			}

			int d1 = 0;
			int d2 = 0;

			hasRolled = true;

			CryptoRandom cryg = new CryptoRandom ();

			d1 = cryg.Next(1, 7);
			d2 = cryg.Next(1, 7);
			dieTotal = d1 + d2;
			CurrentScore.Text = "to use: " + dieTotal.ToString();

//			total.Text = "to use: " + dieTotal.ToString();
			DiceOne.SetImage (UIImage.FromFile(numberToImage (d1) + ".png"), UIControlState.Normal);
			DiceTwo.SetImage (UIImage.FromFile (numberToImage (d2) + ".png"), UIControlState.Normal);

			last[0] = d1;
			last[1] = d2;

			total.Text = "score: " + Score();
			UpdateColors ();

			if (NoMoreOptions(dieTotal))
			{
				GameEnd();
				return;
			}
		}
			

		private void FlapClick(int which)
		{

			if (0 == which) {
				return;
			}

			if (!hasRolled)
			{
				return;
			}
			if (dieTotal > 0 && OwtLeft())
			{
				if (boxes[which])
				{
					if (dieTotal - which >= 0)
					{
						boxes[which] = false;
						numberToButton (which).Enabled = false;
						//						((Button)e.OriginalSource).Opacity = 0;
						dieTotal = dieTotal - which;
						UpdateColors ();

						if (dieTotal == 0)
						{
							if (!OwtLeft())
							{
								GameEnd();

								return;
							}

							hasRolled = false;
							CurrentScore.Text = "roll again";
							//total.Text = "roll again";
							total.Text = "score: " + Score();
						}
						else
						{
							total.Text = "score: " + Score();
							CurrentScore.Text = "to use: " + dieTotal.ToString();
							if (!OwtLeft())
							{
								GameEnd();

								return;
							}

							if (NoMoreOptions(dieTotal))
							{
								GameEnd();
								return;
							}
						}
					}
				}
			}
		}

		private void UpdateColors() {
			foreach (var a in boxes) {
				if (a.Key <= dieTotal && a.Value) {
					//numberToButton (a.Key).TintColor = UIColor.Green;
					numberToButton (a.Key).TintColor = UIColor.FromRGB(34, 135, 34);
				} else {
					numberToButton (a.Key).TintColor = UIColor.White;
				}
			}
		}
		#endregion

		private void frameAdjust(float height) {
			float offsety = 0;
			float offsetx = 0;
			if ((int)height == 480) {
				return;
			}

			if ((int)height == 568) {
				offsety = 40;
			}

			if ((int)height > 666) {
				offsetx = 40;
				offsety = 140;
				LastScore = new UILabel (new CGRect (42, -60, 236, 52));
				LastScore.TextColor = UIColor.White;
				LastScore.Font = UIFont.FromName ("appleberry", 24);
				LastScore.AdjustsFontSizeToFitWidth = true;
				LastScore.Lines = 1;
				LastScore.TextAlignment = UITextAlignment.Center;
				LastScore.Text = "Shut The Box";
				GameBoard.Add (LastScore);
			}

			foreach (UIView element in GameBoard) {
				if (element is UIButton) {
					var frame = element.Frame;
					frame.X = frame.X + offsetx;
					frame.Y = frame.Y + offsety;
					element.Frame = frame;
				}
				if (element is UILabel) {
					var frame = element.Frame;
//					frame.X = frame.X + offsetx;
					frame.Y = frame.Y + offsety;
					element.Frame = frame;
					(element as UILabel).Font = UIFont.FromName ("appleberry", 18);
				}

			}

			if (null != LastScore) {
				LastScore.Font = UIFont.FromName ("appleberry", 24);
			}

			foreach (UIView element in AboutView) {
				if (element is UILabel) {
					(element as UILabel).Font = UIFont.FromName ("appleberry", 14);
				}
			}

			ScoresClearButton.Font = UIFont.FromName ("appleberry", 18);
			AboutTitle.Font = UIFont.FromName ("appleberry", 24);
			AboutBtn.Font = UIFont.FromName ("appleberry", 32);
			ScoresButton.Font = UIFont.FromName ("appleberry", 32);


				var bgframe = BGImage.Frame;
				bgframe.X = 0;
				bgframe.Y = 0;
				bgframe.Height = 736;
				BGImage.Frame = bgframe;


		}



		public ShutTheBoxViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			screenHeight = (float) UIScreen.MainScreen.Bounds.Height;
			Debug.WriteLine ("Screenheight : " + screenHeight);
			var bounds = GameBoard.Frame;
			Debug.WriteLine ("GameBoard frame : " + bounds);
			frameAdjust (screenHeight);
			GetNumbers ();
			scoresDB = new ScoreDB();
			scoresDB.CreateSQLiteDatabase (sqlitePath);
			// Perform any additional setup after loading the view, typically from a nib.

			One.TouchUpInside += (object sender, EventArgs e) => FlapClick(1);
			Two.TouchUpInside += (object sender, EventArgs e) => FlapClick(2);
			Three.TouchUpInside += (object sender, EventArgs e) => FlapClick(3);
			Four.TouchUpInside += (object sender, EventArgs e) => FlapClick(4);
			Five.TouchUpInside += (object sender, EventArgs e) => FlapClick(5);
			Six.TouchUpInside += (object sender, EventArgs e) => FlapClick(6);
			Seven.TouchUpInside += (object sender, EventArgs e) => FlapClick(7);
			Eight.TouchUpInside += (object sender, EventArgs e) => FlapClick(8);
			Nine.TouchUpInside += (object sender, EventArgs e) => FlapClick(9);

			if (!inProgress) {
				SetBoard ();
			}
			DiceOne.TouchUpInside += (object sender, EventArgs e) => RollMe(sender, e);
			DiceTwo.TouchUpInside += (object sender, EventArgs e) => RollMe(sender, e);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}
			

		partial void AboutBtn_TouchUpInside (UIButton sender)
		{
			ScoresView.Hidden = true;
			ScoresButton.SetTitle("!", UIControlState.Normal);
			AboutView.Hidden = !(AboutView.Hidden);
			if(!AboutView.Hidden) {
				CurrentScore.Hidden = true;
				GameBoard.Hidden = true;
				AboutBtn.SetTitle("*", UIControlState.Normal);
			} else {
				CurrentScore.Hidden = false;
				GameBoard.Hidden = false;
				AboutBtn.SetTitle("?", UIControlState.Normal);
			}


		}
		#endregion

		#region Scores

		partial void ScoresButton_TouchUpInside (UIButton sender)
		{
			foreach(UIView a in ScoresView) {
				if(a is UITableView) {
					foreach(UIView b in a) {
						if(b is UITableViewCell) {
							b.RemoveFromSuperview();
						}
					}
					a.RemoveFromSuperview();
				}
			}

			scoresDB.QueryData(sqlitePath);
			AboutView.Hidden = true;

			AboutBtn.SetTitle("?", UIControlState.Normal);
			ScoresView.Hidden = !(ScoresView.Hidden);
			float h = (float) ScoresView.Bounds.Height;
			float w = (float) ScoresView.Bounds.Width;
			var stRect = new RectangleF(0,48,w-42,h-160);
			UITableView ScoresTable = new UITableView(stRect);
			ScoresTable.TintColor = UIColor.Clear;
			ScoresTable.BackgroundColor = UIColor.Clear;
			ScoresTable.Opaque = true;
			ScoresTable.ReloadData();
			if(!ScoresView.Hidden) {
				CurrentScore.Hidden = true;
				GameBoard.Hidden = true;
				//ScoresTable.Bounds = ScoresView.Bounds;
				ScoresButton.SetTitle("*", UIControlState.Normal);
				scoresDB.QueryData (sqlitePath);
				ScoresTable.Source = new ScoresTableSource(scoresDB.scoreDictionary);
				ScoresView.Add(ScoresTable);
			} else {
				CurrentScore.Hidden = false;
				GameBoard.Hidden = false;
				ScoresButton.SetTitle("!", UIControlState.Normal);
				ScoresTable.RemoveFromSuperview();
				}
			}		

			
		


		partial void ScoresClearButton_TouchUpInside (UIButton sender)
		{
			scoresDB.DeleteData(sqlitePath);

			if (ScoresView.Subviews != null) {
				foreach (UIView a in ScoresView.Subviews) {
					Debug.WriteLine (String.Format ("ScoresView contains {0}", a));
					if (a is UITableView) {
						var temp = a as UITableView;
						temp.RemoveFromSuperview();
						temp.Source = new ScoresTableSource(scoresDB.scoreDictionary);
						ScoresView.Add(temp);
						scoresDB.QueryData (sqlitePath);
						temp.ReloadData();
						total.Text = "";
						ScoresView.Hidden = true;
						CurrentScore.Hidden = false;
						GameBoard.Hidden = false;
						ScoresButton.SetTitle("!", UIControlState.Normal);

					}
				}
			}

		}

	




		#endregion


	}





}

