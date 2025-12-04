// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ShutTheBox
{
	[Register ("ShutTheBoxViewController")]
	partial class ShutTheBoxViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton AboutBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel AboutTitle { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView AboutView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView BaseView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel CurrentScore { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton DiceOne { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton DiceTwo { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Eight { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Five { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Four { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView GameBoard { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Nine { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton One { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ScoresButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ScoresClearButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView ScoresView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Seven { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Six { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Three { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel total { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Two { get; set; }

		[Action ("AboutBtn_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void AboutBtn_TouchUpInside (UIButton sender);

		[Action ("ScoresButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ScoresButton_TouchUpInside (UIButton sender);

		[Action ("ScoresClearButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ScoresClearButton_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (AboutBtn != null) {
				AboutBtn.Dispose ();
				AboutBtn = null;
			}
			if (AboutTitle != null) {
				AboutTitle.Dispose ();
				AboutTitle = null;
			}
			if (AboutView != null) {
				AboutView.Dispose ();
				AboutView = null;
			}
			if (BaseView != null) {
				BaseView.Dispose ();
				BaseView = null;
			}
			if (CurrentScore != null) {
				CurrentScore.Dispose ();
				CurrentScore = null;
			}
			if (DiceOne != null) {
				DiceOne.Dispose ();
				DiceOne = null;
			}
			if (DiceTwo != null) {
				DiceTwo.Dispose ();
				DiceTwo = null;
			}
			if (Eight != null) {
				Eight.Dispose ();
				Eight = null;
			}
			if (Five != null) {
				Five.Dispose ();
				Five = null;
			}
			if (Four != null) {
				Four.Dispose ();
				Four = null;
			}
			if (GameBoard != null) {
				GameBoard.Dispose ();
				GameBoard = null;
			}
			if (Nine != null) {
				Nine.Dispose ();
				Nine = null;
			}
			if (One != null) {
				One.Dispose ();
				One = null;
			}
			if (ScoresButton != null) {
				ScoresButton.Dispose ();
				ScoresButton = null;
			}
			if (ScoresClearButton != null) {
				ScoresClearButton.Dispose ();
				ScoresClearButton = null;
			}
			if (ScoresView != null) {
				ScoresView.Dispose ();
				ScoresView = null;
			}
			if (Seven != null) {
				Seven.Dispose ();
				Seven = null;
			}
			if (Six != null) {
				Six.Dispose ();
				Six = null;
			}
			if (Three != null) {
				Three.Dispose ();
				Three = null;
			}
			if (total != null) {
				total.Dispose ();
				total = null;
			}
			if (Two != null) {
				Two.Dispose ();
				Two = null;
			}
		}
	}
}
