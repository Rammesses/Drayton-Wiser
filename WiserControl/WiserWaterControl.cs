﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wiser.DataObjects;
using System.Drawing.Drawing2D;

namespace WiserControl
{
    public partial class WiserWaterControl : UserControl
    {
        public event EventHandler ChangeRequested;

        private int radius = 20;
        [DefaultValue(20)]
        public int Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                this.RecreateRegion();
            }
        }
        private GraphicsPath GetRoundRectagle(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
            path.AddArc(bounds.X + bounds.Width - radius, bounds.Y, radius, radius, 270, 90);
            path.AddArc(bounds.X + bounds.Width - radius, bounds.Y + bounds.Height - radius,
                        radius, radius, 0, 90);
            path.AddArc(bounds.X, bounds.Y + bounds.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            return path;
        }
        private void RecreateRegion()
        {
            var bounds = new Rectangle(this.ClientRectangle.Location, this.ClientRectangle.Size);
            bounds.Inflate(-1, -1);
            using (var path = GetRoundRectagle(bounds, this.Radius))
                this.Region = new Region(path);
            this.Invalidate();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.RecreateRegion();
        }

        private HotWater _hotWater;

        public WiserWaterControl(HotWater hotWater)
        {
            InitializeComponent();
            this.RecreateRegion();

            Refresh(hotWater);
        }

        public void Refresh(HotWater hotWater)
        {
            _hotWater = hotWater;
            toggleSwitch1.Checked = hotWater.HotWaterRelayState == "On";
            if (Program.MainForm.HubWaterBoost)
                autoManualPictureBox.Tag = "FromBoost";
            else
                autoManualPictureBox.Tag = hotWater.HotWaterDescription; 
            ShowAutoManualImage();
        }

        private void WiserRoomControl_MouseDown(object sender, MouseEventArgs e)
        {
            this.DoDragDrop(this, DragDropEffects.All);
        }

        private void WiserRoomControl_Paint(object sender, PaintEventArgs e)
        {
        }

        private void ShowAutoManualImage()
        {
            switch (autoManualPictureBox.Tag.ToString())
            {
                case "FromSchedule":
                    autoManualPictureBox.Image = Resources.ScheduleImage;
                    break;
                case "FromManualOverride":
                    autoManualPictureBox.Image = Resources.ManualImage;
                    break;
                case "FromComfortMode":
                    autoManualPictureBox.Image = Resources.ComfortOnImage;
                    break;
                case "FromEco":
                    autoManualPictureBox.Image = Resources.EcoOnImage;
                    break;
                case "FromBoost":
                    autoManualPictureBox.Image = Resources.BoostImage;
                    break;
                default:
                    autoManualPictureBox.Image = Resources.ScheduleImage;
                    break;

            }
        }

        private void autoManualPictureBox_Click(object sender, EventArgs e)
        {
            if (autoManualPictureBox.Tag.ToString() == "FromSchedule")
            {   
                //boost water for 60 minutes
                Program.wiserConnection.SetHotWaterOverride(_hotWater.id, true, 60);
            }
            else if (autoManualPictureBox.Tag.ToString() == "FromManualOverride" || autoManualPictureBox.Tag.ToString() == "FromBoost")
            {
                Program.wiserConnection.DisableHotWaterOverride(_hotWater.id);
            }

            if (ChangeRequested != null)
                ChangeRequested(this, null);
        }


        private void toggleSwitch1_Layout(object sender, LayoutEventArgs e)
        {
            toggleSwitch1.Left = toggleSwitch1.Parent.Width / 2 - toggleSwitch1.Width / 2;
            toggleSwitch1.Top = toggleSwitch1.Parent.Height / 2 - toggleSwitch1.Height / 2;
        }

        private void toggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void toggleSwitch1_Click(object sender, EventArgs e)
        {
            Program.wiserConnection.SetHotWaterOverride(_hotWater.id, !toggleSwitch1.Checked);
            if (ChangeRequested != null)
                ChangeRequested(this, null);
        }

        private void WiserWaterControl_Load(object sender, EventArgs e)
        {
            toggleSwitch1.Left = toggleSwitch1.Parent.Width / 2 - toggleSwitch1.Width / 2;
            toggleSwitch1.Top = toggleSwitch1.Parent.Height / 2 - toggleSwitch1.Height / 2;

        }

    }
}
