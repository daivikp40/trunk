using System;
using System.Windows;
using System.Windows.Controls;
using DuplicatePhotosFixer.Models;

namespace DuplicatePhotosFixer.UserControls
{
    public partial class ucResultView : UserControl
    {
        private vmResultView ViewModel;

        public ucResultView()
        {
            InitializeComponent();
            LoadStrings();
            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            try
            {
                //if (App.oMainReference.objResultViewModel == null)
                //{
                //    App.oMainReference.objResultViewModel = new vmResultView();
                //}

                //ViewModel = App.oMainReference.objResultViewModel;
                //this.DataContext = ViewModel;

                //// Set DataContext for child controls
                //ucImageView.DataContext = ViewModel;
                //ucDetailedView.DataContext = ViewModel;

                cGlobalSettings.oLogger?.WriteLogVerbose("ucResultView initialized successfully");
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("InitializeViewModel", ex);
            }
        }

        private void LoadStrings()
        {
            try
            {
                btn_autoMark.Content = cResourceManager.LoadString("DPF_RESULT_UC_AUTO_MARK_TEXT");
                btn_unMarkAll.Content = cResourceManager.LoadString("DPF_RESULT_UC_UNMARK_ALL_TEXT");

                // Show image view as default
                btnView.Text = cResourceManager.LoadString("IDS_IMAGE_VIEW");
                lblSelectionAssitant.Text = cResourceManager.LoadString("DPF_RESULT_SELECTION_ASSISTANT_TEXT");
                llbl_deleteMarked.Text = cResourceManager.LoadString("DPF_FOOTER_UC_DELETE_MARKED_TEXT");

                lbl_DuplicatePhotosSubHeadingText.Text = cResourceManager.LoadString("DPF_TRIAL_PHOTO_FOUND_TEXT");
                lbl_SaveSpaceSubHeadingText.Text = cResourceManager.LoadString("DPF_TRIAL_SPACE_SAVED_TEXT");
                llbl_deleteMarked.Text = cResourceManager.LoadString("DPF_FOOTER_UC_DELETE_MARKED_TEXT");
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("LoadStrings", ex);
            }
        }

        /// <summary>
        /// Reload duplicate groups when view becomes visible
        /// </summary>
        public void RefreshData()
        {
            try
            {
                ViewModel?.LoadDuplicateGroups();
                UpdateStatisticsLabels();
                cGlobalSettings.oLogger?.WriteLogVerbose("Result view data refreshed");
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("RefreshData", ex);
            }
        }

        /// <summary>
        /// Update the statistics labels in the header
        /// </summary>
        private void UpdateStatisticsLabels()
        {
            try
            {
                if (ViewModel != null)
                {
                    // Update duplicate count - find the TextBlock in DockPanel
                    var dockPanel = lbl_DuplicatePhotosSubHeadingText.Parent as DockPanel;
                    if (dockPanel != null && dockPanel.Children.Count > 1)
                    {
                        var countTextBlock = dockPanel.Children[1] as TextBlock;
                        if (countTextBlock != null)
                        {
                            countTextBlock.Text = ViewModel.TotalDuplicates.ToString();
                        }
                    }

                    // Update space to save
                    var dockPanel2 = lbl_SaveSpaceSubHeadingText.Parent as DockPanel;
                    if (dockPanel2 != null && dockPanel2.Children.Count > 1)
                    {
                        var spaceTextBlock = dockPanel2.Children[1] as TextBlock;
                        if (spaceTextBlock != null)
                        {
                            spaceTextBlock.Text = ViewModel.SpaceToSave;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("UpdateStatisticsLabels", ex);
            }
        }

        public class ComboBoxItem
        {
            public string ImagePath { get; set; }
            public string Text { get; set; }
        }

        #region Button Click Handlers

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Back button
            try
            {
                App.oMainReference.ShowHome();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("Back button click", ex);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Auto Mark button
            try
            {
                ViewModel?.AutoMark();
                UpdateStatisticsLabels();
                cGlobalSettings.oLogger?.WriteLogVerbose("Auto Mark clicked");
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("Auto Mark button click", ex);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // UnMark All button
            try
            {
                ViewModel?.UnmarkAll();
                UpdateStatisticsLabels();
                cGlobalSettings.oLogger?.WriteLogVerbose("UnMark All clicked");
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("UnMark All button click", ex);
            }
        }

        private void unMarkbtn_Click(object sender, RoutedEventArgs e)
        {
            // UnMark All button (duplicate handler)
            Button_Click_2(sender, e);
        }

        private void btn_selectionAssistanceOptionA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.oMainReference.ShowAssistantSelection();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("Selection Assistant A click", ex);
            }
        }

        private void btn_selectionAssistanceOptionB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.oMainReference.ShowClearCaheDialog();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("Selection Assistant B click", ex);
            }
        }

        #endregion

        #region View Switching

        private void ImgeView_change(object sender, RoutedEventArgs e)
        {
            try
            {
                resultViewGridSelection.IsOpen = true;
                btnDetailedView.Visibility = Visibility.Visible;
                btnImageView.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("Image view change", ex);
            }
        }

        private void Detailed_View_Change(object sender, RoutedEventArgs e)
        {
            try
            {
                resultViewGridSelection.IsOpen = true;
                btnImageView.Visibility = Visibility.Visible;
                btnDetailedView.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("Detailed view change", ex);
            }
        }

        private void btnDetailedView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                imageViewbtn.Visibility = Visibility.Collapsed;
                detailedViewbtn.Visibility = Visibility.Visible;
                btnDetailedView.Visibility = Visibility.Collapsed;
                ucDetailedView.Visibility = Visibility.Visible;
                ucImageView.Visibility = Visibility.Collapsed;

                if (ViewModel != null)
                    ViewModel.IsImageView = false;

                cGlobalSettings.oLogger?.WriteLogVerbose("Switched to Detailed View");
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("btnDetailedView_Click", ex);
            }
        }

        private void btnImageView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                imageViewbtn.Visibility = Visibility.Visible;
                detailedViewbtn.Visibility = Visibility.Collapsed;
                btnImageView.Visibility = Visibility.Collapsed;
                ucImageView.Visibility = Visibility.Visible;
                ucDetailedView.Visibility = Visibility.Collapsed;

                if (ViewModel != null)
                    ViewModel.IsImageView = true;

                cGlobalSettings.oLogger?.WriteLogVerbose("Switched to Image View");
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("btnImageView_Click", ex);
            }
        }

        #endregion
    }
}