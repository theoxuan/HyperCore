﻿using System;
using System.Threading;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace HyperMTG
{
	public partial class MainWindow : NavigationWindow
	{
		private bool _allowDirectNavigation = false;
		private NavigatingCancelEventArgs _navArgs = null;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void NavigationWindow_Navigating(object sender, NavigatingCancelEventArgs e)
		{
			if (Content != null && !_allowDirectNavigation)
			{
				e.Cancel = true;
				_navArgs = e;
				this.IsHitTestVisible = false;
				DoubleAnimation da = new DoubleAnimation(0.3d, new Duration(TimeSpan.FromMilliseconds(300)));
				da.Completed += FadeOutCompleted;
				this.BeginAnimation(OpacityProperty, da);
			}
			_allowDirectNavigation = false;
		}

		private void FadeOutCompleted(object sender, EventArgs e)
		{
			(sender as AnimationClock).Completed -= FadeOutCompleted;

			this.IsHitTestVisible = true;

			_allowDirectNavigation = true;
			switch (_navArgs.NavigationMode)
			{
				case NavigationMode.New:
					if (_navArgs.Uri == null)
					{
						NavigationService.Navigate(_navArgs.Content);
					}
					else
					{
						NavigationService.Navigate(_navArgs.Uri);
					}
					break;
				case NavigationMode.Back:
					NavigationService.GoBack();
					break;

				case NavigationMode.Forward:
					NavigationService.GoForward();
					break;
				case NavigationMode.Refresh:
					NavigationService.Refresh();
					break;
			}

			Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
				(ThreadStart)delegate()
			{
				DoubleAnimation da = new DoubleAnimation(1.0d, new Duration(TimeSpan.FromMilliseconds(200)));
				this.BeginAnimation(OpacityProperty, da);
			});
		}
	}
}
